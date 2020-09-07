using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Accusate : MonoBehaviour
{
	bool confirmAccusation = false;
	public string personToAccuse;
	List<Characters> peopleToAccuse;
	people personID;
	weapons weaponID;
	public Text sugerencia;
	public Text selection;
	Transform originalAccusatedPos;
	public GameObject accusationPanel;

	public GameObject[] buttonsToHide;

	public GameObject acusateButton;

	string selectedCharacter;
	public Dropdown weaponDD;
	string arma;
	string roomName;

	bool accusating = false;

	GameObject personAccusated;

	private void Start()
	{
		peopleToAccuse = new List<Characters>();

		GameObject[] g = FindObjectOfType<GameManager>().characters;

		for (int i = 0; i < g.Length; i++)
		{
			if (!g[i].GetComponent<Player>())
			{
				peopleToAccuse.Add(g[i].GetComponent<Characters>());
			}
		}

		selection.enabled = false;

	}

	public void accusate()
	{
		if (!confirmAccusation)
		{
			if (!FindObjectOfType<MouseOverObject>().getEnable())
			{
				accusating = true;
				confirmAccusation = true;
				acusateButton.GetComponent<Button>().GetComponentInChildren<Text>().text = "CONFIRMAR ACUSACION";
				foreach (GameObject g in buttonsToHide)
				{
					g.SetActive(false);
				}

				foreach (GameObject g in FindObjectOfType<GameManager>().charactersObjects)
				{
					if (g.GetComponent<Characters>().myName == selectedCharacter)
					{
						personAccusated = g;
						originalAccusatedPos = g.transform;
						g.transform.position = FindObjectOfType<GameManager>().playersInGame[FindObjectOfType<GameManager>().actPlayer].transform.position;
						accusationPanel.SetActive(true);
						rooms r = (rooms)FindObjectOfType<GameManager>().playersInGame[FindObjectOfType<GameManager>().actPlayer].GetComponent<Characters>().getCasillaID();
						roomName = r.ToString();
						
					}
				}
			}

			else
			{
				selection.enabled = true;
				Invoke("disableText", 2f);
			}
		}

		else
		{
			confirmAccusation = false;
			acusateButton.GetComponent<Button>().GetComponentInChildren<Text>().text = "ACUSAR";
			acusateButton.SetActive(false);
			accusationPanel.SetActive(false);
			sugerencia.enabled = false;
			personAccusated.transform.position = originalAccusatedPos.position;
			accusating = false;

		}
	}

	void disableText()
	{
		selection.enabled = false;
	}

	public void setPersonToAccuse(string name)
	{
		int i = 0;
		bool found = false;

		while (i < peopleToAccuse.Count && !found)
		{
			if (name == peopleToAccuse[i].myName)
			{
				selectedCharacter = name;
				found = true;
				personID = peopleToAccuse[i].me;
			}

			else i++;
		}
	}

	public void changeWeapon()
	{
		arma = weaponDD.options[weaponDD.value].text;
		weaponID = (weapons)weaponDD.value;
		if(accusating)
			showInformation();
	}

	void showInformation()
	{
		GameManager gm = FindObjectOfType<GameManager>();
		sugerencia.enabled = true;
		sugerencia.text = gm.playersInGame[gm.actPlayer].name + " AFIRMA que el asesino es: " + selectedCharacter + " \n	En la habitación: " + roomName + "\n Con el arma: " + arma;
		
	}
}
