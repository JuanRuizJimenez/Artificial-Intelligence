using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardBehaviour : MonoBehaviour
{

	public bool[] askedCards(rooms r, weapons w,  people p, Player asked)
	{
		bool[] haveThisCards = new bool[3];
		bool found = false;

		int i = 0;

		while (i < asked.roomCards.Count && !found)
		{
			if (asked.roomCards.Contains(r))
			{
				found = true;
				haveThisCards[0] = true;
			}
			else i++;
		}

		i = 0;
		found = false;

		while (i < asked.weaponsCards.Count && !found)
		{
			if (asked.weaponsCards.Contains(w))
			{
				found = true;
				haveThisCards[1] = true;
			}
			else i++;
		}

		i = 0;
		found = false;

		while (i < asked.peopleCards.Count && !found)
		{
			if (asked.peopleCards.Contains(p))
			{
				found = true;
				haveThisCards[2] = true;
			}
			else i++;
		}
		if (haveThisCards[0] || haveThisCards[1] || haveThisCards[2])
		{	
			haveThisCards = random(haveThisCards);
			return haveThisCards;
		}
		else
		{
			bool[] b = new bool[3];
			b[0] = false;
			b[1] = false;
			b[2] = false;
			return b;
		}

			
	}

	bool[] random(bool[] c)
	{
		bool found = false;

		bool []aux = new bool[3];

		aux[0] = false;
		aux[1] = false;
		aux[2] = false;

		while (!found)
		{
			int rnd = Random.Range(0, 3);
			
			if (c[rnd])
			{
				found = true;
				for (int i = 0; i < aux.Length; i++)
				{
					if (i != rnd)
						aux[i] = false;
					else aux[i] = true;
				}
			}
		}

		return aux;
	}
}
