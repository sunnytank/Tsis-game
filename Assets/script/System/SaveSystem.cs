using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    
    public static void SavePlayerAlive(PlayerHealth player)
    {
    BinaryFormatter formatter = new BinaryFormatter();
    string path = Application.persistentDataPath + "/Player.CheckPoint";
    FileStream stream = new FileStream(path, FileMode.Create);

    PlayerDataAlive data = new PlayerDataAlive();
    data.CreatePlayerDataAlive(player);
    
    formatter.Serialize(stream, data);
    stream.Close();
    }

    /*public static void SavePlayerDie(PlayerHealth player)
    {
    BinaryFormatter formatter = new BinaryFormatter();
    string path = Application.persistentDataPath + "/Player.Die";
    FileStream stream = new FileStream(path, FileMode.Create);

    PlayerDataDie data = new PlayerDataDie();
    data.CreatePlayerDataDie(player);

    formatter.Serialize(stream, data);
    stream.Close();
    }*/

    public static PlayerDataAlive LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.CheckPoint";
        if (File.Exists(path))
        {
            BinaryFormatter Formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path,FileMode.Open);

            PlayerDataAlive data = Formatter.Deserialize(stream) as PlayerDataAlive;
            stream.Close();

            return data;
        }
        else{
            Debug.LogError("Save fie not found in " + path);
            return null;
        }

    }

    /*public static PlayerDataDie LoadPlayerD()
    {
        string path = Application.persistentDataPath + "/player.Die";
        if (File.Exists(path))
        {
            BinaryFormatter Formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path,FileMode.Open);

            PlayerDataDie data = Formatter.Deserialize(stream) as PlayerDataDie;
            stream.Close();

            return data;
        }
        else{
            Debug.LogError("Save fie not found in " + path);
            return null;
        }

    }*/

}
