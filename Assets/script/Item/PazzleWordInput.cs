using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PazzleWordInput : MonoBehaviour
{
    public bool Active;
    public Animator animator;

    public PazzleWord PazzleWord;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Active = PazzleWord.setActive;
        
        if(Active)
        {
            animator.SetBool("pass",true);
        }
        
    }
}
