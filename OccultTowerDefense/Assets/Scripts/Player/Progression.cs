using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progression : MonoBehaviour
{
    private int iKillTracker = 0;
    private int iCurrentLevel = 1;
    private int iLevelReq = 100;

    public void IncreaseKills(int amount)
    {
        iKillTracker += amount;
    }

    // Start is called before the first frame update
    void Start()
    {
        iCurrentLevel = 1;
        iKillTracker = 0;
    }


}
