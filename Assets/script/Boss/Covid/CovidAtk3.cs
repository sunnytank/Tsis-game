using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CovidAtk3 : MonoBehaviour
{
    public GameObject gameObject;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {      
        if (gameObject.activeSelf)
        {
            Invoke("End" , 2.5f);
        }
    }

    void End()
    {
        gameObject.SetActive(false);
    }
}
