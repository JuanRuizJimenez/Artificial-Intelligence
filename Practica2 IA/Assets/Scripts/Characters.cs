using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour {

	public string myName;
	bool showName = false;
	SpriteRenderer render;
	public bool cardSelected = false;

	public int casillaID;
	public people me;

	public Color characterColor;

	Vector3 originalPosition;
	Vector3 originalScale;

	Vector3 middlePosition;
	Vector3 middleScale;

	public int i_, j_;

	public void setIJ(int i, int j)
	{
		i_ = i;
		j_ = j;
	}

	public void getIJ(ref int i, ref int j)
	{
		i = i_;
		j = j_;
	}

	public void setCasillaID(int i)
	{
		casillaID = i;
	}

	public int getCasillaID()
	{
		return casillaID;
	}

	private void Start()
	{
		render = GetComponentInChildren<SpriteRenderer>();

		originalPosition = this.transform.position;
		originalScale = this.transform.localScale;

		middlePosition = new Vector3(0, 0, gameObject.transform.position.z);
		middleScale = new Vector3(5, 5, 5);

		characterColor = Color.white;
	}

	// Update is called once per frame
	void Update () {
		if (!showName)
		{
			render.enabled = false;
		}

		else
			render.enabled = true;

		showName = false;

		if (gameObject.GetComponent<Player>())
			showName = true;
	}

	public void showCharacterName()
	{
		showName = true;
	}

	public void selectCard()
	{
		cardSelected = !cardSelected;

		if (!cardSelected)
		{
			//this.transform.position = originalPosition;
			//this.transform.localScale = originalScale;
			this.transform.gameObject.GetComponent<Renderer>().material.color = Color.white;
		}

		else
		{
			//this.transform.position = middlePosition;
			//this.transform.localScale = middleScale;
			this.transform.gameObject.GetComponent<Renderer>().material.color = Color.red;

			FindObjectOfType<Suggest>().findCharacterSelected(myName);

			FindObjectOfType<Accusate>().setPersonToAccuse(myName);
		}
	}
}
