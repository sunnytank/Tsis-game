using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PazzlePopUp : MonoBehaviour
{
    bool playerin = false;
    public GameObject Item;
    public GameObject PanalWord;

    public bool Active;
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        if(UserInput.instance.controls.Interact.interact.WasPressedThisFrame() && playerin == true)
        {
            PanalWord.SetActive(true);
        }

        if(playerin)
        {
            Item.SetActive(true);
            animator.SetBool("pass",true);
        }
        else
        {
            Item.SetActive(false);
            animator.SetBool("pass",false);
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

