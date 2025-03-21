using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("InfoDistance")]
    public GameObject Monsters;
    public GameObject Boss;
    public float distanceToMonster;
    public float closestDistance;
    public float monsterDistance;
    public float Distance;
    public Rigidbody2D rb2d;
    public float knockbackForce = 5f;
    public Vector2 direction;
    public Renderer objectRenderer;
    public Color flashColor = Color.red;
    private Color originalColor;
    public GameObject DeadUiInfo;

    [Header("Potion")]
    public float PotionRed;
    public float Key;
    public int PotionHealAmount;
    public int GetPotion;

    [Header("Health")]
    public float maxHealthPlayer;
    public float currentHealthPlayer;
    public float maxShield;
    public float currentShield;
    public int attackDamage;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float DIES;
    public bool BloackActive;
    [SerializeField]
    int StaminaReduceBolck;
    public bool attacked;
    float attackedTime;
    public float previousHP;
    public float TimeCheckHP;

    [Header("Stamina")]
    public float maxStamina; // พลังงานสูงสุด
    public float currentStamina; // พลังงานปัจจุบัน
    public float staminaRecoveryRate = 20f; // อัตราการฟื้นฟูพลังงานต่อวินาที
    float StaminaBloackReset;

    [Header("Soul")]
    public float SoulAmount;
    public float SoulMax = 100000;
    public float soulDieDrop;

    public bool Dieactive;
    public bool lordSave;
    public bool PlayerInCheckPoint;

    public GameObject SoulExpDrop = null; // กำหนด Prefab ที่ต้องการสร้าง
    public Vector3 positionDropSoul;
    public bool DropSoul;
    public Vector3 positionDrop;
    bool PlayerInNextScene;

    [Header("Dash")]
    public bool CanDash;

    [Header("Effect")]
    public Transform SpawnEffectOnMC;
    public GameObject EffectHeal;
    public GameObject EfBlock;

    [Header("SceneManager")]
    public int LoadSceneSave;
    public GameObject SettingEsc;

    public Playermovement Playermovement;

    [Header("Sound")]
    public AudioSource audioSrc;
    public AudioClip BlockSound;

    public bool AuraBladeCoolDownCountDown;

    // Start is called before the first frame update
    void Start()
    {
        objectRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        Playermovement =  GetComponent<Playermovement>();
        originalColor = objectRenderer.material.color;

        BlockSound = Resources.Load<AudioClip>("mcBlockSword");

        PotionHealAmount = 80;

        if(File.Exists(Application.persistentDataPath + "/Player.CheckPoint"))
        {   
            LoadPlayerCheckpoint();
            Debug.Log("ไฟล์มีอยู่จริง");
        }
        else if (!File.Exists(Application.persistentDataPath + "/Player.CheckPoint"))
        {
            Debug.Log("ไฟล์ไม่มีอยู่จริง");
            maxHealthPlayer = 80;
            currentHealthPlayer = maxHealthPlayer / 2;
            maxStamina = 80;
            maxShield = 3;
            currentShield = 3;
            PotionRed = 0;
            Key = 0;
        }
        currentStamina = maxStamina;
    }

    private void Update()
    {   
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        LoadSceneSave = currentSceneIndex + 1 ;

        Dieactive = Playermovement.DieActive;
        BloackActive = Playermovement.isBlock; // ใช้งาน isBlock จาก Playermovement หลังจากตรวจสอบแล้วว่าไม่ใช่ null
        CanDash = Playermovement.canDash;
        AuraBladeCoolDownCountDown = Playermovement.auraBlade;

        float PlayerX = transform.position.x;
        currentHealthPlayer = Mathf.Clamp(currentHealthPlayer, 0f, maxHealthPlayer);
        currentShield = Mathf.Clamp(currentShield, 0f, maxShield);

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("monsters");

        closestDistance = Mathf.Infinity;
        Distance = PlayerX - monsterDistance;

        foreach (GameObject monster in monsters)
        {
            distanceToMonster = Vector3.Distance(transform.position, monster.transform.position);

            if (distanceToMonster < closestDistance)
            {
                monsterDistance = monster.transform.position.x;
                closestDistance = distanceToMonster;
            }
        }

        UsePotion1();
        NextScene();
        knockBack();
        OpenEsc();

        if(currentHealthPlayer <= 0)
        {
            Invoke("DeadUIDelay" , 2f);
        }

        if(attacked == true)
        {
            attackedTime = Time.deltaTime;
            if(attackedTime <= 0.4f)
            {
                attacked = false;
            }
        }

        BloackActive = Playermovement.isBlock;

        if(BloackActive)
        {
            StaminaBloackReset = 0;
        }
        else if (!BloackActive)
        {
            StaminaBloackReset += Time.deltaTime;
        }

        if(StaminaBloackReset >= 2)
        {
            staminaRecovery();
        }

        SoulAmount = Mathf.Clamp(SoulAmount, 0f, SoulMax);

    }

    public void DeadUIDelay()
    {
        DeadUiInfo.SetActive(true);
    }

    // Update is called once per frame
    public void TakeDamage(int damage)
    {
    if(Dieactive == false)
    {
        if(!CanDash)
        {
        if(BloackActive)
        {
            EfBlock.SetActive(true);
            if(currentShield > 0)
            {
                audioSrc.PlayOneShot(BlockSound);
            }
            else
            {
                audioSrc.PlayOneShot(BlockSound);
            currentHealthPlayer -= damage/2;
            Debug.Log("BlackDamage" + damage/2 + "Damage" + damage/2);

            StaminaReduceBolck = damage/2 *6;
            GetComponent<PlayerHealth>().StaminaDown(StaminaReduceBolck);
            }
        }
        else if(!BloackActive)
        {
            if(currentShield > 0)
            {
                currentShield -= 1;
            }
            else
            {
            currentHealthPlayer -= damage;
            Debug.Log("Damage" + damage);
            attacked = true;
            animator.SetTrigger("IsPlayerHit");
            StartCoroutine(FlashObject());
            }
        }
        }
    }
    }

    public void knockBack()
        {
            if(attacked)
            {
                rb2d.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
                Debug.Log("true");
            }
        }
        
    private IEnumerator FlashObject()
    {
            objectRenderer.material.color = flashColor;
            yield return new WaitForSeconds(0.2f);
            objectRenderer.material.color = originalColor;
            yield return new WaitForSeconds(0.2f);
    }

    public void UsePotion1()
    {
        if(UserInput.instance.controls.ItemInteract.interactPotion.WasPressedThisFrame())
        {
            if(PotionRed > 0)
            {
                PotionRed -= 1;
                currentHealthPlayer += PotionHealAmount;
                Debug.Log("playerHealth +" + PotionHealAmount);
            }
        }
    }

    public void GetRedPotion(int GetPotion)
    {
        PotionRed += GetPotion;
        Debug.Log("player Get Potion +" + GetPotion);
    }
    

    public void Heal(int HealAmount)
    {
        currentHealthPlayer += HealAmount;
        Debug.Log("playerHealth +" + HealAmount);
        var item = Instantiate(EffectHeal, SpawnEffectOnMC.position, SpawnEffectOnMC.rotation);
    }

    public void ShieldPlusPlus(int Shield)
    {
        currentShield += Shield;
        Debug.Log("Shield +" + Shield);
        var item = Instantiate(EffectHeal, SpawnEffectOnMC.position, SpawnEffectOnMC.rotation);
    }

    public void StaminaDown(int StaminaReduce)
    {
        currentStamina -= StaminaReduce;
    }

    public void staminaRecovery()
    {
        currentStamina += staminaRecoveryRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina); // จำกัดค่าให้ไม่เกินสูงสุด
    }

    public void CollectSoul(int SoulCollect)
    {
        SoulAmount += SoulCollect;
    }  

    public void savePlayer()
    {
        SaveSystem.SavePlayerAlive(this);
    }

    public void LoadPlayerCheckpoint()
    {
        PlayerDataAlive data = SaveSystem.LoadPlayer();

        maxHealthPlayer = data.maxHealth;
        currentHealthPlayer = data.Health;
        maxShield = data.maxShield;
        currentShield = data.Shield;
        maxStamina = data.maxStamina;
        SoulAmount = data.Soul;
        attackDamage = data.Attack;
        Key = data.KeyCurrent;
        PotionRed = data.RedPotionCurrent;

    }

    public static void DeletePlayerData()
    {
    string path = Application.persistentDataPath + "/Player.CheckPoint";
    
    if (File.Exists(path))
    {
        File.Delete(path);
        Debug.Log("ลบข้อมูลผู้เล่นแล้ว");
    }
    else
    {
        Debug.Log("ไม่พบไฟล์ข้อมูลผู้เล่นเพื่อทำการลบ");
    }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(LoadSceneSave);
    }



    void NextScene()
    {
        if(UserInput.instance.controls.Interact.interact.WasPressedThisFrame() && PlayerInNextScene == true)
        {
            Debug.Log("Save Player");
            savePlayer();
            Invoke("LoadScene", 0.25f);
        }

    }

    public void MaxHpPlus(int PlusHp)
    {
        maxHealthPlayer += PlusHp;
        Debug.Log("Max Hp +" + PlusHp);
    }

    public void MaxStaminaPlus(int PlusStamina)
    {
        maxStamina += PlusStamina;
        Debug.Log("Max stamina +" + PlusStamina);
    }
    public void MaxShieldPlus(int PlusShield)
    {
        maxShield += PlusShield;
        Debug.Log("Max Shield" + PlusShield);
    }

    public void BuyItemSoul(int Soul)
    {
        SoulAmount -= Soul;
        Debug.Log("Soul - " + Soul);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NextScene"))
        {
            PlayerInNextScene = true;
        }
    } 
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NextScene")) // ตรวจสอบว่าผู้เล่นออกจากพื้นที่สำหรับไอเท็มหรือไม่
        {
            PlayerInNextScene = false;
        }
    }

    public void OpenEsc()
    {
        if(UserInput.instance.controls.Interact.Setting.WasPressedThisFrame())
        {
            SettingEsc.SetActive(true);
        }
    }

}
