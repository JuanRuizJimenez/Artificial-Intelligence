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
	public class RedTeamOriginalPositionBehaviour : Action
	{
		public float speed = 5f;

		private GameObject target;

		GameObject[] myPlayers;

		public override void OnStart()
		{
			myPlayers = GameObject.FindGameObjectsWithTag("RedTeam");

			foreach (GameObject g in myPlayers)
			{
				g.GetComponent<RedTeamPlayer>().setMoveOriginalPositon(true);
				g.GetComponent<RedTeamPlayer>().setAttack(false);
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (!Object.FindObjectOfType<GameManager>().goalTime())
				return TaskStatus.Success;
			else
				return TaskStatus.Running;
		}

		
	}
}
