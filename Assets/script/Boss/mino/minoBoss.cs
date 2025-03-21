using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minoBoss : MonoBehaviour
{
    public CharacterState currentState;
    [Header("Info")]
    public GameObject Boss;
    public GameObject PlayerObject;
    public GameObject Floor;
    public GameObject Axe;
    public Rigidbody2D bossRigidbody;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public LayerMask PlayerMask;
    public LayerMask GroundMask;
    public int attackDamage1;
    public int attackDamage2;
    public int attackDamage3;
    public int attackDamage4;
    public int attackDamage5;
    bool isLock;
    public bool filpX;
    public bool DefBoss;

    public float PlayerX;
    public float PlayerY;
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
    public int AxeLayer;
    public string axeLayerName;

    [Header("skillAtk1")]
    public Vector3 skill1OffSet;
    public Vector2 skill1Range;
    bool skillAtk1;

    [Header("skillAtk2")]
    public Vector3 skill2OffSet;
    public Vector2 skill2Range;
    bool skillAtk2;
    
    [Header("skillAtk3")]
    public Vector3 skill3OffSet;
    public Vector2 skill3Range;
    bool skillAtk3;
    float jumpHeight;
    public float jumpForce = 1000.0f;

    [Header("skillAtk4")]
    public Vector3 skill4OffSet;
    public Vector2 skill4Range;
    public Transform launchPoint;
    public GameObject Item;
    Vector2 direction;
    float moveSpeedSkill4;
    bool Colider;
    bool hasAttack4;
    bool skillAtk4;
    bool hit4;

    [Header("skillAtk5")]
    public Vector3 directionSkill5;
    public Vector3 skill5OffSet;
    public Vector2 skill5Range;
    bool skillAtk5;
    float moveSpeedSkill5;

    [Header("Move")]
    float moveSpeedMove;
    bool CheckPlayer;

    [Header("Face")]
    bool notPlay;
    bool notPlay2;
    bool Sec1;
    bool Sec2;
    bool EndSec1;
    bool EndSec2;
    float timeSet;

    public float distanceTopoint;
    public float closestDistance;
    public float pointDistance;
    public float Distance;

    public BossHealth bosshealth;

    [Header("Effect")]
    public GameObject EFAtk1;
    public GameObject EFAtk21;
    public GameObject EFAtk22;
    public GameObject EFAtk23;
    public GameObject EFAtk3;

    [Header("Audio")]
    public AudioClip AtkV2Sound;
    public AudioClip RunSound;
    public AudioClip SpinSound;
    public AudioSource audioSrc;






    public enum CharacterState {
    idle,
    bossFace1,
    bossFace2,
    Die
    }
    // Start is called before the first frame update
    void Start()
    {
        bossRigidbody = GetComponent<Rigidbody2D>();
        Boss = GameObject.FindGameObjectWithTag("Boss"); 
        PlayerObject = GameObject.FindGameObjectWithTag("Player"); 
        Floor = GameObject.FindGameObjectWithTag("Floor");
        Axe = GameObject.FindGameObjectWithTag("Axe");

        monsterLayerName = "Boss";
        playerLayerName = "Player";
        floorLayerName = "Floor";
        axeLayerName = "Axe";

        int MonsterLayer = LayerMask.NameToLayer(monsterLayerName); // แปลงชื่อ Layer เป็น Layer ที่เป็น int
        int PlayerLayer = LayerMask.NameToLayer(playerLayerName);
        int FloorLayer = LayerMask.NameToLayer(floorLayerName);
        int AxeLayer = LayerMask.NameToLayer(axeLayerName);

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
            if (i == AxeLayer)
            {
                Physics2D.IgnoreLayerCollision(MonsterLayer, i, true);
            }
        }

        AtkV2Sound = Resources.Load<AudioClip>("minoAtkV2");
        RunSound = Resources.Load<AudioClip>("minoRun");
        SpinSound = Resources.Load<AudioClip>("minoSpin");
        audioSrc = GetComponent<AudioSource>();

        
    }

    // Update is called once per frame
    void Update()
    {
        //Health = bosshealth.currentHealth;
        PlayerXno = PlayerObject.transform.position.x;
        BossXno = Boss.transform.position.x;
        DistancePVB = PlayerXno - BossXno;

        spriteRendererFlipX();
        Moveanimate();
        transform.Translate(directionSkill5 * moveSpeedMove * Time.deltaTime);
        transform.Translate(direction * moveSpeedSkill4 * Time.deltaTime);
        transform.Translate(directionSkill5 * moveSpeedSkill5 * Time.deltaTime);

        directionSkill5 = (PlayerObject.transform.position - transform.position).normalized;
        directionSkill5.y = 0;


        GameObject[] Point = GameObject.FindGameObjectsWithTag("Point");

        closestDistance = Mathf.Infinity;
        Distance = BossXno - pointDistance;

        foreach (GameObject point in Point)
        {
            distanceTopoint = Vector3.Distance(transform.position, point.transform.position);

            if (distanceTopoint < closestDistance)
            {
                pointDistance = point.transform.position.x;
                closestDistance = distanceTopoint;
            }
        }

            switch (currentState) {
            case CharacterState.idle:
                idle = true;
                break;
            case CharacterState.bossFace1:
                bossFace1 = true;
                Face1Atk();

                break;
            case CharacterState.bossFace2:
                bossFace2 = true;
                Face2Atk();

                break;
            case CharacterState.Die:
                Diestate = true;
                break;
        }

        if(bosshealth.currentHealth <= 0.5f * bosshealth.maxHealth && bossFace2 == false && bossFace1 == true)
        {
            ChangeState(CharacterState.bossFace2);
            moveSpeedMove = 0f;
            EndSec2 = true;
        }


        if(bosshealth.currentHealth <= 0)
        {
            StopCoroutine(AnimateFace1());
            StopCoroutine(AnimateFace2Sec1());
            StopCoroutine(AnimateFace2Sec2());
            StopCoroutine(AnimateAttack1());
            StopCoroutine(AnimateAttack2());
            StopCoroutine(AnimateAttack3());
            StopCoroutine(AnimateAttack4Sec1());
            StopCoroutine(AnimateAttack5());
            moveSpeedMove = 0f;
            moveSpeedSkill4 = 0f;
            moveSpeedSkill5 = 0f;
            ChangeState(CharacterState.Die);
        }

        timeSet += Time.deltaTime;
        if(timeSet <= 1)
        {
            Invoke("gotoFace1" ,4f);
        }

        Collider2D ColiderInSide = Physics2D.OverlapBox(transform.position + skill4OffSet, skill4Range, 0f , PlayerMask);
        Colider = ColiderInSide != null;

        if(hasAttack4 == false && skillAtk4 == true && Colider == true)
        {
            AreaSkillAtk4();
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
        //Skill Atk 02
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + skill2OffSet, skill2Range);
        //Skill Atk 03
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + skill3OffSet, skill3Range);
        //Skill Atk 04
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + skill4OffSet, skill4Range);
        //Skill Atk 05
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + skill5OffSet, skill5Range);
    }

