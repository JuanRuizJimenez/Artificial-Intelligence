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
	public class Shoot : Action
	{
		public GameObject attacker;
		private GameObject target;
		private bool shootTime = true;
		private NavMeshAgent navMeshAgent;

		public override void OnStart()
		{
			navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

			// Huye en la dirección opuesta
			target = GameObject.FindGameObjectWithTag("ball");
		}

		public override TaskStatus OnUpdate()
		{
			Debug.DrawRay(transform.position, -(transform.position - attacker.transform.position) * 1000, Color.yellow);

			Rigidbody rb = gameObject.GetComponent<Rigidbody>();
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			rb.Sleep();

			if (Object.FindObjectOfType<GameManager>().goalTime()  ||
				Vector3.Distance(transform.position, target.transform.position) > Vector3.Distance(attacker.transform.position, target.transform.position))
			{
				return TaskStatus.Failure;
			}

			chutEnableed ce = gameObject.GetComponent<chutEnableed>();

			if (ce.canIShoot())
			{
				ce.disableShootTime();
				shootTime = false;
				ce.disableShoot();
				Chut();
			}

			navMeshAgent.SetDestination(target.transform.position);

			return TaskStatus.Running;
		}

		private void Chut()
		{
			Vector3 direction = -(transform.position - attacker.transform.position); //-Object.FindObjectOfType<GameManager>().getRedDirection();
			direction = direction.normalized;
			//direction *= -1;

			target.GetComponent<Rigidbody>().AddForce(direction * 1500);

			Object.FindObjectOfType<GameManager>().addBlueTeamChut();
		}
	}
}
