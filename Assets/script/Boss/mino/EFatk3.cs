using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EFatk3 : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; 
    public minoBoss minoBoss;
    public GameObject Zombie;
    public Transform minoboss;
    public bool filpX;
    public GameObject gameObject;
    // Start is called before the first frame update
    void Start()
    {
        minoBoss = Zombie.GetComponent<minoBoss>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        filpX = minoBoss.filpX;

        if(!filpX)
        {
            transform.position = new Vector3 (minoboss.position.x -0.471f ,minoboss.position.y + 0.42f ,minoboss.position.z);
            spriteRenderer.flipX = false;
        }
        else if(filpX)
        {
            transform.position = new Vector3 (minoboss.position.x +0.471f ,minoboss.position.y + 0.42f ,minoboss.position.z);
            spriteRenderer.flipX = true;
        }
        
        if (gameObject.activeSelf)
        {
            Invoke("End" , 0.3f);
        }
    }

    void End()
    {
        gameObject.SetActive(false);
    }
}
