using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bearmonster : MonoBehaviour
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
    public LayerMask targetLayer;

    [Header("Movement")]
    [SerializeField]
    private float moveSpeed;
    public Vector2 direction;
    [SerializeField]
    private Vector3 footOffset;
    [SerializeField]
    private float footRadius;
    [SerializeField]
    private Vector3 floorOffset;
    [SerializeField]
    private float floorRadius;
    private bool isOnGround;
    private bool isHitFloor;

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
    private Vector2 startRaycast;
    [SerializeField]
    private float rayDistance;
    public Vector2 lineOfSight;
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
    private float attackRange;
    [SerializeField]
    private Vector3 areaAttack;
    [SerializeField]
    private float attackDeRange;
    [SerializeField]
    private Vector3 areaDeAttack;
    private bool hitAtkPlayerP;
    private bool hitAtkPlayer;
    [SerializeField]
    private int attackDamage;
    public Transform player;
    [SerializeField]
    public float stoppingDistance = 1;
    [SerializeField]
    private float attackCooldown;
    [SerializeField]
    private float animetionAttackCooldown;
    private float detectionTimeatk;
    bool hasAttacked;
    bool timeToMove = true;
    float detectionTimedelay;

    [Header("Effect")]
    public GameObject EfAttack;
    public GameObject sign1;
    public GameObject sign2;
    bool hasSign;

    [Header("Audio")]
    public AudioClip BearSound;
    public AudioClip BearAtkSound;
    public AudioSource audioSrc;

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

        BearSound = Resources.Load<AudioClip>("bearSound");
        BearAtkSound = Resources.Load<AudioClip>("bearAtk");
        audioSrc = GetComponent<AudioSource>();
    }

    void Update()
    {
        //state();
        //function update
        spriteRendererFlip();
        startRaycast = transform.position;

        if(monsterHealth.currentHealth <= 0)
        {
            ChangeState(CharacterState.Die);
        }

        switch (currentState) {
            case CharacterState.Idle:
                Idlestate = true;
                idleCity();
                DetectObjects();
                // Handle idle behavior
                break;
            case CharacterState.Suspect:
                Suspectstate = true;
                Suspect();
                DetectObjects();
                // Handle walking behavior
                break;
            case CharacterState.Attack:
                Attackstate = true;
                AttackP();
                DetectObjects();
                // Handle walking behavior
                break;
            case CharacterState.Die:
                // Handle running behavior
                BearDie();
                Diestate = true;
                break;
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
        
        Collider2D hitColliderGround = Physics2D.OverlapCircle(transform.position + footOffset, footRadius, groundLayerMask);
        isOnGround = hitColliderGround != null;

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
        Gizmos.color = isOnGround ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + footOffset , footRadius);

        Gizmos.color = isHitFloor ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + floorOffset , floorRadius);

        Gizmos.color = isDetectPlayer ? Color.green : Color.yellow;
        Gizmos.DrawWireCube(transform.position + sightAngle, sightRange);

        Gizmos.color = isDetectPlayerInSide ? Color.green :Color.red;
        Gizmos.DrawWireCube(transform.position + sightAngleInside, sightRangeInside);

        Gizmos.color = hitAtkPlayerP ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + areaDeAttack, attackDeRange);

        Gizmos.color = hitAtkPlayer ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + areaAttack, attackRange);

        /*Gizmos.color = isOnPlayer ? Color.yellow : Color.red;
        Gizmos.DrawWireCube(transform.position + point1 , point2);
        Collider2D[] collidersInArea = Physics2D.OverlapAreaAll(point1, point2);*/

        Gizmos.color = Color.black;
        if (hitPlayerI)
        {
            Gizmos.color = Color.green;
        }
        else if (hitPlayer && hitFloor && hitFloorI)
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

        
        Gizmos.DrawLine(startRaycast ,startRaycast + lineOfSight * rayDistance); // วาดเส้นในกรณีที่ไม่มีการชน

        //Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);


    }

    void BearDie()
    {
        
    }
    

    void idleCity()
    {
        //monster เดิน
        monsterMove();
        transform.Translate(direction * moveSpeed * Time.deltaTime);
       
    }

    void Suspect()
    {
        direction.x = 0;
        sign1.SetActive(true);
        if(detectionTime >= 3f || detectionTime02 >= 10f && detectionTime >= 1f || detectionTime02 >= 24f)
        {
            audioSrc.PlayOneShot(BearSound);
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
            Collider2D hitAttackCollider = Physics2D.OverlapCircle(transform.position + areaDeAttack, attackDeRange, PlayerMask);
            hitAtkPlayerP = hitAttackCollider != null;
            sign1.SetActive(false);

            if(!hasSign)
            {
                sign2.SetActive(true);
                hasSign = true;
            }

            if (hitAttackCollider != null && detectionTimedelay == 0f)
            {
                detectionTimeatk += Time.deltaTime;
                attackDelay();

                float distance = Vector2.Distance(hitAttackCollider.transform.position, hitAttackCollider.transform.position);

                if (distance <= attackRange)
                {

                }
            }
            else if (hitAttackCollider == null && timeToMove == true)
            {
            float distance = Vector3.Distance(transform.position, player.position);

            if(isOnGround == true && isHitFloor == false || isDetectPlayerInSide == true )
            {timeFlip = 0;
                if (distance > stoppingDistance)
                {
                // หาทิศทางไปยังผู้เล่น
                    Vector3 direction = (player.position - transform.position).normalized;
                    direction.y = 0;
                    transform.Translate(direction * moveSpeed*2 * Time.deltaTime);

                    if(direction.x !> 0)
                    {
                        spriteRenderer.flipX = true;
                    }
                    else if(direction.x !< 0)
                    {
                        spriteRenderer.flipX = false;
                    }
                    
                    if(direction.x != 0 )
                    {
                        animator.SetBool("walk", true);
                    }
                    else
                    {
                        animator.SetBool("walk", false);
                    }
                }
                else
                {
                    Debug.LogWarning("Player object is missing or destroyed!");
                }
            }
            else if(isOnGround == false && isDetectPlayerInSide == false || isHitFloor == true && isDetectPlayerInSide == false)
            {  
                timeFlip += Time.deltaTime;
                if(timeFlip >= 1)
                {
                ChangeState(CharacterState.Idle);
                timeFlip = 0;
                }
            }
        }    
    }


        void attackDelay()
                {
                StartCoroutine(AnimateAttack());
                }
        
        void attackInfo(){

             Collider2D[] hitAttackColliderI = Physics2D.OverlapCircleAll(transform.position + areaAttack, attackRange, PlayerMask);
             hitAtkPlayer = hitAttackColliderI != null;

                foreach (Collider2D player in hitAttackColliderI)
                    {
                        Debug.Log("hit R " + player.name);

                        player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

                        detectionTimeatk = 0f;
                    } 
        }
        
        IEnumerator AnimateAttack()
            {
                direction.x = 0;
                timeToMove = false;
                detectionTimedelay += Time.deltaTime;

                animator.SetTrigger("atk");
                
                audioSrc.PlayOneShot(BearAtkSound , 0.7f);


                yield return new WaitForSeconds(0.5f); // รอเวลาอนิเมชัน 1 วินาที
                EfAttack.SetActive(true);

                if (hasAttacked == false) // ตรวจสอบว่ายังไม่มีการโจมตี
                {
                attackInfo();
                hasAttacked = true;
                }
                yield return new WaitForSeconds(0.2f);
                timeToMove = true;
                detectionTimedelay = 0;
                hasAttacked = false;
                detectionTimeatk = 0f;
            }
    

    void ChangeState(CharacterState newState) {
        Idlestate = false;
        Attackstate = false;
        Suspectstate = false;
        currentState = newState;
    }





    public void DetectObjects()
    {
        if (spriteRenderer.flipX == true)
        {
            lineOfSight = Vector2.right;
        }
        else
        {
            lineOfSight = Vector2.left;
        }

        RaycastHit2D hitRayCastF = Physics2D.Raycast(startRaycast, lineOfSight, rayDistance, targetLayer);
        hitFloor = hitRayCastF;

        RaycastHit2D hitRayCastP = Physics2D.Raycast(startRaycast, lineOfSight, rayDistance, PlayerMask);
        hitPlayer = hitRayCastP;

        RaycastHit2D hitRayCastY = Physics2D.Raycast(hitRayCastP.point, lineOfSight - hitRayCastP.point, rayDistance, targetLayer);
        hitFloorI = hitRayCastY;

        Collider2D DetectColider = Physics2D.OverlapBox(transform.position + sightAngle, sightRange, 0f , PlayerMask);
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

        if (hitRayCastF.collider == null && hitRayCastP.collider == null)
        {
            hitPlayerI = false;
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
        else if (hitRayCastF.collider != null && hitRayCastP.collider != null && hitRayCastY.collider != null)
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
        if( isHitFloor == true || isOnGround == false)
        {
            direction.x = 0;
            timeFlip += Time.deltaTime;
                if(timeFlip >= 2){
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
        else if(isHitFloor == false && isOnGround == true)
        {

            if (spriteRenderer.flipX == true)
            {
                direction.x = moveSpeed;
            }
            else if (spriteRenderer.flipX == false)
            {
                direction.x = -moveSpeed;
            }
        }
        else{
            if (spriteRenderer.flipX == true)
            {
                direction.x = moveSpeed;
            }
            else if (spriteRenderer.flipX == false)
            {
                direction.x = -moveSpeed;
            }
        }
    }

    void spriteRendererFlip()
    {
        if (spriteRenderer.flipX == true)
        {
            footOffset = new Vector3(1.2f , footOffset.y , 0);
            floorOffset = new Vector3(1.2f , floorOffset.y , 0);
            areaAttack = new Vector3(1.4f , areaAttack.y , 0);
            areaDeAttack = new Vector3(0.5f , areaDeAttack.y , 0);
        }
        else
        {
            footOffset = new Vector3(-1.2f , footOffset.y , 0);
            floorOffset = new Vector3(-1.2f , floorOffset.y , 0);
            areaAttack = new Vector3(-1.4f , areaAttack.y , 0);
            areaDeAttack = new Vector3(-0.5f , areaDeAttack.y , 0);
        }
        if(direction.x != 0 ){
        animator.SetBool("walk", true);
        }
        else
        {
        animator.SetBool("walk", false);
        }
    }


}
        



