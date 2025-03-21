using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StartMain : MonoBehaviour
{
    public int MainMenuIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            Invoke("MainClickDelay", 0.5f);
            Debug.Log("LoadMenu");
        }
        
    }

    public void GoToMainMenu()
    {
        Invoke("MainClickDelay", 0.5f);
        Debug.Log("LoadMenu");
    }

    public void MainClickDelay()
    {
        SceneManager.LoadScene(MainMenuIndex);
    }

}
