using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulMonsterExp : MonoBehaviour
{
    public GameObject SoulExp = null; // กำหนด Prefab ที่ต้องการสร้าง
    public float numberOfObjects; // จำนวน object ที่ต้องการสร้าง
    public float currentSoul;
    public Vector2 positionExp;
    bool CorrectSoul;
    public float DelaySoul;
    public float Delay;

    private MonsterHealth monsterHealth;

    // Start is called before the first frame update
    void Start()
    {
        monsterHealth = GetComponent<MonsterHealth>();
    }

    // Update is called once per frame
    void Update()
    {

        if(monsterHealth.currentHealth <= 0)
        {
            DelaySoul += Time.deltaTime;
            if(DelaySoul >= Delay)
            {
                SoulDead();
            }
        }
        positionExp = transform.position;
    }

    public void SoulDead()
    {
        if(currentSoul < numberOfObjects && CorrectSoul == false)
            {
                    Vector3 randomPosition = positionExp + Random.insideUnitCircle * 1f; // สุ่มตำแหน่งในรัศมี 5 หน่วย
                    GameObject newSoulExp = Instantiate(SoulExp, randomPosition, Quaternion.identity);
                    // สร้าง object ใหม่จาก Prefab ที่กำหนด ณ ตำแหน่งสุ่มในรัศมีที่กำหนด
                    currentSoul += 1;
            }
        if(currentSoul == numberOfObjects)
        {
            CorrectSoul = true;
        }
    }
}
