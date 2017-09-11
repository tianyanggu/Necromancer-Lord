using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class Vision : MonoBehaviour {
    public EntityStorage entityStorage;
    public BuildingStorage buildingStorage;
    public HexGrid hexGrid;
    public PlayerManager playerManager;
    public EntityStats entityStats;
    public BuildingStats buildingStats;

    public Fog fogPrefab;
    Fog[] fogs;

    // Use this for initialization
    void Start () {
        AddAllFog();
    }

    public void AddAllFog()
    {
        fogs = new Fog[hexGrid.size];
        for (int i = 0; i < hexGrid.size; i++) {
            bool checkFog = hexGrid.GetFog(i);
            if (checkFog == true) {
                Vector3 pos = hexGrid.GetCellPos(i);
                pos.y = 0.1f;
                Fog fog = fogs[i] = Instantiate<Fog>(fogPrefab);
                fog.transform.SetParent(transform, false);
                fog.transform.localPosition = pos;
            }
        }
    }

    public void AddMissingFog()
    {
        for (int i = 0; i < hexGrid.size; i++)
        {
            bool checkFog = hexGrid.GetFog(i);
            if (checkFog == false)
            {
                hexGrid.SetFog(i, true);
                Vector3 pos = hexGrid.GetCellPos(i);
                pos.y = 0.1f;
                Fog fog = fogs[i] = Instantiate<Fog>(fogPrefab);
                fog.transform.SetParent(transform, false);
                fog.transform.localPosition = pos;
            }
        }
    }

    //check each player entity in entitystorage to determine their vision range and remove the fog for that range
    public void EntityCurrPlayerVision() {
        if (playerManager.currPlayer != string.Empty)
        {
            char playerChar = playerManager.currPlayer[0];
            //Debug.Log(entityStorage.PlayerEntityList(playerChar)[0].name.Substring(2));
            foreach (GameObject playerEntity in entityStorage.PlayerEntityList(playerChar))
            {
                int visionDistance = entityStats.GetCurrVision(playerEntity);
                int index = entityStats.GetCellIndex(playerEntity);
                string height = hexGrid.GetTerrain(index);

                Dictionary<int, int> vision = PlayerVisionHelper(index, visionDistance, height);
                foreach (var tile in vision)
                {
                    hexGrid.SetFog(tile.Key, false);
                    Fog fog = fogs[tile.Key];
                    fog.GetComponent<Renderer>().enabled = false;
                }
            }
            foreach (GameObject buildingEntity in buildingStorage.PlayerBuildingList(playerChar))
            {
                int visionDistance = buildingStats.GetCurrVision(buildingEntity);
                int index = buildingStats.GetCellIndex(buildingEntity);
                string height = hexGrid.GetTerrain(index);

                Dictionary<int, int> vision = PlayerVisionHelper(index, visionDistance, height);
                foreach (var tile in vision)
                {
                    if (tile.Value >= -2)
                    {
                        hexGrid.SetFog(tile.Key, false);
                        Fog fog = fogs[tile.Key];
                        fog.GetComponent<Renderer>().enabled = false;
                    }
                }
            }


        }

    }

    //check each hex tile to determine player's vision and remove the fog
    public void CurrPlayerVision()
    {
        if (playerManager.currPlayer != string.Empty)
        {
            char playerChar = playerManager.currPlayer[0];
            for (int i = 0; i<hexGrid.size; i++)
            {
                if (hexGrid.GetHasVision(i).Contains(playerManager.currPlayer))
                {
                    hexGrid.SetFog(i, true);
                } else
                {
                    hexGrid.SetFog(i, false);
                }
            }
        }
    }

    //removes fog of war from an index given the vision distance
    private Dictionary<int, int> PlayerVisionHelper(int index, int visionRange, string height)
    {
        //TODO but needs major revision since algorithm still presents major errors in vision: visited hexes get assigned if can be seen = 1, mountain = 0, in calculation = -1 or -2, cannot be seen equal -3
        Dictionary<int, int> checkedTiles = new Dictionary<int, int>();
        Dictionary<int, int> previousVisited = new Dictionary<int, int>();
        checkedTiles.Add(index, 1);
        previousVisited.Add(index, 1);
        Queue<int> frontier = new Queue<int>();
        frontier.Enqueue(index);

        for (int i = 0; i < visionRange; i++)
        {
            Queue<int> fringe = new Queue<int>();
            for (int j = 0; j < frontier.Count; j++)
            {
                int[] fringeTiles = FringeHexTiles(frontier.Peek());
                for (int f = 0; f < 6; f++)
                {
                    if (!previousVisited.ContainsKey(fringeTiles[f]) && fringeTiles[f] >= 0 && fringeTiles[f] < hexGrid.size)
                    {
                        fringe.Enqueue(fringeTiles[f]);
                        //get curr direction to centre index via coords and check if vision is blocked by checking previous previousVisited tiles in relation to position
                        //if (hexGrid.GetTerrain(fringeTiles[f]) == "Mountain" || checkedTiles.ContainsKey(fringeTiles[f]))
                        //{
                        //HexCoordinates centreCoordinates = hexGrid.GetCellCoord(index);
                        //HexCoordinates fringeCoordinates = hexGrid.GetCellCoord(index);
                        //if (centreCoordinates.X == fringeCoordinates.X)
                        //{
                        //    checkedTiles.Add(fringeTiles[f], 0);
                        //} else if (centreCoordinates.Y == fringeCoordinates.Y)
                        //{

                        //} else if ((centreCoordinates.X + centreCoordinates.Y) == (fringeCoordinates.X + fringeCoordinates.Y))
                        //{

                        //}

                        //}
                        previousVisited.Add(fringeTiles[f],1);
                    }
                }
            }
            frontier = fringe;
        }

        return previousVisited;
    }

    private int[] FringeHexTiles(int index)
    {
        HexCoordinates coord = hexGrid.GetCellCoord(index);
        int coordx = coord.X;
        int coordz = coord.Z;

        int left = hexGrid.GetCellIndexFromCoord(coordx - 1, coordz);
        int right = hexGrid.GetCellIndexFromCoord(coordx + 1, coordz);
        int uleft = hexGrid.GetCellIndexFromCoord(coordx - 1, coordz + 1);
        int uright = hexGrid.GetCellIndexFromCoord(coordx, coordz + 1);
        int lleft = hexGrid.GetCellIndexFromCoord(coordx, coordz - 1);
        int lright = hexGrid.GetCellIndexFromCoord(coordx + 1, coordz - 1);

        //makes the order go clockwise, which is needed for vision calculations
        return new int[] { left, lleft, lright, right, uright, uleft };
    }
}
