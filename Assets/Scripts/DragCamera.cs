using UnityEngine;
using System.Collections;

public class DragCamera : MonoBehaviour
{
	public float dragSpeed = 2;
	private Vector3 dragOrigin;

	public bool cameraDragging = true;

	public static float outerLeft = 0f;
	public static float outerRight = 100f;
	public static float outerTop = 0f;
	public static float outerDown = 100f;

	void Update()
	{



		Vector3 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

		float left = Screen.width * 0.2f;
		float right = Screen.width - (Screen.width * 0.2f);
		float top = Screen.width * 0.2f;
		float down = Screen.width - (Screen.width * 0.2f);

		if(mousePosition.x < left)
		{
			cameraDragging = true;
		}
		else if(mousePosition.x > right)
		{
			cameraDragging = true;
		}
		else if(mousePosition.z > top)
		{
			cameraDragging = true;
		}
		else if(mousePosition.z > down)
		{
			cameraDragging = true;
		}





		if (cameraDragging) {

			if (Input.GetMouseButtonDown(0))
			{
				dragOrigin = Input.mousePosition;
				return;
			}

			if (!Input.GetMouseButton(0)) return;

			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
			//made pos.y since screen is x and y. i.e. no z. Thus needed to make y movement of mouse into z axis movement in game.
			Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);

			if (move.x > 0f)
			{
				if(this.transform.position.x < outerRight)
				{
					transform.Translate(move, Space.World);
				}
			}
			else{
				if(this.transform.position.x > outerLeft)
				{
					transform.Translate(move, Space.World);
				}
			}

			if (move.y > 0f)
			{
				if(this.transform.position.z < outerTop)
				{
					transform.Translate(move, Space.World);
				}
			}
			else{
				if(this.transform.position.z > outerDown)
				{
					transform.Translate(move, Space.World);
				}
			}
		}
	}


}
