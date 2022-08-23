using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public List<GameObject> towers;

    private void Awake()
    {
        towers = new List<GameObject>();
    }
}
