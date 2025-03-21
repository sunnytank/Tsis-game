using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemAxeThrow : MonoBehaviour
{
    public GameObject target;
    public GameObject axeItem;

    public int MonsterLayer;
    public string monsterLayerName;
    public int PlayerLayer;
    public string playerLayerName;
    public int axeLayer;
    public string axeLayerName;
    public Collider2D monsterCollider;
    public float rotationSpeed;
    public SpriteRenderer spriteRenderer;

    float targetX;
    float axeItemX;
    float distance;

    public LayerMask player;
    public Vector3 OffSet;
    public Vector2 Range;
    bool HitX;
    bool Colider;
    bool HitPlayer;

    public int attackDamage;

    Vector2 direction;
    float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player"); 
        axeItem = GameObject.FindGameObjectWithTag("Axe");

        targetX = target.transform.position.x;
        axeItemX = axeItem.transform.position.x;

        distance = targetX - axeItemX;
        moveSpeed = 50;

        monsterCollider = GetComponent<Collider2D>();
        monsterLayerName = "Boss";
        //playerLayerName = "Player";
        axeLayerName = "Axe"; 

        int MonsterLayer = LayerMask.NameToLayer(monsterLayerName); // แปลงชื่อ Layer เป็น Layer ที่เป็น int
        //int PlayerLayer = LayerMask.NameToLayer(playerLayerName);
        int axeLayer = LayerMask.NameToLayer(axeLayerName);

        for (int i = 0; i < 32; i++)
        {
            /*if (i == PlayerLayer)
            {
                Physics2D.IgnoreLayerCollision(axeLayer, i, true);
            }*/
            if (i == MonsterLayer)
            {
                Physics2D.IgnoreLayerCollision(axeLayer, i, true);
            }
            if (i == axeLayer)
            {
                Physics2D.IgnoreLayerCollision(axeLayer, i, true);
            }    
        }

        rotationSpeed = 90;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        if(distance < 0)
        {
            direction.x = -1;
            spriteRenderer.flipX = false;
        }
        else if(distance > 0)
        {
            direction.x = 1;
            spriteRenderer.flipX = true;
        }

        Invoke("destroy" , 10f);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + OffSet, Range);
    }

    void destroy()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
    if (other.CompareTag("Player")) // ตรวจสอบว่าชนกับไอเท็มหรือไม่
    {
        target.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
    }
    }
}

