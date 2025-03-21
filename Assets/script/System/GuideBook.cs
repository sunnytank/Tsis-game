using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GuideBook : MonoBehaviour
{
    public GameObject PanalItem;
    public GameObject PanalMonster;
    public GameObject PanalBoss;
    public GameObject PanalControl;
    public GameObject GuideBookUi;
    public GameObject PauseGameUi;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
        public void ItemPanal()
    {
        PanalItem.SetActive(true);
        PanalMonster.SetActive(false);
        PanalBoss.SetActive(false);
        PanalControl.SetActive(false);
    }

        public void MonsterPanal()
    {
        PanalItem.SetActive(false);
        PanalMonster.SetActive(true);
        PanalBoss.SetActive(false);
        PanalControl.SetActive(false);
    }

        public void BossPanal()
    {
        PanalItem.SetActive(false);
        PanalMonster.SetActive(false);
        PanalBoss.SetActive(true);
        PanalControl.SetActive(false);
    }

        public void ControlPanal()
    {
        PanalItem.SetActive(false);
        PanalMonster.SetActive(false);
        PanalBoss.SetActive(false);
        PanalControl.SetActive(true);
    }

        public void BackButtonToPause()
    {
        GuideBookUi.SetActive(false);
        PauseGameUi.SetActive(true);
    }
}
