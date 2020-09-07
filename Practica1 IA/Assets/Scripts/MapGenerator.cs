using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

// script que efectuara la creacion de mapas
// hay dos posibilidades, crear uno random de tamaño xSize * zSize
// o crear uno vacio definido 

public enum Cells { Empty, Water, Mud, rocks }; // tipo de las celdas que utilizaremos

// cada casilla del mapa es una box, 
// tiene el GO que saldra en el editor y la informacion del tipo
public struct box 
{
	public Cells cell;
	public GameObject mapCell;
}

public class MapGenerator : MonoBehaviour {

	public int xSize, zSize;

	public GameObject floorNode, tank, destiny, way;

	private GameObject tankRepresentation, destObj;

    private List<GameObject> wayResult = new List<GameObject>();

    public box[,] table;

	public bool randomMap = false;

	private bool onlyrock = true;

	public bool mapGenerated = false;

    int heuristic = 0;

	public Button [] buttons;

	Color greenButton;

	// inicio del componente
	private void Start()
	{
		destObj = Instantiate(destiny, new Vector3(0,0,0), Quaternion.Euler(-90, 0, -180));
		destObj.SetActive(false);
		FindObjectOfType<ChangeTileComponent>().enabled = false;

		greenButton = new Color();

		greenButton.r = 136 / 255;
		greenButton.g = 1;
		greenButton.b = 142 / 255;
		greenButton.a = 1;
	}

    // antes de crear un mapa nuevo debemos borrar el antiguo (si lo hay)
	void removePreviousMap()
    {
		FindObjectOfType<ChangeTileComponent>().enabled = true;

		if (mapGenerated)
        {
            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < zSize; j++)
                {
                    table[i, j].mapCell.GetComponent<FloorNodeComponent>().destroyMountain();
                    Destroy(table[i, j].mapCell);
                }
            }

