using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectSize : MonoBehaviour {

	Vector3 originalScale = new Vector3(0.8f, 0.8f, 0.8f);
	
	public void setOriginalScale()
	{
		transform.localScale = originalScale;
	}

	void Update()
	{
		transform.localScale = originalScale + new Vector3(Mathf.Sin(Time.time * 2) / 4, Mathf.Sin(Time.time * 2) / 4, Mathf.Sin(Time.time * 2) / 4);
	}
}
