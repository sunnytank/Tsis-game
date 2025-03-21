using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    // Start is called before the first frame update
    bool PlayerIn;
    public GameObject PopUp;

    void Update()
    {
        if(PlayerIn)
        {
            PopUp.SetActive(true);
        }
        else
        {
            PopUp.SetActive(false);
        }


    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ตรวจสอบว่าชนกับไอเท็มหรือไม่
        {
           PlayerIn = true;
        }
    } 

        void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ตรวจสอบว่าผู้เล่นออกจากพื้นที่สำหรับไอเท็มหรือไม่
        {
            PlayerIn = false;
        }
    }

}
