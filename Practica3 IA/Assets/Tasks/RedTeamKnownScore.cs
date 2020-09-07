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
	[TaskDescription("Comportamiento del equipo rojo.")]
	public class RedTeamKnownScore : Conditional
	{
		public override TaskStatus OnUpdate()
		{

			if (Object.FindObjectOfType<GameManager>().getRedPts() <= Object.FindObjectOfType<GameManager>().getBluePts())
				return TaskStatus.Success;
			else return TaskStatus.Failure;
		}
	}
}

