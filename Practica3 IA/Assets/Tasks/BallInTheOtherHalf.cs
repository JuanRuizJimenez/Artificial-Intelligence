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
	[TaskDescription("Comprueba si se ha marcado un gol.")]
	public class ballInTheOtherHalf : Conditional
	{

		public override TaskStatus OnUpdate()
		{

			if (GameObject.FindGameObjectWithTag("ball").transform.position.x < 0)
			{
				//gameObject.GetComponent<NavMeshAgent>().isStopped = true;
				return TaskStatus.Success;
			}

			//gameObject.GetComponent<NavMeshAgent>().isStopped = false;
			return TaskStatus.Failure;
		}
	}
}

