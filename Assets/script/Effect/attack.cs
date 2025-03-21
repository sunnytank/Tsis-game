using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : MonoBehaviour
{
    public GameObject Player;
    public Transform player;
    public Playermovement playermovement;
    public GameObject gameObject;
    bool flip;
    // Start is called before the first frame update
    void Start()
    {
        playermovement = Player.GetComponent<Playermovement>();
    }

    // Update is called once per frame
    void Update()
    {
        flip = playermovement.Flip;
        if(!flip)
        {
            transform.position = new Vector3 (player.position.x - 1.2f  ,player.position.y  ,player.position.z);
        }
        else if (flip)
        {
            transform.position = new Vector3 (player.position.x + 1.2f ,player.position.y  ,player.position.z);
        }
        if (gameObject.activeSelf)
        {
            Invoke("End" , 0.2f);
        }
    }

    void End()
    {
        gameObject.SetActive(false);
    }
}
