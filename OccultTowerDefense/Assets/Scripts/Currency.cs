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
        UIChangeInt change = GameObject.FindGameObjectsWithTag("SoulText")[0].GetComponent<UIChangeInt>();
        if (change != null) {
	        change.ChangeInt(Mathf.RoundToInt(fNumSouls));
        }
    }

    public void IncreaseSouls(float amount)
    {
        fNumSouls += amount;
        //Debug.Log("Souls: " + fNumSouls);
        // Update GUI
        UIChangeInt change = GameObject.FindGameObjectsWithTag("SoulText")[0].GetComponent<UIChangeInt>();
        if (change != null) {
	        change.ChangeInt(Mathf.RoundToInt(fNumSouls));
        }
    }

    public void DecreaseSouls(float amount)
    {
        fNumSouls -= amount;
        //Debug.Log("Souls: " + fNumSouls);
        UIChangeInt change = GameObject.FindGameObjectsWithTag("SoulText")[0].GetComponent<UIChangeInt>();
        if (change != null) {
	        change.ChangeInt(Mathf.RoundToInt(fNumSouls));
        }
    }
}
