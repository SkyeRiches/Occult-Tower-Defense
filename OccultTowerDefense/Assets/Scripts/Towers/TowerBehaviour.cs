using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    public enum TOWER_STATE
    {
        PLACED,
        UNPLACED
    }

    public enum TOWER_TYPE
    {
        FIRE,
        HEAL
    }

    public TOWER_STATE state = TOWER_STATE.UNPLACED;
    public TOWER_TYPE type = TOWER_TYPE.FIRE;

    public float placementRadius = 3f;

    public void PlaceTower()
    {
        state = TOWER_STATE.PLACED;

        if (type == TOWER_TYPE.FIRE)
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
        }
        else if (type == TOWER_TYPE.HEAL)
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }
    }

    public void EmpowerTower()
    {
        // increase the tower stats 
    }
}
