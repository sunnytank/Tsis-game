using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatSCMap2 : MonoBehaviour
{
    public GameObject Chat1;
    public GameObject Chat2;
    public GameObject ChatPanal;
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
        if(TimeSet <= 4)
        {
            Chat1.SetActive(true);
            Chat2.SetActive(false);
        }
        if(TimeSet >= 4)
        {
            Chat1.SetActive(false);
            Chat2.SetActive(true);
        }
        if(TimeSet >= 8)
        {
            Chat1.SetActive(false);
            Chat2.SetActive(false);
            ChatPanal.SetActive(false);
        }

    }
}
