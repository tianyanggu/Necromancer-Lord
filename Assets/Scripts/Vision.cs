using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class Vision : MonoBehaviour {

    public EntityStorage entityStorage;
    public HexGrid hexGrid;

    public Fog fogPrefab;
    Fog[] fogs;

    // Use this for initialization
    void Start () {
        AddAllFog();
    }
	
	// Update is called once per frame
	void Update () {
        AllPlayerVision();
    }

    public void AddAllFog()
    {
        fogs = new Fog[hexGrid.size];
        for (int i = 0; i < hexGrid.size; i++) {
            bool checkFog = hexGrid.GetFog(i);
            if (checkFog == true) {
                Vector3 pos = hexGrid.GetCellPos(i);
                pos.y = 2;
                Fog fog = fogs[i] = Instantiate<Fog>(fogPrefab);
                fog.transform.localPosition = pos;
            }
        }
    }

    //check each player entity in entitystorage to determine their vision range and remove the fog for that range
    public void AllPlayerVision () {
        foreach (string pEntity in entityStorage.activePlayerEntities) {
            GameObject playerEntity = GameObject.Find(pEntity);
            string cleanpEntity = Regex.Replace(pEntity, @"[\d-]", string.Empty);
            int visionDistance = 0;
            if (cleanpEntity == "Zombie") {
                visionDistance = playerEntity.GetComponent<ZombieBehaviour>().vision;
            } else if (cleanpEntity == "Skeleton") {
                visionDistance = playerEntity.GetComponent<SkeletonBehaviour>().vision;
            } else if (cleanpEntity == "Necromancer") {
                visionDistance = playerEntity.GetComponent<NecromancerBehaviour>().vision;
            }
            Vector3 playerEntityPosition = playerEntity.transform.position;
            int index = hexGrid.GetCellIndex(playerEntityPosition);
            PlayerVisionHelper(index, visionDistance);
        }
    }

    //removes fog of war from an index given the vision distance
    public void PlayerVisionHelper (int index, int visionDistance) {
        if (visionDistance > 0) {
            HexCoordinates coord = hexGrid.GetCellCoord(index);
            int coordx = coord.X;
            int coordz = coord.Z;

            int left = hexGrid.GetCellIndexFromCoord(coordx - 1, coordz);
            int right = hexGrid.GetCellIndexFromCoord(coordx + 1, coordz);
            int uleft = hexGrid.GetCellIndexFromCoord(coordx - 1, coordz + 1);
            int uright = hexGrid.GetCellIndexFromCoord(coordx, coordz + 1);
            int lleft = hexGrid.GetCellIndexFromCoord(coordx, coordz - 1);
            int lright = hexGrid.GetCellIndexFromCoord(coordx + 1, coordz - 1);
            int[] hexdirections = new int[] { left, right, uleft, uright, lleft, lright };

            foreach (int direction in hexdirections) {
                if (direction >= 0 && direction < hexGrid.size) {
                    int newVisionDistance = visionDistance - 1;
                    hexGrid.SetFogOff(direction);
                    if (hexGrid.GetFog(direction) == false) {
                        Fog fog = fogs[index];
                        fog.GetComponent<Renderer>().enabled = false;
                    }
                    PlayerVisionHelper(direction, visionDistance - 1);
                }
            }
        }
    }
}
