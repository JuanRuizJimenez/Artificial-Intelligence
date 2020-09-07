using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// script para el funcionamiento de las casillas
// añade la logica para poder cambiar el tipo y la textura de la casilla
// tambien controla si el tanque esta encima de una casilla concreta

public class FloorNodeComponent : MonoBehaviour {

	public Cells typeOfCell;
	bool tankOnMe = false;
	public Texture[] textures;
	public GameObject mountain;
	GameObject mountainObj = null;
	public int i_,j_;

    // para definir el tipo de casilla y su textura
	public void setTypeOfCell(Cells type, int i, int j)
	{
		typeOfCell = type;

		i_ = i;
		j_ = j;

		gameObject.GetComponent<Renderer>().material.mainTexture = textures[(int)type];

		if(type == Cells.rocks)
		{
			Vector3 pos = transform.position;
			pos.y += 0.3f;
			mountainObj = Instantiate(mountain, pos, Quaternion.Euler(-90, 0, 0));
		}
	}

    // para poder cambiar el tipo de casilla en ejecucion
	public void nextCell()
	{
		int actNumber = (int)typeOfCell;
		actNumber++;

		if (actNumber > textures.Length - 1)
			actNumber = 0;

		gameObject.GetComponent<Renderer>().material.mainTexture = textures[actNumber];

		if(mountainObj != null)
		{
			Destroy(mountainObj);
			mountainObj = null;

		}

		if ((Cells)actNumber == Cells.rocks)
		{
			Vector3 pos = transform.position;
			pos.y += 0.3f;
			mountainObj = Instantiate(mountain, pos, Quaternion.Euler(-90, 0, 0));
		}

		typeOfCell = (Cells)actNumber;
	}

    // metodos de control para comprobar y activar la posicion del tanque
	public bool istankAboveMe()
	{
		return tankOnMe;
	}

	public void setTankOnMe(bool tank)
	{
		tankOnMe = tank;
	}

    // funcion auxiliar
	public void destroyMountain()
	{
		Destroy(mountainObj);
	}
}
