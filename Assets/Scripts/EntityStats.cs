using UnityEngine;
using System.Collections;

public class EntityStats : MonoBehaviour {

    //grabs health info
    public int GetHealthInfo(string entity)
    {
        switch (entity)
        {
            case "Zombie":
                return 250;
            case "Skeleton":
                return 150;
            case "Necromancer":
                return 1000;
            case "SkeletonArcher":
                return 150;
            case "ArmoredSkeleton":
                return 250;
            case "DeathKnight":
                return 500;

            case "Militia":
                return 200;
            case "Archer":
                return 200;
            case "Longbowman":
                return 200;
            case "Crossbowman":
                return 250;
            case "Footman":
                return 300;
            case "MountedKnight":
                return 350;
            case "HeroKing":
                return 800;
        }
        return 0;
    }
}
