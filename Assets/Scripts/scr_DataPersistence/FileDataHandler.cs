using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using scr_Player;
public class FileDataHandler
{
    private string FolderPath 
    {
        get 
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SaveData");
            string profileId = DataPersistenceManager.Instance.GetProfileId();
            string fileName = profileId;
            string folderPath = Path.Combine(filePath, profileId, fileName);

            return folderPath;
        }
    }

    
    private void CreateFolderPath(string value) => Directory.CreateDirectory(Path.GetDirectoryName(value));
    
    public void Save(GameData data)
    {
        try
        {
            CreateFolderPath(FolderPath);
            
            string json = JsonUtility.ToJson(data, true);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(json);
            var base64 = System.Convert.ToBase64String(plainTextBytes);

            using (var stream = File.Open(FolderPath, FileMode.Create))
            {
                using var writer = new StreamWriter(stream); 
                writer.Write(base64);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + FolderPath + "\n" + e);
        }
    }
    public GameData Load()
    {
        GameData loadedData = null;
        if (File.Exists(FolderPath))
        {
            try
            {
                using var streamReader = new StreamReader(FolderPath);

                var dataToLoad = streamReader.ReadToEnd();
                var plainTextBytes = System.Convert.FromBase64String(dataToLoad);
                var json = System.Text.Encoding.UTF8.GetString(plainTextBytes);

                loadedData = JsonUtility.FromJson<GameData>(json);
            }

            catch (System.Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + FolderPath + "\n" + e);
            }

        }

        return loadedData;

    }    
}
