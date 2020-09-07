using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script que permite la comunicacion entre el raton y el tablero
// a partir de aqui se llamara a los metodos que permiten la edicion de las 
// casillas, la creacion del destino y la posicion del tanque

public class ChangeTileComponent : MonoBehaviour {

    FloorNodeComponent previousFNC = null;

	void Update () {

		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit))
			{
				if (hit.transform.gameObject.GetComponent<FloorNodeComponent>())
				{
					FloorNodeComponent fnc = hit.transform.gameObject.GetComponent<FloorNodeComponent>();

					TankBehaviour tb = FindObjectOfType<TankBehaviour>();

					if (fnc.istankAboveMe())
					{
						if (tb.isTankSelected())
						{
							tb.deselecTank();
							fnc.setTankOnMe(true);
						}
						else
						{
							fnc.setTankOnMe(false);
							previousFNC = fnc;
							tb.selectTank(hit.transform.gameObject.GetComponent<FloorNodeComponent>().i_, hit.transform.gameObject.GetComponent<FloorNodeComponent>().j_);
						}
					}

					else
					{
						MapGenerator mg = FindObjectOfType<MapGenerator>();
						int x = hit.transform.gameObject.GetComponent<FloorNodeComponent>().i_;
						int z = hit.transform.gameObject.GetComponent<FloorNodeComponent>().j_;

						if (!tb.isTankSelected())
						{
							fnc.nextCell();
							mg.table[x, z].cell = fnc.typeOfCell;
						}
						else
						{
							if (mg.table[x, z].cell != Cells.rocks)
							{
								bool action = mg.createDestiny(x, z, true);
								if (action)
									fnc.setTankOnMe(true);
								else
									previousFNC.setTankOnMe(true);
							}
						}
					}
				}
			}
		}

		else if (FindObjectOfType<MapGenerator>().mapGenerated && (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0))
		{

			if (FindObjectOfType<TankBehaviour>().selected && !FindObjectOfType<TankBehaviour>().moving)
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out hit))
				{
					if (hit.transform.gameObject.GetComponent<FloorNodeComponent>())
					{
						FloorNodeComponent fnc = hit.transform.gameObject.GetComponent<FloorNodeComponent>();

						MapGenerator mg = FindObjectOfType<MapGenerator>();

						mg.createDestiny(fnc.i_, fnc.j_, false);

					}
				}
			}
		}
	}
}
