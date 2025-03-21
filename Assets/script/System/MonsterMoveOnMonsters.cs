using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMoveOnMonsters : MonoBehaviour
{

    public int MonsterLayer;
    public string monsterLayerName;
    public int PlayerLayer;
    public string playerLayerName;
    public Collider2D monsterCollider;

    public MonsterHealth monsterHealth;

    // Start is called before the first frame update
    void Start()
    {
        monsterHealth = GetComponent<MonsterHealth>();
        monsterCollider = GetComponent<Collider2D>();
        monsterLayerName = "Enemies";
        playerLayerName = "Player";

        int MonsterLayer = LayerMask.NameToLayer(monsterLayerName); // แปลงชื่อ Layer เป็น Layer ที่เป็น int
        int PlayerLayer = LayerMask.NameToLayer(playerLayerName);

        for (int i = 0; i < 32; i++)
        {
            if (i == MonsterLayer)
            {
                Physics2D.IgnoreLayerCollision(MonsterLayer, i, true);
            }
            if (i == PlayerLayer)
            {
                Physics2D.IgnoreLayerCollision(MonsterLayer, i, true);
            }
        }
    
    }

    // Update is called once per frame
    void Update()
    {
        if(monsterHealth.currentHealth <= 0)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Collider2D playerCollider = player.GetComponent<Collider2D>();

            if (playerCollider != null)
            {
                Physics2D.IgnoreCollision(monsterCollider, playerCollider, true);
            }
        }
    }
}
