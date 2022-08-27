using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winMessage;
    [SerializeField] private GameObject failMessage;

    public void FailGame()
    {
        winMessage.SetActive(true);
        StartCoroutine(LoadMenu());
    }

    public void WinGame()
    {
        failMessage.SetActive(true);
        StartCoroutine(LoadMenu());
    }

    private IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }
}