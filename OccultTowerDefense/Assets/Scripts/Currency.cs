using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour
{
    private float fNumSouls = 0f;
    [SerializeField] private float startingSouls = 0f;

    public float GetNumSouls()
    {
        return fNumSouls;
    }

    private void Start()
    {
        fNumSouls = startingSouls;
    }

    public void IncreaseSouls(float amount)
    {
        fNumSouls += amount;
        Debug.Log("Souls: " + fNumSouls);
        // Update GUI
    }

    public void DecreaseSouls(float amount)
    {
        fNumSouls -= amount;
        Debug.Log("Souls: " + fNumSouls);
    }
}
