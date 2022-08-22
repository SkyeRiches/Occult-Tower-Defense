using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    [SerializeField] private float soulWorth = 1f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject.FindGameObjectsWithTag("Managers")[0].GetComponent<Currency>().IncreaseSouls(soulWorth);
            Destroy(gameObject);

        }
    }
}
