using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPool : MonoBehaviour
{
    public float placementRadius = 1f;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Managers").GetComponent<TowerManager>().powerPools.Add(gameObject);
    }
}
