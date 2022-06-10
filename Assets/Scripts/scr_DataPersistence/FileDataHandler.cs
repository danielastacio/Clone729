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
    private void CreateFullPath(string value) => Directory.CreateDirectory(Path.GetDirectoryName(value));
    
    public void Save(GameData data)
    {
        try
        {
            CreateFullPath(FullPath);
            
            string json = JsonUtility.ToJson(data, true);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(json);
            var base64 = System.Convert.ToBase64String(plainTextBytes);

            using var stream = File.Open(FullPath, FileMode.Create);
            using var writer = new StreamWriter(stream);
            writer.Write(base64);
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

        return loadedData;

    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        // loop over all directory names in the data directory path
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(FilePath).EnumerateDirectories();
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            // defensive programming - check if the data file exists
            // if it doesn't, then this folder isn't a profile and should be skipped
            if (!File.Exists(FullPath))
            {
                Debug.LogWarning("Skipping directory when loading all profiles because it does not contain data: "
                    + profileId);
                continue;
            }

            // load the game data for this profile and put it in the dictionary
            GameData profileData = Load();
            // defensive programming - ensure the profile data isn't null,
            // because if it is then something went wrong and we should let ourselves know
            if (profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }
            else
            {
                Debug.LogError("Tried to load profile but something went wrong. ProfileId: " + profileId);
            }
        }

        return profileDictionary;
    }
}
