using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class askForCard : MonoBehaviour {

	public void askPlayer(rooms r, weapons w,  people p)
	{

		int person = 0;

		bool []b = new bool[3];

		Player pla = FindObjectOfType<GameManager>().playersInGame[FindObjectOfType<StateMachine>().getNextState()];

		b = FindObjectOfType<cardBehaviour>().askedCards(r, w, p, pla);

		person = FindObjectOfType<StateMachine>().getNextState();

		if (!b[0] && !b[1] && !b[2])
		{
			pla = FindObjectOfType<GameManager>().playersInGame[FindObjectOfType<StateMachine>().getSecondNextState()];

			b = FindObjectOfType<cardBehaviour>().askedCards(r, w, p, pla);

			person = FindObjectOfType<StateMachine>().getSecondNextState();
		}

		if (b[0] || b[1] || b[2])
		{
			Player thisPlayer = FindObjectOfType<GameManager>().playersInGame[FindObjectOfType<StateMachine>().getCurrentState()];

			knownCards kc = new knownCards();
			string carta = "";
			if (b[0])
			{
				kc.knownRoomCards = r;
				kc.knownWeapondCards = weapons.noDef;
				kc.knownPeopleCards = people.noDef;
				carta = r.ToString();
			}
			else if (b[1])
			{
				kc.knownWeapondCards = w;
				kc.knownRoomCards = rooms.noDef;
				kc.knownPeopleCards = people.noDef;
				carta = w.ToString();
			}
			else
			{
				kc.knownPeopleCards = p;
				kc.knownRoomCards = rooms.noDef;
				kc.knownWeapondCards = weapons.noDef;
				carta = p.ToString();
			}

			kc.person = person;

			if(thisPlayer.humanPlayer)
				GameObject.FindGameObjectWithTag("askForCardTag").GetComponent<Text>().text = "El jugador " + pla.name + "\n" + "te dio la carta " + carta;
			else
			{
				GameObject.FindGameObjectWithTag("askForCardTag").GetComponent<Text>().text = "El jugador " + pla.name + "\n" + " dio una carta a otro jugador";
			}


			Invoke("quitText", 4f);
			thisPlayer.otherPlayersCards.Add(kc);
		}
	}

	void quitText()
	{
		GameObject.FindGameObjectWithTag("askForCardTag").GetComponent<Text>().text = "";
		FindObjectOfType<StateMachine>().nextState();
	}
}
