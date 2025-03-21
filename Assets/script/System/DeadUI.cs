using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class DeadUI : MonoBehaviour
{
    public int MainMenuIndex;
    public int GotoStartIndex;
    
    public void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        Debug.Log("LoadScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(MainMenuIndex);
        Debug.Log("LoadMenu");
    }

    public void gotoStart()
    {
        SceneManager.LoadScene(GotoStartIndex);
    }

}
