using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigzommoveSpawn : MonoBehaviour
{
    public MuscleZombie muscleZombie;
    public GameObject Zombie;
    public bool spriteRendererZom;
    public Transform Musclezombie;
    public Vector3 Transform1;

    // Start is called before the first frame update
    void Start()
    {
        muscleZombie = Zombie.GetComponent<MuscleZombie>();
        
    }

    // Update is called once per frame
    void Update()
    {
        spriteRendererZom = muscleZombie.SpriteRendererZom;


        if(spriteRendererZom == true)
        {
        transform.position = new Vector3 (Musclezombie.position.x -1.6f ,Musclezombie.position.y + 2f ,Musclezombie.position.z);
        }
        else if(spriteRendererZom == false)
        {
        transform.position = new Vector3 (Musclezombie.position.x + 1.6f ,Musclezombie.position.y + 2f ,Musclezombie.position.z);
        }


    }
}
