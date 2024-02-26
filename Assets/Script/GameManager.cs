using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject restartbut;
    public GameObject tapTorestart;
    public GameObject High;
    public GameObject Image;

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void StartGame()
    {
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Fnish() 
    {
        restartbut.SetActive(true);
        tapTorestart.SetActive(true);
        High.SetActive(true);
        Image.SetActive(true);
        //Over.Play();
    }
}
