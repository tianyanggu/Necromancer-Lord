using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

	public static int width = 8;
	public static int height = 8;

	public Color defaultColor = Color.white;

	public HexCell cellPrefab;
	public Text cellLabelPrefab;

	HexCell[] cells;

	Canvas gridCanvas;
	HexMesh hexMesh;

	void Awake () {
		gridCanvas = GetComponentInChildren<Canvas>();
		hexMesh = GetComponentInChildren<HexMesh>();

		cells = new HexCell[height * width];

		for (int z = 0, i = 0; z < height; z++) {
			for (int x = 0; x < width; x++) {
				CreateCell(x, z, i++);
			}
		}
	}

	void Start () {
		hexMesh.Triangulate(cells);
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

	public void EntityCellIndex (int index, string newEntity) {
		HexCell cell = cells[index];
		cell.entity = newEntity;
	}

	// ------------GET --------------------------
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

	public Vector3 GetCellPos (int index) {
		HexCell cell = cells[index];
		HexCoordinates coord = cell.coordinates;
		Vector3 position = GetCellPosFromIndex (coord.X, coord.Z, coord.Y);
		return position;
	}

	public Vector3 GetCellPosFromIndex (int x, int z, int i) {
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);
		return position;
	}

	public string GetEntity (int index) {
		HexCell cell = cells[index];
		string currEntity = cell.entity;
		return currEntity;
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

		cell.terrain = "empty";
		cell.entity = "empty";

		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();
	}
}