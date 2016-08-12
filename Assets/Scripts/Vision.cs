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
                pos.y = 0.1f;
                Fog fog = fogs[i] = Instantiate<Fog>(fogPrefab);
                fog.transform.SetParent(transform, false);
                fog.transform.localPosition = pos;
            }
        }
    }

    //check each player entity in entitystorage to determine their vision range and remove the fog for that range
    public void AllPlayerVision () {
        foreach (GameObject playerEntity in entityStorage.activePlayerEntities) {
            string cleanPlayerEntity = Regex.Replace(playerEntity.name, @"[\d-]", string.Empty);
            int visionDistance = 0;
            switch (cleanPlayerEntity)
            {
                case "Zombie":
                    visionDistance = playerEntity.GetComponent<ZombieBehaviour>().vision;
                    break;
                case "Skeleton":
                    visionDistance = playerEntity.GetComponent<SkeletonBehaviour>().vision;
                    break;
                case "Necromancer":
                    visionDistance = playerEntity.GetComponent<NecromancerBehaviour>().vision;
                    break;
                case "SkeletonArcher":
                    visionDistance = playerEntity.GetComponent<SkeletonArcherBehaviour>().vision;
                    break;
                case "ArmoredSkeleton":
                    visionDistance = playerEntity.GetComponent<ArmoredSkeletonBehaviour>().vision;
                    break;
                case "DeathKnight":
                    visionDistance = playerEntity.GetComponent<DeathKnightBehaviour>().vision;
                    break;
            }

            //TODO Future update for human class
            //else if (cleanpEntity == "Militia") {
            //    visionDistance = playerEntity.GetComponent<MilitiaBehaviour>().vision;
            //} else if (cleanpEntity == "Archer") {
            //    visionDistance = playerEntity.GetComponent<ArcherBehaviour>().vision;
            //} else if (cleanpEntity == "Longbowman") {
            //    visionDistance = playerEntity.GetComponent<LongbowmanBehaviour>().vision;
            //}
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
