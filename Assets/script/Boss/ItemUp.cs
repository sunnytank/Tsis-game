using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUp : MonoBehaviour
{
    // Start is called before the first frame update
[Header("monsterMove")]
    public int MonsterLayer;
    public string monsterLayerName;
    public int PlayerLayer;
    public string playerLayerName;
    public int UpLayer;
    public string UpLayerName;
    public int BombLayer;
    public string BombLayerName;
    public int GroundLayer;
    public string GroundLayerName;
    public int AuraLayer;
    public string AuraLayerName;
    public Collider2D monsterCollider;

    public Vector2 RadiusRange;

    public LayerMask Player;

    bool hasHit;
    public int attackDamage;
    float Destroy;
    float moveTime;

    public Vector3 movePosition;

    // Start is called before the first frame update
    void Start()
    {
        monsterCollider = GetComponent<Collider2D>();
        monsterLayerName = "Boss";
        playerLayerName = "Player";
        UpLayerName = "UpItem"; 
        BombLayerName = "Bomb"; 
        GroundLayerName = "Ground"; 
        AuraLayerName = "Aurablade";

        int MonsterLayer = LayerMask.NameToLayer(monsterLayerName); // แปลงชื่อ Layer เป็น Layer ที่เป็น int
        int PlayerLayer = LayerMask.NameToLayer(playerLayerName);
        int GroundLayer = LayerMask.NameToLayer(GroundLayerName);
        int UpLayer = LayerMask.NameToLayer(UpLayerName);
        int BombLayer = LayerMask.NameToLayer(BombLayerName);
        int AuraLayer = LayerMask.NameToLayer(AuraLayerName);

        for (int i = 0; i < 32; i++)
        {
            if (i == PlayerLayer)
            {
                Physics2D.IgnoreLayerCollision(UpLayer, i, true);
            }
            if (i == MonsterLayer)
            {
                Physics2D.IgnoreLayerCollision(UpLayer, i, true);
            }
            if (i == UpLayer)
            {
                Physics2D.IgnoreLayerCollision(UpLayer, i, true);
            }    
            if (i == GroundLayer)
            {
                Physics2D.IgnoreLayerCollision(UpLayer, i, true);
            }
            if (i == BombLayer)
            {
                Physics2D.IgnoreLayerCollision(UpLayer, i, true);
            }     
            if (i == AuraLayer)
            {
                Physics2D.IgnoreLayerCollision(UpLayer, i, true);
            }  
        }

    }

    // Update is called once per frame
    void Update()
    {
        OnHit();

        movePosition = new Vector3(0f , 1f ,0f);

        if(transform.position != movePosition &&  moveTime < 1)
        {
        transform.position += movePosition * Time.deltaTime;
        moveTime += Time.deltaTime;
        }
        else
        {
            hasHit = true;
        }

        Destroy += Time.deltaTime;

        if(Destroy >= 5)
        {
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos()
    {
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(transform.position , RadiusRange);
    }

    void OnHit()
    {
        Collider2D hitColliderP = Physics2D.OverlapBox(transform.position, RadiusRange , 0f, Player);


        if (hitColliderP != null && !hasHit)
            {
                hitColliderP.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

                hasHit = true;
            }
    }
}
