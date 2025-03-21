using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeartBarBoss : MonoBehaviour
{
    public Image healthBarBoss;
    public float HealthBoss;
    public float MaxHealthBoss;

    public BossHealth BossHealth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HealthBoss = BossHealth.currentHealth;
        MaxHealthBoss = BossHealth.maxHealth;

        healthBarBoss.fillAmount = HealthBoss / MaxHealthBoss;

    }
}
