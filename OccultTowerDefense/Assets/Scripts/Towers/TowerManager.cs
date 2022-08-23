using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public List<GameObject> towers;
    public List<GameObject> powerPools;

    private void Awake()
    {
        towers = new List<GameObject>();
        powerPools = new List<GameObject>();
    }
}
