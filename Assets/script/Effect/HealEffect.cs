using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffect : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Player;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player"); 
        
    }

    // Update is called once per frame
    void Update()
    {
        player = Player.GetComponent<Transform>();
        transform.position = new Vector3 (player.position.x  ,player.position.y  ,player.position.z);
        Invoke("DesTroy",0.3f);
    }

    void DesTroy()
    {
        Destroy(gameObject);
    }
}
