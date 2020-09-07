using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverObject : MonoBehaviour {


	Characters selectedCard;
	bool enable = true;
	bool isOverCharacter = false;

	public bool getEnable()
	{
		return enable;
	}

	// Update is called once per frame
	void Update() {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit))
		{
			if (hit.transform.gameObject.GetComponent<Characters>())
			{
				isOverCharacter = true;

				if (enable && !hit.transform.gameObject.GetComponent<Player>())
				{
					selectedCard = hit.transform.gameObject.GetComponent<Characters>();

					selectedCard.showCharacterName();
				}
			}

			else isOverCharacter = false;
		}

		if (Input.GetMouseButtonDown(0) && isOverCharacter && selectedCard != null)
		{
			selectedCard.selectCard();

			if (selectedCard.cardSelected)
			{
				enable = false;
			}

			else enable = true;
		}


	}
}
