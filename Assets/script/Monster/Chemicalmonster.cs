using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chemicalmonster : MonoBehaviour
{
    // Start is called before the first frame update
    public CharacterState currentState;
    [Header("Info")]
    [SerializeField]
    private Rigidbody2D rb2d;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    public SpriteRenderer spriteRenderer;
    public bool SpriteRendererZom;

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
    bool hasDie;

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
    bool isDetectPlayerInSide;

    [Header("Detect02")]
    private float detectionTime;
    private float detectionTime02;
    private bool inDetectionZone;

    [Header("Attack")]
    public int attackDamage = 2;
    [SerializeField]
    private Vector2 attackRange;
    [SerializeField]
    private Vector3 areaAttack;
    [SerializeField]
    private Vector2 attackDeRange;
    [SerializeField]
    private Vector3 areaDeAttack;
    private bool hitAtkPlayerP;
    public Transform player;
    [SerializeField]
    public float stoppingDistance = 0.8f;
    private float detectionTimeatk;
    bool hasAttacked;
    bool hasSpawn;
    bool timeToMove = true;
    public float detectionTimedelay;

    [Header("Throw")]
    public Transform launchPoint;
    public Transform launchPointDie;
    public GameObject Item;
    public float launchSpeed = 5f;
    private bool isThrow;
    private float delayThrow;

    [Header("Effect")]
    public GameObject sign1;
    public GameObject sign2;
    bool hasSign;


    [Header("Audio")]
    public AudioClip AtkSound;
    public AudioClip DieSound;
    public AudioSource audioSrc;
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
        player = GameObject.FindWithTag("Player").transform; // หา Player ด้วย Tag "Player"

        AtkSound = Resources.Load<AudioClip>("chimeAwake");
        DieSound = Resources.Load<AudioClip>("ZombieDie");
        audioSrc = GetComponent<AudioSource>();
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
                SpawnAwakeOnDie();
                Diestate = true;
                break;
        }

        if(monsterHealth.currentHealth <= 0)
        {
            ChangeState(CharacterState.Die);
            if(!hasSoundDie)
            {
                audioSrc.PlayOneShot(DieSound);
                hasSoundDie = true;
            }
        }

        if(Input.GetKey(KeyCode.V) && isThrow == false)
        {
            SpawnThrowItem();
        }
        
        if(isThrow == true)
        {
            delayThrow += Time.deltaTime;
            if(delayThrow >= 2)
            {
            isThrow = false;
            delayThrow = 0;
            }
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

    void ChangeState(CharacterState newState) {
        Idlestate = false;
        Attackstate = false;
        Suspectstate = false;
        currentState = newState;
    }

    void SpawnThrowItem()
    {
        var itemThrow = Instantiate(Item, launchPoint.position, launchPoint.rotation);
        isThrow = true;
        detectionTimeatk = 0f;
    }

    void SpawnAwakeOnDie()
    {
        if(hasDie == false)
        {
            var itemThrow = Instantiate(Item, launchPointDie.position, launchPointDie.rotation);
            hasDie = true;
        }
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

        Gizmos.color = Color.black;
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
        if(detectionTime >= 1f || detectionTime02 >= 6f && detectionTime >= 2f || detectionTime02 >= 12f)
        {
            ChangeState(CharacterState.Attack);
            sign1.SetActive(false);
        }
        if(inDetectionZone == false)
        {
            ChangeState(CharacterState.Idle);
            sign1.SetActive(false);
        }

    }

    void DetectObjects()
    {

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
        if(Idlestate || Suspectstate)
        {
            hasSign = false;
        }

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
            sign1.SetActive(false);

            if(!hasSign)
            {
                sign2.SetActive(true);
                hasSign = true;
            }

            if (hitAttackCollider != null && detectionTimedelay == 0f)
            {
                detectionTimeatk += Time.deltaTime;
                StartCoroutine(AnimateAttack());

                float distance = Vector2.Distance(hitAttackCollider.transform.position, hitAttackCollider.transform.position);
                
                float distancePB = transform.position.x - player.position.x;

                if(distancePB < 0)
                    {
                        spriteRenderer.flipX = true;
                    }
                if(distancePB > 0)
                    {
                        spriteRenderer.flipX = false;
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
                    transform.Translate(direction * moveSpeed * Time.deltaTime);

                    if(direction.x > 0)
                    {
                        spriteRenderer.flipX = true;
                    }
                    else if(direction.x < 0)
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

        void AwakeArea()
    {
        Collider2D hitPlayerColliderG = Physics2D.OverlapBox(transform.position + areaAttack, attackRange, 0f, PlayerMask);

           if (hitPlayerColliderG != null)
        {
            hitPlayerColliderG.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            detectionTimeatk = 0f;
        }

    }

        IEnumerator AnimateAttack()
            {
                direction.x = 0;
                timeToMove = false;
                detectionTimedelay += Time.deltaTime;

                animator.SetTrigger("atk");
                audioSrc.PlayOneShot(AtkSound , 0.7f);

                yield return new WaitForSeconds(1f); // รอเวลาอนิเมชัน 1 วินาที
                if (hasAttacked == false) // ตรวจสอบว่ายังไม่มีการโจมตี
                {
                AwakeArea();
                hasAttacked = true;
                }

                yield return new WaitForSeconds(0.8f);
                if (hasSpawn == false) // ตรวจสอบว่ายังไม่มีการโจมตี
                {
                SpawnThrowItem();
                hasSpawn = true;
                }
                yield return new WaitForSeconds(0.5f);
                timeToMove = true;
                detectionTimedelay = 0;
                hasAttacked = false;
                hasSpawn = false;
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
        if (spriteRenderer.flipX == true)
        {
            footOffset = new Vector3(1f , footOffset.y , 0);
            floorOffset = Vector2.right;
            lineOfSight = Vector2.right;
            SpriteRendererZom = true;
            areaAttack = new Vector3(1.75f , areaAttack.y , 0);
        }
        else
        {
            footOffset = new Vector3(-1f , footOffset.y , 0);
            floorOffset = Vector2.left;
            lineOfSight = Vector2.left;
            SpriteRendererZom = false;
            areaAttack = new Vector3(-1.75f , areaAttack.y , 0);
        }
    }
}
