using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BeforeEnding : MonoBehaviour
{
    public GameObject Vid;
    public GameObject Chat;

    public float TimeSet;
    public float forSet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Chat.activeSelf)
        {
            TimeSet += Time.deltaTime;
        }

        if(TimeSet >= forSet)
        {
            Vid.SetActive(true);
        }

        if(Vid.activeSelf)
        {
            Chat.SetActive(false);
        }
    }
}
