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
	public class RedTeamSupportAttack : Action
	{
		GameObject[] otherPlayers;

		public override void OnStart()
		{
			otherPlayers = GameObject.FindGameObjectsWithTag("RedTeam");
		}

		public override TaskStatus OnUpdate()
		{
			bool firstPartner = true;

			foreach (GameObject g in otherPlayers)
			{

				if (!g.GetComponent<RedTeamPlayer>().getAttack())
				{
					GameObject ga = Object.FindObjectOfType<GameManager>().getRedTeamAttacker();
					Vector3 pos = ga.transform.position; 

					if (firstPartner)
					{
						firstPartner = false;
						pos = new Vector3(ga.transform.position.x - 20, ga.transform.position.y, ga.transform.position.z - 30);
					}

					else
					{
						pos = new Vector3(ga.transform.position.x - 20, ga.transform.position.y, ga.transform.position.z + 30);
					}
					
					g.GetComponent<RedTeamPlayer>().setSupport(pos);
				}

			}
			return TaskStatus.Success;
		}
	}
}