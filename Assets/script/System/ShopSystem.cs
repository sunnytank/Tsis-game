using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    public TextMeshProUGUI SoulAmountUI;
    public GameObject textError;
    public float SoulAmount;
    public float ItemHeal;
    public float ItemShield;

    public PlayerHealth PlayerHealth;
    public GameObject player;

    public GameObject PanalItem1;
    public GameObject PanalItem2;
    public GameObject PanalItem3;
    public GameObject PanalItem4;
    public GameObject PanalItem5;
    public GameObject Shop;
    public GameObject Ui;
    public GameObject PanalBuyCheck;
    public GameObject Item1;
    public GameObject Item2;
    public GameObject Item3;
    public GameObject Item4;
    public GameObject Item5;

    int PlusHp;
    int PlusStamina;
    int PlusShield;
    int RedPotionAmount;
    int Shield;

    int Soulitem1;
    int Soulitem2;
    int Soulitem3;
    int Soulitem4;
    int Soulitem5;

    bool CheckBuyItem;

    public float HealthCharecter;
    public float MaxHealthCharecter;
    public TextMeshProUGUI textHP;

    public float StaminaCharecter;
    public float MaxStaminaCharecter;
    public TextMeshProUGUI textSTA;

    public float MaxShield;
    public float ShieldCharecter;
    public TextMeshProUGUI textAR;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
        PlusHp = 20;
        PlusStamina =10;
        PlusShield = 2;
        RedPotionAmount = 1;
        Shield = 1;

        Soulitem1 = 350;
        Soulitem2 = 300;
        Soulitem3 = 300;
        Soulitem4 = 80;
        Soulitem5 = 125;
        
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

        textHP.SetText(HealthCharecter.ToString() + " / " + MaxHealthCharecter.ToString());
        textSTA.SetText(StaminaCharecter.ToString() + " / " + MaxStaminaCharecter.ToString());
        textAR.SetText(ShieldCharecter.ToString() + " / " + MaxShield.ToString());


        SoulAmount = PlayerHealth.SoulAmount;
        SoulAmountUI.text = SoulAmount.ToString();

        ClosePanalError();
        
    }

    public void PanalItem_1()
    {
        PanalItem1.SetActive(true);
        PanalItem2.SetActive(false);
        PanalItem3.SetActive(false);
        PanalItem4.SetActive(false);
        PanalItem5.SetActive(false);
    }

    public void PanalItem_2()
    {
        PanalItem1.SetActive(false);
        PanalItem2.SetActive(true);
        PanalItem3.SetActive(false);
        PanalItem4.SetActive(false);
        PanalItem5.SetActive(false);
    }

    public void PanalItem_3()
    {
        PanalItem1.SetActive(false);
        PanalItem2.SetActive(false);
        PanalItem3.SetActive(true);
        PanalItem4.SetActive(false);
        PanalItem5.SetActive(false);
    }

    public void PanalItem_4()
    {
        PanalItem1.SetActive(false);
        PanalItem2.SetActive(false);
        PanalItem3.SetActive(false);
        PanalItem4.SetActive(true);
        PanalItem5.SetActive(false);
    }

    public void PanalItem_5()
    {
        PanalItem1.SetActive(false);
        PanalItem2.SetActive(false);
        PanalItem3.SetActive(false);
        PanalItem4.SetActive(false);
        PanalItem5.SetActive(true);
    }

    public void CloseShop()
    {
        Shop.SetActive(false);
        Ui.SetActive(true);
    }

    public void ClosePanalError()
    {
        if(textError.activeSelf)
        {
            Invoke("CloseInvoke",2f);
        }
    }
    public void CloseInvoke()
    {
        textError.SetActive(false);
    }

    public void selectItemBuy()
    {
        Debug.Log("dawdsa");
            if(PanalItem1.activeSelf)
            {   
                PanalBuyCheck.SetActive(true);
                Item1.SetActive(true);
                Item2.SetActive(false);
                Item3.SetActive(false);
                Item4.SetActive(false);
                Item5.SetActive(false);
            }
            else if(PanalItem2.activeSelf)
            {
                PanalBuyCheck.SetActive(true);
                Item1.SetActive(false);
                Item2.SetActive(true);
                Item3.SetActive(false);
                Item4.SetActive(false);
                Item5.SetActive(false);
            }
            else if(PanalItem3.activeSelf)
            {
                PanalBuyCheck.SetActive(true);
                Item1.SetActive(false);
                Item2.SetActive(false);
                Item3.SetActive(true);
                Item4.SetActive(false);
                Item5.SetActive(false);
            }
            else if(PanalItem4.activeSelf)
            {
                PanalBuyCheck.SetActive(true);
                Item1.SetActive(false);
                Item2.SetActive(false);
                Item3.SetActive(false);
                Item4.SetActive(true);
                Item5.SetActive(false);
            }
            else if(PanalItem5.activeSelf)
            {
                PanalBuyCheck.SetActive(true);
                Item1.SetActive(false);
                Item2.SetActive(false);
                Item3.SetActive(false);
                Item4.SetActive(false);
                Item5.SetActive(true);
            }
    }


    public void Yes()
    {
            if(Item1.activeSelf)
            {
                if(Soulitem1 < SoulAmount)
                {
                    player.GetComponent<PlayerHealth>().BuyItemSoul(Soulitem1);
                    player.GetComponent<PlayerHealth>().MaxHpPlus(PlusHp);
                    Debug.Log("PanalItem1.active");
                    PanalBuyCheck.SetActive(false);
                }
                else
                {
                    textError.SetActive(true);
                }
                
            }
            else if(Item2.activeSelf)
            {
                if(Soulitem2 < SoulAmount)
                {
                    player.GetComponent<PlayerHealth>().BuyItemSoul(Soulitem2);
                    player.GetComponent<PlayerHealth>().MaxStaminaPlus(PlusStamina);
                    Debug.Log("PanalItem2.active");
                    PanalBuyCheck.SetActive(false);
                }
                else
                {
                    textError.SetActive(true);
                }
            }
            else if(Item3.activeSelf)
            {
                if(Soulitem3 < SoulAmount)
                {
                    player.GetComponent<PlayerHealth>().BuyItemSoul(Soulitem3);
                    player.GetComponent<PlayerHealth>().MaxShieldPlus(PlusShield);
                    Debug.Log("PanalItem3.active");
                    PanalBuyCheck.SetActive(false);
                }
                else
                {
                    textError.SetActive(true);
                }
            }
            else if(Item4.activeSelf)
            {
                if(Soulitem4 < SoulAmount)
                {
                    player.GetComponent<PlayerHealth>().GetRedPotion(RedPotionAmount);
                    player.GetComponent<PlayerHealth>().BuyItemSoul(Soulitem4);
                    Debug.Log("PanalItem4.active");
                    PanalBuyCheck.SetActive(false);
                }
                else
                {
                    textError.SetActive(true);
                }
            }
            else if(Item5.activeSelf)
            {
                if(Soulitem5 < SoulAmount)
                {
                    player.GetComponent<PlayerHealth>().ShieldPlusPlus(Shield);
                    player.GetComponent<PlayerHealth>().BuyItemSoul(Soulitem5);
                    Debug.Log("PanalItem5.active");
                    PanalBuyCheck.SetActive(false);
                }
                else
                {
                    textError.SetActive(true);
                }
            }
        }

        public void No()
        {
            PanalBuyCheck.SetActive(false);
            textError.SetActive(false);
        }
    }