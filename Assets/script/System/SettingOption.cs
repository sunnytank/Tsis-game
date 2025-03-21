using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SettingOption : MonoBehaviour
{

    public GameObject OptionSetting;
    public GameObject SettingEscPanal;
    public GameObject GuideBook;
    public int MainMenuIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CloseSetting()
    {
        SettingEscPanal.SetActive(false);
    }

    public void OpenOptionSound()
    {
        OptionSetting.SetActive(true);
        SettingEscPanal.SetActive(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(MainMenuIndex);
        Debug.Log("LoadMenu");
    }

    public void GuideBookOpen()
    {
        GuideBook.SetActive(true);
        SettingEscPanal.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
