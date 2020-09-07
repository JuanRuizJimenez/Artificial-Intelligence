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
	public class RedTeamSelectCloserPlayer : Action
	{
		GameObject[] otherPlayers;

		public override void OnStart()
		{
			otherPlayers = GameObject.FindGameObjectsWithTag("RedTeam");
		}

		public override TaskStatus OnUpdate()
		{

			float minimunDistance = 10000000000000f;
			Vector3 ball = GameObject.FindGameObjectWithTag("ball").transform.position;

			GameObject choosenOne = null;

			foreach (GameObject g in otherPlayers)
			{
				g.GetComponent<RedTeamPlayer>().setMoveOriginalPositon(true);
				g.GetComponent<RedTeamPlayer>().setAttack(false);

				if (Vector3.Distance(g.transform.position, ball) < minimunDistance)
				{
					minimunDistance = Vector3.Distance(g.transform.position, ball);
					choosenOne = g;
				}
			}

			choosenOne.GetComponent<RedTeamPlayer>().setMoveOriginalPositon(false);
			choosenOne.GetComponent<RedTeamPlayer>().setAttack(true);

			Object.FindObjectOfType<GameManager>().setRedTeamAttacker(choosenOne);

			return TaskStatus.Success;
		}
	}
}

