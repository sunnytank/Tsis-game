using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CovidAtk2 : MonoBehaviour
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
            Invoke("End" , 0.5f);
        }
    }

    void End()
    {
        gameObject.SetActive(false);
    }
}
