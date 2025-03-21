using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICommand : MonoBehaviour
{

    public GameObject Ui;
    public GameObject PazzleNumber;
    public GameObject PazzleWord;
    public GameObject PazzleDetail;
    public GameObject Shop;
    public GameObject DeadUi;
    public GameObject SettingOption;
    public GameObject OptionSound;
    public GameObject GuideBook;
    // Start is called before the first frame update
    void Start()
    {

    }
    
    // Update is called once per frame
    void Update()
    {
        if(Shop != null)
        {
            if(PazzleNumber.activeSelf || PazzleWord.activeSelf || PazzleDetail.activeSelf || Shop.activeSelf || SettingOption.activeSelf || OptionSound.activeSelf || DeadUi.activeSelf || GuideBook.activeSelf)
            {
            Ui.SetActive(false);
            }
            else
            {
            Ui.SetActive(true);
            }
        }
        else
        {
            if(PazzleNumber.activeSelf || PazzleWord.activeSelf || PazzleDetail.activeSelf || SettingOption.activeSelf || OptionSound.activeSelf || DeadUi.activeSelf || GuideBook.activeSelf)
            {
            Ui.SetActive(false);
            }
            else
            {
            Ui.SetActive(true);
            }
        }
    }
}
