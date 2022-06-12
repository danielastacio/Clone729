using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using scr_Player;
public class FileDataHandler
{
    private string FilePath { get; } = Path.Combine(Directory.GetCurrentDirectory(), "Saved Data");
    private string FullPath
    {
        get
        {
            string profileId = DataPersistenceManager.Instance.GetProfileId();
            string fileName = profileId;

            return Path.Combine(FilePath, profileId, fileName);
        }
    }
    
    public void Save(GameData data)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FullPath));
            
            string json = JsonUtility.ToJson(data, true);
            //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(json);
            //var base64 = System.Convert.ToBase64String(plainTextBytes);

            using var stream = File.Open(FullPath, FileMode.Create);
            using var writer = new StreamWriter(stream);
            writer.Write(json);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + FullPath + "\n" + e);
        }
    }
    public Dictionary<string, GameData> Load()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        // loop over all directory names in the data directory path
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(FilePath).EnumerateDirectories();
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;
            GameData profileData;
            // defensive programming - check if the data file exists
            // if it doesn't, then this folder isn't a profile and should be skipped

            if (File.Exists(FullPath))
            {
                try
                {
                    using var streamReader = new StreamReader(FullPath);

                    var dataToLoad = streamReader.ReadToEnd();
                   // var plainTextBytes = System.Convert.FromBase64String(dataToLoad);
                   // var json = System.Text.Encoding.UTF8.GetString(plainTextBytes);

                    profileData = JsonUtility.FromJson<GameData>(dataToLoad);
                    profileDictionary.Add(profileId, profileData);
                }

                catch (System.Exception e)
                {
                    Debug.LogError("Error occured when trying to load data from file: " + FullPath + "\n" + e);
                }

            }

            else
            {
                Debug.LogWarning("Skipping directory when loading all profiles because it does not contain data: "
                    + DataPersistenceManager.Instance.GetProfileId());
                continue;
            }
        }
        return profileDictionary;
    }
}
