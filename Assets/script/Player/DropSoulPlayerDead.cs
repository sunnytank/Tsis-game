using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSoulPlayerDead : MonoBehaviour
{
    public GameObject SoulExpDrop = null; // กำหนด Prefab ที่ต้องการสร้าง
    public Vector3 positionDropSoul;
    public bool DropSoul;

    public bool DieActive;

    public Playermovement Playermovement;
    // Start is called before the first frame update
    void Start()
    {
        Playermovement = GetComponent<Playermovement>();
    }


    // Update is called once per frame
    void Update()
    {
        DieActive = Playermovement.DieActive;

        if(DieActive == true)
        {
            SoulPlayerDrop();
        }

        positionDropSoul = transform.position;
    }

    public void SoulPlayerDrop()
    {
        if(DropSoul == false)
            {
                Vector3 spawnDropSoul = new Vector3 (positionDropSoul.x, positionDropSoul.y + 2f, positionDropSoul.z);
                GameObject newSoulExpDrop = Instantiate(SoulExpDrop, spawnDropSoul, Quaternion.identity);
                // สร้าง object ใหม่จาก Prefab ที่กำหนด ณ ตำแหน่งสุ่มในรัศมีที่กำหนด
                DropSoul = true;
            }
    }
}
