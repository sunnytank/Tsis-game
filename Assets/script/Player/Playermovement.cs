using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    [Header("PLAYER INFO")]
    [SerializeField]
    public float playerHealth;
    [SerializeField]
    public float playerStamina;
    [SerializeField]
    public int attackDamage;
    [SerializeField]
    public int landingAttackDamage;
    [SerializeField]
    public float playerSoul;
    bool attackedBool;
    public Vector2 TransformPlayer;

    private CameraFollowObject cameraFollowObject;
    [Header("camera stuff")]
    [SerializeField] 
    private GameObject cameraFollowGO;

    [Header("Layer Mask")]
    [SerializeField]
    private LayerMask groundLayerMask;
    [SerializeField]
    private LayerMask EnemyLayers;
    [SerializeField]
    private LayerMask BossLayers;
    [SerializeField]
    private LayerMask groundAndMonsterLayerMask;
    [SerializeField]
    private LayerMask itemLayerMask;
    bool groundAndMonster;

    [SerializeField]
    private Rigidbody2D rb2d;
    [SerializeField]
    private BoxCollider2D bc2d;
    [SerializeField]
    private float moveSpeed = 10;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private TrailRenderer tr;
    public float direction;
    public float JumpInput;
    float moveHorizontal;
    [SerializeField]
    bool isLocked;
    public bool isFacingRight;

    [SerializeField]
    Vector2 boxWidthHeight;
    [SerializeField]
    Vector3 pointBox;

    [Header("Jump")]
    [SerializeField]
    float jumpVelocity;
    bool canlanding;
    bool superlanding;
    bool superland;
    [SerializeField]
    Vector3 landingPoint;
    [SerializeField]
    float landingRange;
    bool LandingHit;
    bool haslanding;
    float landingDamageTime;
    float landinghitTime;
    int StaminaReducelanding = 30;

    [Header("Move")]
    [SerializeField]
    Vector3  footOffset;
    [SerializeField]
    float footRadius;
    bool Ground;
    bool canDoubleJump;
    public bool walk;

    [Header("Attack")]
    [SerializeField]
    Vector3 attackPoint;
    [SerializeField]
    float attackRange;
    [SerializeField]
    bool attackHit;
    [SerializeField]
    float DelayAttack;
    [SerializeField]
    float DelayTimeAttack;
    bool hasAttacked;
    public int StaminaReduceAtk;
    public bool canAtkStamina;
    

    [Header("Dash")]
    public bool canDash;
    bool dashStaminaReduce;
    bool isDashing;
    [SerializeField]
    float dashSpeed = 50;
    [SerializeField]
    float dashDuration;
    [SerializeField]
    int StaminaReduceDash;
    [SerializeField]
    public bool isBlock;

    [Header("Dies")]
    [SerializeField]
    float DIES;
    public bool DieActive;
    public bool lordSave;
    public bool Flip;

    [Header("AuraBlade")]
    public bool auraBlade;
    public float coolDownAuraBlade;
    public float coolDownAuraBladeForSet;
    public GameObject AuraBladeItem;
    public Transform AuraBladeSpawn;

    [Header("Effect")]
    public GameObject EfAtk1;
    public GameObject EfAtk2;
    public GameObject EfBlock;
    public GameObject EfLanding;

    [Header("Sound")]
    public AudioClip WalkSound;
    public AudioSource audioSrc;
    public AudioSource audioSrc2;
    bool haswalk;
    float moveSoundTime;

    public AudioClip AttackSound , DashSound;
    bool hasAtkSound;
    bool hasDashSound;
    bool hasBlockSound;


    public bool OptionGame;
    private Vector2 initialPosition;

    public PlayerHealth PlayerHealth;
    public GotoPazzleWord GotoPazzleWord;
    public cilckStationHeal cilckStationHeal;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        isDashing = true;
        bc2d = GetComponent<BoxCollider2D>();

        spriteRenderer.flipX = false;
        isFacingRight = true;

        cameraFollowObject = cameraFollowGO.GetComponent<CameraFollowObject>();

        WalkSound = Resources.Load<AudioClip>("mcFastRunning");
        AttackSound = Resources.Load<AudioClip>("mcHitSword");
        
    }

    // Update is called once per frame
    void Update()
    {
        lordSave = PlayerHealth.lordSave;
        attackDamage = PlayerHealth.attackDamage;
        playerStamina = PlayerHealth.currentStamina;
        playerHealth = PlayerHealth.currentHealthPlayer;
        attackedBool = PlayerHealth.attacked;
        playerSoul = PlayerHealth.SoulAmount;
        OptionGame = cilckStationHeal.Optionselect;
        //DieActiveT();
    
    if(DieActive == false)
    {        
        SpriteRendererFlip();
        jumpSet();
        TimeSetting();
        StaminaActive();
        Colider2D();
        moveSound();
        InputKey();
        AuraBladeCountDown();

        transform.Translate(TransformPlayer * dashSpeed * Time.deltaTime);
     
        //Die
        if(playerHealth <= 0)
        {
            moveSpeed = 0f;
            dashSpeed = 0f;
            StartCoroutine(Die());
        }        
    }
    }

    private void InputKey()
    {
        if(UserInput.instance.controls.Dashing.Dash.WasPressedThisFrame()&& isBlock == false)
        
            {            
                Dash();
            }
        else
            {
                MovePlayer();
            }

        //atack ground
        if(UserInput.instance.controls.Attacking.Attack.WasPressedThisFrame() && DelayAttack == 0 && Ground == true && isBlock == false)
        {
            DelayAttack += Time.deltaTime;
            animator.SetTrigger("IsPlayerAttack2"); // เล่น Animation A
            Invoke("playerEffectAtk1" , 0.35f);
            Invoke("Attack" , 0.35f);
        }

        //attack on air
        if(UserInput.instance.controls.Attacking.Attack.WasPressedThisFrame() && DelayAttack == 0 && Ground == false && isBlock == false)
        {
            DelayAttack += Time.deltaTime;     
            animator.SetTrigger("IsPlayerAttack2"); // เล่น Animation A
            Invoke("playerEffectAtk1" , 0.35f);
            Invoke("Attack" , 0.35f);
        }

        //Attack AuraBlade
        if(UserInput.instance.controls.Attacking.Special.WasPressedThisFrame() && coolDownAuraBladeForSet == 0 && auraBlade == false && isBlock == false)
        {
            auraBlade = true;
            animator.SetTrigger("IsPlayerAttack3"); // เล่น Animation B
            Invoke("playerEffectAtk2" , 0.3f);
            Invoke("SpawnAuraBlade" , 0.3f);
        }

        //dash

        //superlanding
        if(UserInput.instance.controls.Blocking.Block.IsPressed() && canlanding == true)
        {
            Debug.Log("canlanding");
            superLanding();
        }

        //superlanding
        if(superlanding == true && groundAndMonster == true && landinghitTime == 0)
        {
            EfLanding.SetActive(true);
            SuperLandingDamage();
        }

        //attacked โดนโจมตี
        if(attackedBool == true)
        {
            Debug.Log("HiT Attacked");
            isLocked = true;
        }

        //Blaock
        if(UserInput.instance.controls.Blocking.Block.IsPressed() && Ground == true && DelayAttack == 0)
        {
            Block();
        }
        else
        {
            isBlock = false;
            animator.SetBool("IsPlayerBlock", false);
        }

    }

    void playerEffectAtk1()
    {
        EfAtk1.SetActive(true);
    }

    void playerEffectAtk2()
    {
        EfAtk2.SetActive(true);
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Ground ? Color.green : groundAndMonster ? Color.black : canDoubleJump ? Color.yellow: Color.red;
        Gizmos.DrawWireSphere(transform.position + footOffset , footRadius);

        Gizmos.color = !hasAttacked ? Color.yellow : attackHit ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + attackPoint , attackRange);

        Gizmos.color = !hasAttacked ? Color.yellow : attackHit ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + landingPoint , landingRange);
    }

    #region Move
    void MovePlayer()
    {
        direction = UserInput.instance.moveInput.x;
        rb2d.velocity = new Vector2(direction * moveSpeed , rb2d.velocity.y);
        if(direction > 0 || direction < 0 )
        {
            TurnCheck();
            animator.SetBool("IsPlayerRun",true);
            walk = true;
        }
        else
        {
            animator.SetBool("IsPlayerRun",false);
            walk = false;
            haswalk = false;
            StopSoundWalk();
        }
    }
    void moveSound()
    {
            if(!Ground || !walk || isBlock)
            {
                StopSoundWalk();
            }

            if(Ground && walk)
            {
                if(!haswalk)
                {
                    StartSoundWalk();
                    haswalk = true;
                }
            }
            if(haswalk)
            {
                moveSoundTime = Time.deltaTime;
                if(moveSoundTime >= 6)
                {
                    haswalk = false;
                    moveSoundTime = 0f;
                }
            }
    }

    public void StartSoundWalk ()
    {
        audioSrc.clip = WalkSound;
        audioSrc.volume = 0.5f;
        audioSrc.Play();
    } 

    public void StopSoundWalk ()
    {
        audioSrc.clip = WalkSound;
        audioSrc.volume = 0.5f;
        audioSrc.Stop();
    } 

    #endregion

    #region Turn
    private void TurnCheck()
    {
        if(UserInput.instance.moveInput.x > 0 && isFacingRight == false)
        {
            Turn();
        }
        else if(UserInput.instance.moveInput.x < 0 && isFacingRight == true)
        {
            Turn();
        }
    }

    private void Turn()
    {
        if(!isLocked)
        {
        
        if(isFacingRight)
        {
            Vector3 ratator = new Vector3(transform.rotation.x, 180f,transform.rotation.z);
                    transform.rotation = Quaternion.Euler(ratator);
                    isFacingRight = !isFacingRight;
                    cameraFollowObject.CallTurn();
        }
        else 
        {
             Vector3 ratator = new Vector3(transform.rotation.x, 0f,transform.rotation.z);
                    transform.rotation = Quaternion.Euler(ratator);
                    isFacingRight = !isFacingRight;
                    cameraFollowObject.CallTurn();
        }
        }
    }
    #endregion


    #region Jump
    void jump(float jumpVelocityMultiplier)
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x , jumpVelocity * jumpVelocityMultiplier);
        animator.SetTrigger("IsPlayerJump");
    }

    void jumpSet()
    {
        if(UserInput.instance.controls.Jumping.Jump.WasPressedThisFrame())
        {
            if(Ground == true)
            {
                jump(1f);
            }
            else if(canDoubleJump)
            {
                jump(1f);
                canDoubleJump = false;
                canlanding = true;
            }
        }
        
    }
    #endregion


    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position + attackPoint, attackRange, EnemyLayers);
        attackHit = hitEnemies != null;

        Collider2D[] hitBoss = Physics2D.OverlapCircleAll(transform.position + attackPoint, attackRange, BossLayers);
        attackHit = hitBoss != null;

        isLocked = true;

        DelayAttack += Time.deltaTime;

        canAtkStamina = true;

        audioSrc2.PlayOneShot(AttackSound);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("attack hit" + enemy.name);

            enemy.GetComponent<MonsterHealth>().TakeDamage(attackDamage);

            hasAttacked = true;
        }
        
        foreach (Collider2D Boss in hitBoss)
        {
            Debug.Log("attack hit Boss" + Boss.name);

            Boss.GetComponent<BossHealth>().TakeDamage(attackDamage);

            hasAttacked = true;
        }

    }

    public void Dash()
    {
        if(playerStamina >= 50){
            if(Ground == false)
            {
                dashStaminaReduce = true;
                canDash = true;
                animator.SetBool("IsPlayerAirIdle",false);
                animator.SetBool("IsPlayerDashAir",true);
                StartCoroutine(dash());
            }
            else if(Ground == true)
            {
                dashStaminaReduce = true;
                canDash = true;
                animator.SetTrigger("IsPlayerDash");
                StartCoroutine(dash());
            }
        }
        else
        {
            Debug.Log("Not Have Stamina");
        }
    }

   public IEnumerator dash()
    {
        isDashing = false;
        rb2d.gravityScale = 0f;
        tr.emitting = true;
        TransformPlayer = new Vector2(dashSpeed, 0f);
        rb2d.velocity = new Vector2(0f , 0f);
        yield return new WaitForSeconds(dashDuration);
        TransformPlayer.x = 0;
        animator.SetBool("IsPlayerDashAir",false);
        tr.emitting = false;
        rb2d.gravityScale = 5f;
        isDashing = true;
        canDash = false;
    }
    
    public void Block()
    {
        if(playerStamina >= 10){
            isBlock = true;
            animator.SetBool("IsPlayerBlock", true);
            Debug.Log("Block Damage");
            canDash = false;
            rb2d.velocity = new Vector2(0 ,0);
            isLocked = true;
        }
        else
        {
            Debug.Log("Can not stamina");
        }
    }

    IEnumerator Die()
    {
        animator.SetBool("IsPlayerDie",true);
        Debug.Log("die");
        DieActive = true;
        moveSpeed = 0;
        dashSpeed = 0;
        yield return new WaitForSeconds(DIES);
        animator.SetBool("IsPlayerDie",false);
        Debug.Log("Des enemy");
    }

    void superLanding()
    {
        //animator.SetBool("",true);
        if(playerStamina >= 50){
        Debug.Log("SuperLanding");
        animator.SetBool("IsPlayerLanding", true);
        animator.SetBool("IsPlayerAirIdle",false);
        isLocked = true;
        superlanding = true;
        superland = true;
        rb2d.gravityScale = 10f;
        rb2d.velocity = new Vector2(0 , rb2d.velocity.y);
        }
    }

    public void SuperLandingDamage()
    {
        Collider2D[] superLandingHit = Physics2D.OverlapCircleAll(transform.position + landingPoint, landingRange, EnemyLayers);
        LandingHit = superLandingHit != null;

        superlanding = false;

        landinghitTime += Time.deltaTime;

        foreach (Collider2D enemy in superLandingHit)
        {
            Debug.Log("Super landinghit " + enemy.name);

            enemy.GetComponent<MonsterHealth>().TakeDamage(landingAttackDamage);

            haslanding = true;
            superlanding = false;
        }

    }

    void SpriteRendererFlip()
    {
        if(isLocked){

        }
        else
        {
            if (isFacingRight == false)
                {
                    attackPoint = new Vector3(-1, 0.11f, 0);
                    dashSpeed = -4;
                    Flip = false;
                }
            else if (isFacingRight == true)
                {
                    attackPoint = new Vector3(1, 0.11f, 0);
                    dashSpeed = 4;
                    Flip = true;
                }
        }

        if(Ground)
        {
            canDoubleJump = true;
            canlanding = false;
            rb2d.gravityScale = 5f;
            animator.SetBool("IsPlayerLanding", false);
            animator.SetBool("IsPlayerAirIdle",false);
        }
        else if(!Ground)
        {
            haslanding = false;
            animator.SetBool("IsPlayerRun", false);
            animator.SetBool("IsPlayerAirIdle",true);
            if(isDashing == true)
            {
                animator.SetBool("IsPlayerAirIdle",true);
            }
        }

    }

    void TimeSetting(){
        if(DelayAttack > 0)
        {
            DelayTimeAttack += Time.deltaTime;

            attackHit = false;

                if(DelayTimeAttack >= 0.6f)
                {
                    DelayAttack = 0;
                    DelayTimeAttack = 0;
                    hasAttacked = false;
                    isLocked = false;
                    if(!Ground)
                    {
                        animator.SetBool("IsPlayerAirIdle",true);
                    }
                }
                if(DelayTimeAttack >= 0.4f)
                {
                    isLocked = false;
                }
        }
        if(landinghitTime > 0)
        {
            landingDamageTime +=Time.deltaTime;

                if(landingDamageTime >=1.5f)
                {
                    landingDamageTime = 0;
                    landinghitTime = 0;
                }
        }

        if(isBlock == false)
        {
            if(DelayTimeAttack == 0 || superlanding == false && haslanding == true)
            {
                isLocked = false;
            }
        }
    }

    public void SpawnAuraBlade()
    {
        var itemAuraBlade = Instantiate(AuraBladeItem, AuraBladeSpawn.position, AuraBladeSpawn.rotation);
    }

    public void AuraBladeCountDown()
    {
        if(auraBlade)
        {
            coolDownAuraBlade += Time.deltaTime;
        }

        if(coolDownAuraBlade > 0f)
        {
            coolDownAuraBladeForSet += Time.deltaTime;
        }

        if(coolDownAuraBladeForSet >= 5f)
        {
            auraBlade = false;
            coolDownAuraBlade = 0f;
            coolDownAuraBladeForSet = 0f;
        }

    }
    
    public void StaminaActive()
    {
        if(dashStaminaReduce)
        {
            GetComponent<PlayerHealth>().StaminaDown(StaminaReduceDash);

            dashStaminaReduce = false;
        }
        if(superland)
        {
            GetComponent<PlayerHealth>().StaminaDown(StaminaReducelanding);

            superland = false;
        }
        if(canAtkStamina)
        {
            GetComponent<PlayerHealth>().StaminaDown(StaminaReduceAtk);
            
            canAtkStamina = false;
        }
        
    }

    public void Colider2D()
    {
        Collider2D hitGroundColider = Physics2D.OverlapCircle(transform.position + footOffset , footRadius, groundLayerMask);
        Ground = hitGroundColider != null;

        Collider2D hitGroundAndMonsterColider = Physics2D.OverlapCircle(transform.position + footOffset , footRadius, groundAndMonsterLayerMask);
        groundAndMonster = hitGroundAndMonsterColider != null;
    }

    public void ColSoul()
    {
        GetComponent<PlayerHealth>().StaminaDown(StaminaReducelanding);
    } 

    public void DieActiveT()
    {
        if(lordSave == true)
        {
            DieActive = false;
            moveSpeed = 10;
            animator.SetBool("IsPlayerDie",false);
        }

        if(DieActive == true)
        {
            moveSpeed = 0;
        }
        else
        {

        }
    }

}
