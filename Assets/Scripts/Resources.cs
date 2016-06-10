using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Resources : MonoBehaviour {

	public GameObject corpses;

	//change amount of corpses
	public void ChangeCorpses(int change) {
		corpses = GameObject.Find ("Corpses");
		//gets amount of corpses
//		corpses.GetComponent<Corpses> ().amount = corpses.GetComponent<Corpses> ().amount + change;
//		int corpsesAmount = corpses.GetComponent<Corpses> ().amount;
		int corpsesAmount = PlayerPrefs.GetInt("Corpses") + change;

		//sets to new amount
		Text corpsesNum = corpses.GetComponent<Text> ();
		corpsesNum.text = "Corpses:" + corpsesAmount.ToString ();

		//saves to playerprefs
		PlayerPrefs.SetInt ("Corpses", corpsesAmount);
	}

	public void SetCorpses(int amount) {
		corpses = GameObject.Find ("Corpses");

		Text corpsesNum = corpses.GetComponent<Text> ();
		corpsesNum.text = "Corpses:" + amount.ToString ();

		PlayerPrefs.SetInt ("Corpses", amount);
	}
}
