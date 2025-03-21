using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObjects/MonsterData", order = 1)]
public class MonsterData : ScriptableObject
{
    public Vector3 position;
    public int health;
    public int damage;
    // Start is called before the first frame update
    }

public class ManagerMonster : MonoBehaviour
{
    public static ManagerMonster instance;
    public MonsterData monsterData;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveMonsterData(Vector3 position, int health, int damage)
    {
        monsterData.position = position;
        monsterData.health = health;
        monsterData.damage = damage;
        // บันทึกข้อมูลลงใน ScriptableObject หรือไฟล์อื่นๆตามต้องการ
    }

    public MonsterData LoadMonsterData()
    {
        return monsterData;
        // โหลดข้อมูลจาก ScriptableObject หรือไฟล์อื่นๆและส่งคืนค่า
    }
}
