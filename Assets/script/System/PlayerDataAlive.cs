using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDataAlive
{
     public float maxHealth;
     public float Health;
     public float maxStamina;
     public float maxShield;
     public float Shield;
     public int Attack;
     public float Soul;
     public int ScenePlayer;
     public float KeyCurrent;
     public float RedPotionCurrent;

   public void CreatePlayerDataAlive(PlayerHealth player)
   {
     maxHealth = player.maxHealthPlayer;
     Health = player.currentHealthPlayer;
     maxShield = player.maxShield;
     Shield = player.currentShield;
     maxStamina = player.maxStamina;
     Attack = player.attackDamage;
     Soul = player.SoulAmount;
     ScenePlayer = player.LoadSceneSave;
     KeyCurrent = player.Key;
     RedPotionCurrent = player.PotionRed;
   }

}
