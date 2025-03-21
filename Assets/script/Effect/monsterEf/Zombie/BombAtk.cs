using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAtk : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject monster;
    public Transform Monster;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3 (Monster.position.x - 0.1f  ,Monster.position.y  ,Monster.position.z);
    }
}
