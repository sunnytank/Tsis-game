using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{

    [Header("monsterMove")]
    public int MonsterLayer;
    public string monsterLayerName;
    public int PlayerLayer;
    public string playerLayerName;
    public int BombLayer;
    public string bombLayerName;
    public int UpLayer;
    public string UpLayerName;
    public int AuraLayer;
    public string AuraLayerName;
    public Collider2D monsterCollider;

    public Vector3 OffSet;
    public float RadiusBomb;
    public float RadiusGround;

    public LayerMask Ground;
    public LayerMask Player;

    bool hitGround;
    bool hasHit;
    public int attackDamage;

    float Destroy;

    // Start is called before the first frame update
    void Start()
    {
        monsterCollider = GetComponent<Collider2D>();
        monsterLayerName = "Boss";
        playerLayerName = "Player";
        bombLayerName = "Bomb"; 
        UpLayerName = "UpItem";
        AuraLayerName = "Aurablade";

        int MonsterLayer = LayerMask.NameToLayer(monsterLayerName); // แปลงชื่อ Layer เป็น Layer ที่เป็น int
        int PlayerLayer = LayerMask.NameToLayer(playerLayerName);
        int BombLayer = LayerMask.NameToLayer(bombLayerName);
        int UpLayer = LayerMask.NameToLayer(UpLayerName);
        int AuraLayer = LayerMask.NameToLayer(AuraLayerName);

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
            if (i == UpLayer)
            {
                Physics2D.IgnoreLayerCollision(BombLayer, i, true);
            }  
            if (i == AuraLayer)
            {
                Physics2D.IgnoreLayerCollision(BombLayer, i, true);
            }  
        }
    }

    // Update is called once per frame
    void Update()
    {
        OnHitGround();
        OnHit();
       
        if(hitGround == true)
        {
            Destroy += Time.deltaTime;
        }

        if(Destroy >= 3)
        {
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos()
    {
    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(transform.position + OffSet, RadiusBomb);
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position + OffSet, RadiusGround);
    }

    void OnHitGround()
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(transform.position + OffSet, RadiusGround , Ground);
        hitGround = hitCollider != null;
    }

    void OnHit()
    {
        Collider2D hitColliderP = Physics2D.OverlapCircle(transform.position + OffSet, RadiusBomb , Player);

        if (hitColliderP != null && !hitGround && !hasHit)
            {
                hitColliderP.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

                hasHit = true;
            }
    }
}
