using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChat2 : MonoBehaviour
{
    public GameObject player;
    public GameObject Item;

    public GameObject Chat1;

    public int randomNumber;

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

        if(canOpen && TimeDelay < 4f)
        {
            TimeDelay += Time.deltaTime;
            Chat1.SetActive(true);
        }

        if(TimeDelay >= 4f)
        {
            Chat1.SetActive(false);
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
