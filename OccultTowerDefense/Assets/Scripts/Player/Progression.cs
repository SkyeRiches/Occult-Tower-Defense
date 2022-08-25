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

    private void Update()
    {
        if (iKillTracker == iLevelReq && iCurrentLevel < 11)
        {
            LevelUP();
        }
    }

    private void LevelUP()
    {
        iCurrentLevel++;
        // increase player hp by 10
        // increase damage dealt by x0.2
        gameObject.GetComponent<PlayerMovement>().ReduceFireCooldown(0.1f);
        iLevelReq += 100;
    }
}
