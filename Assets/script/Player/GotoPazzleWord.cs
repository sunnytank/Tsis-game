using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoPazzleWord : MonoBehaviour
{
    public GameObject panelInputWord;
    public GameObject panelInputNumber; 
    public GameObject panelUIGame;

    public bool InPanelWord;
    public bool InPanelNumber;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GotoPanel();
    }

    public void GotoPanel()
    {
        if(panelInputWord.activeSelf || panelInputNumber.activeSelf)
        {
            panelUIGame.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PazzleWord"))
        {
            InPanelWord = true;
        }
        else
        {
            InPanelWord = false;
        }
        if (other.CompareTag("PazzleNumber"))
        {
            InPanelNumber = true;
        }
        else
        {
            InPanelNumber = false;
        }
    } 
}
