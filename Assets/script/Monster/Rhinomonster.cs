using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Rhinomonster : MonoBehaviour
{
    public enum CharacterState {
    Idle,
    Attack,
    Suspect,
    Die
    }
    
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
    private Vector2 attackRangeLarge;
    [SerializeField]
    private Vector3 areaAttackLarge;
    private bool hitAtkPlayerP;
    [SerializeField]
    private int attackDamage;
    public Transform player;
    [SerializeField]
    public float stoppingDistance = 1;
    [SerializeField]
    private float detectionTimeatk;
    [SerializeField]
    bool hasAttacked;
    [SerializeField]
    float detectionTimedelay;
    [SerializeField]
    bool hitInAtkPlayerP;
    [SerializeField]
    float rushSpeed;
    [SerializeField]
    bool hitAtkFloor;
    float TimeToAtk;

    [Header("Effect")]
    public GameObject sign1;
    public GameObject sign2;
    bool hasSign;

    [Header("Audio")]
    public AudioClip RhinoSound;
    public AudioClip RhinoAtkSound;
    public AudioSource audioSrc;
    bool hasSound1;
    bool hasSound2;




    void Start()
    {
        monsterHealth = GetComponent<MonsterHealth>();
        player = GameObject.FindWithTag("Player").transform; // หา Player ด้วย Tag "Player"
        rushSpeed = 0.8f;

        RhinoSound = Resources.Load<AudioClip>("RhinoSound");
        RhinoAtkSound = Resources.Load<AudioClip>("RhinoAtk");
        audioSrc = GetComponent<AudioSource>();
    }

    void Update()
    {
        //state();
        //function update
        spriteRendererFlip();
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

        Gizmos.color = isHitFloor ? Color.yellow : Color.red;
        Gizmos.DrawWireSphere(transform.position + floorOffset , floorRadius);

        Gizmos.color = isDetectPlayer ? Color.green : Color.yellow;
        Gizmos.DrawWireCube(transform.position + sightAngle, sightRange);

        Gizmos.color = isDetectPlayerInSide ? Color.green :Color.red;
        Gizmos.DrawWireCube(transform.position + sightAngleInside, sightRangeInside);

        Gizmos.color = hitAtkPlayerP ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + areaAttack, attackRange);

        Gizmos.color = hitInAtkPlayerP ? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position + areaAttackLarge, attackRangeLarge);

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
        monsterMove();

        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    void Suspect()
    {
        direction.x = 0;
        sign1.SetActive(true);
        if(detectionTime >= 3f || detectionTime02 >= 10f && detectionTime >= 1f || detectionTime02 >= 24f)
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

    void moveAttackP()
    {
        
    }

    

    void AttackP()
    {
            Collider2D hitAttackColliderLarge = Physics2D.OverlapBox(transform.position + areaAttackLarge, attackRangeLarge, PlayerMask);
            hitInAtkPlayerP = hitAttackColliderLarge != null;

            if(!hasSign)
            {
                sign2.SetActive(true);
                hasSign = true;
            }

            animator.SetBool("walk", false);

            if (hitAttackColliderLarge != null && detectionTimedelay == 0f)
            {
                detectionTimeatk += Time.deltaTime;
                attackDelay();

            }
            else if (hitAttackColliderLarge == null)
            {
                ChangeState(CharacterState.Idle);
            }
            
    }    
    


        void attackDelay()
                {
                StartCoroutine(AnimateAttack());
                }
        
        void attackInfo(){

             Collider2D[] hitAttackCollider = Physics2D.OverlapCircleAll(transform.position + areaAttack, attackRange, PlayerMask);
             hitAtkPlayerP = hitAttackCollider != null;
             Collider2D[] hitAttackColliderF = Physics2D.OverlapCircleAll(transform.position + areaAttack, attackRange, floorLayerMask);
             hitAtkFloor = hitAttackColliderF != null;

                foreach (Collider2D player in hitAttackCollider)
                    {
                        player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

                        detectionTimeatk = 0f;

                        rushSpeed = 0f;
                        
                        animator.SetBool("atk", false);

                        hasAttacked = true;
                    } 

                if (hitAtkFloor == false) 
                {
                        rushSpeed = 0f;
                        
                        animator.SetBool("atk", false);

                        hasAttacked = true;
                }
        }
        
        IEnumerator AnimateAttack()
            {
                TimeToAtk += Time.deltaTime;
                Vector3 directionT = (player.position - transform.position);
                if (directionT.x > 0)
                        {
                            Debug.Log("right");
                            spriteRenderer.flipX = false; // หันหน้าไปทางขวา เมื่อผู้เล่นอยู่ด้านขวา
                            directionT.x += 20f;
                        }
                    else if (directionT.x < 0)
                        {
                            Debug.Log("left");
                            spriteRenderer.flipX = true; // หันหน้าไปทางซ้าย เมื่อผู้เล่นอยู่ด้านซ้าย
                            directionT.x -= 20f;
                        }
                    if(TimeToAtk >= 0.8f)
                    {
                        animator.SetBool("atk", true);
                        /*if(!hasSound1)
                        {
                        audioSrc.PlayOneShot(RhinoAtkSound);
                        hasSound1 = true;
                        }*/
                    }
                yield return new WaitForSeconds(1f);
                TimeToAtk = 0f;
                detectionTimedelay += Time.deltaTime;

                directionT.y = 0;
                transform.Translate(directionT * rushSpeed * Time.deltaTime);

                if(isHitFloor || !isOnGround)
                {
                    rushSpeed = 0;
                    detectionTimeatk = 0f;
                    animator.SetBool("atk", false);
                }
                
                if (hasAttacked == false) // ตรวจสอบว่ายังไม่มีการโจมตี
                {
                    attackInfo();
                }

                yield return new WaitForSeconds(1f);
                animator.SetBool("atk", false);
                /*if(!hasSound2)
                {
                    audioSrc.PlayOneShot(RhinoSound);
                    hasSound2 = true;
                }*/
                rushSpeed = 0f;
                yield return new WaitForSeconds(2f);
                hasAttacked = false;
                detectionTimeatk = 0f;
                detectionTimedelay = 0f;
                rushSpeed = 0.8f;
                hasSound1 = false;
                hasSound2 = false;
                
            }
    

    void ChangeState(CharacterState newState) {
        Idlestate = false;
        Attackstate = false;
        Suspectstate = false;
        currentState = newState;
    }





    void DetectObjects()
    {
        startRaycast = transform.position; // ตำแหน่งเริ่มต้นของ Raycast
        if (spriteRenderer.flipX == false)
        {
            lineOfSight = Vector2.right;
        }
        else
        {
            lineOfSight = Vector2.left;
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
        Debug.Log("hit player");
    }
    else if (hitCollider.CompareTag("Water"))
    {
        Debug.Log("hit water");
        // ไม่มีการกระทำเพิ่มเติมเมื่อชนกับน้ำ
    }
    else
    {
        inDetectionZone = false;
        // ทำงานเมื่อชนกับสิ่งอื่นที่ไม่ใช่ Player หรือ Water
        Debug.Log("Touched something other than Player or Water!");
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
        Debug.Log("Touched something other than Player or Water!");
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
                direction.x = -moveSpeed;
            }
            else if (spriteRenderer.flipX == false)
            {
                direction.x = moveSpeed;
            }
        }
        else{
            if (spriteRenderer.flipX == true)
            {
                direction.x = -moveSpeed;
            }
            else if (spriteRenderer.flipX == false)
            {
                direction.x = moveSpeed;
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

    void spriteRendererFlip()
    {
        if (spriteRenderer.flipX == false)
        {
            footOffset = new Vector3(1.2f , footOffset.y , 0);
            floorOffset = new Vector2(1.5f , floorOffset.y);
        }
        else
        {
            footOffset = new Vector3(-1.2f , footOffset.y , 0);
            floorOffset = new Vector2(-1.5f , floorOffset.y);
        }
    }


}
        



