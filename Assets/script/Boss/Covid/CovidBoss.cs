using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CovidBoss : MonoBehaviour
{

    public CharacterState currentState;
    [Header("Info")]
    public GameObject Boss;
    public GameObject PlayerObject;
    public GameObject Floor;
    public GameObject Covid1;
    public GameObject Covid2;
    public GameObject Covid3;
    public GameObject Covid4;
    public GameObject groundPo;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public LayerMask PlayerMask;
    public LayerMask GroundMask;
    public int attackDamage1;
    public int attackDamage3;
    public int attackDamage4;
    bool isLock;
    public bool filpX;
    public bool DefBoss;
    public TrailRenderer tr;

    public float Covid1X;
    public float Covid1Y;
    public float Covid2X;
    public float Covid2Y;
    public float Covid3X;
    public float Covid3Y;
    public float PlayerX;
    public float PlayerY;
    public float groundPoY;
    public float PlayerXno;
    public float BossX;
    public float BossXno;
    public float BossY;
    private float DistancePVB;

    [Header("State")]
    [SerializeField]
    private bool idle;
    [SerializeField]
    private bool bossFace1;
    [SerializeField]
    private bool bossFace2;
    [SerializeField]
    public bool Diestate;



    [Header("Colider")]
    public int MonsterLayer;
    public string monsterLayerName;
    public int PlayerLayer;
    public string playerLayerName;
    public int FloorLayer;
    public string floorLayerName;
    
    [Header("Skill1")]
    public Vector3 skill1OffSet;
    public Vector2 skill1Range;
    bool skillAtk1;
    Vector3 directionSkill1;
    float moveSpeed1;
    bool Colider1;
    bool hasAttack1;  

    [Header("Skill2")]
    public Vector3 skill2OffSet;
    public Vector2 skill2Range;
    bool skillAtk2;
    bool hasAttack2;
    public GameObject Item;
    public Transform launchPoint1;
    public Transform launchPoint2;
    public Transform launchPoint3;
    public Transform launchPoint4;
    public Transform launchPoint5;
    public Transform launchPoint6;
    public Transform launchPoint7;
    public Transform launchPoint8;

    [Header("Skill3")]
    public Vector3 skill3OffSet;
    public Vector2 skill3Range;
    bool skillAtk3;
    bool hasAttack3;
    Vector3 directionSkill3Jump;
    float moveSpeed3Jump;
    public Vector3 directionSkill31;
    public Vector3 directionSkill32;
    float moveSpeed31;
    float moveSpeed32;
    public Vector3 directionSkill4Down;
    float moveSpeed4Down;
    bool Colider3;

    [Header("Skill4")]
    public Vector3 skill4OffSet;
    public Vector2 skill4Range;
    bool skillAtk4;
    bool hasAttack4;
    Vector3 Skill4Po;
    float moveSpeed4;
    float moveSpeed42;
    Vector3 directionSkill4;
    Vector3 directionSkill42;
    Vector3 Skill4Down;

    [Header("Face")]
    bool face1;
    bool face2;
    float timeSet;
    Vector3 direcstart;
    float moveSpeedstart;
    bool Entra;

    [Header("Effect")]
    public GameObject EFatk11;
    public GameObject EFatk12;
    public GameObject EFatk2;
    public GameObject EFatk3;

    public BossHealth bosshealth;

    [Header("Audio")]
    public AudioClip AtkSound;
    public AudioClip AtkAirSound;
    public AudioClip IdleSound;
    public AudioClip SmashSound;
    public AudioClip DieSound;
    public AudioSource audioSrc;
    bool hasSoundDie;


    public enum CharacterState {
    idle,
    bossFace1,
    bossFace2,
    Die
    }
    // Start is called before the first frame update
    void Start()
    {
        Boss = GameObject.FindGameObjectWithTag("Boss"); 
        PlayerObject = GameObject.FindGameObjectWithTag("Player"); 
        Floor = GameObject.FindGameObjectWithTag("Floor");
        Covid1 = GameObject.FindGameObjectWithTag("Covid1");
        Covid2 = GameObject.FindGameObjectWithTag("Covid2");
        Covid3 = GameObject.FindGameObjectWithTag("Covid3");
        Covid4 = GameObject.FindGameObjectWithTag("Covid4");
        groundPo = GameObject.FindGameObjectWithTag("groundPo");

        monsterLayerName = "Boss";
        playerLayerName = "Player";
        floorLayerName = "Floor";

        int MonsterLayer = LayerMask.NameToLayer(monsterLayerName); // แปลงชื่อ Layer เป็น Layer ที่เป็น int
        int PlayerLayer = LayerMask.NameToLayer(playerLayerName);
        int FloorLayer = LayerMask.NameToLayer(floorLayerName);

        for (int i = 0; i < 32; i++)
        {
            if (i == PlayerLayer)
            {
                Physics2D.IgnoreLayerCollision(MonsterLayer, i, true);
            }
            if (i == FloorLayer)
            {
                Physics2D.IgnoreLayerCollision(MonsterLayer, i, true);
            }
        }

        AtkSound = Resources.Load<AudioClip>("covidAtk");
        AtkAirSound = Resources.Load<AudioClip>("covidAtkAir");
        IdleSound = Resources.Load<AudioClip>("covidIdle");
        SmashSound = Resources.Load<AudioClip>("covidSmash");
        DieSound = Resources.Load<AudioClip>("covidDie");
        audioSrc = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerXno = PlayerObject.transform.position.x;
        BossXno = Boss.transform.position.x;
        DistancePVB = PlayerXno - BossXno;
        Covid1X = Covid1.transform.position.x;
        Covid1Y = Covid1.transform.position.y;
        Covid2X = Covid2.transform.position.x;
        Covid2Y = Covid2.transform.position.y;
        Covid3X = Covid3.transform.position.x;
        Covid3Y = Covid3.transform.position.y;
        groundPoY = groundPo.transform.position.y;

        spriteRendererFlipX();

        transform.Translate(directionSkill1 * moveSpeed1 * Time.deltaTime);
        transform.Translate(directionSkill3Jump * moveSpeed3Jump * Time.deltaTime);
        transform.Translate(directionSkill31 * moveSpeed31 * Time.deltaTime);
        transform.Translate(directionSkill32 * moveSpeed32 * Time.deltaTime);
        transform.Translate(directionSkill4Down * moveSpeed4Down * Time.deltaTime);
        transform.Translate(directionSkill4 * moveSpeed4 * Time.deltaTime);
        transform.Translate(directionSkill42 * moveSpeed42 * Time.deltaTime);
        transform.Translate(direcstart * moveSpeedstart * Time.deltaTime);

        directionSkill3Jump.y = 1;

        directionSkill4 = (Skill4Po - transform.position).normalized;
        directionSkill42 = (Skill4Down  - transform.position).normalized;

        direcstart = (groundPo.transform.position - transform.position).normalized;

        //ซ้ายไปขวา
        directionSkill31 = (Covid2.transform.position - transform.position).normalized;
        //ขวาไปซ็าย
        directionSkill32 = (Covid1.transform.position - transform.position).normalized;

        directionSkill4Down = (Covid4.transform.position - transform.position).normalized;


            switch (currentState) {
            case CharacterState.idle:
                idle = true;
                FistEntra();
                break;
            case CharacterState.bossFace1:
                bossFace1 = true;
                Face1();

                break;
            case CharacterState.bossFace2:
                bossFace2 = true;
                Face2();

                break;
            case CharacterState.Die:
                Diestate = true;
                break;
        }
        

        if(bosshealth.currentHealth <= 0.5f * bosshealth.maxHealth && bossFace2 == false && bossFace1 == true)
        {
            ChangeState(CharacterState.bossFace2);
        }


        if(bosshealth.currentHealth <= 0)
        {
            StopCoroutine(TheFace1());
            StopCoroutine(AnimateAttack1());
            StopCoroutine(AnimateAttack2());
            StopCoroutine(AnimateAttack3());
            StopCoroutine(AnimateAttack4());
            StopCoroutine(TheFace2());
            ChangeState(CharacterState.Die);
            audioSrc.PlayOneShot(DieSound , 1f);
        }


        timeSet += Time.deltaTime;
        if(timeSet <= 1)
        {
            Invoke("gotoFace1" ,5f);
        }







        Collider2D ColiderInSide = Physics2D.OverlapBox(transform.position + skill1OffSet, skill1Range, 0f , PlayerMask);
        Colider1 = ColiderInSide != null;

        if(hasAttack1 == false && skillAtk1 == true && Colider1 == true)
        {
            AreaSkillAtk1();
        }

 
        Collider2D ColiderInSide3 = Physics2D.OverlapBox(transform.position + skill3OffSet, skill3Range, 0f , PlayerMask);
        Colider3 = ColiderInSide3 != null;

        if (hasAttack3 == false && skillAtk3 == true && Colider3 == true)
        {
            AreaSkillAtk3();
        }
    
    }

    void gotoFace1()
    {
        ChangeState(CharacterState.bossFace1);
    }

        void ChangeState(CharacterState newState) {
        idle = false;
        bossFace1 = false;
        bossFace2 = false;
        currentState = newState;
    }

        void OnDrawGizmos()
    {
        //Skill Atk 01
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + skill1OffSet, skill1Range);

        //Skill Atk 03
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + skill3OffSet, skill3Range);

        //Skill Atk 04
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + skill4OffSet, skill4Range);

    }


    void Face1()
    {
        if(!face1)
        {   
        StartCoroutine(TheFace1());
        face1 = true;
        }
    }

    IEnumerator TheFace1()
            {   
                face1 = true;
                SkillAtk4();
                yield return new WaitForSeconds(3.5f); // รอเวลาอนิเมชัน 1 วินาที
                SkillAtk1();
                yield return new WaitForSeconds(3.0f); // รอเวลาอนิเมชัน 1 วินาที
                SkillAtk1();
                yield return new WaitForSeconds(3.0f); // รอเวลาอนิเมชัน 1 วินาที
                SkillAtk2();
                yield return new WaitForSeconds(6.5f); // รอเวลาอนิเมชัน 1 วินาที
                face1 = false;
            }

    void Face2()
    {
        if(!face2 && !face1)
        {   
        StartCoroutine(TheFace2());
        face2 = true;
        }
    }

    IEnumerator TheFace2()
            {   
                face2 = true;
                SkillAtk4();
                yield return new WaitForSeconds(3.5f); // รอเวลาอนิเมชัน 1 วินาที
                SkillAtk3();
                yield return new WaitForSeconds(12f); // รอเวลาอนิเมชัน 1 วินาที
                SkillAtk1();
                yield return new WaitForSeconds(3.0f); // รอเวลาอนิเมชัน 1 วินาที
                SkillAtk1();
                yield return new WaitForSeconds(6.0f); // รอเวลาอนิเมชัน 1 วินาที
                SkillAtk3();
                yield return new WaitForSeconds(8f); // รอเวลาอนิเมชัน 1 วินาที
                SkillAtk2();
                yield return new WaitForSeconds(8.5f); // รอเวลาอนิเมชัน 1 วินาที
                face2 = false;
            }


    //start First Entra

    void FistEntra()
    {
        if(!Entra)
        {
            StartCoroutine(AnimateEntra());
            Entra = true;
        }

    }
     IEnumerator AnimateEntra()
            {   
                isLock = true;
                DefBoss = true;
                animator.SetTrigger("entra");
                yield return new WaitForSeconds(2.5f); // รอเวลาอนิเมชัน 1 วินาที
                moveSpeedstart = 30f;
                yield return new WaitForSeconds(2.0f); // รอเวลาอนิเมชัน 1 วินาที
                moveSpeedstart = 0f;
                isLock = false;
                DefBoss = false;
            }
    //end First Entra




        //start SkillAtk 01

    void SkillAtk1()
    {
        if(!skillAtk1)
        {   
        StartCoroutine(AnimateAttack1());
        skillAtk1 = true;
        }
    }

    void AreaSkillAtk1()
    {
        Collider2D ColiderInSide = Physics2D.OverlapBox(transform.position + skill1OffSet, skill1Range, 0f , PlayerMask);

        if (ColiderInSide != null)
        {
            ColiderInSide.GetComponent<PlayerHealth>().TakeDamage(attackDamage1);
            hasAttack1 = true;
        }
    }

    IEnumerator AnimateAttack1()
            {   
                isLock = true;
                DefBoss = true;
                skillAtk1 = true;
                animator.SetTrigger("atk1");
                yield return new WaitForSeconds(0.6f); // รอเวลาอนิเมชัน 1 วินาที
                audioSrc.PlayOneShot(AtkSound , 1f);
                yield return new WaitForSeconds(0.5f); // รอเวลาอนิเมชัน 1 วินาที
                AreaSkillAtk1();
                EFatk11.SetActive(true);
                yield return new WaitForSeconds(0.8f); // รอเวลาอนิเมชัน 1 วินาที
                isLock = false;
                yield return new WaitForSeconds(0.2f); // รอเวลาอนิเมชัน 1 วินาที
                hasAttack1 = false;
                isLock = true;
                moveSpeed1 = 20;
                EFatk12.SetActive(true);
                audioSrc.PlayOneShot(AtkSound , 1f);
                yield return new WaitForSeconds(0.8f); // รอเวลาอนิเมชัน 1 วินาที
                moveSpeed1 = 0;
                skillAtk1 = false;
                hasAttack1 = true;
                DefBoss = false;
                isLock = false;
            }
        
        //end SkillAtk 01

        //start SkillAtk 02

    void SkillAtk2()
    {
        if(!skillAtk2)
        {   
        StartCoroutine(AnimateAttack2());
        skillAtk2 = true;
        }
    }

    IEnumerator AnimateAttack2()
            {   
                isLock = true;
                DefBoss = true;
                moveSpeed3Jump = 4;
                yield return new WaitForSeconds(1.0f); // รอเวลาอนิเมชัน 1 วินาที
                moveSpeed3Jump = 0;
                itempoint();
                animator.SetTrigger("atk2");
                EFatk3.SetActive(true);
                yield return new WaitForSeconds(0.3f);
                audioSrc.PlayOneShot(AtkAirSound , 1f);
                yield return new WaitForSeconds(0.3f);
                audioSrc.PlayOneShot(AtkAirSound , 1f);
                yield return new WaitForSeconds(0.3f);
                audioSrc.PlayOneShot(AtkAirSound , 1f);
                yield return new WaitForSeconds(0.3f);
                audioSrc.PlayOneShot(AtkAirSound , 1f);
                yield return new WaitForSeconds(1.3f);
                moveSpeed3Jump = -4;
                yield return new WaitForSeconds(1.0f);
                moveSpeed3Jump = 0;
                DefBoss = false;
                isLock = false;
                skillAtk2 = false;
            }
    
    void itempoint()
    {
        var itemThrow1 = Instantiate(Item, launchPoint1.position, launchPoint1.rotation);
        var itemThrow2 = Instantiate(Item, launchPoint2.position, launchPoint2.rotation);
        var itemThrow3 = Instantiate(Item, launchPoint3.position, launchPoint3.rotation);
        var itemThrow4 = Instantiate(Item, launchPoint4.position, launchPoint4.rotation);
        var itemThrow5 = Instantiate(Item, launchPoint5.position, launchPoint5.rotation);
        var itemThrow6 = Instantiate(Item, launchPoint6.position, launchPoint6.rotation);
        var itemThrow7 = Instantiate(Item, launchPoint7.position, launchPoint7.rotation);
        var itemThrow8 = Instantiate(Item, launchPoint8.position, launchPoint8.rotation);
    }


        
        //end SkillAtk 02
    
        //start SkillAtk 03

    void SkillAtk3()
    {
        if(!skillAtk3)
        {   
        StartCoroutine(AnimateAttack3());
        skillAtk3 = true;
        }
    }

    void AreaSkillAtk3()
    {
        Collider2D ColiderInSide = Physics2D.OverlapBox(transform.position + skill3OffSet, skill3Range, 0f , PlayerMask);

        if (ColiderInSide != null)
        {
            ColiderInSide.GetComponent<PlayerHealth>().TakeDamage(attackDamage3);
            hasAttack3 = true;
        }
    }

    IEnumerator AnimateAttack3()
            {   
                isLock = true;
                DefBoss = true;
                skillAtk3 = true;
                moveSpeed3Jump = 20;
                tr.emitting = true;
                yield return new WaitForSeconds(1f); // รอเวลาอนิเมชัน 1 วินาที
                moveSpeed3Jump = 0;
                tr.emitting = false;
                goToStart();
                yield return new WaitForSeconds(0.2f); // รอเวลาอนิเมชัน 1 วินาที
                animator.SetTrigger("atk3-1");
                tr.emitting = true;
                hasAttack3 = false;
                moveSpeed31 = 75f;
                audioSrc.PlayOneShot(IdleSound , 1f);
                yield return new WaitForSeconds(1.3f); // รอเวลาอนิเมชัน 1 วินาที
                moveSpeed31 = 0f;
                hasAttack3 = true;
                isLock = false;
                yield return new WaitForSeconds(0.2f); // รอเวลาอนิเมชัน 1 วินาที
                isLock = true;
                hasAttack3 = false;
                animator.SetTrigger("atk3-1");
                moveSpeed32 = 75f;
                audioSrc.PlayOneShot(IdleSound , 1f);
                yield return new WaitForSeconds(1.3f); // รอเวลาอนิเมชัน 1 วินาที
                moveSpeed32 = 0f;
                hasAttack3 = true;
                isLock = false;
                yield return new WaitForSeconds(0.2f); // รอเวลาอนิเมชัน 1 วินาที
                moveSpeed31 = 75f;
                hasAttack3 = false;
                animator.SetTrigger("atk3-1");
                isLock = true;
                audioSrc.PlayOneShot(IdleSound , 1f);
                yield return new WaitForSeconds(1.3f); // รอเวลาอนิเมชัน 1 วินาที
                isLock = false;
                hasAttack3 = true;
                moveSpeed31 = 0f;
                yield return new WaitForSeconds(0.2f);
                moveSpeed32 = 75f;
                hasAttack3 = false;
                animator.SetTrigger("atk3-1");
                isLock = true;
                audioSrc.PlayOneShot(IdleSound , 1f);
                yield return new WaitForSeconds(1.5f); // รอเวลาอนิเมชัน 1 วินาที
                moveSpeed32 = 0f;
                tr.emitting = false;
                hasAttack3 = true;
                isLock = false;
                gotoEnd();
                yield return new WaitForSeconds(2.25f); // รอเวลาอนิเมชัน 1 วินาที
                moveSpeed4Down = 8f;
                yield return new WaitForSeconds(4f); // รอเวลาอนิเมชัน 1 วินาที
                moveSpeed4Down = 0f;
                skillAtk3 = false;
                DefBoss = false;
                isLock = false;

            }

    void goToStart()
    {
        transform.position = new Vector3(Covid1X, Covid1Y, transform.position.z);
    }

    void gotoEnd()
    {
        transform.position = new Vector3(Covid3X, Covid3Y, transform.position.z);
    }
        
        //end SkillAtk 03

        //start SkillAtk 04

    void SkillAtk4()
    {
        if(!skillAtk4)
        {   
        StartCoroutine(AnimateAttack4());
        skillAtk4 = true;
        }
    }

    void AreaSkillAtk4()
    {
        Collider2D ColiderInSide = Physics2D.OverlapBox(transform.position + skill4OffSet, skill4Range, 0f , PlayerMask);

        if (ColiderInSide != null)
        {
            ColiderInSide.GetComponent<PlayerHealth>().TakeDamage(attackDamage4);
            hasAttack1 = true;
        }
    }

    IEnumerator AnimateAttack4()
            {   
                isLock = true;
                DefBoss = true;
                Skill4Po = new Vector3(PlayerXno , transform.position.z , transform.position.z);
                moveSpeed3Jump = 10f;
                moveSpeed4 = 20f;
                yield return new WaitForSeconds(1.0f); // รอเวลาอนิเมชัน 1 วินาที
                animator.SetTrigger("atk4");
                moveSpeed3Jump = 0f;
                moveSpeed4 = 0f;
                moveSpeed42 = 20f;
                Skill4Down = new Vector3(transform.position.x , groundPoY , transform.position.z);
                yield return new WaitForSeconds(1.0f); // รอเวลาอนิเมชัน 1 วินาที
                audioSrc.PlayOneShot(SmashSound , 1f);
                yield return new WaitForSeconds(0.2f); // รอเวลาอนิเมชัน 1 วินาที
                moveSpeed42 = 0f;
                EFatk2.SetActive(true);
                AreaSkillAtk4();
                yield return new WaitForSeconds(1.3f); // รอเวลาอนิเมชัน 1 วินาที
                skillAtk4 = false;
                hasAttack4 = true;
                DefBoss = false;
                isLock = false;
            }

        //end SkillAtk 04


    void spriteRendererFlipX()
    {
        if(isLock == false)
        {
            if(DistancePVB < 0)
            {
            spriteRenderer.flipX = false;
            directionSkill1.x = -1;
            filpX = false;
            }
            else if(DistancePVB > 0)
            {
            spriteRenderer.flipX = true;
            directionSkill1.x = 1;
            filpX = true;
            } 
        }

        if(spriteRenderer.flipX == false)
        {
            skill1OffSet = new Vector3(-1.25f , skill1OffSet.y , skill1OffSet.z);

        }
        else if(spriteRenderer.flipX == true)
        {
            skill1OffSet = new Vector3(1.25f , skill1OffSet.y , skill1OffSet.z);

        }

    }

}
