using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	private int redTeamPts;
	private int blueTeamPts;
	public GameObject RedTeamText;
	public GameObject BlueTeamText;
	private Animator goalAnim_;
	public Animator light1;
	public Animator light2;
	public Light mainLight;
	public float SkySpeed = 0.5f;
	public GameObject canvasToStart;
	bool gamePaused = false;
	private int redTeamChuts = 0;
	private int blueTeamChuts = 0;
	public GameObject RedTeamChuts;
	public GameObject BlueTeamChuts;

	private bool goalScored = false;

	Vector3 auxRedGoalPos;
	Vector3 redDirection;

	Vector3 auxBlueGoalPos;
	Vector3 blueDirection;

	private GameObject redGoalPosition;
	private GameObject blueGoalPosition;
	private GameObject ballPosition;

	private GameObject redTeamAttacker;

    public AudioSource audioSource;
    public AudioSource backgroundAudioSource;
    public AudioClip goalAudioClip;
    public AudioClip chutAudioClip;
    public AudioClip whistleClip;

	public void quitGame()
	{
		Application.Quit();
	}

	public void reloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void setStartGame()
	{
		canvasToStart.SetActive(false);
		Time.timeScale = 2;

        audioSource.clip = whistleClip;
        audioSource.Play();
        backgroundAudioSource.Play();
	}

	public void Pause()
	{
		gamePaused = !gamePaused;

		if (gamePaused)
			Time.timeScale = 0;
		else
			Time.timeScale = 2;
	}

	public void addRedTeamChut()
	{
		redTeamChuts++;

		if (redTeamChuts < 10)
			RedTeamChuts.GetComponent<TextMesh>().text = "00" + redTeamChuts.ToString();
		else if (redTeamChuts < 100)
			RedTeamChuts.GetComponent<TextMesh>().text = "0" + redTeamChuts.ToString();
		else
			RedTeamChuts.GetComponent<TextMesh>().text = redTeamChuts.ToString();

        audioSource.clip = chutAudioClip;
        audioSource.Play();
	}

	public void addBlueTeamChut()
	{
		blueTeamChuts++;

		if (blueTeamChuts < 10)
			BlueTeamChuts.GetComponent<TextMesh>().text = "00" + blueTeamChuts.ToString();
		else if (blueTeamChuts < 100)
			BlueTeamChuts.GetComponent<TextMesh>().text = "0" + blueTeamChuts.ToString();
		else
			BlueTeamChuts.GetComponent<TextMesh>().text = blueTeamChuts.ToString();

        audioSource.clip = chutAudioClip;
        audioSource.Play();
    }

	public void Start()
	{
		Time.timeScale = 0;
		redGoalPosition = GameObject.FindGameObjectWithTag("PorteriaRoja");
		blueGoalPosition = GameObject.FindGameObjectWithTag("PorteriaAzul");
		ballPosition = GameObject.FindGameObjectWithTag("ball");

		light1.gameObject.SetActive(false);
		light2.gameObject.SetActive(false);
		mainLight.intensity = 1;

		goalAnim_ = GameObject.FindGameObjectWithTag("goal").GetComponent<Animator>();

		float rnd = Random.Range(0, 0.6f);

		if (Random.Range(0, 2) == 0)
		{
			rnd *= -1;
		}

		GameObject ball = GameObject.FindGameObjectWithTag("ball");
		ball.transform.position = new Vector3(rnd, 25, 0);

		RedTeamText.GetComponent<TextMesh>().text = "000";
		BlueTeamText.GetComponent<TextMesh>().text = "000";
		RedTeamChuts.GetComponent<TextMesh>().text = "000";
		BlueTeamChuts.GetComponent<TextMesh>().text = "000";
	}

	private void Update()
	{
		auxRedGoalPos = new Vector3(redGoalPosition.transform.position.x, ballPosition.transform.position.y, redGoalPosition.transform.position.z);

		auxBlueGoalPos = new Vector3(blueGoalPosition.transform.position.x, ballPosition.transform.position.y, blueGoalPosition.transform.position.z);

		redDirection = -(auxRedGoalPos - ballPosition.transform.position);

		blueDirection = -(auxBlueGoalPos - ballPosition.transform.position);

		Debug.DrawRay(auxRedGoalPos, redDirection * 1.1f, Color.red);

		Debug.DrawRay(auxBlueGoalPos, blueDirection * 1.1f, Color.blue);

		RenderSettings.skybox.SetFloat("_Rotation", Time.time * SkySpeed);

		if (Input.GetKeyDown(KeyCode.A))
		{
			addPts("RedTeam", 1);
		}

		else if (Input.GetKeyDown(KeyCode.D))
		{
			addPts("BlueTeam", 1);
		}
	}

	public void setRedTeamAttacker(GameObject g)
	{
		redTeamAttacker = g;
	}

	public GameObject getRedTeamAttacker()
	{
		return redTeamAttacker;
	}

	public Vector3 getAuxRedGoalPos()
	{
		return auxRedGoalPos;
	}

	public Vector3 getRedDirection()
	{
		return redDirection;
	}

	public Vector3 getAuxBlueGoalPos()
	{
		return auxBlueGoalPos;
	}

	public Vector3 getBlueDirection()
	{
		return blueDirection;
	}

	public int getRedPts()
	{
		return redTeamPts;
	}

	public int getBluePts()
	{
		return blueTeamPts;
	}

	public void addPts(string team, int pts)
	{
		GameObject ball = GameObject.FindGameObjectWithTag("ball");

		Rigidbody ballRb = ball.GetComponent<Rigidbody>();

		ballRb.velocity = Vector3.zero;
		ballRb.angularVelocity = Vector3.zero;
		ballRb.Sleep();

		ball.transform.position = new Vector3(0, -25, 0);

		light1.gameObject.SetActive(true);
		light2.gameObject.SetActive(true);

		light1.SetBool("GoalFalling", true);
		light2.SetBool("GoalFalling", true);

		mainLight.intensity = 0;

		goalAnim_.SetBool("GoalFalling", true);


		if (team == "RedTeam")
		{
			redTeamPts += pts;
		}

		else blueTeamPts += pts;

		if(redTeamPts < 10)
			RedTeamText.GetComponent<TextMesh>().text = "00" + redTeamPts.ToString();
		else if(redTeamPts < 100)
			RedTeamText.GetComponent<TextMesh>().text = "0" + redTeamPts.ToString();
		else
			RedTeamText.GetComponent<TextMesh>().text = redTeamPts.ToString();

		if(blueTeamPts < 10)
			BlueTeamText.GetComponent<TextMesh>().text = "00" + blueTeamPts.ToString();
		else if(blueTeamPts < 100)
			BlueTeamText.GetComponent<TextMesh>().text = "0" + blueTeamPts.ToString();
		else
			BlueTeamText.GetComponent<TextMesh>().text = blueTeamPts.ToString();

		goalScored = true;

		Invoke("midTime", 10f);

        audioSource.clip = goalAudioClip;
        audioSource.Play();
	}

	public bool goalTime()
	{
		return goalScored;
	}

	public void midTime()
	{
		mainLight.intensity = 1;

		light1.Rebind();
		light2.Rebind();

		goalAnim_.SetBool("GoalFalling", false);
		light1.SetBool("GoalFalling", false);
		light2.SetBool("GoalFalling", false);

		light1.gameObject.SetActive(false);
		light2.gameObject.SetActive(false);

		goalScored = false;
		next();
	}


	public void next()
	{
		float rnd = Random.Range(0, 0.6f);

		if (Random.Range(0, 2) == 0)
		{
			rnd *= -1;
		}

		GameObject ball = GameObject.FindGameObjectWithTag("ball");
		ball.transform.position = new Vector3(rnd, 25, 0);
	}
}
