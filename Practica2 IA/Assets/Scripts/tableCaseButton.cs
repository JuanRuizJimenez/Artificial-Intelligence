using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class tableCaseButton : MonoBehaviour
{
	public bool canInteract = false;
	public GameObject table;
	bool active;
	public GameObject [] panelButtonsToHide;
	public GameObject[] accusateButtons;
	bool askingCards = false;

	private void Start()
	{
		active = false;
	}

	public void disableMe()
	{
		this.gameObject.SetActive(false);
	}

	public void onClick()
	{
		if (canInteract)
		{
			active = !active;

			TextMeshProUGUI[] tmp = table.GetComponentsInChildren<TextMeshProUGUI>();

			if (active)
			{
				for (int i = 0; i < panelButtonsToHide.Length; i++)
				{
					if (panelButtonsToHide[i] != null)
						panelButtonsToHide[i].SetActive(true);
				}

				gameObject.GetComponentInChildren<Text>().text = "Hide table case";
				gameObject.GetComponent<Image>().color = Color.green;


				Color c = new Color();

				foreach (TextMeshProUGUI t in tmp)
				{
					c.r = t.color.r;
					c.g = t.color.g;
					c.b = t.color.b;
					c.a = 1;

					t.color = c;
				}

				c = table.GetComponent<Image>().color;
				c.a = 1;

				table.GetComponent<Image>().color = c;

				GameManager g = FindObjectOfType<GameManager>();

				g.actPlayer = 0;

				g.drawPlayerCards(g.playersInGame[0], true);
				FindObjectOfType<PlayerName>().setUniqueName(g.playersInGame[0].name);
			}

			else
			{
				for (int i = 0; i < panelButtonsToHide.Length; i++)
				{
					if (panelButtonsToHide[i] != null)
						panelButtonsToHide[i].SetActive(false);
				}

				gameObject.GetComponentInChildren<Text>().text = "Show table case";
				gameObject.GetComponent<Image>().color = Color.white;

				Color c = new Color();

				foreach (TextMeshProUGUI t in tmp)
				{

					c.r = t.color.r;
					c.g = t.color.g;
					c.b = t.color.b;
					c.a = 0;

					t.color = c;
				}

				c = table.GetComponent<Image>().color;
				c.a = 0;

				table.GetComponent<Image>().color = c;

				resetTable();
			}
		}
	}

	public void changePlayer()
	{
		FindObjectOfType<GameManager>().nextPlayer();
	}

	public void askCards()
	{
		askingCards = !askingCards;

		if (askingCards)
		{
			for (int i = 0; i < accusateButtons.Length; i++)
				if (accusateButtons[i] != null)
					accusateButtons[i].SetActive(true);
			GameManager gm = FindObjectOfType<GameManager>();

			FindObjectOfType<PlayerName>().setName(gm.playersInGame[0].name, "","");

		}
		else
		{
			resetTable();
			
		}
	}

	void resetTable()
	{
		for (int i = 0; i < accusateButtons.Length; i++)
			if (accusateButtons[i] != null)
				accusateButtons[i].SetActive(false);

		GameManager gm = FindObjectOfType<GameManager>();

		FindObjectOfType<PlayerName>().setName(gm.playersInGame[0].name, gm.playersInGame[1].name, gm.playersInGame[2].name);
	}
}
