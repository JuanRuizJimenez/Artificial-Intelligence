using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// script del HUD, que nos mostrara informacion sobre el tiempo que se ha tardado
// en calcular el camino y el numero de pasos (en casillas) que el tanque debera dar

public class HUDControllScript : MonoBehaviour {

	public Text timeText;
	public Text stepText;

	private void Start()
	{
		timeText.text = "Tiempo empleado: 0 milisegundos.";
		stepText.text = "Nº pasos para destino: 0";
	}

	public void setTime(float t)
	{
		timeText.text = "Tiempo empleado: " + t +" milisegundos.";
	}

	public void setSteps(int s)
	{
		stepText.text = "Nº pasos para destino: " + s;
	}
}
