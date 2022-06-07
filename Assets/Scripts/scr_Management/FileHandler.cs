using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using scr_Player;
public class FileHandler : MonoBehaviour
{
    private string dataDirPath => Path.Combine(Directory.GetCurrentDirectory(), "SaveData");
    private string dataFileName = "BinaryJson.json";
    private string fullPath => Path.Combine(dataDirPath, dataFileName);
    private string json => File.ReadAllText(fullPath);
    public class GameData 
    {
        public float playerCurrentHp;
        public Transform playerSpawnPoint;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals)) Save();
        //if (Input.GetKeyDown(KeyCode.Minus)) Load();
    }
    public void Save()
    {
        GameData data = new GameData();
        data.playerCurrentHp = PlayerController.Instance.currentHp;
        data.playerSpawnPoint = SpawnManager.Instance.spawnPoints[1];
        // use Path.Combine to account for different OS's having different path separators
        try
        {
            // create the directory the file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(dataToStore);
            var encryptData = System.Convert.ToBase64String(plainTextBytes);

            using (var stream = File.Open(fullPath, FileMode.Create))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(encryptData);
                }
            }

            Debug.Log("Saved Data!");

        }
        catch (System.Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }
    public void Load()
    {
        if (File.Exists(fullPath))
        {
            GameData data = JsonUtility.FromJson<GameData>(json);

            PlayerController.Instance.currentHp = data.playerCurrentHp;
            PlayerController.Instance.transform.position = data.playerSpawnPoint.position;
        }

        Debug.Log("Loaded Saved Data!");
    }



}