//start Face1

void Face1Atk()
{
    Collider2D ColiderInSide = Physics2D.OverlapBox(transform.position + skill5OffSet, skill5Range, 0f , PlayerMask);
    CheckPlayer = ColiderInSide != null;

    if(CheckPlayer == false && notPlay == false)
    {
        moveSpeedMove = 5f;
    }
    else if(CheckPlayer == true && notPlay == false)
    {
        moveSpeedMove = 0f;
        notPlay = true;
        StartCoroutine(AnimateFace1());
    }
}

IEnumerator AnimateFace1()
            {   
                SkillAtk1();
                yield return new WaitForSeconds(1.85f); // รอเวลาอนิเมชัน 1 วินาที
                SkillAtk5();
                yield return new WaitForSeconds(4.8f); // รอเวลาอนิเมชัน 1 วินาที
                SkillAtk3();
                yield return new WaitForSeconds(4.3f); // รอเวลาอนิเมชัน 1 วินาที
                notPlay = false;
                yield return new WaitForSeconds(0.2f);
            }

//end Face1

//start Face2

void Face2Atk()
{
    Collider2D ColiderInSide = Physics2D.OverlapBox(transform.position + skill5OffSet, skill5Range, 0f , PlayerMask);
    CheckPlayer = ColiderInSide != null;

    if(notPlay2 == false && Sec1 == false && EndSec1 == false && EndSec2 == true && !notPlay)
    {
        StartCoroutine(AnimateFace2Sec1());
        Debug.Log("1");
    }
    else if(CheckPlayer == false  && EndSec1 == true && notPlay2 == false && Sec2 == false && EndSec2 == false && !notPlay)
    {
        moveSpeedMove = 5f;
        Debug.Log("2");
    }
    else if(CheckPlayer == true  && EndSec1 == true && notPlay2 == false && Sec2 == false && EndSec2 == false && !notPlay)
    {
        moveSpeedMove = 0f;
        StartCoroutine(AnimateFace2Sec2());
        Debug.Log("3");
    }
}

