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
	public class AttackerVuelveAlSitio : Action
	{
		Vector3 originalPosition;

		private NavMeshAgent navMeshAgent;

		private Rigidbody rb;

		GameManager gm;

		public override void OnAwake()
		{
			originalPosition = transform.position;
		}

		public override void OnStart()
		{
			navMeshAgent = GetComponent<NavMeshAgent>();
			gm = gm = Object.FindObjectOfType<GameManager>();
			rb = GetComponent<Rigidbody>();
		}

		public override TaskStatus OnUpdate()
		{
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			rb.Sleep();

			if (transform.position.x > originalPosition.x || ((gm.getAuxRedGoalPos() + (gm.getRedDirection() * 1.1f)).x < transform.position.x + 10))
				return TaskStatus.Success;

			else
			{
				navMeshAgent.speed = 20;
				navMeshAgent.SetDestination(new Vector3(originalPosition.x + 10, originalPosition.y, originalPosition.z));
			}

			return TaskStatus.Running;
		}
	}
}