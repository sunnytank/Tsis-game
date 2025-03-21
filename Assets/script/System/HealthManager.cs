using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float HealthCharecter;
    public float MaxHealthCharecter;

    public Image staminaBar;
    public float StaminaCharecter;
    public float MaxStaminaCharecter;

    public float MaxShield;
    public float ShieldCharecter;
    public TextMeshProUGUI textShield;

    public float SoulAmountUi;
    public TextMeshProUGUI textSoul;

    public float RedPotionCurrent;
    public TextMeshProUGUI textRedPo;

    public bool AuraBlade;
    public VideoPlayer playerCooldown;

    public PlayerHealth PlayerHealth; 

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        HealthCharecter = PlayerHealth.currentHealthPlayer;
        MaxHealthCharecter = PlayerHealth.maxHealthPlayer;
        StaminaCharecter = PlayerHealth.currentStamina;
        MaxStaminaCharecter = PlayerHealth.maxStamina;
        MaxShield = PlayerHealth.maxShield;
        ShieldCharecter = PlayerHealth.currentShield;
        RedPotionCurrent = PlayerHealth.PotionRed;
        AuraBlade = PlayerHealth.AuraBladeCoolDownCountDown;

        SoulAmountUi = PlayerHealth.SoulAmount;

        healthBar.fillAmount = HealthCharecter / MaxHealthCharecter;

        staminaBar.fillAmount = StaminaCharecter / MaxStaminaCharecter;

        textSoul.SetText(SoulAmountUi.ToString());

        textShield.SetText(ShieldCharecter.ToString());


        textRedPo .SetText(RedPotionCurrent.ToString());

        Cooldown();
    }

    public void Cooldown()
    {
        if(AuraBlade)
        {
            playerCooldown.Play();
        }
    }
    
}
