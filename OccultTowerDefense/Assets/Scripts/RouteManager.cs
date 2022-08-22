using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteManager : MonoBehaviour
{
    public Vector2[] destinations;
    public static Vector2[] destinationsForEnemies;

    void Awake() {
        destinationsForEnemies = destinations;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
