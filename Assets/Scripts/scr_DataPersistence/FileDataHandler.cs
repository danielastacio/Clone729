using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using scr_Player;
public class FileDataHandler
{
    private string FullPath 
    {
        get 
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SaveData");
            string fileName = DataPersistenceManager.Instance.currentSave.ToString();
            string fullPath = Path.Combine(filePath, fileName, fileName);

            return fullPath;
        }
    }


    private void CreateFolderPath() => Directory.CreateDirectory(Path.GetDirectoryName(FullPath));
    
    public void Save(GameData data)
    {
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
    public GameData Load()
    {
        GameData loadedData = null;
        if (File.Exists(FullPath))
        {
            try
            {
                using var streamReader = new StreamReader(FullPath);

                var dataToLoad = streamReader.ReadToEnd();
                var plainTextBytes = System.Convert.FromBase64String(dataToLoad);
                var json = System.Text.Encoding.UTF8.GetString(plainTextBytes);

                loadedData = JsonUtility.FromJson<GameData>(json);
            }

            catch (System.Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + FullPath + "\n" + e);
            }

        }

        Debug.Log("Loaded Saved Data!");

        return loadedData;

    }    
}
