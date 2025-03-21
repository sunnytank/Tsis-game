using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalZombie : MonoBehaviour
{
    public CharacterState currentState;
    [Header("Info")]
    [SerializeField]
    private Rigidbody2D rb2d;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public Vector2 spawnPoint;

    public Vector2 SizeMoveBox;

    private MonsterHealth monsterHealth;

    [Header("State")]
    [SerializeField]
    private bool Idlestate;
    [SerializeField]
    private bool Suspectstate;
    [SerializeField]
    private bool Attackstate;
    [SerializeField]
    public bool Diestate;
    [SerializeField]
    float timeFlip;

    [Header("Layer Mask")]
    [SerializeField]
    private LayerMask groundLayerMask;
    [SerializeField]
    private LayerMask floorLayerMask;
    [SerializeField]
    private LayerMask PlayerMask;

    [Header("Movement")]
    [SerializeField]
    private float moveSpeed;
    public Vector2 direction;
    public Vector2 SDirection;
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
    public float TimeFlipIdle;

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
    private Vector2 attackRange;
    [SerializeField]
    private Vector3 areaAttack;
    [SerializeField]
    private Vector2 attackDeRange;
    [SerializeField]
    private Vector3 areaDeAttack;
    private bool hitAtkPlayerP;
    private bool hitAtkPlayer;
    [SerializeField]
    private int attackDamage;
    public Transform player;
    [SerializeField]
    public float stoppingDistance = 0.8f;
    [SerializeField]
    private float attackCooldown;
    [SerializeField]
    private float animetionAttackCooldown;
    private float detectionTimeatk;
    bool hasAttacked;
    bool timeToMove = true;
    public float detectionTimedelay;

    [Header("Effect")]
    public GameObject EfAtk;
    public GameObject sign1;
    public GameObject sign2;

    [Header("Audio")]
    public AudioClip ZombieSound;
    public AudioClip ZombieAtkSound;
    public AudioClip ZombieDieSound;
    public AudioSource audioSrc1;
    public AudioSource audioSrc2;
    bool haswalk;
    float moveSoundTime;
    bool hasSoundDie;



    public enum CharacterState {
    Idle,
    Attack,
    Suspect,
    Die
    }




    // Start is called before the first frame update
    void Start()
    {
        monsterHealth = GetComponent<MonsterHealth>();
        spawnPoint = transform.position;
        player = GameObject.FindWithTag("Player").transform; // หา Player ด้วย Tag "Player"

        ZombieSound = Resources.Load<AudioClip>("ZombieWalkV1");
        ZombieDieSound = Resources.Load<AudioClip>("zombieDie");
        ZombieAtkSound = Resources.Load<AudioClip>("zombieAtk");
    }


    // Update is called once per frame
    void Update()
    {
        spriteRendererFlip();
        OnTrig();

        startRaycast = transform.position; // ตำแหน่งเริ่มต้นของ Raycast
        switch (currentState) {
            case CharacterState.Idle:
                Idlestate = true;
                DetectObjects();
                movementIdle();

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

                Diestate = true;
                break;
        }

        if(monsterHealth.currentHealth <= 0)
        {
            ChangeState(CharacterState.Die);
            if(!hasSoundDie)
            {
                audioSrc1.PlayOneShot(ZombieDieSound);
                hasSoundDie = true;
            }
            audioSrc2.clip = ZombieSound;
            audioSrc2.Stop();
        }
        else
        {
            if(!haswalk)
            {
                audioSrc2.clip = ZombieSound;
                audioSrc2.Play();
                audioSrc2.volume = 0.1f;
                haswalk = true;
            }
        }

        if(haswalk)
        {
            moveSoundTime = Time.deltaTime;
            if(moveSoundTime >= 1.8f)
                {
                    haswalk = false;
                    moveSoundTime = 0f;
                }
        }

    }

    void ChangeState(CharacterState newState) {
        Idlestate = false;
        Attackstate = false;
        Suspectstate = false;
        currentState = newState;
    }

    void movementIdle()
    {
        monsterMove();
        transform.Translate(direction * moveSpeed * Time.deltaTime);  
    }

    void OnDrawGizmos()
    {
        Gizmos.color = isOnGround ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + footOffset , footRadius);

        Gizmos.color = isHitFloor ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + floorOffset , floorRadius);

        Gizmos.color = isDetectPlayer ? Color.green : Color.yellow;
        Gizmos.DrawWireCube(transform.position + sightAngle, sightRange);

        Gizmos.color = isDetectPlayerInSide ? Color.green :Color.red;
        Gizmos.DrawWireCube(transform.position + sightAngleInside, sightRangeInside);

        Gizmos.color = hitAtkPlayerP ? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position + areaDeAttack, attackDeRange);

        Gizmos.color = hitAtkPlayer ? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position + areaAttack, attackRange);
       

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
        Gizmos.DrawLine(startRaycast ,startRaycast + lineOfSight * rayDistance);

    }


    void monsterMove()
    {
        if( isHitFloor == true || isOnGround == false)
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

        if(direction.x != 0 ){
        animator.SetBool("walk", true);
        }
        else
        {
        animator.SetBool("walk", false);
        }

    }

        void Suspect()
    {
        direction.x = 0;
        sign1.SetActive(true);
        if(detectionTime >= 0.25f || detectionTime02 >= 5f && detectionTime >= 0.125f || detectionTime02 >= 8f)
        {
            ChangeState(CharacterState.Attack);
            sign1.SetActive(false);
        }

    }

    void DetectObjects()
    {
        if (spriteRenderer.flipX == true)
        {
            lineOfSight = Vector2.right;
        }
        else
        {
            lineOfSight = Vector2.left;
        }

        RaycastHit2D hitRayCastF = Physics2D.Raycast(startRaycast, lineOfSight, rayDistance, floorLayerMask);
        hitFloor = hitRayCastF;

        RaycastHit2D hitRayCastP = Physics2D.Raycast(startRaycast, lineOfSight, rayDistance, PlayerMask);
        hitPlayer = hitRayCastP;

        RaycastHit2D hitRayCastY = Physics2D.Raycast(hitRayCastP.point, lineOfSight - hitRayCastP.point, rayDistance, floorLayerMask);
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

        void AttackP()
    {
            Collider2D hitAttackCollider = Physics2D.OverlapBox(transform.position + areaDeAttack, attackDeRange, 0f, PlayerMask);
            hitAtkPlayerP = hitAttackCollider != null;

            sign2.SetActive(true);

            if (hitAttackCollider != null)
            {
                detectionTimeatk += Time.deltaTime;
                atkTime();

                float distance = Vector2.Distance(hitAttackCollider.transform.position, hitAttackCollider.transform.position);

                if (distance <= attackRange.x)
                {

                }
            }
            else if (hitAttackCollider == null && timeToMove == true)
            {
            float distance = Vector3.Distance(transform.position, player.position);
            
                if (distance > stoppingDistance)
                {
                // หาทิศทางไปยังผู้เล่น
                    Vector3 direction = (player.position - transform.position).normalized;
                    direction.y = 0;
                    transform.Translate(direction * moveSpeed * Time.deltaTime);

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
    }

        
        void attackInfo(){

             Collider2D hitPlayerColliderI = Physics2D.OverlapBox(transform.position + areaAttack, attackRange, 0f , PlayerMask);
             hitAtkPlayer = hitPlayerColliderI != null;

                if (hitPlayerColliderI != null)
                    {
                        EfAtk.SetActive(true);
                        hitPlayerColliderI.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

                        detectionTimeatk = 0f;
                    } 
        }

            void atkTime()
            {
                detectionTimedelay += Time.deltaTime;
                if (hasAttacked == false) // ตรวจสอบว่ายังไม่มีการโจมตี
                {
                audioSrc1.PlayOneShot(ZombieAtkSound);
                attackInfo();
                Debug.Log("attackInfo();");
                hasAttacked = true;
                }

                if(detectionTimeatk >= 0.8f)
                {
                    Debug.Log("detectionTimedelay = 0;");
                    detectionTimedelay = 0;
                    hasAttacked = false;
                    detectionTimeatk = 0f;
                }

            }

    void OnTrig()
    {
        Collider2D hitColliderGround = Physics2D.OverlapCircle(transform.position + footOffset, footRadius, groundLayerMask);
        isOnGround = hitColliderGround != null;

        Collider2D hitFloorColider = Physics2D.OverlapCircle(transform.position + floorOffset , floorRadius, floorLayerMask);
        isHitFloor = hitFloorColider != null;

    }

     void spriteRendererFlip()
    {
        if (spriteRenderer.flipX == false)
        {
            footOffset = new Vector3(-1f , footOffset.y , 0);
            floorOffset = Vector2.left;
            areaAttack = new Vector3(-0.2f ,areaAttack.y , 0);
            areaDeAttack = new Vector3(-0.2f ,areaAttack.y , 0);
        }
        else
        {
            footOffset = new Vector3(1f , footOffset.y , 0);
            floorOffset = Vector2.right;
            areaAttack = new Vector3(0.2f ,areaAttack.y , 0);
            areaDeAttack = new Vector3(0.2f ,areaAttack.y , 0);
        }
    }
}
