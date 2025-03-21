using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class RelordScene : MonoBehaviour
{
    [Header("SceneManager")]
    public int SceneBuildIndex;
    public int MainMenuIndex;
    public int ScenePlayer;
    public int CreditScreenX;
    // Start is called before the first frame update
    public void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        Debug.Log("LoadScene");
    }

    public void QuitGame()
    {
        // ใช้คำสั่ง Application.Quit() เพื่อปิดเกม
        Application.Quit();
    }

    public void Newgame()
    {
    string path = Application.persistentDataPath + "/Player.CheckPoint";
    
    if (File.Exists(path))
    {
        File.Delete(path);
        SceneManager.LoadScene(SceneBuildIndex);
    }
    else if (!File.Exists(path))
    {
        SceneManager.LoadScene(SceneBuildIndex);
    }

    }

    public void ButtonLoadSceneSave()
    {
        SceneManager.LoadScene(ScenePlayer);
        Debug.Log("LoadSave");
    }

    public void LoadPlayer()
    {
        PlayerDataAlive data = SaveSystem.LoadPlayer();

        ScenePlayer = data.ScenePlayer;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(MainMenuIndex);
        Debug.Log("LoadMenu");
    }

    public void Update()
    {
        if(File.Exists(Application.persistentDataPath + "/Player.CheckPoint"))
        {   
            LoadPlayer();
            Debug.Log("ไฟล์มีอยู่จริง");
        }
        else if (!File.Exists(Application.persistentDataPath + "/Player.CheckPoint"))
        {
            Debug.Log("ไฟล์ไม่มีอยู่จริง");
        }
    }

    public void CreditScreen()
    {
        SceneManager.LoadScene(CreditScreenX);
    }
    
}
