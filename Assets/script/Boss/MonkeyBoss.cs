using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyBoss : MonoBehaviour
{
    public CharacterState currentState;
    [Header("Info")]
    public GameObject Boss;
    public GameObject PlayerObject;
    public GameObject Floor;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public LayerMask PlayerMask;
    public LayerMask GroundMask;
    public int attackDamage1;
    public int attackDamage2;
    public int attackDamage3;
    public int attackDamage4;
    public int attackDamage5;
    private bool isLock;
    public bool DefBoss;
    public float Health;

    public BossHealth bosshealth;

    public float PlayerX;
    public float PlayerY;
    public float PlayerXno;
    public float BossX;
    public float BossXno;
    public float BossY;
    private float DistancePVB;

    [Header("State")]
    [SerializeField]
    private bool bossFace1;
    [SerializeField]
    private bool bossFace2;
    [SerializeField]
    private bool bossFace3;
    [SerializeField]
    public bool Diestate;
    bool Face1T;
    bool Face2T;
    bool Face3T;
    bool SetDie1;
    bool SetDie2;
    bool SetDieEnd1;
    bool SetDieEnd2;

    [Header("Colider")]
    public int MonsterLayer;
    public string monsterLayerName;
    public int PlayerLayer;
    public string playerLayerName;
    public int FloorLayer;
    public string floorLayerName;

    [Header("SkillAtk1")]
    public Vector3 skillAtk1OffSet;
    public Vector2 skillAtk1Range;
    public Vector3 skillAtk1OffSet2;
    public Vector2 skillAtk1Range2;
    bool hasAttacked;
    bool hasAttacked2;
    bool animetionSkill1;
    float MoveSetAtk1;
    float valueMoveAtk1;
    float moveDistance;

    [Header("SkillAtk2")]
    bool animetionSkill2;    
    public Transform launchPointAtk11;
    public Transform launchPointAtk12;
    public Transform launchPointAtk21;
    public Transform launchPointAtk22;
    public Transform launchPointAtk31;
    public Transform launchPointAtk32;
    public GameObject ItemUp;
    public Vector3 skillAtk2OffSet;
    public Vector2 skillAtk2Range;

    [Header("SkillAtk3")]
    public Vector2 direction;
    public float moveSpeedSkill3;
    public Vector3 skillAtk3OffSet;
    public Vector2 skillAtk3Range;
    public bool hasAttacked3;
    public bool inside3;
    bool animetionSkill3;

    [Header("SkillAtk4")]
    public Vector3 skillAtk4OffSet1;
    public Vector2 skillAtk4Range1;
    public Vector3 skillAtk4OffSet2;
    public Vector2 skillAtk4Range2;
    public Vector3 skillAtk4OffSet3;
    public Vector2 skillAtk4Range3;
    bool hasAttacked4;
    public float DistancePVBinSkill;
    public float nextX;
    public float baseY;
    public float height;
    Vector3 movePosition;
    float speed = 10;
    bool hitGround;
    float GroundDelay;
    public Vector3 groundSet;
    public Vector2 GroundRange;
    bool SetMove;
    public float SetMoveTime;
    public float MoveTime;
    bool animetionSkill4;
    bool hasStart;

    [Header("SkillRaor")]
    public Vector3 skillRoarOffSet;
    public Vector2 skillRoarRange;
    bool hasRaor;
    public Transform launchPoint1;
    public Transform launchPoint2;
    public Transform launchPoint3;
    public Transform launchPoint4;
    public Transform launchPoint5;
    public Transform launchPoint6;
    public Transform launchPoint7;
    public Transform launchPoint8;
    public GameObject Item;

    [Header("Effect")]
    public GameObject Fun1;
    public GameObject Fun2;
    public GameObject Roar1;
    public GameObject Roar2;
    public GameObject Roar3;
    public GameObject EfGround;

    [Header("Audio")]
    public AudioClip AtkSound;
    public AudioClip DieSound;
    public AudioSource audioSrc;
    bool hasSoundDie;


    public enum CharacterState {
    bossFace1,
    bossFace2,
    bossFace3,
    Die
    }

    // Start is called before the first frame update
    void Start()
    {
        Boss = GameObject.FindGameObjectWithTag("Boss"); 
        PlayerObject = GameObject.FindGameObjectWithTag("Player"); 
        Floor = GameObject.FindGameObjectWithTag("Floor");

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

        moveSpeedSkill3 = 10;
        
    }

    // Update is called once per frame
    void Update()
    {
        Health = bosshealth.currentHealth;
        PlayerXno = PlayerObject.transform.position.x;
        BossXno = Boss.transform.position.x;
        DistancePVB = PlayerXno - BossXno;
        VarUpdate();
        spriteRendererFlipX();
        SkillAtk4();
        transform.Translate(direction * moveSpeedSkill3 * Time.deltaTime);

        switch (currentState) {
            case CharacterState.bossFace1:
                bossFace1 = true;
                FaceBoss1();

                break;
            case CharacterState.bossFace2:
                bossFace2 = true;
                FaceBoss2();


                break;
            case CharacterState.bossFace3:
                bossFace3 = true;
                FaceBoss3();


                break;
            case CharacterState.Die:
                Diestate = true;
                break;
        }

        if(SetDie2)
        {
            StopCoroutine(AnimateAttack3());
        }

        if(bosshealth.currentHealth <= 0.6f * bosshealth.maxHealth)
        {
            ChangeState(CharacterState.bossFace2);
        }
        if(bosshealth.currentHealth <= 0.2f * bosshealth.maxHealth)
        {
            ChangeState(CharacterState.bossFace3);
        }

        if(bosshealth.currentHealth <= 0)
        {
            StopCoroutine(AnimateAttack3());
            ChangeState(CharacterState.Die);
        }

        if(bossFace1 == true)
        {
        }
        else if (bossFace2 == true)
        {
        }
        else if (bossFace3 == true)
        {
        }
        else if (Diestate == true)
        {
        }
    }

    void ChangeState(CharacterState newState) {
        bossFace1 = false;
        bossFace2 = false;
        bossFace3 = false;
        currentState = newState;
    }

    void VarUpdate()
    {

        Collider2D ColiderInSide3 = Physics2D.OverlapBox(transform.position + skillAtk3OffSet, skillAtk3Range , 0f, PlayerMask);
        inside3 = ColiderInSide3 != null;

        hitground();

        if(hasAttacked3 == false && animetionSkill3 == true && inside3 == true)
        {
            AreaSkillAtk3Sec1();
        }
        if(SetMove == true)
        {
            SetMoveTime += Time.deltaTime;
        }
        if(SetMoveTime != 0)
        {
            MoveTime += Time.deltaTime;
        }
    }

    void FaceBoss1()
    {
        if(!Face1T)
        {
            StartCoroutine(Face1());
        }
    }

    IEnumerator Face1()
            {   
                Face1T = true;
                SkillRaor();  
                yield return new WaitForSeconds(3.6f); // รอเวลาอนิเมชัน 1 วินาที
                SkillAtk2();
                yield return new WaitForSeconds(6.1f); // รอเวลาอนิเมชัน 1 วินาที
                Face1T = false;
            }

    void FaceBoss2()
    {
        StopCoroutine(AnimateAttack1());
        if(!SetDie1)
        {
            StartCoroutine(DieFace1());
        }
        if(!Face2T && SetDieEnd1)
        {
            StartCoroutine(Face2());
        }
    }

        IEnumerator Face2()
            {   
                Face2T = true;
                SkillRaor();  
                yield return new WaitForSeconds(3.1f); // รอเวลาอนิเมชัน 1 วินาที
                FollowTarget();
                SetMoveTime += Time.deltaTime;
                animetionSkill4 = true;
                yield return new WaitForSeconds(4.5f); // รอเวลาอนิเมชัน 1 วินาที
                SkillAtk2();
                yield return new WaitForSeconds(5.1f);
                SkillAtk1();
                yield return new WaitForSeconds(5.8f);
                Face2T = false;
            }

        IEnumerator DieFace1()
        {
        animator.SetTrigger("die1");
        SetDie1 = true;
        yield return new WaitForSeconds(3f);
        SetDieEnd1 = true;
        }

    void FaceBoss3()
    {
        StopCoroutine(AnimateAttack2());
        if(!SetDie2)
        {
            StartCoroutine(DieFace2());
        }
        if(!Face3T && SetDieEnd2)
        {
            
            StartCoroutine(Face3());
        }

    }

    IEnumerator Face3()
            {   
                Face3T = true;
                SkillAtk3();
                yield return new WaitForSeconds(9.6f);
                FollowTarget();
                SetMoveTime += Time.deltaTime;
                animetionSkill4 = true;
                yield return new WaitForSeconds(5f); // รอเวลาอนิเมชัน 1 วินาที
                SkillAtk2();
                yield return new WaitForSeconds(4.0f);
                SkillRaor();  
                yield return new WaitForSeconds(5.1f);
                SkillAtk1();
                yield return new WaitForSeconds(4.8f);
                Face3T = false;
            }

        IEnumerator DieFace2()
        {
        SetDie2 = true;
        animator.SetTrigger("die1");
        yield return new WaitForSeconds(5f);
        SetDieEnd2 = true;
        }

    void spriteRendererFlipX()
    {
        if(isLock == false)
        {
            if(DistancePVB < 0)
            {
            spriteRenderer.flipX = false;
            valueMoveAtk1 = 500;
            }
            else if(DistancePVB > 0)
            {
            spriteRenderer.flipX = true;
            valueMoveAtk1 = -500;
            } 
        }
        if(spriteRenderer.flipX == false)
        {
            skillAtk3OffSet = new Vector3(-2.75f , skillAtk3OffSet.y , skillAtk3OffSet.z);
        }
        else if(spriteRenderer.flipX == true)
        {
            skillAtk3OffSet = new Vector3(2.75f , skillAtk3OffSet.y , skillAtk3OffSet.z);
        }

    }

    void OnDrawGizmos()
    {
        //Skill Atk 01
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + skillAtk1OffSet, skillAtk1Range);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + skillAtk1OffSet2, skillAtk1Range2);
        //Skill Atk 02
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + skillAtk2OffSet, skillAtk2Range);
        //Skill Atk 03
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + skillAtk3OffSet, skillAtk3Range);
        //Skill Atk04
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + skillAtk4OffSet1, skillAtk4Range1);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + skillAtk4OffSet2, skillAtk4Range2);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + skillAtk4OffSet3, skillAtk4Range3);
        //Ground
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(transform.position + groundSet, GroundRange);


    }


    //Skill1 Start


    void SkillAtk1()
    {
        if(animetionSkill1 == false)
        {
        StartCoroutine(AnimateAttack1());
        animetionSkill1 = true;
        }
        moveDistance = valueMoveAtk1 * Time.deltaTime;
    }

    void AreaSkillAtk1Sec1()
    {
        Collider2D ColiderInSide = Physics2D.OverlapBox(transform.position + skillAtk1OffSet, skillAtk1Range, 0f , PlayerMask);

        if (ColiderInSide != null)
        {
            ColiderInSide.GetComponent<PlayerHealth>().TakeDamage(attackDamage1);
        }
    }

    void AreaSkillAtk1Sec2()
    {
        Collider2D ColiderInSideDown = Physics2D.OverlapBox(transform.position + skillAtk1OffSet2, skillAtk1Range2, 0f , PlayerMask);

        if (ColiderInSideDown != null)
        {
            ColiderInSideDown.GetComponent<PlayerHealth>().TakeDamage(attackDamage1);
        }
    }

        IEnumerator AnimateAttack1()
            {   
                isLock = true;
                animator.SetTrigger("atk1");
                MoveSetAtk1 = BossXno;
                DefBoss = true;
                yield return new WaitForSeconds(0.6f); // รอเวลาอนิเมชัน 1 วินาที

                if (hasAttacked == false) // ตรวจสอบว่ายังไม่มีการโจมตี
                {
                AreaSkillAtk1Sec1();
                hasAttacked = true;
                }
                yield return new WaitForSeconds(0.4f);
                yield return new WaitForSeconds(0.7f);
                if (hasAttacked2 == false) // ตรวจสอบว่ายังไม่มีการโจมตี
                {
                AreaSkillAtk1Sec2();
                hasAttacked2 = true;
                }
                yield return new WaitForSeconds(0.8f);
                Vector3 newPosition = new Vector3(MoveSetAtk1 + moveDistance, transform.position.y, transform.position.z);
                transform.position = newPosition;
                yield return new WaitForSeconds(1.3f);
                DefBoss = false;
                hasAttacked = false;
                hasAttacked2 = false;
                animetionSkill1 = false;
                isLock = false;
            }

        
        //Skill1 End


        //Skill2 Start

        void SkillAtk2()
        {
            if(animetionSkill2 == false)
            {
                StartCoroutine(AnimateAttack2());
            }            
        }

        void AreaSkillAtk2()
        {
        Collider2D ColiderInSide = Physics2D.OverlapBox(transform.position + skillAtk2OffSet, skillAtk2Range, 0f , PlayerMask);

        if (ColiderInSide != null)
        {
            ColiderInSide.GetComponent<PlayerHealth>().TakeDamage(attackDamage2);
        }
        }

            void SpawnItemUp1()
        {
        var itemThrow1 = Instantiate(ItemUp, launchPointAtk11.position, launchPointAtk11.rotation);
        var itemThrow2 = Instantiate(ItemUp, launchPointAtk12.position, launchPointAtk12.rotation);
        }
            void SpawnItemUp2()
        {
        var itemThrow1 = Instantiate(ItemUp, launchPointAtk21.position, launchPointAtk21.rotation);
        var itemThrow2 = Instantiate(ItemUp, launchPointAtk22.position, launchPointAtk22.rotation);
        }
            void SpawnItemUp3()
        {
        var itemThrow1 = Instantiate(ItemUp, launchPointAtk31.position, launchPointAtk31.rotation);
        var itemThrow2 = Instantiate(ItemUp, launchPointAtk32.position, launchPointAtk32.rotation);
        }

        IEnumerator AnimateAttack2()
            {   
                isLock = true;
                animator.SetTrigger("atk2");
                animetionSkill2 = true;
                MoveSetAtk1 = BossXno;
                DefBoss = true;
                yield return new WaitForSeconds(0.8f);
                EfGround.SetActive(true);
                yield return new WaitForSeconds(0.2f); // รอเวลาอนิเมชัน 1 วินาที
                SpawnItemUp1();
                AreaSkillAtk2();
                yield return new WaitForSeconds(0.7f);
                EfGround.SetActive(true);
                yield return new WaitForSeconds(0.2f); // รอเวลาอนิเมชัน 1 วินาที
                SpawnItemUp2();
                AreaSkillAtk2();
                yield return new WaitForSeconds(0.7f);
                EfGround.SetActive(true);
                yield return new WaitForSeconds(0.2f); // รอเวลาอนิเมชัน 1 วินาที
                SpawnItemUp3();
                AreaSkillAtk2();
                yield return new WaitForSeconds(0.3f);
                hasAttacked = false;
                animetionSkill2 = false;
                isLock = false;
                DefBoss = false;
            }


        //Skill2 End


        //Skill3 Start
        void SkillAtk3()
        {
            StartCoroutine(AnimateAttack3());
        }

        void AreaSkillAtk3Sec1()
        {
                Collider2D ColiderInSide3 = Physics2D.OverlapBox(transform.position + skillAtk3OffSet, skillAtk3Range , 0f, PlayerMask);
                inside3 = ColiderInSide3 != null;

                if(ColiderInSide3 != null)
                {
                ColiderInSide3.GetComponent<PlayerHealth>().TakeDamage(attackDamage3);
                hasAttacked3 = true;
                Debug.Log("HIT");
                }
        }

        IEnumerator AnimateAttack3()
            {   
                animetionSkill3 = true;
                isLock = true;
                DefBoss = true;
                spriteRendererXAttack(); 
                animator.SetTrigger("atk3");    
                yield return new WaitForSeconds(2.4f); // รอเวลาอนิเมชัน 1 วินาที
                hasAttacked3 = false;
                direction.x = 0;
                yield return new WaitForSeconds(0.75f);
                spriteRenderer.flipX = !spriteRenderer.flipX;
                spriteRendererXAttack(); 
                animator.SetTrigger("atk3"); 
                yield return new WaitForSeconds(2.4f);
                hasAttacked3 = false;
                direction.x = 0;
                yield return new WaitForSeconds(0.75f);
                spriteRenderer.flipX = !spriteRenderer.flipX;
                spriteRendererXAttack(); 
                animator.SetTrigger("atk3");
                yield return new WaitForSeconds(2.4f);
                hasAttacked3 = false;
                direction.x = 0;
                yield return new WaitForSeconds(0.75f);
                isLock = false;
                DefBoss = false;
                direction.x = 0;
                animetionSkill3 = false;
            }

        void spriteRendererXAttack()
        {
            if(spriteRenderer.flipX == false)
            {
                direction.x = -1;
            }
            else if(spriteRenderer.flipX == true)
            {
                direction.x = 1;
            }
        }

    void SkillAtk4()
    {
        nextX = Mathf.MoveTowards(transform.position.x , PlayerX , speed * Time.deltaTime);
        baseY = Mathf.Lerp(BossY  , PlayerY , Mathf.Clamp01((nextX - BossX) / DistancePVBinSkill));
        height = 7.5f * (nextX - BossX) * (nextX - PlayerX) / (-0.25f * DistancePVBinSkill * DistancePVBinSkill);

        movePosition = new Vector3(nextX, baseY + height, transform.position.z);

        if(MoveTime >= 0.2)
        {
            if(transform.position.x != movePosition.x)
            {
                transform.position = movePosition;
                animator.SetBool("atk4-1",true);
                DefBoss = true;

            }
            else
            {
                transform.position = transform.position;
                if(!hasStart)
                {
                    DefBoss = true;
                    StartCoroutine(AnimateAtk4());
                }
            }
        }
    }

    void FollowTarget()
    {
            PlayerX = PlayerObject.transform.position.x;
            PlayerY = PlayerObject.transform.position.y;
            BossX = Boss.transform.position.x;
            BossY = Boss.transform.position.y;

            DistancePVBinSkill = PlayerX - BossX;
    }

    IEnumerator AnimateAtk4()
            {   
                DefBoss = true;
                hasStart = true;
                animator.SetBool("atk4-1",false);
                isLock = true;
                animator.SetTrigger("atk4-2");      
                    SkillAtk4AreaSec1();
                yield return new WaitForSeconds(0.8f);
                    SkillAtk4AreaSec2();
                yield return new WaitForSeconds(0.8f);
                    SkillAtk4AreaSec3();
                yield return new WaitForSeconds(1.4f);
                isLock = false;
                DefBoss = false;
                SetMoveTime = 0;
                MoveTime = 0;
                animetionSkill4 = false;
                hasStart = false;
            }

    void hitground()
    {
        Collider2D ColiderInSide = Physics2D.OverlapBox(transform.position + groundSet, GroundRange , 0f, GroundMask);
        hitGround = ColiderInSide !=null;
    }





    void SkillAtk4AreaSec1()
    {
            Collider2D ColiderInSide = Physics2D.OverlapBox(transform.position + skillAtk4OffSet1, skillAtk4Range1 , 0f, PlayerMask);

            if(ColiderInSide != null)
            {
            ColiderInSide.GetComponent<PlayerHealth>().TakeDamage(attackDamage4);
            }
            hasAttacked4 = true;
    }

    void SkillAtk4AreaSec2()
    {
            Collider2D ColiderInSide = Physics2D.OverlapBox(transform.position + skillAtk4OffSet2, skillAtk4Range2 , 0f, PlayerMask);

            if(ColiderInSide != null)
            {
            ColiderInSide.GetComponent<PlayerHealth>().TakeDamage(attackDamage4);
            }
            hasAttacked4 = true;
    }

    void SkillAtk4AreaSec3()
    {
            Collider2D ColiderInSide = Physics2D.OverlapBox(transform.position + skillAtk4OffSet3, skillAtk4Range3 , 0f, PlayerMask);

            if(ColiderInSide != null)
            {
            ColiderInSide.GetComponent<PlayerHealth>().TakeDamage(attackDamage4);
            }
            hasAttacked4 = true;
    }

    

    void SkillRaor()
    {
        StartCoroutine(AnimateRaor());
    }

    void RaorArea()
    {
            Collider2D ColiderInSide = Physics2D.OverlapBox(transform.position + skillRoarOffSet, skillRoarRange , 0f, PlayerMask);

            if(ColiderInSide != null)
            {
            ColiderInSide.GetComponent<PlayerHealth>().TakeDamage(attackDamage5);
            }
            hasRaor = true;
    }

    IEnumerator AnimateRaor()
            {   
                isLock = true;
                DefBoss = true;
                animator.SetTrigger("raor");    
                yield return new WaitForSeconds(0.8f);   
                RaorArea();
                    Roar1.SetActive(true);
                    SpawnItem1();
                    SpawnItem5();
                yield return new WaitForSeconds(0.4f);
                RaorArea();
                    Roar2.SetActive(true);
                    SpawnItem4();
                    SpawnItem7();
                yield return new WaitForSeconds(0.45f);
                RaorArea();
                    Roar3.SetActive(true);
                    SpawnItem2();
                    SpawnItem6();
                yield return new WaitForSeconds(0.45f);
                RaorArea();
                    SpawnItem3();
                    SpawnItem8();
                yield return new WaitForSeconds(0.4f);
                isLock = false;
                DefBoss = false;
            }
    void SpawnItem1()
    {
        var itemThrow = Instantiate(Item, launchPoint1.position, launchPoint1.rotation);
    }
    void SpawnItem2()
    {
        var itemThrow = Instantiate(Item, launchPoint2.position, launchPoint2.rotation);
    }
    void SpawnItem3()
    {
        var itemThrow = Instantiate(Item, launchPoint3.position, launchPoint3.rotation);
    }
    void SpawnItem4()
    {
        var itemThrow = Instantiate(Item, launchPoint4.position, launchPoint4.rotation);
    }
    void SpawnItem5()
    {
        var itemThrow = Instantiate(Item, launchPoint5.position, launchPoint5.rotation);
    }
    void SpawnItem6()
    {
        var itemThrow = Instantiate(Item, launchPoint6.position, launchPoint6.rotation);
    }
    void SpawnItem7()
    {
        var itemThrow = Instantiate(Item, launchPoint7.position, launchPoint7.rotation);
    }
    void SpawnItem8()
    {
        var itemThrow = Instantiate(Item, launchPoint8.position, launchPoint8.rotation);
    }



    
}
