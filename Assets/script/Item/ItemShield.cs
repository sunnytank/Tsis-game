using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShield : MonoBehaviour
{
    bool ShieldComplate;
    float DelayTime;
    public int Shield;

    public float speed = 1.0f; // ความเร็วในการเคลื่อนที่
    public float magnitude = 0.2f; // ระยะของการเคลื่อนที่

    private Vector3 startingPosition;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * magnitude; // คำนวณตำแหน่งตามฟังก์ชัน Sin() เพื่อให้เกิดการเคลื่อนที่ตามจังหวะ

        Vector3 newPosition = startingPosition + Vector3.up * offset; // คำนวณตำแหน่งใหม่ตามแกน Y โดยใช้ค่า offset

        transform.position = newPosition; // กำหนดตำแหน่งใหม่ให้กับวัตถุ
        
        if(ShieldComplate == true)
        {
            DelayTime += Time.deltaTime;
            if(DelayTime >= 1)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D HitPlayer)
    {
        if (HitPlayer.CompareTag("Player")) // ตรวจสอบว่าชนกับไอเท็มหรือไม่
        {
           if(ShieldComplate == false)
            {
            HitPlayer.GetComponent<PlayerHealth>().ShieldPlusPlus(Shield);
            ShieldComplate = true;
            }
        }
    } 
}
