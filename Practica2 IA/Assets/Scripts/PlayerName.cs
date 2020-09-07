using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerName : MonoBehaviour {

	public GameObject[] playersNames;

	public void setName(string player, string player2, string player3)
	{
		gameObject.GetComponent<TextMeshProUGUI>().text = player;
		playersNames[0].GetComponent<TextMeshProUGUI>().text = player;
		playersNames[1].GetComponent<TextMeshProUGUI>().text = player2;
		playersNames[2].GetComponent<TextMeshProUGUI>().text = player3;
	}

	public void setUniqueName(string name)
	{
		gameObject.GetComponent<TextMeshProUGUI>().text = name;
	}
}
