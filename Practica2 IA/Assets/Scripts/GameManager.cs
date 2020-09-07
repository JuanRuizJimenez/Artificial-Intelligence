using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum rooms { Biblioteca, Cocina, Comedor, Estudio, Pasillo, Recibidor, SalaBillar, SalonBaile, Terraza, noDef };
public enum weapons { Candelabro, Cuerda, Herramienta, Pistola, Puñal, Tuberia, noDef };
public enum people { Amapola, Blanco, Celeste, Mora, Prado, Rubio, noDef };

public struct tablePos
{
	public bool occupied;
	public GameObject physicalRepresentation;
	public int ID;
	public int i, j;
}

public class GameManager : MonoBehaviour
{
	public int actPlayer = 0;

	public GameObject standPoint;
	public GameObject[] characters;
	public GameObject[] charactersObjects;

	public Light spawnLight;

	public Animator envelopeStartMovement;
	public Animator clasiffiedMovement;

	caseEnvelope murderEnvelope;

	public Player[] playersInGame;
	private GameObject[] playerButtons;

	struct caseEnvelope
	{
		public rooms crimeRoom;
		public weapons crimeWeapon;
		public people murderer;
	}

	List<rooms> mazeRooms = new List<rooms>();
	List<weapons> mazeWeapons = new List<weapons>();
	List<people> mazePeople = new List<people>();

	tablePos[,] table;

