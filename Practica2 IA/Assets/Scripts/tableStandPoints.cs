using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tableStandPoints : MonoBehaviour {

	public string roomName;
	public int TableId = 0;
	public bool emptyCasilla = true;
	public int numberOfPeople = 0;
	public rooms room;

	public void substractPerson()
	{
		numberOfPeople--;
		if (numberOfPeople <= 0)
		{
			emptyCasilla = true;
		}
	}

	public void addPerson()
	{
		numberOfPeople++;
		emptyCasilla = false;
	}
}
