using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class covidDashPosition1 : MonoBehaviour
{
    public GameObject player;
    public float playerY;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
    }

    // Update is called once per frame
    void Update()
    {
        playerY = player.transform.position.y;

        transform.position = new Vector3 (transform.position.x ,playerY ,transform.position.z);
    }
}
