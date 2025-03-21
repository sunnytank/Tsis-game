using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEF : MonoBehaviour
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
       
            transform.position = new Vector3 (Monster.position.x + 0.24f  ,Monster.position.y + 0.8f ,Monster.position.z);
        
        if (gameObject.activeSelf)
        {
            Invoke("End" , 0.3f);
        }
    }

    void End()
    {
        gameObject.SetActive(false);
    }
}
