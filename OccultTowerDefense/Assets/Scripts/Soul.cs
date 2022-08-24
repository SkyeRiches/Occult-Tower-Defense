using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    [SerializeField] private float soulWorth = 1f;
    private GameObject player;

    private float distance;
    [SerializeField] private float speed;
    [SerializeField] private float attractDistance;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject.FindGameObjectsWithTag("Managers")[0].GetComponent<Currency>().IncreaseSouls(soulWorth);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        distance = Vector2.Distance(player.transform.position, transform.position);
        
        if (distance <= attractDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, (speed / distance) / 100); // magic number at the end to slow it down
        }
    }
}
