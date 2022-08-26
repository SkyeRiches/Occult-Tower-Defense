using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCircle : MonoBehaviour
{
    [SerializeField] private GameObject rangeIndicator;

    // Start is called before the first frame update
    void Start()
    {
        float range = gameObject.GetComponent<TowerBehaviour>().GetRange() * 2;
        rangeIndicator.transform.localScale = new Vector3(range, range, range);
    }
}
