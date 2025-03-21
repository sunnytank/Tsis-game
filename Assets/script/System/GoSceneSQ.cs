using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GoSceneSQ : MonoBehaviour
{
    public int GotoScene;
    public float TimeToGo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("GotoPage", TimeToGo);
        
    }

    public void GotoPage()
    {
        SceneManager.LoadScene(GotoScene);
    }

}