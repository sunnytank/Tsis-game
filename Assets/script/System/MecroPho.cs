using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MecroPho : MonoBehaviour
{
    public GameObject player;
    public GameObject Chat;
    public GameObject Item;

    public bool playerin;

    public float TimeDelay;

    public bool canOpen;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canOpen && TimeDelay < 9f)
        {
            Chat.SetActive(true);
            TimeDelay += Time.deltaTime;
        }

        if(TimeDelay >= 9f)
        {
            Chat.SetActive(false);
        }

        if(playerin && !canOpen)
        {
            Item.SetActive(true);
        }
        else
        {
            Item.SetActive(false);
        }

        if(UserInput.instance.controls.Interact.interact.WasPressedThisFrame() && playerin && !canOpen)
        {
            canOpen = true;
        }
        
    }

        void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ตรวจสอบว่าชนกับไอเท็มหรือไม่
        {
            playerin = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ตรวจสอบว่าผู้เล่นออกจากพื้นที่สำหรับไอเท็มหรือไม่
        {
            playerin = false;
        }
    }
}
