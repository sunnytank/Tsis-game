using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    bool InZone;
    public GameObject PanalShop;
    public GameObject PopUp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(UserInput.instance.controls.Interact.interact.WasPressedThisFrame() && InZone)
        {
            PanalShop.SetActive(true);
        }

        if(InZone)
        {
            PopUp.SetActive(true);
        }
        else
        {
            PopUp.SetActive(false);
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InZone = true;
        }
    } 
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InZone = false;
        }
    } 
}
