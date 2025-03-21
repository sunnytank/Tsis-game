using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDrop4 : MonoBehaviour
{
    public MonkeyBoss monkeyboss;
    public GameObject Zombie;
    public Transform MONskeyBoss;
    // Start is called before the first frame update
    void Start()
    {
        monkeyboss = Zombie.GetComponent<MonkeyBoss>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3 (MONskeyBoss.position.x + 7f ,MONskeyBoss.position.y + 7f ,MONskeyBoss.position.z);
        
    }
}
