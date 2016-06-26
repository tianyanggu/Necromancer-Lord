﻿using UnityEngine;
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

	HexCell[] cells;

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

	public void SetEntity (int index, string newEntity) {
		HexCell cell = cells[index];
		cell.entity = newEntity;
	}

	public void SetTerrain (int index, string newTerrain) {
		HexCell cell = cells[index];
		cell.terrain = newTerrain;
	}

	public void SetBuilding (int index, string newBuilding) {
		HexCell cell = cells[index];
		cell.building = newBuilding;
	}

	public void SetCorpses (int index, string corpse) {
		int availCorpseNum = AvailableCorpseNum (index);

		//if corpses not over 5, if over 5 then do not add to pile
		if (availCorpseNum != 5) {
			string cleanCorpse = Regex.Replace(corpse, @"[\d-]", string.Empty);
			HexCell cell = cells [index];
			cell.corpses.Add (cleanCorpse);

			//set to playerprefs
			PlayerPrefs.SetString ("HexCorpses" + index + "corpse" + availCorpseNum, cleanCorpse);
		}
	}

	public void RemoveCorpses (int index, string corpse) {
		HexCell cell = cells [index];
		cell.corpses.Remove (corpse);

		//get index of the corpse to be removed
		int corpseIndex = 5;
		for (int i = 0; i < 4; i++) {
			string allEntity = PlayerPrefs.GetString ("HexCorpses" + index + "corpse" + i);
			if (allEntity == corpse) {
				corpseIndex = i;
			}
		}

		//set to playerprefs
		PlayerPrefs.DeleteKey ("HexCorpses" + index + "corpse" + corpseIndex);
	}

	//Check for next available setstring number for corpses on that tile
	private int AvailableCorpseNum (int index) {
		for (int i = 0; i < 4; i++) {
			string allEntity = PlayerPrefs.GetString ("HexCorpses" + index + "corpse" + i);
			if (allEntity == "") {
				return i;
			}
		}
		return 5; //TODO error message if no available spaces, should not be possible to give null
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

	public Vector3 GetCellPos (int index) {
		HexCell cell = cells[index];
		HexCoordinates coord = cell.coordinates;
		//offset is the first x coord of each row times -1
		int xoffset = index / (width + height);
		Vector3 position = GetCellPosFromIndex (coord.X + xoffset, coord.Z);
		return position;
	}

	public HexCoordinates GetCellCoord (int index) {
		HexCell cell = cells[index];
		HexCoordinates coord = cell.coordinates;
		return coord;
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

	public string GetEntity (int index) {
		HexCell cell = cells[index];
		string currEntity = cell.entity;
		return currEntity;
	}

	public string GetTerrain (int index) {
		HexCell cell = cells[index];
		string currTerrain = cell.terrain;
		return currTerrain;
	}

	public string GetBuilding (int index) {
		HexCell cell = cells[index];
		string currBuilding = cell.building;
		return currBuilding;
	}

	public List<string> GetCorpses (int index) {
		HexCell cell = cells[index];
		List<string> currCorpses = cell.corpses;
		return currCorpses;
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
		cell.building = "Empty";
		cell.entity = "Empty";

		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();
	}
}