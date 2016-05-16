using UnityEngine;
using System.Collections;

public class DragCamera : MonoBehaviour
{
	public float dragSpeed = 2;
	private Vector3 dragOrigin;

	public bool cameraDragging = true;

	public static float outerLeft = 0f;
	public static float outerRight = 200f;
	public static float outerTop = 150f;
	public static float outerDown = -50f;

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
		//else if(mousePosition.y > top)
		//{
		//	cameraDragging = true;
		//}
		//else if(mousePosition.y > down)
		//{
		//	cameraDragging = true;
		//}





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

			transform.Translate(move, Space.World);

			if (this.transform.position.x < outerLeft) {
				this.transform.position = new Vector3 (outerLeft, this.transform.position.y, this.transform.position.z);
			}
			if (this.transform.position.x > outerRight) {
				this.transform.position = new Vector3 (outerRight, this.transform.position.y, this.transform.position.z);
			}
			if (this.transform.position.z > outerTop) {
				this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, outerTop);
			}
			if (this.transform.position.z < outerDown) {
				this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, outerDown);
			}

			//if (move.x > 0f)
			//{
			//	if(this.transform.position.x < outerRight)
			//	{
			//		transform.Translate(move, Space.World);
			//	}
			//}
			//else{
			//	if(this.transform.position.x > outerLeft)
			//	{
			//		transform.Translate(move, Space.World);
			//	}
			//}
//
			//if (move.z > 0f)
			//{
			//	if(this.transform.position.z < outerTop)
			//	{
			//		transform.Translate(move, Space.World);
			//	}
			//}
			//else{
			//	if(this.transform.position.z > outerDown)
			//	{
			//		transform.Translate(move, Space.World);
			//	}
			//}
		}
	}


}
