using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUp22 : MonoBehaviour
{
    // Start is called before the first frame update
public MonkeyBoss monkeyboss;
    public GameObject Zombie;
    public Transform MONkeyBoss;
    // Start is called before the first frame update
    void Start()
    {
        monkeyboss = Zombie.GetComponent<MonkeyBoss>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3 (MONkeyBoss.position.x +5f ,MONkeyBoss.position.y - 2.5f ,MONkeyBoss.position.z);
        
    }
}
