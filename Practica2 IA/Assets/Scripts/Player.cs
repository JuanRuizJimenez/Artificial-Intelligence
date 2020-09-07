using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct knownCards
{
	public rooms knownRoomCards;
	public weapons knownWeapondCards;
	public people knownPeopleCards;

	public int person;
}

public enum botPersonallity { silly, clever};

public class Player : MonoBehaviour {

	public string name;
	public int playerNumber;
	public List<rooms> roomCards;
	public List<weapons> weaponsCards;
	public List<people> peopleCards;

	public List<knownCards> otherPlayersCards;
	
	public int numberOfCards;
	public bool humanPlayer;
	public int i_, j_;
	public int characterID;
	public int casillaID;

	bool myTurn = false;

	public botPersonallity myPersonality;

	private void Start()
	{
		numberOfCards = 0;

		roomCards = new List<rooms>();
		weaponsCards = new List<weapons>();
		peopleCards = new List<people>();
		otherPlayersCards = new List<knownCards>();
	}

	public void setIJ(int i, int j)
	{
		i_ = i;
		j_ = j;
	}


	public void startTurn()
	{
		myTurn = true;

		if (humanPlayer)
		{
			FindObjectOfType<GameManager>().sowPlayerButtons();
		}

		else
		{
			FindObjectOfType<GameManager>().hidePlayerButtons();

			FindObjectOfType<botsAI>().startAISystem(myPersonality);
		}
	}
}
