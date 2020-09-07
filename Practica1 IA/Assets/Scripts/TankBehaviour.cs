using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script de control para el tanque
// añadira logica para seleccionarlo y comprobar si esta seleccionado
// tambien controlara si se esta moviendo para no interrumpir su proceso

public class TankBehaviour : MonoBehaviour {

	Texture originalTexture;

	public bool selected = false;
    public bool moving = false;

	public Texture selectedTexture;

	public int i_, j_;

	private void Start()
	{
		originalTexture = gameObject.GetComponent<Renderer>().material.mainTexture;
	}

	public void selectTank(int i, int j)
	{
		i_ = i;
		j_ = j;
		gameObject.GetComponent<Renderer>().material.mainTexture = selectedTexture;
		selected = true;
	}
	
	public void deselecTank()
	{
		gameObject.GetComponent<Renderer>().material.mainTexture = originalTexture;
		selected = false;
	}

	public bool isTankSelected()
	{
		return selected;
	}
}
