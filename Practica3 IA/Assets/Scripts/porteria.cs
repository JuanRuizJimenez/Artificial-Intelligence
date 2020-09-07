using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class porteria : MonoBehaviour {

	public string myTeam;
	GameManager gm;
	Rigidbody ballRb;
	public void Start()
	{
		gm = FindObjectOfType<GameManager>();
		ballRb = GameObject.FindGameObjectWithTag("ball").GetComponent<Rigidbody>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("ball"))
		{
			if (myTeam == "RedTeam")
			{
				gm.addPts("BlueTeam", 1);
			}

			else
			{
				gm.addPts("RedTeam", 1);
			}
		}
	}
}
