using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatSCMap3 : MonoBehaviour
{
    public GameObject Chat1;
    public GameObject Chat2;
    public GameObject Chat3;
    public GameObject Chat4;
    public GameObject Chat5;
    public GameObject Chat6;
    public GameObject Chat7;
    public GameObject Chat8;
    public GameObject Chat9;
    public float TimeSet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeSet += Time.deltaTime;
        Time.timeScale = 1f;
        if(TimeSet < 3)
        {
            Chat1.SetActive(true);
            Chat2.SetActive(false);
            Chat3.SetActive(false);
            Chat4.SetActive(false);
            Chat5.SetActive(false);
            Chat6.SetActive(false);
            Chat7.SetActive(false);
            Chat8.SetActive(false);
            Chat9.SetActive(false);
        }
        else if(TimeSet >= 3 && TimeSet < 6)
        {
            Chat1.SetActive(false);
            Chat2.SetActive(true);
        }
        else if(TimeSet >= 6 && TimeSet < 9)
        {   
            Chat2.SetActive(false);
            Chat3.SetActive(true);
        }
        else if(TimeSet >= 9 && TimeSet < 12)
        {
            Chat3.SetActive(false);
            Chat4.SetActive(true);
        }
        else if(TimeSet >= 12 && TimeSet < 15)
        {
            Chat4.SetActive(false);
            Chat5.SetActive(true);
        }
        else if(TimeSet >= 15 && TimeSet < 18)
        {
            Chat5.SetActive(false);
            Chat6.SetActive(true);
        }
        else if(TimeSet >= 18 && TimeSet < 21)
        {
            Chat6.SetActive(false);
            Chat7.SetActive(true);
        }
        else if(TimeSet >= 21 && TimeSet < 24)
        {
            Chat7.SetActive(false);
            Chat8.SetActive(true);
        }
        else if(TimeSet >= 24 && TimeSet < 27)
        {
            Chat8.SetActive(false);
            Chat9.SetActive(true);
        }
        else if(TimeSet >= 27)
        {
            Chat1.SetActive(false);
            Chat2.SetActive(false);
            Chat3.SetActive(false);
            Chat4.SetActive(false);
            Chat5.SetActive(false);
            Chat6.SetActive(false);
            Chat7.SetActive(false);
            Chat8.SetActive(false);
            Chat9.SetActive(false);
        }
    }
}
