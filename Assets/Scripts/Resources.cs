using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Resources : MonoBehaviour {

	public GameObject souls;

	//change amount of souls
	public void ChangeSouls(int change) {
		souls = GameObject.Find ("Souls");
		int soulsAmount = PlayerPrefs.GetInt("Souls") + change;

		//sets to new amount
		Text soulsNum = souls.GetComponent<Text> ();
		soulsNum.text = "Souls:" + soulsAmount.ToString ();

		//saves to playerprefs
		PlayerPrefs.SetInt ("Souls", soulsAmount);
	}

	public void SetSouls(int amount) {
		souls = GameObject.Find ("Souls");

		Text soulsNum = souls.GetComponent<Text> ();
		soulsNum.text = "Souls:" + amount.ToString ();

		PlayerPrefs.SetInt ("Souls", amount);
	}
}
