using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropCovid : MonoBehaviour
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
    public Collider2D monsterCollider;

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

        int MonsterLayer = LayerMask.NameToLayer(monsterLayerName); // แปลงชื่อ Layer เป็น Layer ที่เป็น int
        int PlayerLayer = LayerMask.NameToLayer(playerLayerName);
        int BombLayer = LayerMask.NameToLayer(bombLayerName);
        int UpLayer = LayerMask.NameToLayer(UpLayerName);

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
        }
        attackDamage = 20;
    }

    // Update is called once per frame
    void Update()
    {
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
    Gizmos.DrawWireSphere(transform.position, RadiusBomb);
    }


    void OnHit()
    {
        Collider2D hitColliderP = Physics2D.OverlapCircle(transform.position, RadiusBomb , Player);

        if (hitColliderP != null && !hitGround && !hasHit)
            {
                hitColliderP.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

                hasHit = true;
            }
    }
}