            Destroy(tankRepresentation);
        }

        mapGenerated = true;
    }

    // genera un mapa predefinido vacio (solo casillas de hierba (empty))
    public void generateDefMap()
    {
        removePreviousMap();

        xSize = 5;
        zSize = 5;

        table = new box[xSize, zSize];

        Vector3 auxPos = new Vector3(0, 0, 0);

        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < zSize; j++)
            {
                auxPos = new Vector3(i, 0, j);

                GameObject auxG = Instantiate(floorNode, auxPos, Quaternion.identity);

                auxG.GetComponent<FloorNodeComponent>().setTypeOfCell(0, i, j);

                table[i, j].cell = 0;
                table[i, j].mapCell = auxG;
                auxG.GetComponent<FloorNodeComponent>().setTankOnMe(false);
            }
        }

        setCamera();

        Vector3 p = table[0, 0].mapCell.transform.position;
        p.y += 0.5f;
        tankRepresentation = Instantiate(tank, p, Quaternion.identity);
        table[0, 0].mapCell.GetComponent<FloorNodeComponent>().setTankOnMe(true);
    }

    // genera un mapa de fomra aleatoria
    public void generateRandomMap() {

		removePreviousMap();

		xSize = Random.Range(10, xSize);
		zSize = Random.Range(10, zSize);

		table = new box[xSize, zSize];

		Vector3 auxPos = new Vector3(0, 0, 0);

		for(int i = 0; i < xSize; i++)
		{
			for(int j = 0; j < zSize; j++)
			{
				auxPos = new Vector3(i, 0, j);

				GameObject auxG = Instantiate(floorNode, auxPos, Quaternion.identity);

				if (randomMap)
				{
					int rnd = Random.Range(0, 4);

					auxG.GetComponent<FloorNodeComponent>().setTypeOfCell((Cells)rnd, i,j);

					if ((Cells)rnd != Cells.rocks)
						onlyrock = false;

					table[i, j].cell = (Cells)rnd;
					table[i, j].mapCell = auxG;
					auxG.GetComponent<FloorNodeComponent>().setTankOnMe(false);
				}
			}
		}

		setCamera();

		if (!onlyrock)
		{
			bool tankCreated = false;

			while (!tankCreated)
			{
				tankCreated = createTank();
			}
		}
	}

    // para que la camara se coloque correctamente en funcion del tamaño del mapa
    void setCamera()
    {
       float high = 0;

		if (xSize > zSize)
			high = xSize + 1;
		else high = zSize + 1;

		Camera.main.transform.position = new Vector3(((float)xSize / 2f) + 0.5f, high, ((float)zSize / 2f) + 0.5f);
    }

    // creamos tanque teniendo en cuenta que no se cree sobre una roca
    private bool createTank()
	{
		bool wasCreated = false;

		//Now we setUp the tank
		int PosX = Random.Range(0, xSize);
		int PosZ = Random.Range(0, zSize);

		if (table[PosX, PosZ].cell != Cells.rocks)
		{
			Vector3 p = table[PosX, PosZ].mapCell.transform.position;
			p.y += 0.5f;
			tankRepresentation = Instantiate(tank, p, Quaternion.identity);
			wasCreated = true;
			table[PosX, PosZ].mapCell.GetComponent<FloorNodeComponent>().setTankOnMe(true);
		}

		return wasCreated;		
	}

    // corrutina que usaremos para mover el tanque
    IEnumerator MoveTank(GameObject tank, List<Node> myPath)
    {
        tank.GetComponent<TankBehaviour>().moving = true;

		int counter = 1;

        foreach (Node n in myPath)
        {
            Vector3 nextPos = tank.transform.position;
            nextPos.x = n.i_;
            nextPos.z = n.j_;

			tank.gameObject.transform.position = nextPos;

			if (n != myPath[myPath.Count - 1])
			{

				float zDir = 0;

				if (myPath[counter].i_ > n.i_)
				{
					zDir = 90;
				}

				else if (myPath[counter].i_ < n.i_)
				{
					zDir = -90;
				}

				else if (myPath[counter].j_ < n.j_)
				{
					zDir = 180;
				}

				tank.transform.rotation = Quaternion.Euler(0, zDir, 0);
			}

			tank.GetComponent<TankBehaviour>().i_ = (int)nextPos.x;
            tank.GetComponent<TankBehaviour>().j_ = (int)nextPos.z;

			//Vector3 p; p.x = n.i_; p.y = 0.5f; p.z = n.j_;
			//GameObject aux = Instantiate(way, p, Quaternion.identity);
			//wayResult.Add(aux);
			counter++;
			yield return new WaitForSeconds(.5f); ;
        }

		destObj.SetActive(false);

        tank.GetComponent<TankBehaviour>().moving = false;
        tank.GetComponent<TankBehaviour>().deselecTank();

        for (int i = 0; i < wayResult.Count; i++)
        {
            Destroy(wayResult[i]);
            wayResult[i] = null;
        }

        wayResult.Clear();
    }

    // creamos destino y avisamos a AStar para que realice el calculo del camino
    public bool createDestiny(int x, int z, bool move)
    {
        bool haySol = false;

        GameObject tank = GameObject.FindGameObjectWithTag("Tank");

        if (!tank.GetComponent<TankBehaviour>().moving)
        {
            Vector3 pos;
            pos.x = x;
            pos.y = 0.7f;
            pos.z = z;

           // destObj = null;
            //destObj = GameObject.FindGameObjectWithTag("Destiny");

			GameObject[] arrows = GameObject.FindGameObjectsWithTag("Arrow");

			destObj.SetActive(false);

			if(arrows.Length != 0)
			{
				foreach (GameObject g in arrows)
				{
					Destroy(g);
				}
			}


			destObj.SetActive(true);
			destObj.transform.position = pos;

            haySol = tank.GetComponent<AStar>().Begin(heuristic, x, z, tank.GetComponent<TankBehaviour>().i_, tank.GetComponent<TankBehaviour>().j_,
                FindObjectOfType<MapGenerator>().table);

            List<Node> myPath = tank.GetComponent<AStar>().getPath();

			if (move)
			{
				IEnumerator coroutine = MoveTank(tank, myPath);
				StartCoroutine(coroutine);
			}
        }

        return haySol;
    }

    // creamos flechas que indicaran el camino
	public void createArrows(int i, int j, float zDir)
	{
		Vector3 p; p.x = i; p.y = 2f; p.z = j;
		GameObject aux = Instantiate(way, p, Quaternion.Euler(-90,0,zDir));
		wayResult.Add(aux);
	}

    // metodos auxiliares para cambiar la heuristica desde los botones
    public void setManhattan() { heuristic = 0; changeButtonColor();  }
    public void setChebyshev() { heuristic = 1; changeButtonColor(); }
    public void setBadHeu() { heuristic = 2; changeButtonColor(); }

	void changeButtonColor()
	{ 
	

		for(int i = 0; i < buttons.Length; i++)
		{
			if (i != heuristic)
			{
				buttons[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
			}

			else { buttons[heuristic].GetComponent<Image>().color = greenButton; }
		}
	}
}
