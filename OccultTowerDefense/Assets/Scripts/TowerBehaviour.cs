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

    public TOWER_STATE state = TOWER_STATE.UNPLACED;

    public float placementRadius = 3f;

    public void PlaceTower()
    {
        state = TOWER_STATE.PLACED;
    }
}
