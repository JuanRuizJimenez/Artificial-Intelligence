using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

	public float offset = 0;
	// Update is called once per frame
	void Update () {
		Vector3 pos = GameObject.FindGameObjectWithTag("ball").transform.position;
		transform.position = new Vector3(pos.x + offset, transform.position.y, transform.position.z);
	}
}
