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
	public class RedTeamKickBallOut : Action
	{
		GameObject[] otherPlayers;

		public override void OnStart()
		{
			otherPlayers = GameObject.FindGameObjectsWithTag("RedTeam");
		}

		public override TaskStatus OnUpdate()
		{

			foreach (GameObject g in otherPlayers)
			{
				g.GetComponent<RedTeamPlayer>().setMoveOriginalPositon(false);
				g.GetComponent<RedTeamPlayer>().setAttack(true);
			}

			return TaskStatus.Success;
		}
	}
}
