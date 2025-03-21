using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Birdmonster : MonoBehaviour
{
    public CharacterState currentState;
    [Header("Info")]
    [SerializeField]
    private Rigidbody2D rb2d;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float Delay;

    [Header("Layer Mask")]
    [SerializeField]
    private LayerMask groundLayerMask;
    [SerializeField]
    private LayerMask floorLayerMask;
    [SerializeField]
    private LayerMask PlayerMask;
    [SerializeField]
    private LayerMask groundandfloorLayerMask;

    [Header("Movement")]
    [SerializeField]
    private float moveSpeed;
    public Vector2 direction;
    [SerializeField]
    private Vector3 floorOffset;
    [SerializeField]
    private float floorRadius;
    private bool isHitFloor;
    [SerializeField]
    float groundDistance;
    [SerializeField]
    float groundDistanceOut;
    [SerializeField]
    Vector3 lineOfGround;
    [SerializeField]
    bool hitrayground;
    [SerializeField]
    bool hitraygroundOut;
    [SerializeField]
    private Vector3  RaycastGround;


    [Header("State")]
    [SerializeField]
    private bool Idlestate;
    [SerializeField]
    private bool Suspectstate;
    [SerializeField]
    private bool Attackstate;
    [SerializeField]
    private bool Diestate;
    [SerializeField]
    float timeFlip;

    private MonsterHealth monsterHealth;

    [Header("Detect")]
    [SerializeField]
    private Vector3 sightAngle;
    [SerializeField]
    private Vector2 sightRange;
    [SerializeField]
    private Vector3 sightAngleInside;
    [SerializeField]
    private Vector2 sightRangeInside;
    private bool isOnPlayer;
    [SerializeField]
    private Vector3 startRaycast;
    [SerializeField]
    private float rayDistance;
    public Vector3 lineOfSight;
    private bool hitPlayer;
    private bool hitFloor;
    private bool hitPlayerI;
    private bool hitFloorI;
    private bool donthitPlayerO;
    private bool isDetectPlayer;
    private bool dontseePlayer;
    bool isDetectPlayerInSide;

    [Header("Detect02")]
    private float detectionTime;
    private float detectionTime02;
    private bool inDetectionZone;

    [Header("Attack")]
    [SerializeField]
    private Vector3 startRaycastAtk;
    [SerializeField]
    private Vector3 lineOfSightAtk;
    [SerializeField]
    private float rayDistanceAtk;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private Vector3 areaAttack;
    private bool hitAtkPlayerP;
    [SerializeField]
    private int attackDamage;
    public Transform player;
    private float detectionTimeatk;
    bool hasAttacked;
    float detectionTimedelay;
    float rushSpeed = 2f;
    bool attackPlayer;
    bool hitAtkFloorGround;
    float relayTime;

    [Header("Effect")]
    public GameObject EfAtk;
    public GameObject sign1;
    public GameObject sign2;
    bool hasSign;

    [Header("Audio")]
    public AudioClip BirdSound;
    public AudioClip BirdAtkSound;
    public AudioSource audioSrc;
    bool hasSound;

    public enum CharacterState {
    Idle,
    Attack,
    Suspect,
    Die
    }
    void Start()
    {
        monsterHealth = GetComponent<MonsterHealth>();
        player = GameObject.FindWithTag("Player").transform; // หา Player ด้วย Tag "Player"

        BirdSound = Resources.Load<AudioClip>("birdAggro");
        BirdAtkSound = Resources.Load<AudioClip>("birdAtk");

        audioSrc = GetComponent<AudioSource>();

    }

    void Update()
    {
        //state();
        //function update
        monsterMove();
        switch (currentState) {
            case CharacterState.Idle:
                Idlestate = true;
                DetectObjects();
                idleCity();
                // Handle idle behavior
                break;
            case CharacterState.Suspect:
                Suspectstate = true;
                DetectObjects();
                Suspect();
                // Handle walking behavior
                break;
            case CharacterState.Attack:
                Attackstate = true;
                AttackP();
                // Handle walking behavior
                break;
            case CharacterState.Die:
                // Handle running behavior
                BirdDie();
                Diestate = true;
                break;
        }
        
        if(Input.GetKeyDown(KeyCode.P))
        {
            ChangeState(CharacterState.Idle);
            if(spriteRenderer.flipX == true)
            {
                direction.x = 2;
            }
            else if(spriteRenderer.flipX == false)
            {
                direction.x = -2;
            }
        }


        if(monsterHealth.currentHealth <= 0)
        {
            ChangeState(CharacterState.Die);
        }

        if(Idlestate == true)
        {
        }
        else if (Suspectstate == true)
        {
        }
        else if (Attackstate == true)
        {
        }
        else if (Diestate == true)
        {
        }

        RaycastGround = transform.position;

        RaycastHit2D hitRayGround = Physics2D.Raycast(RaycastGround, lineOfGround, groundDistance, groundLayerMask);
        hitrayground = hitRayGround;

        RaycastHit2D hitRayGroundOut = Physics2D.Raycast(RaycastGround, lineOfGround, groundDistanceOut , groundLayerMask);
        hitraygroundOut = hitRayGroundOut;

        Collider2D hitFloorColider = Physics2D.OverlapCircle(transform.position + floorOffset , floorRadius, floorLayerMask);
        isHitFloor = hitFloorColider != null;

        if(sign2.activeSelf)
        {
            Invoke("Factive2", 2f);
        }
    }

    void Factive2()
    {
        sign2.SetActive(false);
    }

    private void OnDrawGizmos(){

        Gizmos.color = isHitFloor ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + floorOffset , floorRadius);

        Gizmos.color = isDetectPlayer ? Color.green : Color.yellow;
        Gizmos.DrawWireCube(transform.position + sightAngle, sightRange);

        Gizmos.color = isDetectPlayerInSide ? Color.green :Color.red;
        Gizmos.DrawWireCube(transform.position + sightAngleInside, sightRangeInside);

        Gizmos.color = hitAtkPlayerP ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + areaAttack, attackRange);

        /*Gizmos.color = isOnPlayer ? Color.yellow : Color.red;
        Gizmos.DrawWireCube(transform.position + point1 , point2);
        Collider2D[] collidersInArea = Physics2D.OverlapAreaAll(point1, point2);*/

        Gizmos.color = Color.black;

        if (hitPlayer && hitFloor && hitFloorI)
        {
            Gizmos.color = Color.red;
        }
        else if (hitPlayer && hitFloor && !hitFloorI)
        {
            Gizmos.color = Color.green;
        }
        else if (hitPlayer && !hitFloor)
        {
            Gizmos.color = Color.green;
        }

        
        Gizmos.DrawLine(RaycastGround + startRaycast ,RaycastGround + startRaycast + lineOfSight * rayDistance); // วาดเส้นในกรณีที่ไม่มีการชน

        Gizmos.color = Color.red;
        if (hitrayground)
        {
            Gizmos.color = Color.green;
        }
        Gizmos.color = Color.black;
        if (hitraygroundOut)
        {
            Gizmos.color = Color.yellow;
        }

        Gizmos.DrawLine(RaycastGround ,RaycastGround + lineOfGround * groundDistance);

        Gizmos.DrawLine(RaycastGround ,RaycastGround + lineOfGround * groundDistanceOut); // วาดเส้นในกรณีที่ไม่มีการชน

        //Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
    }

    void BirdDie()
    {
        direction.y = -2f;
        direction.x = 0f;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

    }
    

    void idleCity()
    {
        monsterFly();
        monsterMove();
        transform.Translate(direction * moveSpeed * Time.deltaTime);

    }

    void Suspect()
    {
        monsterFly();
        direction.x = 0;
        sign1.SetActive(true);
        if(detectionTime >= 3f || detectionTime02 >= 10f && detectionTime >= 1f || detectionTime02 >= 24f)
        {
            audioSrc.PlayOneShot(BirdSound);
            ChangeState(CharacterState.Attack);
            sign1.SetActive(false);
        }

        if(inDetectionZone == false)
        {
            ChangeState(CharacterState.Idle);
            sign1.SetActive(false);
        }
    }


    void AttackP()
    {

            Collider2D hitAttackCollider = Physics2D.OverlapCircle(transform.position + areaAttack, attackRange, PlayerMask);
            hitAtkPlayerP = hitAttackCollider != null;

            Vector3 directionT = (player.position - transform.position);   

            if(!hasSign)
            {
                sign2.SetActive(true);
                hasSign = true;
            } 

            if (hitAttackCollider != null && detectionTimedelay == 0f)
            {
                EfAtk.SetActive(false);
                attackInfo();
            }

            else if (hitAttackCollider == null && detectionTimeatk == 0f )
            {
                relayTime += Time.deltaTime;

                animator.SetBool("atk", true);

                float speedX = rushSpeed + 2f;
                float speedY = rushSpeed;

                Vector3 rushTheRush = new Vector3(directionT.x * speedX, directionT.y * speedY, 0f);

                if(relayTime >= 1)
                {
                    transform.Translate(rushTheRush * rushSpeed * Time.deltaTime);
                    EfAtk.SetActive(true);
                    detectionTimedelay = 0f;

                    if(!hasSound)
                    {
                        audioSrc.PlayOneShot(BirdAtkSound);
                        hasSound = true;
                    }
                }

            }   
            else
            {
            }

            if(detectionTimeatk > 0f)
                {
                    if(hitrayground == false && hitraygroundOut == true)
                    //บินในระยะปกติ
                    {
                    detectionTimeatk = 0f;
                    rushSpeed = 2f;
                    relayTime = 0f;
                    hasSound = false;
                    }
                    else
                    {
                    monsterFly();
                    }
                }

            if (directionT.x > 0)
                        {
                            spriteRenderer.flipX = true; // หันหน้าไปทางขวา เมื่อผู้เล่นอยู่ด้านขวา
                        }
            else if (directionT.x < 0)
                        {
                            spriteRenderer.flipX = false; // หันหน้าไปทางซ้าย เมื่อผู้เล่นอยู่ด้านซ้าย
                        }

            

    }
        
        void attackInfo(){

             Collider2D[] hitAttackCollider = Physics2D.OverlapCircleAll(transform.position + areaAttack, attackRange, PlayerMask);
             attackPlayer = hitAttackCollider != null;

             Collider2D[] hitAttackColliderFloorGround = Physics2D.OverlapCircleAll(transform.position + areaAttack, attackRange, groundandfloorLayerMask);
             hitAtkFloorGround = hitAttackColliderFloorGround != null;

                    animator.SetBool("atk", false);

                    if(hitAttackCollider != null && detectionTimedelay == 0)
                    {
                        player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

                        rushSpeed = 0f;

                        detectionTimedelay += Time.deltaTime;

                        detectionTimeatk += Time.deltaTime;

                        relayTime = 0f;
                    }
                    else if(hitAttackColliderFloorGround != null)
                    {
                        rushSpeed = 0f;

                        detectionTimedelay += Time.deltaTime;

                        detectionTimeatk += Time.deltaTime;

                        relayTime = 0f;
                    }
        }
    
    

    void ChangeState(CharacterState newState) {
        Idlestate = false;
        Attackstate = false;
        Suspectstate = false;
        currentState = newState;
    }





    void DetectObjects()
    {

         // ตำแหน่งเริ่มต้นของ Raycast
        if (spriteRenderer.flipX == true)
        {
            lineOfSight = new Vector2(1, 0);
        }
        else
        {
            lineOfSight = new Vector2(-1, 0);
        }

        RaycastHit2D hitRayCastF = Physics2D.Raycast(RaycastGround + startRaycast, lineOfSight, rayDistance, floorLayerMask);
        hitFloor = hitRayCastF;

        RaycastHit2D hitRayCastP = Physics2D.Raycast(RaycastGround + startRaycast, lineOfSight, rayDistance, PlayerMask);
        hitPlayer = hitRayCastP;

        Vector3 startPoint = new Vector3(hitRayCastP.point.x, hitRayCastP.point.y, 0f); // แปลง Vector2 เป็น Vector3
        Vector3 direction = lineOfSight - startPoint; // ทำการลบเวกเตอร์

        RaycastHit2D hitRayCastY = Physics2D.Raycast(startPoint, direction.normalized, rayDistance, floorLayerMask);
        hitFloorI = hitRayCastY; 


        Collider2D DetectColider = Physics2D.OverlapBox(RaycastGround + sightAngle, sightRange, 0f , PlayerMask);
        isDetectPlayer = DetectColider;

        Collider2D DetectColiderInSide = Physics2D.OverlapBox(transform.position + sightAngleInside, sightRangeInside, 0f , PlayerMask);
        isDetectPlayerInSide = DetectColiderInSide;
        
        // เพิ่มเวลาที่ detect

        // ทำสิ่งที่ต้องการ เช่น รีเซ็ตการนับเวลาหรือส่งอีเวนต์ต่าง ๆ
        if (DetectColiderInSide != null)
        {
            ChangeState(CharacterState.Attack);
        }

        if(Idlestate || Suspectstate)
        {
            hasSign = false;
        }

        
        // เพิ่มเวลาที่ detect

        // ทำสิ่งที่ต้องการ เช่น รีเซ็ตการนับเวลาหรือส่งอีเวนต์ต่าง ๆ

        if (hitRayCastF.collider == null && hitRayCastP.collider == null)
        {
            hitPlayer = false;
            hitFloorI = false;
        }
        else if (hitRayCastP.collider != null && hitRayCastF.collider == null)
        {
            if (Attackstate == true)
            {
            }
            else if(Idlestate == true && Attackstate == false)
            {
                ChangeState(CharacterState.Suspect);
            }

            OnPlayerEnterDetect2D(hitRayCastP.collider);
            
            if(hitRayCastP.distance <= sightRange.x)
            {
                detectionTime += Time.deltaTime;
            }       
        }
        else if (hitRayCastF.collider != null && hitRayCastP.collider != null && hitRayCastY.collider == null)
        {
            //hitRayCastP.point, lineOfSight - hitRayCastP.point กำหนดระยะระหว่างทาง
            if(Idlestate == true && Attackstate == false)
            {
                ChangeState(CharacterState.Suspect);
            }
            if(hitRayCastP.distance <= sightRange.x)
            {
                detectionTime += Time.deltaTime;
            }
        }
        else{
            // รีเซ็ตเวลาที่ detect ถ้าอยู่นอกระยะ detect
            //detectionTime = 0f;
        }

        if (DetectColider != null)
        {
            inDetectionZone = true;
        }
        else if(DetectColider == null)
        {
            inDetectionZone = false;
            detectionTime = 0f;
            detectionTime02 = 0f;
        }

        if (inDetectionZone == true)
        {
            detectionTime02 += Time.deltaTime;  
        }

    }
    
    void OnPlayerEnterDetect2D(Collider2D hitCollider)
    {
    if (hitCollider.CompareTag("Player"))
    {
        // ไม่มีการกระทำเพิ่มเติมเมื่อผู้เล่นโดนเส้นทาง
    }
    else if (hitCollider.CompareTag("Water"))
    {
        // ไม่มีการกระทำเพิ่มเติมเมื่อชนกับน้ำ
    }
    else
    {
        inDetectionZone = false;
        // ทำงานเมื่อชนกับสิ่งอื่นที่ไม่ใช่ Player หรือ Water
    }
    // ทำสิ่งที่ต้องการ เช่น รีเซ็ตการนับเวลาหรือส่งอีเวนต์ต่าง ๆ
    }
    
    
    void OnPlayerEnterDetect3D(Collider hitCollider)
    {
    if (hitCollider.CompareTag("Player"))
    {
        // ไม่มีการกระทำเพิ่มเติมเมื่อผู้เล่นโดนเส้นทาง
    }
    else if (hitCollider.CompareTag("Water"))
    {
        // ไม่มีการกระทำเพิ่มเติมเมื่อชนกับน้ำ
    }
    else
    {
        inDetectionZone = false;
        // ทำงานเมื่อชนกับสิ่งอื่นที่ไม่ใช่ Player หรือ Water
    }
    // ทำสิ่งที่ต้องการ เช่น รีเซ็ตการนับเวลาหรือส่งอีเวนต์ต่าง ๆ
    }

    void monsterMove()
    {
        if(direction.x != 0 ){
        animator.SetBool("walk", true);
        }
        else
        {
        animator.SetBool("walk", false);
        }

        if( isHitFloor == true)
        {
            direction.x = 0;
            timeFlip += Time.deltaTime;
                if(timeFlip >= 1){
                    if (spriteRenderer.flipX == true)
                    {
                        spriteRenderer.flipX = false;
                        timeFlip = 0;
                    }
                    else if (spriteRenderer.flipX == false)
                    {
                        spriteRenderer.flipX = true;
                        timeFlip = 0;
                    }
                }
        }
        else{
            if (spriteRenderer.flipX == false)
                        {
                            direction.x = -moveSpeed;
                        }
                    else if (spriteRenderer.flipX == true)
                        {
                            direction.x = moveSpeed;
                        }
        }

        if (spriteRenderer.flipX == true)
        {
            floorOffset = new Vector3(2 , floorOffset.y , 0);
        }
        else
        {
            floorOffset = new Vector3(-2 , floorOffset.y , 0);
        }
        

    }

    void monsterFly()
    {
        Vector3 movement = Vector3.zero;

        if(hitrayground == true)
        {
            movement.y = 0.01f; // เคลื่อนที่ตามแกน y ขึ้น

        }
        else if(hitrayground == false && hitraygroundOut == false)
        {
            movement.y = -0.01f; // เคลื่อนที่ตามแกน y ลง

        }
        transform.Translate(movement);
    }


}
        



