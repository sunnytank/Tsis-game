using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDataDie
{
   
   public float soul;
   public float[] position;

   public void CreatePlayerDataDie(PlayerHealth player)
   {
        soul = player.SoulAmount;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

   }

}
