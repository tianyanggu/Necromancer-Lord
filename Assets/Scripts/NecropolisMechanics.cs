using UnityEngine;
using System.Collections;

public class NecropolisMechanics : MonoBehaviour {

	public int health = 200;

	public int lasthealth = 200;

	public string currConstruction = "Empty";
    public int currConstructionTimer;

    public string currRecruitment = "Empty";
    public int currRecruitmentTimer;

    public void UpdateProductionTimer ()
    {
        if (currConstruction != "Empty" )
        {
            currConstructionTimer--;
        }
    }
}
