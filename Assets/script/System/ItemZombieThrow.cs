using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemZombieThrow : MonoBehaviour
{
    public GameObject tower;
    public GameObject target;

    public float speed = 10f;

    private float towerX;
    private float towerY;
    private float targetX;
    private float targetY;

    private float dist;
    private float nextX;
    private float baseY;
    private float height;
    private bool Hit;
    private bool HitP;

    public LayerMask LayerMask;
    public LayerMask PlayerMask;

    [Header("monsterMove")]
    public int MonsterLayer;
    public string monsterLayerName;
    public int PlayerLayer;
    public string playerLayerName;
    public int BombLayer;
    public string bombLayerName;
    public Collider2D monsterCollider;
    float delayhitTime;
    Vector3 movePosition;
    float distance;
    public bool ItHit;

    [Header("Bomb")]
    public Vector2 Radius;
    public Vector3 OffSetBomb;
    public float RadiusBomb;
    public float RadiusHitThrow;
    private bool HitPlayer;
    private bool HitPlayerThrow;
    public int attackDamage = 10;
    public int attackDamageThrow = 5;
    bool detectionatk;
    bool Throwatk;
    float DelayBomb;
    public GameObject EfBomb;

    [Header("Audio")]
    public AudioClip BombSound;
    public AudioSource audioSrc;
    bool hasSoundBomb;
    // Start is called before the first frame update

    void Start()
    {
        tower = GameObject.FindGameObjectWithTag("spawnItemBigZombie"); 
        target = GameObject.FindGameObjectWithTag("Player"); 
        targetX = target.transform.position.x;
        targetY = target.transform.position.y;

        towerX = tower.transform.position.x;
        towerY = tower.transform.position.y;

        dist = targetX - towerX;

        BombSound = Resources.Load<AudioClip>("bombBoom");
        audioSrc = GetComponent<AudioSource>();

        monsterCollider = GetComponent<Collider2D>();
        monsterLayerName = "Enemies";
        playerLayerName = "Player";
        bombLayerName = "Bomb"; 

        int MonsterLayer = LayerMask.NameToLayer(monsterLayerName); // แปลงชื่อ Layer เป็น Layer ที่เป็น int
        int PlayerLayer = LayerMask.NameToLayer(playerLayerName);
        int BombLayer = LayerMask.NameToLayer(bombLayerName);

        for (int i = 0; i < 32; i++)
        {
            if (i == PlayerLayer)
            {
                Physics2D.IgnoreLayerCollision(BombLayer, i, true);
            }
            if (i == MonsterLayer)
            {
                Physics2D.IgnoreLayerCollision(BombLayer, i, true);
            }
            if (i == BombLayer)
            {
                Physics2D.IgnoreLayerCollision(BombLayer, i, true);
            }    
        }
    }

    // Update is called once per frame
void Update()
{

    if(dist < 0)
    {
        nextX = Mathf.MoveTowards(transform.position.x, targetX - 5, speed * Time.deltaTime);
        baseY = Mathf.Lerp(towerY, targetY -0.75f , Mathf.Clamp01((nextX - towerX) / dist));
        height = 2 * (nextX - towerX) * (nextX - targetX) / (-0.25f * dist * dist);
    }
    else if(dist > 0)
    {
        nextX = Mathf.MoveTowards(transform.position.x, targetX + 5, speed * Time.deltaTime);
        baseY = Mathf.Lerp(towerY, targetY -0.75f , Mathf.Clamp01((nextX - towerX) / dist));
        height = 2 * (nextX - towerX) * (nextX - targetX) / (-0.25f * dist * dist);
    }


    movePosition = new Vector3(nextX, baseY + height, transform.position.z);

    if(transform.position != movePosition && ItHit == false)
    {
    transform.rotation = LookAtTarget(new Vector2(nextX - towerX, baseY + height));
    transform.position = movePosition;
    }
    else
    {
    transform.position = transform.position;
    }
    
    OnTrig();
    ActivateBomb();

    if(Hit == true)
    {
        ItHit = true;
    }
}

Quaternion LookAtTarget(Vector2 rotation)
{
    return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
}

void OnTrig()
{
    Collider2D hitCollider = Physics2D.OverlapBox(transform.position, Radius , 0f, LayerMask);
    Hit = hitCollider != null;

    Collider2D hitColliderS = Physics2D.OverlapBox(transform.position, Radius , 0f, PlayerMask);
    HitP = hitColliderS != null;
}

void ThrowHit()
{
    Collider2D hitColliderP = Physics2D.OverlapCircle(transform.position, RadiusHitThrow , PlayerMask);
    HitPlayerThrow = hitColliderP != null;

        if (hitColliderP != null)
            {
                hitColliderP.GetComponent<PlayerHealth>().TakeDamage(attackDamageThrow);

                Throwatk = true;
            }
}

void Bomb()
{
    Collider2D hitColliderPBomb = Physics2D.OverlapCircle(transform.position + OffSetBomb, RadiusBomb , PlayerMask);
    HitPlayer = hitColliderPBomb != null;

    EfBomb.SetActive(true);

        if (hitColliderPBomb != null)
            {
                hitColliderPBomb.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

                detectionatk = true;
            }
}

void ActivateBomb()
{
    if(Hit == true)
    {
        DelayBomb += Time.deltaTime;
    }

    if(DelayBomb >= 1.75f && detectionatk == false)
    {
        Bomb();
        if(!hasSoundBomb)
        {
            audioSrc.PlayOneShot(BombSound);
            hasSoundBomb = true;
        }
        Invoke("BombDes" , 0.4f);
    }

    if(HitP == true && Throwatk == false && ItHit == false)
    {
        ThrowHit();
    }
    /*if(DelayBomb >= 2.5f)
    {
        Destroy(gameObject);
    }*/

}

void BombDes()
{
    Destroy(gameObject);
}

void OnDrawGizmos()
{
    Gizmos.color = Hit ? Color.green : Color.red;
    Gizmos.DrawWireCube(transform.position , Radius);
    Gizmos.color = HitPlayer ? Color.green : Color.red;
    Gizmos.DrawWireSphere(transform.position+ OffSetBomb, RadiusBomb);
    Gizmos.color = HitPlayerThrow ? Color.green : Color.red;
    Gizmos.DrawWireSphere(transform.position, RadiusHitThrow);
}

    
    
/*void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Ground")) // ตรวจสอบว่าชนกับไอเท็มหรือไม่
    {
        transform.position = transform.position;
        Debug.Log("Hit1");
        Hit = true;
    }
    if (other.CompareTag("Floor")) // ตรวจสอบว่าชนกับไอเท็มหรือไม่
    {
        transform.position = transform.position;
        Debug.Log("Hit1");
        Hit = true;
    }
} */


}
