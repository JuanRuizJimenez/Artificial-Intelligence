using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chutEnableed : MonoBehaviour {

	bool canShoot = false;

	bool shootTime = true;

	public void activeShoot()
	{
		canShoot = true;
	}

	public void disableShoot()
	{
		canShoot = false;
	}

	public bool canIShoot()
	{
		return canShoot && shootTime;
	}

	private void res()
	{
		shootTime = true;
	}

	public void disableShootTime()
	{
		shootTime = false;
	}

	public void resetShootTime()
	{
		Invoke("res", 2f);
	}
}
