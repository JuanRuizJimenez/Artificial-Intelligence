using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RedTeamPlayer : MonoBehaviour {

	private bool moveOriginalPosition = false;

	public float AtackSpeed = 10f;
	public float BackSpeed = 20f;

	private GameObject target;

	private bool enteredInMySide;

	Vector3 originalPosition;

	private NavMeshAgent navMeshAgent;

	Rigidbody rb;

	private bool attack = false;

	//---------------------------------------------------------------------------


	public Vector3 getOriginalPosition()
	{
		return originalPosition;
	}

	public void setMoveOriginalPositon(bool b)
	{
		moveOriginalPosition = b;
	}

	public bool getMoveOriginalPosition()
	{
		return moveOriginalPosition;
	}

	public void setAttack(bool a)
	{
		attack = a;
	}

	public bool getAttack()
	{
		return attack;
	}

	public void Awake()
	{
		// Cachear para acceder más rápido
		navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

		originalPosition = transform.position;

		// Establece la velocidad linear
		navMeshAgent.speed = AtackSpeed;
	}

	public void Start()
	{
		navMeshAgent.enabled = true;
		rb = gameObject.GetComponent<Rigidbody>();
		target = GameObject.FindGameObjectWithTag("ball");
	}

	public void setSupport(Vector3 pos)
	{
		navMeshAgent.SetDestination(pos);
	}

	public void Update()
	{
		
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		rb.Sleep();

		//if (transform.position.x < originalPosition.x)
		//moveOriginalPosition = false;



		if (moveOriginalPosition)
		{
			// x - 10
			navMeshAgent.speed = BackSpeed;
			navMeshAgent.SetDestination(new Vector3(originalPosition.x, originalPosition.y, originalPosition.z));
		}

		if (attack)
		{
			navMeshAgent.speed = AtackSpeed;
			if (transform.position.x > target.transform.position.x || transform.position.z > target.transform.position.z + 5 || transform.position.z < target.transform.position.z - 5)
			{
				Vector3 v = new Vector3();
				v = target.transform.position;

				navMeshAgent.SetDestination(new Vector3(v.x - 5, v.y, v.z));
			}

			else navMeshAgent.SetDestination(target.transform.position);

			chutEnableed ce = gameObject.GetComponent<chutEnableed>();

			if (ce.canIShoot())
			{
				ce.disableShootTime();
				ce.disableShoot();
				Chut();
			}
		}
	}

	private void Chut()
	{
		Vector3 direction = FindObjectOfType<GameManager>().getBlueDirection();
		direction = direction.normalized;
		direction *= -1;

		if (direction.x > 0)
			target.GetComponent<Rigidbody>().AddForce(direction * 1500);

		FindObjectOfType<GameManager>().addRedTeamChut();
	}
}
