using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
	Vector3 scale;
	Vector3 oriScale;
	GameObject[] floor;
	bool enabled = false;
	tablePos[] casillas;

	// Use this for initialization
	void Start()
	{
		floor = GameObject.FindGameObjectsWithTag("Floor");
		oriScale = floor[0].transform.localScale;
		scale = new Vector3(0, 0, 0);
	}

	public void setEnabled()
	{
		enabled = !enabled;
	}

	// Update is called once per frame
	void Update()
	{
		foreach (GameObject g in floor)
		{
			g.transform.localScale = oriScale;
		}

		if (enabled)
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit))
			{
				if (hit.transform.gameObject.CompareTag("Floor"))
				{
					hit.transform.localScale = scale;

					if (Input.GetMouseButtonDown(0))
					{
						Player[] p = FindObjectsOfType<Player>();

						foreach (Player pl in p)
						{
							if (pl.humanPlayer)
							{
								tableStandPoints ts = hit.transform.gameObject.GetComponent<tableStandPoints>();

								enabled = false;

								if (ts.emptyCasilla)
								{
									FindObjectOfType<StateMachine>().nextState();

									foreach (GameObject g in floor)
									{
										g.transform.localScale = oriScale;
									}
								}

								else
								{
									FindObjectOfType<Suggest>().selectPeople(ts.TableId, pl.name);
								}

								movePlayer(ts.TableId, pl, ts);

							}
						}

					}

				}
			}
		}
	}

	public void movePlayer(int ID, Player pl, tableStandPoints tsp)
	{

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
}
