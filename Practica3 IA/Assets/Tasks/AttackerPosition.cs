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
	public class AttackerPosition : Action
	{
		Vector3 originalPosition;

		private NavMeshAgent navMeshAgent;

		private Rigidbody rb;

		GameManager gm;

		public override void OnStart()
		{
			navMeshAgent = GetComponent<NavMeshAgent>();
			rb = GetComponent<Rigidbody>();

			gm = Object.FindObjectOfType<GameManager>();
		}

		public override TaskStatus OnUpdate()
		{
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			rb.Sleep();

			if (gm.goalTime())
			{
				return TaskStatus.Failure;
			}

			Vector3 pos = (gm.getAuxRedGoalPos() + (gm.getRedDirection() * 1.1f));

			if (GameObject.FindGameObjectWithTag("ball").transform.position.x >= 0)
			{
				return TaskStatus.Failure;
			}


			if (pos.x < transform.position.x || pos.x >= transform.position.x + 10)
			{
				navMeshAgent.speed = 25;
				navMeshAgent.SetDestination(Object.FindObjectOfType<GameManager>().getAuxRedGoalPos() + (gm.getRedDirection() * 1.1f));
			}

			else return TaskStatus.Success;

			return TaskStatus.Running;
		}
	}
}