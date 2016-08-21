using UnityEngine;
using System.Collections.Generic;

public class HexCell : MonoBehaviour {

	public HexCoordinates coordinates;

	public Color color;

	public string terrain;

	public string buildingName;

    public GameObject buildingObj;

    public string entityName;

    public GameObject entityObj;

	public List<string> corpses = new List<string>();

    public bool fog;
}