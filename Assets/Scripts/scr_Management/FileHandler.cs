using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using scr_Player;
public class FileHandler : MonoBehaviour
{
    private string FullPath 
    {
        get 
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SaveData");
            string fileName = "BinaryJson.json";
            string fullPath = Path.Combine(filePath, fileName);

            return fullPath;
        }
    }

    private void CreateFolderPath() => Directory.CreateDirectory(Path.GetDirectoryName(FullPath));

    public class GameData 
    {
        public float playerCurrentHp;
        public Transform playerSpawnPoint;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals)) Save();
        if (Input.GetKeyDown(KeyCode.Minus)) Load();
    }
    public void Save()
    {
        var data = new GameData
        {
            playerCurrentHp = PlayerController.Instance.currentHp,
            playerSpawnPoint = SpawnManager.Instance.spawnPoints[1]
        };

        try
        {
            CreateFolderPath();
            
            string json = JsonUtility.ToJson(data, true);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(json);
            var base64 = System.Convert.ToBase64String(plainTextBytes);

            using (var stream = File.Open(FullPath, FileMode.Create))
            {
                using var writer = new StreamWriter(stream); 
                writer.Write(base64);
            }
            Debug.Log("Saved Data!");

        }
        catch (System.Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + FullPath + "\n" + e);
        }
    }
    public void Load()
    {
        GameData data = null;
        if (File.Exists(FullPath))
        {
            try
            {
                using var streamReader = new StreamReader(FullPath);

                var dataToLoad = streamReader.ReadToEnd();
                var plainTextBytes = System.Convert.FromBase64String(dataToLoad);
                var json = System.Text.Encoding.UTF8.GetString(plainTextBytes);

                data = JsonUtility.FromJson<GameData>(json);
            }

            catch (System.Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + FullPath + "\n" + e);
            }

            PlayerController.Instance.currentHp = data.playerCurrentHp;
            PlayerController.Instance.transform.position = data.playerSpawnPoint.position;
        }

        Debug.Log("Loaded Saved Data!");
    }    
}
