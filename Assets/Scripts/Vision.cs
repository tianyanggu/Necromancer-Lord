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

    //check each player entity in entitystorage to determine their vision range and remove the fog for that range
    public void EntityCurrPlayerVision() {
        if (playerManager.currPlayer != string.Empty)
        {
            char playerChar = playerManager.currPlayer[0];
            //Debug.Log(entityStorage.PlayerEntityList(playerChar)[0].name.Substring(2));
            foreach (GameObject playerEntity in entityStorage.PlayerEntityList(playerChar))
            {
                int visionDistance = 0;
                visionDistance = entityStats.GetCurrVision(playerEntity);
                int index = entityStats.GetCellIndex(playerEntity);
                string height = hexGrid.GetTerrain(index);
                PlayerVisionHelper(index, visionDistance, height);
            }
            foreach (GameObject buildingEntity in buildingStorage.PlayerBuildingList(playerChar))
            {
                int visionDistance = 0;
                visionDistance = buildingStats.GetCurrVision(buildingEntity);
                int index = buildingStats.GetCellIndex(buildingEntity);
                string height = hexGrid.GetTerrain(index);
                PlayerVisionHelper(index, visionDistance, height);
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
    private void PlayerVisionHelper(int index, int visionRange, string height)
    {
        //visited hexes get assigned if can be seen = 1, mountain = 0, in calculation = -1 or -2, cannot be seen equal -3
        Dictionary<int, int> visited = new Dictionary<int, int>();
        visited.Add(index, 1);
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
                    if (fringeTiles[f] >= 0 && fringeTiles[f] < hexGrid.size)
                    {
                        if (!visited.ContainsKey(fringeTiles[f]) || fringeTiles[f] != -1)
                        {
                            fringe.Enqueue(fringeTiles[f]);
                        }
                        if (hexGrid.GetTerrain(fringeTiles[f]) == "Mountain")
                        {
                            visited.Add(fringeTiles[f], 0);
                        } else
                        {

                        }
                    }
                }
            }
            frontier = fringe;
        }
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
