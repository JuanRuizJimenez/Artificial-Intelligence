using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Suggest : MonoBehaviour
{

	List<Characters> charactersInMyRoom;

	public Text sugerencia;
	public GameObject CallSuspectButton, accusationPanel;
	string arma = "Candelabro";
	public Dropdown weaponDD;
	string roomName = "";
	rooms roomID;
	weapons weaponID;
	people personID;
	string myName;

	string selectedCharacter;

	private void Start()
	{
		charactersInMyRoom = new List<Characters>();
		CallSuspectButton.SetActive(false);
		accusationPanel.SetActive(false);
		sugerencia.enabled = false;
	}

	public void selectPeople(int tableId, string playerName)
	{
		myName = playerName;
		charactersInMyRoom.Clear();

		foreach (Characters g in FindObjectsOfType<Characters>())
		{
			if (g.gameObject.GetComponent<Player>() == null)
			{
				if (g.casillaID == tableId)
				{
					charactersInMyRoom.Add(g);
					g.gameObject.GetComponent<objectSize>().enabled = true;
				}
			}
		}

		CallSuspectButton.SetActive(true);
		accusationPanel.SetActive(true);
		sugerencia.enabled = true;

		tableStandPoints[] tsp = FindObjectsOfType<tableStandPoints>();

		foreach (tableStandPoints t in tsp)
		{
			if (t.TableId == tableId)
			{
				roomName = t.roomName;
				roomID = t.room;
				
			}
		}

		if (charactersInMyRoom.Count == 1)
		{
			selectedCharacter = charactersInMyRoom[0].myName;
			personID = charactersInMyRoom[0].me;
			showInformation();

		}
	}

	void showInformation()
	{
		GameManager gm = FindObjectOfType<GameManager>();
		sugerencia.text = gm.playersInGame[gm.actPlayer].name + " suguiere que el asesino es: " + selectedCharacter + " \n	En la habitación: " + roomName + "\n Con el arma: " + arma;
	}

	public void changeWeapon()
	{
		arma = weaponDD.options[weaponDD.value].text;
		weaponID = (weapons)weaponDD.value;
		showInformation();
	}

	public void findCharacterSelected(string name)
	{
		int i = 0;
		bool found = false;

		while (i < charactersInMyRoom.Count && !found)
		{
			if (name == charactersInMyRoom[i].myName)
			{
				selectedCharacter = name;
				found = true;
				personID = charactersInMyRoom[i].me;
			}

			else i++;
		}

		if (found) showInformation();
	}

	public void suggest()
	{
		CallSuspectButton.SetActive(false);
		sugerencia.text = "";

		foreach (Characters g in charactersInMyRoom)
		{
			g.gameObject.GetComponent<objectSize>().setOriginalScale();
			g.gameObject.GetComponent<objectSize>().enabled = false;
		}


		foreach (Characters g in FindObjectsOfType<Characters>())
		{
			if (g.gameObject.GetComponent<Player>() != null)
			{
				if (g.gameObject.GetComponent<Player>().name == myName)
				{
					g.gameObject.GetComponent<askForCard>().askPlayer(roomID, weaponID, personID);
				}
			}
		}

		charactersInMyRoom.Clear();
	}
}
