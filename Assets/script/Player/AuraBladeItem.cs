using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraBladeItem : MonoBehaviour
{
    [Header("Area")]
    public Vector3 areaAttack;
    public Vector2 attackRange;

    [Header("Interac")]
    public bool HitMonster;
    public bool HitBoss;
    public LayerMask MonsterMask;
    public LayerMask BossMask;

    [Header("Info")]
    public int attackDamage;
    public SpriteRenderer spriteRenderer; 
    public GameObject Target;
    public GameObject Item;
    public float TargetX;
    public float ItemX;
    public float distance;

    [Header("Move")]
    public Vector2 direction;
    public float moveSpeed;
    public float MoveSpeed;
    public float moveSpeedT;

    [Header("LayerName")]
    public int MonsterLayer;
    public string monsterLayerName;
    public int PlayerLayer;
    public string playerLayerName;
    public int BombLayer;
    public string bombLayerName;
    public int UpLayer;
    public string UpLayerName;
    public int GroundLayer;
    public string GroundLayerName;
    public int AuraLayer;
    public string AuraLayerName;
    public Collider2D monsterCollider;



    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player"); 
        Item = GameObject.FindGameObjectWithTag("Aurablade"); 

        TargetX = Target.transform.position.x;
        ItemX = Item.transform.position.x;

        distance = TargetX - ItemX;
        moveSpeedT = 10;
        MoveSpeed = 2;
        attackDamage = 15;


        /*monsterCollider = GetComponent<Collider2D>();
        bombLayerName = "Bomb"; 
        playerLayerName = "Player";
        UpLayerName = "UpItem";
        AuraLayerName = "Aurablade";

        int PlayerLayer = LayerMask.NameToLayer(playerLayerName);// แปลงชื่อ Layer เป็น Layer ที่เป็น int
        int BombLayer = LayerMask.NameToLayer(bombLayerName);
        int UpLayer = LayerMask.NameToLayer(UpLayerName);
        int AuraLayer = LayerMask.NameToLayer(AuraLayerName);

        for (int i = 0; i < 32; i++)
        {
            if (i == PlayerLayer)
            {
                Physics2D.IgnoreLayerCollision(AuraLayer, i, true);
            }
            if (i == BombLayer)
            {
                Physics2D.IgnoreLayerCollision(AuraLayer, i, true);
            }    
            if (i == UpLayer)
            {
                Physics2D.IgnoreLayerCollision(AuraLayer, i, true);
            }  
            if (i == AuraLayer)
            {
                Physics2D.IgnoreLayerCollision(AuraLayer, i, true);
            }  
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        attackInfo();
        if(distance > 0)
        {
            direction.x = MoveSpeed;
            moveSpeed = moveSpeedT;
        }
        else if(distance < 0)
        {
            direction.x = -MoveSpeed;
            moveSpeed = -moveSpeedT;
        }

        transform.Translate(direction * moveSpeed * Time.deltaTime);

        Invoke("destroy" , 1f);
    }

    void destroy()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {

        Gizmos.color = HitMonster ? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position + areaAttack, attackRange);
        Gizmos.color = HitBoss ? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position + areaAttack, attackRange);
    }


        void attackInfo()
            {

             Collider2D hitMonster = Physics2D.OverlapBox(transform.position + areaAttack, attackRange, 0f , MonsterMask);
             HitMonster = hitMonster != null;

             Collider2D hitBoss = Physics2D.OverlapBox(transform.position + areaAttack, attackRange, 0f , BossMask);
             HitBoss = hitBoss != null;

                if (hitMonster != null)
                    {
                        hitMonster.GetComponent<MonsterHealth>().TakeDamageAuraBlade(attackDamage);
                    } 

                if (hitBoss != null)
                    {
                        hitBoss.GetComponent<BossHealth>().TakeDamageAuraBlade(attackDamage);
                    } 
            }

}
