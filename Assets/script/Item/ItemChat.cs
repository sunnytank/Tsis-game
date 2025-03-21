using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChat : MonoBehaviour
{
    public GameObject player;
    public GameObject Item;

    public GameObject Chat1;
    public GameObject Chat2;
    public GameObject Chat3;
    public GameObject Chat4;

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

            if(randomNumber == 1 || randomNumber == 5)
            {
            Chat1.SetActive(true);
            }
            else if(randomNumber == 2 || randomNumber == 6)
            {
            Chat2.SetActive(true);
            }
            else if(randomNumber == 3 || randomNumber == 7)
            {
            Chat3.SetActive(true);
            }
            else if(randomNumber == 4 || randomNumber == 8)
            {
            Chat4.SetActive(true);
            }

        }

        if(TimeDelay >= 4f)
        {
            Chat1.SetActive(false);
            Chat2.SetActive(false);
            Chat3.SetActive(false);
            Chat4.SetActive(false);
        }

        if(playerin && !canOpen)
        {
            randomNumber = Random.Range(1, 9);
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
