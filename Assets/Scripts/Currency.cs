using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Currency : MonoBehaviour {

	public GameObject soulsObject;
    public int souls = 0;
    public int gold = 0;

    //change amount of souls
    public void ChangeSouls(int change) {
		souls += change;

		//sets to new amount
		Text soulsNum = soulsObject.GetComponent<Text> ();
		soulsNum.text = "Souls:" + souls.ToString ();
	}

    public void SetSouls(int amount)
    {
        souls = amount;
        Text soulsNum = soulsObject.GetComponent<Text>();
        soulsNum.text = "Souls:" + amount.ToString();
    }
}
