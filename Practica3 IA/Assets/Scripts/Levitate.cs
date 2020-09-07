using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitate : MonoBehaviour {

	Animator anim_;
	// Use this for initialization
	void Start () {
		anim_ = GetComponent<Animator>();
		int rnd = Random.Range(0, 3);
		Invoke("animationStart", rnd);
	}

	void animationStart()
	{
		anim_.SetBool("start", true);
	}
}
