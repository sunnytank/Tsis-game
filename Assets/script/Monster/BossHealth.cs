using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField]
    public float maxHealth;
    public float currentHealth;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float DIES;
    bool DieF;

    public bool invisDef;

    public GameObject Boss;

    public MonkeyBoss monkeyBoss;

    public minoBoss minoBoss;

    public CovidBoss CovidBoss;

    public GameObject Door;

    public Renderer objectRenderer;
    public Color flashColor = Color.red;
    private Color originalColor;
    public float DalayDamage;

    


    [Header("DropSoul")]
    public GameObject SoulExp = null; // กำหนด Prefab ที่ต้องการสร้าง
    public float numberOfObjects; // จำนวน object ที่ต้องการสร้าง
    public float currentSoul;
    public Vector2 positionExp;
    bool CorrectSoul;
    public float DelaySoul;
    public float Delay;

    // Start is called before the first frame update
    void Start()
    {
        objectRenderer = GetComponent<SpriteRenderer>();
        originalColor = objectRenderer.material.color;
        Boss = GameObject.FindGameObjectWithTag("Boss"); 
        currentHealth = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        DalayDamage += Time.deltaTime;

        if(monkeyBoss != null)
        {
            invisDef = monkeyBoss.DefBoss;
        }
        if(minoBoss != null)
        {
            invisDef = minoBoss.DefBoss;
        }
        if(CovidBoss != null)
        {
            invisDef = CovidBoss.DefBoss;
        }

        if(currentHealth <= 0)
        {
            DelaySoul += Time.deltaTime;
            if(DelaySoul >= Delay)
            {
                SoulDead();
            }
        }
        positionExp = transform.position;
        
    }

    public void TakeDamage(int damage)
    {
        if(!DieF)
        {
            if(!invisDef)
            {
            currentHealth -= damage;
            Debug.Log("Damage" + damage);
            StartCoroutine(FlashObject());
            }
            if(invisDef)
            {
            currentHealth -= damage/2;
            Debug.Log("Boss Block " + damage);
            StartCoroutine(FlashObject());
            }
        }
        if(currentHealth <= 0) 
        {
            DieF = true;
            StartCoroutine(Die());
        }
        if(DieF == true)
        {
            Door.SetActive(true);
        }
    }

    public void TakeDamageAuraBlade(int damage)
    {
        if(DalayDamage >= 0.2f)
        {
            if(!DieF)
            {
                if(!invisDef)
            {
            currentHealth -= damage;
            Debug.Log("Damage" + damage);
            StartCoroutine(FlashObject());
            DalayDamage = 0f;
            }
                if(invisDef)
            {
            currentHealth -= damage/2;
            Debug.Log("Boss Block " + damage);
            StartCoroutine(FlashObject());
            DalayDamage = 0f;
            }
            }
        }
        if(currentHealth <= 0) 
        {
            DieF = true;
            StartCoroutine(Die());
        }
        if(DieF == true)
        {
            Door.SetActive(true);
        }
    }

            private IEnumerator FlashObject()
    {
            objectRenderer.material.color = flashColor;
            yield return new WaitForSeconds(0.2f);
            objectRenderer.material.color = originalColor;
            yield return new WaitForSeconds(0.2f);
    }

        IEnumerator Die()
    {
        animator.SetTrigger("die2");
        yield return new WaitForSeconds(DIES);
        Destroy(gameObject);
        Debug.Log("Des enemy");

    }

    public void SoulDead()
    {
        SoulCreate();
        if(currentSoul == numberOfObjects)
        {
            CorrectSoul = true;
        }
    }

    public void SoulCreate()
    {
        if(currentSoul < numberOfObjects && CorrectSoul == false)
            {
                    Vector3 randomPosition = positionExp + Random.insideUnitCircle * 1f; // สุ่มตำแหน่งในรัศมี 5 หน่วย
                    GameObject newSoulExp = Instantiate(SoulExp, randomPosition, Quaternion.identity);
                    // สร้าง object ใหม่จาก Prefab ที่กำหนด ณ ตำแหน่งสุ่มในรัศมีที่กำหนด
                    currentSoul += 1;
            }
        if(currentSoul < numberOfObjects && CorrectSoul == false)
            {
                    Vector3 randomPosition = positionExp + Random.insideUnitCircle * 1f; // สุ่มตำแหน่งในรัศมี 5 หน่วย
                    GameObject newSoulExp = Instantiate(SoulExp, randomPosition, Quaternion.identity);
                    // สร้าง object ใหม่จาก Prefab ที่กำหนด ณ ตำแหน่งสุ่มในรัศมีที่กำหนด
                    currentSoul += 1;
            }
        if(currentSoul < numberOfObjects && CorrectSoul == false)
            {
                    Vector3 randomPosition = positionExp + Random.insideUnitCircle * 1f; // สุ่มตำแหน่งในรัศมี 5 หน่วย
                    GameObject newSoulExp = Instantiate(SoulExp, randomPosition, Quaternion.identity);
                    // สร้าง object ใหม่จาก Prefab ที่กำหนด ณ ตำแหน่งสุ่มในรัศมีที่กำหนด
                    currentSoul += 1;
            }
    }
}
