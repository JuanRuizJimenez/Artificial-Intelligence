using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateMachine : MonoBehaviour
{

	public Text stateText;
	int currState = 0;
	int numPlayers = 0;

	private void Start()
	{
		stateText.enabled = false;
	}

	public void initialState()
	{
		stateText.enabled = true;
		numPlayers = FindObjectOfType<GameManager>().playersInGame.Length;
		FindObjectOfType<GameManager>().playersInGame[currState].GetComponent<Player>().startTurn();
		stateText.text = "Es el turno del jugador: " + FindObjectOfType<GameManager>().playersInGame[currState].name;
	}

	public int getCurrentState()
	{
		return currState;
	}
	public void nextState()
	{
		currState++;

		if (currState >= numPlayers)
			currState = 0;

		stateText.text = "Es el turno del jugador: " + FindObjectOfType<GameManager>().playersInGame[currState].name;

		FindObjectOfType<GameManager>().playersInGame[currState].GetComponent<Player>().startTurn();

	}

	public int getNextState()
	{
		int aux = currState;
		aux ++;
		if (aux >= numPlayers)
			aux = 0;

		return aux;
	}

	public int getSecondNextState()
	{
		if (currState == 0)
			return 2;
		else if (currState == 1)
			return 0;
		else return 1;
	}
}