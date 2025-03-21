using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PazzleNumber : MonoBehaviour
{

    public TextMeshProUGUI textCall;
    public TMP_InputField NumberSet;
    string inputText;

    int valueToCheck;
    public int expectedValue;
    int CheckNum;

    public GameObject player;
    bool ItCheck;
    bool PlayerGet;
    int PlusHP;
    int PlusHeal;
    int PlusShield;

    public void ButtonCheck()
    {
        //แปลงstringเป็นint
        string inputTextg = NumberSet.text;
        int.TryParse(inputTextg, out valueToCheck);

        CheckIntValue(valueToCheck , expectedValue );        
    }

    public void ButtonTopFunction()
    {
        Debug.Log("Button Clicked!");
    }

    public void ButtonDownFunction()
    {
        Debug.Log("Button Clicked!");
    }

    public void ReadStringInput(string inputText)
    {
        Debug.Log("Input Text");
    }

    public void CheckStringInput(string inputText)
        {
    // ตรวจสอบว่า inputText มีค่าและมีเฉพาะตัวเลขหรือไม่
            if (string.IsNullOrEmpty(inputText))
            {
                textCall.text = "Please input a number";
            }
    
        }

    public void CheckIntValue(int valueToCheck, int expectedValue)
    {
        CheckNum = valueToCheck - expectedValue;
        
        if(CheckNum == 0 )
        {
            textCall.text = "Number  " + valueToCheck + "\n      It Correct";
            ItCheck = true;
        }
        else if(CheckNum >= -10 && CheckNum < 0)
        {
            textCall.text = "Number  " + valueToCheck + "\n      It's close. Let's add a little more.";
        }
        else if(CheckNum <= 10 && CheckNum > 0)
        {
            textCall.text = "Number  " + valueToCheck + "\n      It's close. Try reducing it a little more.";
        }
        else if(CheckNum >= -50 && CheckNum < -10)
        {
            textCall.text = "Number  " + valueToCheck + "\n      Alright, try adding a little more.";
        }
        else if(CheckNum <= 50 && CheckNum > 10)
        {
            textCall.text = "Number  " + valueToCheck + "\n      Alright, try reducing it a little more.";
        }
        else if(CheckNum >= -100 && CheckNum < -50 || CheckNum <= 100 && CheckNum > 50)
        {
            textCall.text = "Number  " + valueToCheck + "\n      Looks like you're on the right track.";
        }
        else if(CheckNum < -100 || CheckNum > 100)
        {
            textCall.text = "Number  " + valueToCheck + "\n      Not even close, try again.";
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PlusHP = 20;
        PlusHeal = 20;
        PlusShield = 1;
    }



    // Update is called once per frame
    void Update()
    {
        if(ItCheck)
        {
            if(!PlayerGet)
            {
                player.GetComponent<PlayerHealth>().MaxHpPlus(PlusHP);
                player.GetComponent<PlayerHealth>().Heal(PlusHeal);
                player.GetComponent<PlayerHealth>().ShieldPlusPlus(PlusShield);
                PlayerGet = true;
            }
        }
        
    }




}
