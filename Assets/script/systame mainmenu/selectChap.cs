using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class selectChap : MonoBehaviour
{
    [Header("SceneManager")]
    public int Chap1_1;
    public int Chap1_2;
    public int Chap1_3;
    public int Chap2_1;
    public int Chap2_2;
    public int Chap2_3;
    public int Chap3_1;
    public int Chap3_2;
    public int Chap3_3;
    public int Chap3_4;
    
    [Header("GameObject")]
    public GameObject Chp1;
    public GameObject Chp2;
    public GameObject Chp3;
    public GameObject PanelSelectChap;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GotoChap1()
    {
        Chp1.SetActive(true);
        Chp2.SetActive(false);
        Chp3.SetActive(false);
    }

    public void GotoChap2()
    {
        Chp1.SetActive(false);
        Chp2.SetActive(true);
        Chp3.SetActive(false);
    }

    public void GotoChap3()
    {
        Chp1.SetActive(false);
        Chp2.SetActive(false);
        Chp3.SetActive(true);
    }

    public void ExitPanal()
    {
        PanelSelectChap.SetActive(false);
        Chp1.SetActive(false);
        Chp2.SetActive(false);
        Chp3.SetActive(false);
    }

    public void GotoSelectChap()
    {
        PanelSelectChap.SetActive(true);
        Chp1.SetActive(true);
        Chp2.SetActive(false);
        Chp3.SetActive(false);
    }

    public void GotoSt1_1()
    {
        SceneManager.LoadScene(Chap1_1);
        Debug.Log("LoadScene(Chap1_1)");
    }

    public void GotoSt1_2()
    {
        SceneManager.LoadScene(Chap1_2);
        Debug.Log("LoadScene(Chap1_3");
    }

    public void GotoSt1_3()
    {
        SceneManager.LoadScene(Chap1_3);
        Debug.Log("LoadScene(Chap1_3");
    }

    public void GotoSt2_1()
    {
        SceneManager.LoadScene(Chap2_1);
        Debug.Log("LoadScene(Chap2_1");
    }

    public void GotoSt2_2()
    {
        SceneManager.LoadScene(Chap2_2);
        Debug.Log("LoadScene(Chap2_2");
    }

    public void GotoSt2_3()
    {
        SceneManager.LoadScene(Chap2_3);
        Debug.Log("LoadScene(Chap2_3");
    }

    public void GotoSt3_1()
    {
        SceneManager.LoadScene(Chap3_1);
        Debug.Log("LoadScene(Chap3_1");
    }

    public void GotoSt3_2()
    {
        SceneManager.LoadScene(Chap3_2);
        Debug.Log("LoadScene(Chap3_2");
    }

    public void GotoSt3_3()
    {
        SceneManager.LoadScene(Chap3_3);
        Debug.Log("LoadScene(Chap3_3");
    }

    public void GotoSt3_4()
    {
        SceneManager.LoadScene(Chap3_4);
        Debug.Log("LoadScene(Chap3_4");
    }
    


}
