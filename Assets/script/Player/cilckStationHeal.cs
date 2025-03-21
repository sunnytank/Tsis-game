using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cilckStationHeal : MonoBehaviour
{
    bool PlayerInHeal;
    bool inOption;
    bool HealComplate;
    public bool Optionselect;
    int HealAmountOption1 = 10;
    int HealAmountOption2 = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ClickStation();
        OptionHeal();

    }
    
    void ClickStation()
    {
        if(UserInput.instance.controls.Interact.interact.WasPressedThisFrame() && PlayerInHeal == true && HealComplate == false  && inOption == false)
        {
            Invoke("inOp", 0.25f);
            Optionselect = true;
        }

        if(UserInput.instance.controls.Interact.interact.WasPressedThisFrame() && inOption == true)
        {
            inOption = false;
            Debug.Log("inOption = false;");
        }
    }

    void inOp()
    {
        inOption = true;
        Debug.Log("inOption = true;");
    }

    void OptionHeal()
    {
        if(Input.GetKeyDown(KeyCode.J) && Optionselect)
        {   
            GetComponent<PlayerHealth>().Heal(HealAmountOption1);
            Optionselect = false;
            inOption = false;
            HealComplate = true;
        }
        else if(Input.GetKeyDown(KeyCode.K) && Optionselect)
        {   
            GetComponent<PlayerHealth>().Heal(HealAmountOption2);
            Optionselect = false;
            inOption = false;
            HealComplate = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Healstation")) // ตรวจสอบว่าชนกับไอเท็มหรือไม่
        {
           PlayerInHeal = true;
        }
    } 
        void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Healstation")) // ตรวจสอบว่าผู้เล่นออกจากพื้นที่สำหรับไอเท็มหรือไม่
        {
            PlayerInHeal = false;
        }
    }
}
