using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAwakeZombie : MonoBehaviour
{
    public Vector3 OffSetAwake;
    public Vector2 RadiusAwake;
    public LayerMask PlayerMask;
    bool HitPlayer;
    bool detectionatk;
    float DotTime;
    float TimeDes;
    int attackDamage = 5;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AwakeDamageOverTime(); 
    }

    void AwakeDamage()
    {
    Collider2D hitColliderPBomb = Physics2D.OverlapBox(transform.position + OffSetAwake, RadiusAwake , 0f , PlayerMask);
    HitPlayer = hitColliderPBomb != null;

        if (hitColliderPBomb != null)
            {
                hitColliderPBomb.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

                detectionatk = true;
                DotTime = 0 ;
            }
    }

    void AwakeDamageOverTime()
    {
        DotTime += Time.deltaTime;
        TimeDes += Time.deltaTime;

        if(detectionatk == false)
        {
            AwakeDamage();
        }
        if(DotTime >= 1f)
        {
            detectionatk = false;
        }
        if(TimeDes >= 4f)
        {
            Destroy(gameObject);
        }
    }
    
    void OnDrawGizmos()
    {
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireCube(transform.position+ OffSetAwake, RadiusAwake);
    }
}