	void Start()
	{
		table = new tablePos[9, 9];
		spawnLight.enabled = false;

		charactersObjects = new GameObject[characters.Length];

		playerButtons = GameObject.FindGameObjectsWithTag("PlayerButtons");
		hidePlayerButtons();

		int x, y;
		x = -4; y = 4;
		for (int j = 0; j < 9; j++)
		{
			for (int i = 0; i < 9; i++)
			{
				table[i, j].occupied = false;
				table[i, j].physicalRepresentation = Instantiate(standPoint, new Vector3(x, y, standPoint.transform.position.z), Quaternion.identity);

				table[i, j].ID = (int)(i / 3) + ((int)(j / 3) * 3);
				table[i, j].i = i;
				table[i, j].j = j;
				x++;
			}

			y--;
			x = -4;
		}
		StartCoroutine(createPositions());
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			FindObjectOfType<StateMachine>().nextState();
		}
	}

	public tablePos[,] getTableMatrix()
	{
		return table;
	}

	public void changeOccupiedMatrix(int i, int j, bool b)
	{
		table[i, j].occupied = b;
	}

	IEnumerator createPositions()
	{

		for (int p = 0; p < characters.Length; p++)
		{
			bool created = false;

			while (!created)
			{
				created = createCharacter(p);
			}

			yield return new WaitForSeconds(.5f);
		}
		spawnLight.enabled = false;
		createCase();

		clasiffiedMovement.gameObject.GetComponent<SpriteRenderer>().enabled = false;

		envelopeStartMovement.SetBool("startMovement", true);
		clasiffiedMovement.SetBool("startMovement", true);
		Invoke("startClassifiedAnimation", 1.4f);

	}

	void startClassifiedAnimation()
	{
		clasiffiedMovement.gameObject.GetComponent<SpriteRenderer>().enabled = true;
		FindObjectOfType<tableCaseButton>().canInteract = true;
		FindObjectOfType<StateMachine>().initialState();
	}

	private void createCase()
	{
		for (int i = 0; i < 9; i++)
		{
			mazeRooms.Add((rooms)i);
		}

		for (int i = 0; i < 6; i++)
		{
			mazeWeapons.Add((weapons)i);
		}

		for (int i = 0; i < 6; i++)
		{
			mazePeople.Add((people)i);
		}

		int randomRoom = Random.Range(0, 9);
		murderEnvelope.crimeRoom = (rooms)randomRoom;
		mazeRooms.Remove((rooms)randomRoom);// = rooms.noDef;

		int randomWeapon = Random.Range(0, 6);
		murderEnvelope.crimeWeapon = (weapons)randomWeapon;
		mazeWeapons.Remove((weapons)randomWeapon);// = weapons.noDef;

		int randomMurderer = Random.Range(0, 6);
		murderEnvelope.murderer = (people)randomMurderer;
		mazePeople.Remove((people)randomMurderer);// = people.noDef;

		giveCards();

		setTableName();

	}

	private void giveCards()
	{
		playersInGame = FindObjectsOfType<Player>();

		foreach (Player p in playersInGame)
		{
			p.setIJ(characters[p.characterID].GetComponent<Characters>().i_, characters[p.characterID].GetComponent<Characters>().j_);
			p.casillaID = characters[p.characterID].GetComponent<Characters>().casillaID;
		}

		hidePlayerButtons();

		int roomCards = mazeRooms.Count;
		int weaponCards = mazeWeapons.Count;
		int peopleCards = mazePeople.Count;

		int i = 0;

		while (roomCards > 0 || weaponCards > 0 || peopleCards > 0)
		{
			int maze = Random.Range(0, 3);

			if (maze == 0 && roomCards > 0)
			{
				int rnd = Random.Range(0, mazeRooms.Count - 1);

				roomCards--;
				rooms r = mazeRooms[rnd];

				playersInGame[i].numberOfCards++;
				playersInGame[i].roomCards.Add(r);


				mazeRooms.Remove(r);
			}

			else if (maze == 1 && weaponCards > 0)
			{
				int rnd = Random.Range(0, mazeWeapons.Count - 1);

				weaponCards--;
				weapons r = mazeWeapons[rnd];

				playersInGame[i].numberOfCards++;
				playersInGame[i].weaponsCards.Add(r);


				mazeWeapons.Remove(r);

			}

			else if (peopleCards > 0)
			{
				int rnd = Random.Range(0, mazePeople.Count - 1);

				peopleCards--;
				people r = mazePeople[rnd];

				playersInGame[i].numberOfCards++;
				playersInGame[i].peopleCards.Add(r);

				mazePeople.Remove(r);

			}

			if (playersInGame[i].numberOfCards > 5)
			{
				i++;
			}
		}
	}

	public void nextPlayer()
	{
		actPlayer++;

		if (actPlayer >= playersInGame.Length + 1)
			actPlayer = 0;


		if (actPlayer < playersInGame.Length)
			drawPlayerCards(playersInGame[actPlayer], true);
		else
		{
			for (int i = 0; i < playersInGame.Length; i++)
			{
				drawPlayerCards(playersInGame[i], false);
			}

			allPlayers(murderEnvelope.crimeRoom, murderEnvelope.crimeWeapon, murderEnvelope.murderer);
		}


		if (actPlayer < playersInGame.Length)
			FindObjectOfType<PlayerName>().setUniqueName(playersInGame[actPlayer].name);
		else FindObjectOfType<PlayerName>().setUniqueName("ALL");
	}

	public void drawPlayerCards(Player player, bool clear)
	{
		if (clear)
			clearTable();

		foreach (rooms r in player.roomCards)
		{
			crossPlayerCards(r, weapons.noDef, people.noDef, player.playerNumber);
		}

		foreach (weapons r in player.weaponsCards)
		{
			crossPlayerCards(rooms.noDef, r, people.noDef, player.playerNumber);
		}

		foreach (people r in player.peopleCards)
		{
			crossPlayerCards(rooms.noDef, weapons.noDef, r, player.playerNumber);
		}

		foreach (knownCards kc in player.otherPlayersCards)
		{
			if (kc.knownRoomCards != rooms.noDef)
			{
				crossPlayerCards(kc.knownRoomCards, weapons.noDef, people.noDef, kc.person);
			}

			if (kc.knownWeapondCards != weapons.noDef)
			{
				crossPlayerCards(rooms.noDef, kc.knownWeapondCards, people.noDef, kc.person);
			}

			if (kc.knownPeopleCards != people.noDef)
			{
				crossPlayerCards(rooms.noDef, weapons.noDef, kc.knownPeopleCards, kc.person);
			}
		}
	}

	private void clearTable()
	{
		Room[] r = FindObjectsOfType<Room>();
		Weapons[] w = FindObjectsOfType<Weapons>();
		Suspects[] s = FindObjectsOfType<Suspects>();


		for (int i = 0; i < r.Length; i++)
		{
			r[i].SP[0].GetComponent<TextMeshProUGUI>().text = "";
			r[i].SP[1].GetComponent<TextMeshProUGUI>().text = "";
			r[i].SP[2].GetComponent<TextMeshProUGUI>().text = "";
			r[i].gameObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
		}

		for (int p = 0; p < w.Length; p++)
		{
			w[p].SP[0].GetComponent<TextMeshProUGUI>().text = "";
			w[p].SP[1].GetComponent<TextMeshProUGUI>().text = "";
			w[p].SP[2].GetComponent<TextMeshProUGUI>().text = "";
			w[p].gameObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;

		}

		for (int q = 0; q < s.Length; q++)
		{
			s[q].SP[0].GetComponent<TextMeshProUGUI>().text = "";
			s[q].SP[1].GetComponent<TextMeshProUGUI>().text = "";
			s[q].SP[2].GetComponent<TextMeshProUGUI>().text = "";
			s[q].gameObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
		}
	}

	private void allPlayers(rooms room, weapons weapon, people person)
	{
		Room[] r = FindObjectsOfType<Room>();
		Weapons[] w = FindObjectsOfType<Weapons>();
		Suspects[] s = FindObjectsOfType<Suspects>();


		for (int i = 0; i < r.Length; i++)
		{
			if (r[i].tipo == room)
			{
				r[i].SP[0].GetComponent<TextMeshProUGUI>().color = Color.red;
				r[i].SP[1].GetComponent<TextMeshProUGUI>().color = Color.red;
				r[i].SP[2].GetComponent<TextMeshProUGUI>().color = Color.red;

				r[i].SP[0].GetComponent<TextMeshProUGUI>().text = "???";
				r[i].SP[1].GetComponent<TextMeshProUGUI>().text = "???";
				r[i].SP[2].GetComponent<TextMeshProUGUI>().text = "???";
			}
		}

		for (int p = 0; p < w.Length; p++)
		{
			if (w[p].tipo == weapon)
			{

				w[p].SP[0].GetComponent<TextMeshProUGUI>().color = Color.red;
				w[p].SP[1].GetComponent<TextMeshProUGUI>().color = Color.red;
				w[p].SP[2].GetComponent<TextMeshProUGUI>().color = Color.red;

				w[p].SP[0].GetComponent<TextMeshProUGUI>().text = "???";
				w[p].SP[1].GetComponent<TextMeshProUGUI>().text = "???";
				w[p].SP[2].GetComponent<TextMeshProUGUI>().text = "???";
			}
		}

		for (int q = 0; q < s.Length; q++)
		{
			if (s[q].tipo == person)
			{
				s[q].SP[0].GetComponent<TextMeshProUGUI>().color = Color.red;
				s[q].SP[1].GetComponent<TextMeshProUGUI>().color = Color.red;
				s[q].SP[2].GetComponent<TextMeshProUGUI>().color = Color.red;

				s[q].SP[0].GetComponent<TextMeshProUGUI>().text = "???";
				s[q].SP[1].GetComponent<TextMeshProUGUI>().text = "???";
				s[q].SP[2].GetComponent<TextMeshProUGUI>().text = "???";
			}
		}
	}

	private void crossPlayerCards(rooms room, weapons weapon, people person, int playerNumber)
	{
		string first, second, third;

		first = second = third = "X";

		if (playerNumber == 0) first = "O";
		else if (playerNumber == 1) second = "O";
		else third = "O";


		Room[] r = FindObjectsOfType<Room>();
		Weapons[] w = FindObjectsOfType<Weapons>();
		Suspects[] s = FindObjectsOfType<Suspects>();


		for (int i = 0; i < r.Length; i++)
		{
			if (r[i].tipo == room)
			{
				if (playerNumber == 0) r[i].SP[0].GetComponent<TextMeshProUGUI>().color = Color.green;
				else if (playerNumber == 1) r[i].SP[1].GetComponent<TextMeshProUGUI>().color = Color.green;
				else r[i].SP[2].GetComponent<TextMeshProUGUI>().color = Color.green;


				r[i].SP[0].GetComponent<TextMeshProUGUI>().text = first;
				r[i].SP[1].GetComponent<TextMeshProUGUI>().text = second;
				r[i].SP[2].GetComponent<TextMeshProUGUI>().text = third;

				r[i].gameObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
			}
		}

		for (int p = 0; p < w.Length; p++)
		{
			if (w[p].tipo == weapon)
			{

				if (playerNumber == 0) w[p].SP[0].GetComponent<TextMeshProUGUI>().color = Color.green;
				else if (playerNumber == 1) w[p].SP[1].GetComponent<TextMeshProUGUI>().color = Color.green;
				else w[p].SP[2].GetComponent<TextMeshProUGUI>().color = Color.green;

				w[p].SP[0].GetComponent<TextMeshProUGUI>().text = first;
				w[p].SP[1].GetComponent<TextMeshProUGUI>().text = second;
				w[p].SP[2].GetComponent<TextMeshProUGUI>().text = third;

				w[p].gameObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
			}
		}

		for (int q = 0; q < s.Length; q++)
		{
			if (s[q].tipo == person)
			{

				if (playerNumber == 0) s[q].SP[0].GetComponent<TextMeshProUGUI>().color = Color.green;
				else if (playerNumber == 1) s[q].SP[1].GetComponent<TextMeshProUGUI>().color = Color.green;
				else s[q].SP[2].GetComponent<TextMeshProUGUI>().color = Color.green;

				s[q].SP[0].GetComponent<TextMeshProUGUI>().text = first;
				s[q].SP[1].GetComponent<TextMeshProUGUI>().text = second;
				s[q].SP[2].GetComponent<TextMeshProUGUI>().text = third;

				s[q].gameObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
			}
		}
	}

	void setTableName()
	{
		FindObjectOfType<PlayerName>().setName(playersInGame[0].name, playersInGame[1].name, playersInGame[2].name);
	}

	private bool createCharacter(int number)
	{
		bool wasCreated = false;

		int PosX = Random.Range(0, 9);
		int PosZ = Random.Range(0, 9);

		if (!table[PosX, PosZ].occupied)
		{
			Vector3 p = table[PosX, PosZ].physicalRepresentation.transform.position;
			GameObject g = Instantiate(characters[number], p, Quaternion.identity);
			spawnLight.transform.position = new Vector3(p.x, p.y, -1.5f);
			spawnLight.color = characters[number].GetComponent<Characters>().characterColor;
			g.GetComponent<Characters>().setIJ(PosX, PosZ);
			g.GetComponent<Characters>().casillaID = table[PosX, PosZ].ID;
			spawnLight.enabled = true;
			wasCreated = true;
			table[PosX, PosZ].occupied = true;
			charactersObjects[number] = g;

			tableStandPoints[] tsp = FindObjectsOfType<tableStandPoints>();

			foreach (tableStandPoints ts in tsp)
			{
				if (table[PosX, PosZ].ID == ts.TableId)
				{
					ts.addPerson();
				}
			}

		}

		return wasCreated;
	}

	public void hidePlayerButtons()
	{
		foreach (GameObject b in playerButtons)
		{
			b.SetActive(false);
		}
	}

	public void sowPlayerButtons()
	{
		foreach (GameObject b in playerButtons)
		{
			b.SetActive(true);
		}
	}
}
