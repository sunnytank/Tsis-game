using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigZomThrow : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject Item;
    public float launchSpeed = 10f;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.V))
        {
            var itemThrow = Instantiate(Item, launchPoint.position, launchPoint.rotation);
        }
    }
}