IEnumerator AnimateFace2Sec1()
            {   
                notPlay2 = true;
                Sec1 = true;
                SkillAtk4Sec1();
                yield return new WaitForSeconds(4.6f); // รอเวลาอนิเมชัน 1 วินาที
                SkillAtk4Sec1();
                yield return new WaitForSeconds(4.6f); // รอเวลาอนิเมชัน 1 วินาที
                SkillAtk1();
                yield return new WaitForSeconds(1.85f); // รอเวลาอนิเมชัน 1 วินาที
                notPlay2 = false;
                Sec1 = false;
                EndSec2 = false;
                EndSec1 = true;
            }

IEnumerator AnimateFace2Sec2()
            {   
                notPlay2 = true;
                Sec2 = true;
                SkillAtk2();
                yield return new WaitForSeconds(3.6f); // รอเวลาอนิเมชัน 1 วินาที
                SkillAtk5();
                yield return new WaitForSeconds(4.8f); // รอเวลาอนิเมชัน 1 วินาที
                SkillAtk3();
                yield return new WaitForSeconds(5.5f); // รอเวลาอนิเมชัน 1 วินาที
                notPlay2 = false;
                Sec2 = false;
                EndSec1 = false;
                EndSec2 = true;
            }



//end Face2





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
        }
    }

    IEnumerator AnimateAttack1()
            {   
                isLock = true;
                DefBoss = true;
                animator.SetTrigger("atk1");
                yield return new WaitForSeconds(0.5f); // รอเวลาอนิเมชัน 1 วินาที
                audioSrc.PlayOneShot(AtkV2Sound);
                yield return new WaitForSeconds(0.6f); // รอเวลาอนิเมชัน 1 วินาที
                EFAtk1.SetActive(true);
                AreaSkillAtk1();
                yield return new WaitForSeconds(0.2f); // รอเวลาอนิเมชัน 1 วินาที

                yield return new WaitForSeconds(0.55f); // รอเวลาอนิเมชัน 1 วินาที
                skillAtk1 = false;
                DefBoss = false;
                isLock = false;
            }

    //End SkillAtk 01

    //start SkillAtk 02

    void SkillAtk2()
    {
        if(!skillAtk2)
        {   
        StartCoroutine(AnimateAttack2());
        skillAtk2 = true;
        }
    }

    void AreaSkillAtk2()
    {
        Collider2D ColiderInSide = Physics2D.OverlapBox(transform.position + skill2OffSet, skill2Range, 0f , PlayerMask);

        if (ColiderInSide != null)
        {
            ColiderInSide.GetComponent<PlayerHealth>().TakeDamage(attackDamage2);
        }
    }
    IEnumerator AnimateAttack2()
            {   
                isLock = true;
                animator.SetTrigger("atk2");
                DefBoss = true;
                yield return new WaitForSeconds(1.0f); // รอเวลาอนิเมชัน 1 วินาที
                EFAtk21.SetActive(true);
                EFAtk22.SetActive(true);
                AreaSkillAtk2();
                audioSrc.PlayOneShot(AtkV2Sound);
                yield return new WaitForSeconds(0.9f); // รอเวลาอนิเมชัน 1 วินาที
                EFAtk23.SetActive(true);
                AreaSkillAtk1();
                audioSrc.PlayOneShot(AtkV2Sound);
                yield return new WaitForSeconds(0.55f); // รอเวลาอนิเมชัน 1 วินาที
                EFAtk23.SetActive(true);
                AreaSkillAtk1();
                audioSrc.PlayOneShot(AtkV2Sound);
                yield return new WaitForSeconds(0.55f); // รอเวลาอนิเมชัน 1 วินาที
                EFAtk23.SetActive(true);
                AreaSkillAtk1();
                audioSrc.PlayOneShot(AtkV2Sound);
                yield return new WaitForSeconds(0.55f); // รอเวลาอนิเมชัน 1 วินาที
                skillAtk2 = false;
                DefBoss = false;
                isLock = false;
            }

    //End SkillAtk 02




    //Start SkillAtk 03

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
        }
    }   

    IEnumerator AnimateAttack3()
    {
        isLock = true;
        DefBoss = true;
        animator.SetTrigger("atk3");
        yield return new WaitForSeconds(0.3f);
        Jump();
        yield return new WaitForSeconds(1.7f); // รอเวลาอนิเมชัน 1 วินาที
        EFAtk3.SetActive(true);
        AreaSkillAtk3();
        DefBoss = false;
        yield return new WaitForSeconds(1.4f); // รอเวลาอนิเมชัน 1 วินาที
        skillAtk3 = false;
        isLock = false;
    }
    //End SkillAtk 03

    void Jump()
    {
        bossRigidbody.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
    }


    //Start SkillAtk 04

    void SkillAtk4Sec1()
    {
        if(!skillAtk4)
        {   
        StartCoroutine(AnimateAttack4Sec1());
        skillAtk4 = true;
        }
    }

    void AreaSkillAtk4()
    {
        Collider2D ColiderInSide = Physics2D.OverlapBox(transform.position + skill4OffSet, skill4Range, 0f , PlayerMask);
        Colider = ColiderInSide != null;

        if (ColiderInSide != null)
        {
            ColiderInSide.GetComponent<PlayerHealth>().TakeDamage(attackDamage4);
            hasAttack4 = true;
        }
    }

    IEnumerator AnimateAttack4Sec1()
    {
        isLock = true;
        DefBoss = true;
        animator.SetTrigger("atk4");
        yield return new WaitForSeconds(1.5f);
        ThorwAxe();
        yield return new WaitForSeconds(0.4f);
        spriteRendererXAttack();
        hasAttack4 = false;
        yield return new WaitForSeconds(1.2f); // รอเวลาอนิเมชัน 1 วินาที
        moveSpeedSkill4 = 20f;
        yield return new WaitForSeconds(1.4f); // รอเวลาอนิเมชัน 1 วินาที
        moveSpeedSkill4 = 0f;
        hasAttack4 = true;
        skillAtk4 = false;
        isLock = false;
        DefBoss = false;
    }

    void ThorwAxe()
    {
        var itemThrow = Instantiate(Item, launchPoint.position, launchPoint.rotation);
    }
    //End SkillAtk 04

    //ปาขวาน - พุ่งอัด





    //Start SkillAtk 05

    void SkillAtk5()
    {
        if(!skillAtk5)
        {   
        StartCoroutine(AnimateAttack5());
        skillAtk5 = true;
        }
    }

    void AreaSkillAtk5()
    {
        Collider2D ColiderInSide = Physics2D.OverlapBox(transform.position + skill5OffSet, skill5Range, 0f , PlayerMask);

        if (ColiderInSide != null)
        {
            ColiderInSide.GetComponent<PlayerHealth>().TakeDamage(attackDamage5);
            hasAttack4 = true;
        }
    }

    IEnumerator AnimateAttack5()
    {
        isLock = true;
        DefBoss = true;
        animator.SetTrigger("atk5");
        yield return new WaitForSeconds(1.2f);
        moveSpeedSkill5 = 7f;
        audioSrc.PlayOneShot(SpinSound);
        yield return new WaitForSeconds(0.6f);
        AreaSkillAtk5();
        yield return new WaitForSeconds(0.6f);
        AreaSkillAtk5();
        yield return new WaitForSeconds(0.6f);
        AreaSkillAtk5();
        yield return new WaitForSeconds(0.6f);
        AreaSkillAtk5();
        yield return new WaitForSeconds(0.6f);
        AreaSkillAtk5();
        yield return new WaitForSeconds(0.5f);
        AreaSkillAtk5();
        yield return new WaitForSeconds(0.1f); // รอเวลาอนิเมชัน 1 วินาที
        moveSpeedSkill5 = 0f;
        skillAtk5 = false;
        DefBoss = false;
        isLock = false;
    }








    //End SkillAtk 05



    void Moveanimate()
    {
        if(moveSpeedMove != 0)
        {
            animator.SetBool("walk",true);
        }
        else
        {
            animator.SetBool("walk",false);
        }
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
            
    void spriteRendererFlipX()
    {
        if(isLock == false)
        {
            if(DistancePVB < 0)
            {
            spriteRenderer.flipX = false;
            filpX = false;
            }
            else if(DistancePVB > 0)
            {
            spriteRenderer.flipX = true;
            filpX = true;
            } 
        }

        if(spriteRenderer.flipX == false)
        {
            skill1OffSet = new Vector3(-1.5f , skill1OffSet.y , skill1OffSet.z);
            skill3OffSet = new Vector3(-2.5f , skill3OffSet.y , skill3OffSet.z);
        }
        else if(spriteRenderer.flipX == true)
        {
            skill1OffSet = new Vector3(1.5f , skill1OffSet.y , skill1OffSet.z);
            skill3OffSet = new Vector3(2.5f , skill3OffSet.y , skill3OffSet.z);
        }

    }


}
