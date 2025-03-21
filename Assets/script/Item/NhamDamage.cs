using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NhamDamage : MonoBehaviour
{
    public GameObject player;
    bool playerin;
    int attackDamage;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        attackDamage = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerin)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ตรวจสอบว่าชนกับไอเท็มหรือไม่
        {
            playerin = true;
        }
    }
}
