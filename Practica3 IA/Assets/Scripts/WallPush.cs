using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPush : MonoBehaviour {

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("ball"))
		{
			Rigidbody rb = other.GetComponent<Rigidbody>();
			rb.AddForce(-rb.velocity.normalized * 1500);
		}
	}
}
