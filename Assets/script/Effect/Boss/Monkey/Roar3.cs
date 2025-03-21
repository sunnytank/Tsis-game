using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roar3 : MonoBehaviour
{
    public GameObject monster;
    public Transform Monster;
    public GameObject gameObject;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
            transform.position = new Vector3 (Monster.position.x - 1.73f  ,Monster.position.y + 8.15f  ,Monster.position.z);
        
        if (gameObject.activeSelf)
        {
            Invoke("End" , 0.6f);
        }
    }

    void End()
    {
        gameObject.SetActive(false);
    }
}
