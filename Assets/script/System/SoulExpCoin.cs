using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulExpCoin : MonoBehaviour
{
    public Transform player;
    private float moveSpeedSoul = 7.5f;
    private float DelaySoul;
    float DelaySoulDie;
    public int SoulEXP = 1;
    bool soulDesT;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform; // หา Player ด้วย Tag "Player"
    }

    // Update is called once per frame
    void Update()
    {

        DelaySoul += Time.deltaTime;

        if(DelaySoul >= 2 )
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * moveSpeedSoul*2 * Time.deltaTime);
        }

        if(soulDesT == true)
        {
                Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter2D(Collider2D HitPlayer)
    {
        if (HitPlayer.CompareTag("Player")) // ตรวจสอบว่าชนกับไอเท็มหรือไม่
        {
            HitPlayer.GetComponent<PlayerHealth>().CollectSoul(SoulEXP);
            Debug.Log("EXP+1");
            soulDesT = true;
        }
    } 
}
