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
	public class Goal : Conditional
	{

		public override TaskStatus OnUpdate()
		{

			if (Object.FindObjectOfType<GameManager>().goalTime())
			{
				return TaskStatus.Success;
			}

			return TaskStatus.Failure;
		}
	}
}

