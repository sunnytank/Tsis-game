using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCampSystem : MonoBehaviour
{
    public bool CampSaveSet;
    public bool PlayerInCheckPoint;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
     if(PlayerInCheckPoint)
     {
        animator.SetBool("savecamp",true);
     }   

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ตรวจสอบว่าชนกับไอเท็มหรือไม่
        {
           PlayerInCheckPoint = true;
        }
    } 
}
