using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour {

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("BlueTeam") || other.CompareTag("RedTeam"))
		{
			other.GetComponent<chutEnableed>().activeShoot();
			other.GetComponent<chutEnableed>().resetShootTime();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("BlueTeam") || other.CompareTag("RedTeam"))
		{
			other.GetComponent<chutEnableed>().disableShoot();
			other.GetComponent<chutEnableed>().resetShootTime();
		}
	}
}
