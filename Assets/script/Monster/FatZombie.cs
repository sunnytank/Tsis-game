using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatZombie : MonoBehaviour
{
    public CharacterState currentState;
    [Header("Info")]
    [SerializeField]
    private Rigidbody2D rb2d;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    int SacrificeDamage = 1000;

    [SerializeField]
    private float Delay;

    public Vector2 movelocation;
    
    private MonsterHealth monsterHealth;

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

    public Vector3 AttackPosition;
    public Vector2 SizeAttackBox;

    public Vector3 DetectPosition;
    public Vector2 SizeDetectBox;


    public bool PlayerInAttackRadio;
    public bool PlayerInDetectArea;

    [Header("Effect")]
    public GameObject sign1;
    public GameObject sign2;
    bool hasSign;

    [Header("Audio")]
    public AudioClip ZombieSound;
    public AudioClip ZombieAtkSound;
    public AudioClip ZombieDieSound;
    public AudioSource audioSrc;
    bool hasBoom;
    bool hasAggro;
    bool hasDie;
    
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
        player = GameObject.FindWithTag("Player").transform; // หา Player ด้วย Tag "Player"
        movelocation = transform.position;

        ZombieSound = Resources.Load<AudioClip>("zombieFatAggro");
        ZombieDieSound = Resources.Load<AudioClip>("ZombieDie");
        ZombieAtkSound = Resources.Load<AudioClip>("zombieBoom");
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        monsterMove();
        OnTrig();
        spriteRendererFlip();
        startRaycast = transform.position;

                switch (currentState) {
            case CharacterState.Idle:
                Idlestate = true;
                DetectObjects();
                IdleMove();
                FlipTimeAfterIdle();
                moveSpeed = 0;

                // Handle idle behavior
                break;
            case CharacterState.Suspect:
                Suspectstate = true;
                DetectObjects();
                Suspect();
                moveSpeed = 0;
                // Handle walking behavior
                break;
            case CharacterState.Attack:
                Attackstate = true;
                DetectObjects();
                Attack();
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
            if(!hasDie)
            {
                audioSrc.PlayOneShot(ZombieDieSound);
                hasDie = true;
            }
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
        
        if(sign2.activeSelf)
        {
            Invoke("Factive2", 2f);
        }
    }

    void Factive2()
    {
        sign2.SetActive(false);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = isOnGround ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + footOffset , footRadius);

        Gizmos.color = isHitFloor ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + floorOffset , floorRadius);

       // Gizmos.color = Color.white;
       // Gizmos.DrawWireSphere(transform.position, explosionRadius);

        Gizmos.color = PlayerInAttackRadio ? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position + AttackPosition , SizeAttackBox);

        Gizmos.color = PlayerInDetectArea ? Color.green : Color.blue;
        Gizmos.DrawWireCube(transform.position + DetectPosition , SizeDetectBox);

        Gizmos.color = isDetectPlayer ? Color.green : Color.yellow;
        Gizmos.DrawWireCube(transform.position + sightAngle, sightRange);

        Gizmos.color = isDetectPlayerInSide ? Color.green :Color.yellow;
        Gizmos.DrawWireCube(transform.position + sightAngleInside, sightRangeInside);

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

    void IdleMove()
    {
        float distance = Vector3.Distance(transform.position, movelocation);
            
            if (distance > 0.5f)
                {
                // หาทิศทางไปยังผู้เล่น
                    Vector2 SDirection = (movelocation - startRaycast).normalized;
                    direction.y = 0;
                    moveSpeed = 2;
                    transform.Translate(SDirection * moveSpeed * Time.deltaTime);  

                    if(SDirection.x != 0)
                    {
                        animator.SetBool("walk",true);                        
                    }
                    else if(SDirection.x == 0)
                    {
                        animator.SetBool("walk",false);
                    }  

                    if(SDirection.x < 0)
                        {
                        spriteRenderer.flipX = false;
                        }
                    else if(SDirection.x > 0)
                        {
                        spriteRenderer.flipX = true;
                        }                
                }
    }

    void FlipTimeAfterIdle()
    {
        if (spriteRenderer.flipX == false)
        {
            TimeFlipIdle += Time.deltaTime;
            if(TimeFlipIdle > 5)
            {
                TimeFlipIdle = 0;
                spriteRenderer.flipX = true;
            }
        }
        else if (spriteRenderer.flipX == true)
        {
            TimeFlipIdle += Time.deltaTime;
            if(TimeFlipIdle > 5)
            {
                TimeFlipIdle = 0;
                spriteRenderer.flipX = false;
            }
        }
    }

    void FatZombieMove()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    void Suspect()
    {
        direction.x = 0;
        sign1.SetActive(true);
        if(detectionTime >= 1f || detectionTime02 >= 10f && detectionTime >= 0.5f || detectionTime02 >= 24f)
        {
            ChangeState(CharacterState.Attack);
            sign1.SetActive(false);
            if(!hasAggro)
            {
                audioSrc.PlayOneShot(ZombieSound);
                hasAggro = true;
            }
        }

        if(inDetectionZone == false)
        {
            ChangeState(CharacterState.Idle);
            sign1.SetActive(false);
        }
    }

    void OnTrig()
    {
        Collider2D hitColliderGround = Physics2D.OverlapCircle(transform.position + footOffset, footRadius, groundLayerMask);
        isOnGround = hitColliderGround != null;

        Collider2D hitFloorColider = Physics2D.OverlapCircle(transform.position + floorOffset , floorRadius, floorLayerMask);
        isHitFloor = hitFloorColider != null;

    }

    void Attack()
    {
        Collider2D ColliderDetect = Physics2D.OverlapBox(transform.position + DetectPosition , SizeDetectBox ,0f , PlayerMask);
        PlayerInDetectArea = ColliderDetect != null;

        hasAggro = false;
            if(!hasSign)
            {
                sign2.SetActive(true);
                hasSign = true;
            }

        if (ColliderDetect != null && detectionTimedelay == 0f)
            {
                detectionTimeatk += Time.deltaTime;

                StartCoroutine(AnimateAttack());
                Debug.Log("Bomb Activate");

                float distance = Vector2.Distance(ColliderDetect.transform.position, ColliderDetect.transform.position);

                if (distance <= attackRange)
                {

                }
            }
            else if (ColliderDetect == null && timeToMove == true)
            {
            float distance = Vector3.Distance(transform.position, player.position);
            
            if(isOnGround == true && isHitFloor == false || isDetectPlayerInSide == true )
            {timeFlip = 0;
                if (distance > stoppingDistance)
                {
                // หาทิศทางไปยังผู้เล่น
                    Vector3 direction = (player.position - transform.position).normalized;
                    direction.y = 0;
                    moveSpeed = 2;
                    transform.Translate(direction * moveSpeed * Time.deltaTime);

                    if(direction.x !> 0)
                    {
                        spriteRenderer.flipX = true;
                    }
                    else if(direction.x !< 0)
                    {
                        spriteRenderer.flipX = false;
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

    void BombArea()
    {
        Collider2D hitPlayerCollider = Physics2D.OverlapBox(transform.position + AttackPosition , SizeAttackBox , 0f , PlayerMask);
        PlayerInAttackRadio = hitPlayerCollider != null;

        GetComponent<MonsterHealth>().TakeDamage(SacrificeDamage);

        if (hitPlayerCollider != null)
        {
            hitPlayerCollider.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            detectionTimeatk = 0f;
        }

    }

    IEnumerator AnimateAttack()
            {
                direction.x = 0;
                timeToMove = false;
                detectionTimedelay += Time.deltaTime;

                animator.SetTrigger("bomb");

                yield return new WaitForSeconds(0.9f); // รอเวลาอนิเมชัน 1 วินาที

                if(!hasBoom)
                {
                    audioSrc.PlayOneShot(ZombieAtkSound);
                    hasBoom = true;
                }

                if (hasAttacked == false) // ตรวจสอบว่ายังไม่มีการโจมตี
                {
                BombArea();
                hasAttacked = true;
                }
                yield return new WaitForSeconds(7f);
                timeToMove = true;
                detectionTimedelay = 0;
                hasAttacked = false;
                detectionTimeatk = 0f;
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

        void ChangeState(CharacterState newState) {
        Idlestate = false;
        Attackstate = false;
        Suspectstate = false;
        currentState = newState;
    }

        void DetectObjects()
    {
        if (spriteRenderer.flipX == false)
        {
            lineOfSight = Vector2.left;
        }
        else
        {
            lineOfSight = Vector2.right;
        }

        if(Idlestate || Suspectstate)
        {
            hasSign = false;
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

    void spriteRendererFlip()
    {
        if (spriteRenderer.flipX == false)
        {
            footOffset = new Vector3(-1.25f , footOffset.y , 0);
            floorOffset = Vector2.left;
        }
        else
        {
            footOffset = new Vector3(1.25f , footOffset.y , 0);
            floorOffset = Vector2.right;
        }
    }

}
