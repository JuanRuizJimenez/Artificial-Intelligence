namespace BehaviorDesigner.Samples
{

	using UnityEngine;
	// En versiones antiguas de Unity al parece hacía falta usar este otro espacio de nombres
#if !(UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4)
	using UnityEngine.AI;
#endif
	using BehaviorDesigner.Runtime;
	using BehaviorDesigner.Runtime.Tasks;
	using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

	[TaskCategory("FootBall")]
	[TaskDescription("Cuando el balon entra en su zona, trata de echarlo fuera.")]
	public class RedTeamAtackBehaviour : Action
	{
		public float speed = 5f;

		private GameObject target;

		private bool enteredInMySide;

		private bool shootTime = true;

		// fleedDistance * fleedDistance, calcular la raíz cuadrada es caro cuando en realidad no hace falta
		//private float sqrFleedDistance;

		private NavMeshAgent navMeshAgent;

		public override void OnAwake()
		{
			// Cachear para acceder más rápido
			navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

			// Establece la velocidad linear y angular
			navMeshAgent.speed = speed;
			//navMeshAgent.angularSpeed = rotationSpeed.Value;
			//sqrFleedDistance = fleedDistance * fleedDistance;
		}

		public override void OnStart()
		{
			// Huye en la dirección opuesta
			target = GameObject.FindGameObjectWithTag("ball");
			navMeshAgent.enabled = true;
		}

		public override TaskStatus OnUpdate()
		{
			Rigidbody rb = gameObject.GetComponent<Rigidbody>();
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			rb.Sleep();

			float minimunDistance = 10000000000000f;
			Vector3 ball = target.transform.position;

			foreach (GameObject g in GameObject.FindGameObjectsWithTag("RedTeam"))
			{
				if (g != gameObject)
				{
					if (Vector3.Distance(g.transform.position, ball) < minimunDistance)
					{
						minimunDistance = Vector3.Distance(g.transform.position, ball);
					}
				}
			}

			if (Vector3.Distance(transform.position, ball) > minimunDistance)
				return TaskStatus.Failure;

			if (Object.FindObjectOfType<GameManager>().goalTime())
			{
				return TaskStatus.Failure;
			}

			if (transform.position.x > ball.x || transform.position.z > ball.z + 5 || transform.position.z < ball.z - 5)
			{
				Vector3 v = new Vector3();
				v = target.transform.position;

				navMeshAgent.SetDestination(new Vector3(v.x - 5, v.y, v.z));
			}
			
			else navMeshAgent.SetDestination(ball);

			chutEnableed ce = gameObject.GetComponent<chutEnableed>();

			if (ce.canIShoot())
			{
				ce.disableShootTime();
				shootTime = false;
				ce.disableShoot();
				Chut();
			}

			return TaskStatus.Running;
		}

		private void Chut()
		{
			Vector3 direction = -Object.FindObjectOfType<GameManager>().getBlueDirection();
			direction = direction.normalized;
			direction *= -1;

			if (direction.x > 0)
				target.GetComponent<Rigidbody>().AddForce(direction * 1500);

			Object.FindObjectOfType<GameManager>().addRedTeamChut();
		}
	}
}
