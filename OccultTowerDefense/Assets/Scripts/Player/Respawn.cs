using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform respawnPoint;

    public void BeginRespawn()
    {
        StartCoroutine(RespawnPlayer());
    }

    private IEnumerator RespawnPlayer()
    {
        player.SetActive(false);
        yield return new WaitForSeconds(10);
        player.SetActive(true);
        player.transform.position = respawnPoint.position;
        player.GetComponent<HealthScript>().HealEntityToMax();
    }
}
