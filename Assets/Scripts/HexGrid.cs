using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class HexGrid : MonoBehaviour {

	public int width;
	public int height;
	public int size;

	public Color defaultColor = Color.white;

	public HexCell cellPrefab;
	public Text cellLabelPrefab;

	public HexCell[] cells;

	Canvas gridCanvas;
	HexMesh hexMesh;

	void Awake () {
		gridCanvas = GetComponentInChildren<Canvas>();
		hexMesh = GetComponentInChildren<HexMesh>();
	}

	void Start () {
		hexMesh.Triangulate(cells);
    }

    // ------------SET--------------------------
    public void SetSize (int newheight, int newwidth) {
		height = newheight;
		width = newwidth;
		size = newheight * newwidth;

		cells = new HexCell[height * width];

		for (int z = 0, i = 0; z < height; z++) {
			for (int x = 0; x < width; x++) {
				CreateCell(x, z, i++);
			}
		}
	}

	public void ColorCell (Vector3 position, Color color) {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
		HexCell cell = cells[index];
		cell.color = color;
		hexMesh.Triangulate(cells);
	}

	//coordy equals coordx*(-1) + coordz*(-1). e.g. for 2,-5,3 : 2*(-1) + 3*(-1) which equals -5
	public void ColorCellCoordinates (int coordx, int coordz, Color color) {
		int index = coordx + coordz * width + coordz / 2;
		HexCell cell = cells[index];
		cell.color = color;
		hexMesh.Triangulate(cells);
	}

	public void ColorCellIndex (int index, Color color) {
		HexCell cell = cells[index];
		cell.color = color;
		hexMesh.Triangulate(cells);
	}

    public void SetEntityObject(int index, GameObject newEntityObj)
    {
        HexCell cell = cells[index];
        cell.entityObj = newEntityObj;
    }

    public void SetTerrain (int index, string newTerrain) {
		HexCell cell = cells[index];
		cell.terrain = newTerrain;
	}

    public void SetBuildingObject(int index, GameObject newBuildingObj)
    {
        HexCell cell = cells[index];
        cell.buildingObj = newBuildingObj;
    }

    public void SetCorpses(int index, List<string> corpses)
    {
        HexCell cell = cells[index];
        cell.corpses = corpses;
    }

    public void AddCorpse(int index, string corpse) {
		HexCell cell = cells [index];
	    cell.corpses.Add(corpse);
	}

	public void RemoveCorpse (int index, string corpse) {
		HexCell cell = cells [index];
		cell.corpses.Remove(corpse);
	}

    public void SetGroundEffects(int index, List<string> effects)
    {
        HexCell cell = cells[index];
        cell.groundEffects = effects;
    }

    public void AddGroundEffects(int index, string effect)
    {
        HexCell cell = cells[index];
        cell.groundEffects.Add(effect);
    }

    public void RemoveGroundEffects(int index, string effect)
    {
        HexCell cell = cells[index];
        cell.groundEffects.Remove(effect);
    }

    public void SetHasVision(int index, List<string> players)
    {
        HexCell cell = cells[index];
        cell.hasVision = players;
    }

    public void AddHasVision(int index, string player)
    {
        HexCell cell = cells[index];
        cell.hasVision.Add(player);
    }

    public void RemoveHasVision(int index, string player)
    {
        HexCell cell = cells[index];
        cell.hasVision.Remove(player);
    }

    public void SetFog (int index, bool toggle) {
        HexCell cell = cells[index];
        cell.fog = toggle;
    }

    // ------------GET--------------------------
    public Color GetCellColor (Vector3 position) {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
		HexCell cell = cells[index];
		Color currColor = cell.color;
		return currColor;
	}

	public int GetCellIndex (Vector3 position) {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
		return index;
	}

    public int GetCellIndexFromGameObject(GameObject entity)
    {
        Vector3 position = entity.transform.position;
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        return index;
    }

    public Vector3 GetCellPos (int index) {
		HexCell cell = cells[index];
		HexCoordinates coord = cell.coordinates;
		//offset is the first x coord of each row times -1
		int xoffset = index / (width + height);
		Vector3 position = GetCellPosFromIndex (coord.X + xoffset, coord.Z);
		return position;
	}

	public HexCoordinates GetCellCoord (int index) {
		return cells[index].coordinates;
	}

	public int GetCellIndexFromCoord (int coordx, int coordz) {
		int index = coordx + coordz * width + coordz / 2;
		return index;
	}

	public Vector3 GetCellPosFromIndex (int x, int z) {
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);
		return position;
	}

    public GameObject GetEntityObject(int index)
    {
        return cells[index].entityObj;
    }

    public string GetTerrain (int index) {
		return cells[index].terrain;
	}

    public GameObject GetBuildingObject(int index)
    {
        return cells[index].buildingObj;
    }

    public List<string> GetCorpses (int index) {
		return cells[index].corpses;
	}

    public List<string> GetGroundEffects(int index)
    {
        return cells[index].groundEffects;
    }

    public List<string> GetHasVision(int index)
    {
        return cells[index].hasVision;
    }

    public bool GetFog(int index) {
        return cells[index].fog;
    }

    //-----------CREATE-----------------------------
    void CreateCell (int x, int z, int i) {
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		cell.color = defaultColor;

		cell.terrain = "Empty";
        cell.buildingObj = null;
        cell.entityObj = null;
        cell.fog = true;

        Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();
	}
}