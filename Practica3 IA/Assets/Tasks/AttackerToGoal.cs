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
	[TaskDescription("Vuelve a su posición original.")]
	public class AttackerToGoal : Action
	{
		Vector3 originalPosition;

		private bool shootTime = true;

		private NavMeshAgent navMeshAgent;

		private Rigidbody rb;

		private GameObject ballPosition;

		GameManager gm;

		public override void OnStart()
		{
			ballPosition = GameObject.FindGameObjectWithTag("ball");

			gm = Object.FindObjectOfType<GameManager>();

			navMeshAgent = GetComponent<NavMeshAgent>();
			rb = GetComponent<Rigidbody>();
		}

		public override TaskStatus OnUpdate()
		{
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			rb.Sleep();

			if (gm.goalTime())
			{
				return TaskStatus.Success;
			}

			if (Vector3.Distance(transform.position, (gm.getAuxRedGoalPos() + (gm.getRedDirection() * 1.1f))) > 10)
			{
				return TaskStatus.Failure;
			}

			navMeshAgent.speed = 10;
			navMeshAgent.SetDestination((gm.getAuxRedGoalPos() + (gm.getRedDirection() * 0.9f)));

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
			Vector3 direction = -gm.getRedDirection();
			direction = direction.normalized;
			
			if (direction.x < 0)
				ballPosition.GetComponent<Rigidbody>().AddForce(direction * 1500);

			Object.FindObjectOfType<GameManager>().addBlueTeamChut();
		}
	}
}