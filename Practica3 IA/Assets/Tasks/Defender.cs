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
	public class Defender : Action
	{
		public float speed = 5f;

		private GameObject target;

		private bool enteredInMySide;

		private bool shootTime = true;

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

			if (Object.FindObjectOfType<GameManager>().goalTime())
			{
				return TaskStatus.Failure;
			}

			if (target.transform.position.x > transform.position.x)
				return TaskStatus.Failure;

			chutEnableed ce = gameObject.GetComponent<chutEnableed>();

			if (ce.canIShoot())
			{
				ce.disableShootTime();
				shootTime = false;
				ce.disableShoot();
				Chut();
			}

			if (target.transform.position.x > 0)
			{
				 navMeshAgent.SetDestination(target.transform.position);

			}

			else return TaskStatus.Success;

			return TaskStatus.Running;
		}

		private void Chut()
		{
			Vector3 direction = -Object.FindObjectOfType<GameManager>().getRedDirection();
			direction = direction.normalized;
			//direction *= -1;

			if (direction.x < 0)
				target.GetComponent<Rigidbody>().AddForce(direction * 1500);

			Object.FindObjectOfType<GameManager>().addBlueTeamChut();

		}
	}
}
