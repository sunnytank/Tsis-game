using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealStation : MonoBehaviour
{
    public GameObject Item;
    bool playerin;
    public GameObject player;
    public int Heal;
    public int Shield;
    bool Use;
    // Start is called before the first frame update

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        Heal = 80;
        Shield = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(UserInput.instance.controls.Interact.interact.WasPressedThisFrame() && playerin && !Use)
        {
            UseStation();
        }

        if(playerin)
        {
            Item.SetActive(true);
        }
        else
        {
            Item.SetActive(false);
        }
    }

    void UseStation()
    {
        if(!Use)
        {
        player.GetComponent<PlayerHealth>().Heal(Heal);
        player.GetComponent<PlayerHealth>().ShieldPlusPlus(Shield);
        Use = true;
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
