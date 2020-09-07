using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class botsAI : MonoBehaviour {

	Player me;
	public Text sugerencia;
	int tableID;
	people personToSuggest;
	botPersonallity botpers;

	public void startAISystem(botPersonallity bp)
	{
		botpers = bp;

		Player[] p = FindObjectsOfType<Player>();

		foreach (Player pla in p)
		{
			if (pla.myPersonality == bp)
			{
				me = pla;
			}
		}


		if (bp == botPersonallity.silly)
		{
			sillyBot();
		}

		else if(bp == botPersonallity.clever)
		{
			cleverBot();
		}
	}

	void sillyBot()
	{
		int randomMoveOrAccuse = Random.Range(0, 101);

		if(randomMoveOrAccuse < 80)
		{
			GameObject.FindGameObjectWithTag("askForCardTag").GetComponent<Text>().text = "El jugador " + me.name + "\n" + "ha decidido moverse.";


			Invoke("selectWhereToMoveRandom", 2f);		
		}

		else 
			FindObjectOfType<StateMachine>().nextState();
	}

	void cleverBot()
	{
		//FindObjectOfType<StateMachine>().nextState();
	}

	void selectWhereToMoveRandom()
	{
		int randomPos = Random.Range(1, 10);
		GameObject.FindGameObjectWithTag("askForCardTag").GetComponent<Text>().text = "";
		foreach (tableStandPoints tbs in FindObjectsOfType<tableStandPoints>())
		{
			if (tbs.TableId == randomPos)
			{
				tableID = tbs.TableId;
				if (tbs.emptyCasilla)
				{
					FindObjectOfType<StateMachine>().nextState();
				}

				else
				{
					List<Characters> character = new List<Characters>();

					foreach (Characters g in FindObjectsOfType<Characters>())
					{
						if (g.gameObject.GetComponent<Player>() == null)
						{
							if (g.casillaID == tableID)
							{
								character.Add(g);
							}
						}
					}

					int rnd = Random.Range(0, character.Count);

					//personToSuggest = character[rnd].me;

					suggest();
				}

				moveBot(tableID, me, tbs);
			}
		}
	}

	void moveBot(int ID, Player pl, tableStandPoints tsp)
	{
		Debug.Log(me.name);
		tablePos[,] tp = FindObjectOfType<GameManager>().getTableMatrix();

		int i, j; i = j = 0;

		bool found = false;

		while (j < 9 && !found)
		{
			while (i < 9 && !found)
			{
				if (tp[i, j].ID == ID && !tp[i, j].occupied)
				{
					found = true;
					pl.gameObject.transform.position = new Vector3(tp[i, j].physicalRepresentation.transform.position.x, tp[i, j].physicalRepresentation.transform.position.y, -0.48f);

					FindObjectOfType<GameManager>().changeOccupiedMatrix(pl.i_, pl.j_, false);
					pl.i_ = i;
					pl.j_ = j;

					FindObjectOfType<GameManager>().changeOccupiedMatrix(i, j, true);

					tableStandPoints[] t = FindObjectsOfType<tableStandPoints>();

					foreach (tableStandPoints ts in t)
					{
						if (pl.casillaID == ts.TableId)
						{
							ts.substractPerson();
						}
					}

					pl.casillaID = tsp.TableId;
					tsp.addPerson();
				}
				else i++;
			}

			i = 0; j++;
		}
	}

	void suggest()
	{
		rooms room = (rooms)tableID;
		GameManager gm = FindObjectOfType<GameManager>();
		sugerencia.enabled = true;
		sugerencia.text = me.name + " suguiere que el asesino es: " + personToSuggest.ToString() + " \n	En la habitación: " + room.ToString() + "\n Con el arma: " + weapons.Candelabro.ToString();

		Invoke("askCard", 5f);

		//charactersInMyRoom.Clear();
	}

	void askCard()
	{
		sugerencia.enabled = false;
		foreach (Characters g in FindObjectsOfType<Characters>())
		{
			if (g.gameObject.GetComponent<Player>() != null)
			{
				if (g.gameObject.GetComponent<Player>().name == me.name)
				{
					g.gameObject.GetComponent<askForCard>().askPlayer((rooms)tableID, weapons.Candelabro, people.Blanco);
				}
			}
		}
	}
}
