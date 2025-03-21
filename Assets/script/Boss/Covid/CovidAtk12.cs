using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CovidAtk12 : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; 
    public CovidBoss covidBoss;
    public GameObject Zombie;
    public Transform covidboss;
    public bool filpX;
    public GameObject gameObject;
    // Start is called before the first frame update
    void Start()
    {
        covidBoss = Zombie.GetComponent<CovidBoss>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        filpX = covidBoss.filpX;

        if(!filpX)
        {
            transform.position = new Vector3 (covidboss.position.x -0.225f ,covidboss.position.y -2.34f ,covidboss.position.z);
            spriteRenderer.flipX = true;
        }
        else if(filpX)
        {
            transform.position = new Vector3 (covidboss.position.x +0.225f ,covidboss.position.y -2.34f ,covidboss.position.z);
            spriteRenderer.flipX = false;
        }
        
        if (gameObject.activeSelf)
        {
            Invoke("End" , 0.8f);
        }
    }

    void End()
    {
        gameObject.SetActive(false);
    }
}
