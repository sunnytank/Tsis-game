using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAwake : MonoBehaviour
{
    public Chemicalmonster chemicleZombie;
    public GameObject Zombie;
    public bool spriteRendererZom;
    public Transform ChemicleZombie;
    public Vector3 Transform1;

    // Start is called before the first frame update
    void Start()
    {
        chemicleZombie = Zombie.GetComponent<Chemicalmonster>();   
    }

    // Update is called once per frame
    void Update()
    {
        spriteRendererZom = chemicleZombie.SpriteRendererZom;


        if(spriteRendererZom == false)
        {
        transform.position = new Vector3 (ChemicleZombie.position.x - 3.4f ,ChemicleZombie.position.y -1.0f ,ChemicleZombie.position.z);
        }
        else if(spriteRendererZom == true)
        {
        transform.position = new Vector3 (ChemicleZombie.position.x + 4.2f ,ChemicleZombie.position.y -1.0f ,ChemicleZombie.position.z);
        }
        
    }
}
