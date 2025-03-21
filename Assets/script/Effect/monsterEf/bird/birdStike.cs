using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdStike : MonoBehaviour
{
    // Start is called before the first frame update
public GameObject monster;
    public Transform Monster;
    public MonsterHealth monsterHealth;
    public SpriteRenderer spriteRenderer;
    bool flip;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        monsterHealth = monster.GetComponent<MonsterHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        flip = monsterHealth.flip;
        if(!flip)
        {
            spriteRenderer.flipX = false;
            transform.position = new Vector3 (Monster.position.x  ,Monster.position.y  ,Monster.position.z);
        }
        else if (flip)
        {
            spriteRenderer.flipX = true;
            transform.position = new Vector3 (Monster.position.x  ,Monster.position.y  ,Monster.position.z);
        }

    }
}


