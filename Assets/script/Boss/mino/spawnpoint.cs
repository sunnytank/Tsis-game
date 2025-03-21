using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnpoint : MonoBehaviour
{
    // Start is called before the first frame update
    public minoBoss minoBoss;
    public GameObject Zombie;
    public Transform minoboss;
    public bool filpX;
    // Start is called before the first frame update
    void Start()
    {
        minoBoss = Zombie.GetComponent<minoBoss>();
    }

    // Update is called once per frame
    void Update()
    {
        filpX = minoBoss.filpX;

        if(!filpX)
        {
            transform.position = new Vector3 (minoboss.position.x -2.74f ,minoboss.position.y - 0.22f ,minoboss.position.z);
        }
        else if(filpX)
        {
            transform.position = new Vector3 (minoboss.position.x +2.74f ,minoboss.position.y - 0.22f ,minoboss.position.z);
        }
        
    }
}
