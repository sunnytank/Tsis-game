using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PazzleWord : MonoBehaviour
{

    public TMP_InputField charecter1;
    public TMP_InputField charecter2;
    public TMP_InputField charecter3;
    public TMP_InputField charecter4;
    public TMP_InputField charecter5;

    public GameObject panelInputWord;
    public GameObject panelDetailWord; 
    public GameObject panelInputNumber; 
    public GameObject panelHint; 
    public GameObject ButtonToDetailword;
    public GameObject panalUIGame;

    public string correctWord;

    public bool setActive;

    string InputWord;

    bool hint;

    public GameObject player;
    bool ItCheck;
    bool PlayerGet;
    int Plushp;
    int Plusstamina;


    public void Checkcharecter()
    {

    }

    public void GotoDetailWord()
    {
        panelDetailWord.SetActive(true); // ปิดการใช้งาน Panel โดยกำหนดเป็น false
        panelInputWord.SetActive(false); // ปิดการใช้งาน Panel โดยกำหนดเป็น false
        panalUIGame.SetActive(false);
    }

    public void ExitPanel()
    {
        panelDetailWord.SetActive(false); // ปิดการใช้งาน Panel โดยกำหนดเป็น false
        panelInputWord.SetActive(false); // ปิดการใช้งาน Panel โดยกำหนดเป็น false
        panelInputNumber.SetActive(false);
        panalUIGame.SetActive(true);
    }

    public void ButtonHint()
    {
        if(hint == false)
        {
            panelHint.SetActive(true);
            hint = true;
        }
        else if (hint == true)
        {
            panelHint.SetActive(false);
            hint = false;
        }
    }




    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Plusstamina = 20;
        Plushp = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if(panalUIGame.activeSelf)
        {
            Time.timeScale = 1f;
        }
        if(!panalUIGame.activeSelf)
        {
            Time.timeScale = 0f;
        }
        
        if(panelInputWord.activeSelf)
        {
        InputWord = charecter1.text + charecter2.text + charecter3.text + charecter4.text + charecter5.text;

        if(InputWord.Equals(correctWord, StringComparison.CurrentCultureIgnoreCase))
        {
            if(charecter1.text.Length != 0 && charecter3.text.Length != 0 && charecter5.text.Length != 0)
            {
            ButtonToDetailword.SetActive(true);
            setActive = true;
            ItCheck = true;
            }
        }

        if(ItCheck)
        {
            if(!PlayerGet)
            {
                player.GetComponent<PlayerHealth>().MaxStaminaPlus(Plusstamina);
                player.GetComponent<PlayerHealth>().MaxHpPlus(Plushp);
                PlayerGet = true;
            }
        }
        
        if(panelInputWord.activeSelf || panelInputNumber.activeSelf || panelDetailWord.activeSelf)
        {
            panalUIGame.SetActive(false);
        }
        else
        {
            panalUIGame.SetActive(true);
        }

        if(charecter1.text.Length == 0)
        {
            charecter1.Select();
        }
        else if(charecter2.text.Length == 0)
        {
            charecter2.Select();
        }
        else if(charecter3.text.Length == 0)
        {
            charecter3.Select();
        }
        else if(charecter4.text.Length == 0)
        {
            charecter4.Select();
        }
        else if(charecter5.text.Length == 0)
        {
            charecter5.Select();
        }
        }
    }

        public void ClearText()
    {
        if(!ButtonToDetailword.activeSelf)
        {
            charecter1.text = "";
            charecter2.text = "";
            charecter3.text = "";
            charecter4.text = "";
            charecter5.text = "";
        }
        else if(ButtonToDetailword.activeSelf)
        {
            ButtonToDetailword.SetActive(false);
        }
    }
}
