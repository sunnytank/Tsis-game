using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDieDoorOpen : MonoBehaviour
{
    public BossHealth bosshealth;
    public float HPBOSS;
    public GameObject Door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HPBOSS = bosshealth.currentHealth;

        if(HPBOSS <= 0)
        {
            Door.SetActive(true);
        }
        else
        {
            Door.SetActive(false);
        }
        
    }
}
