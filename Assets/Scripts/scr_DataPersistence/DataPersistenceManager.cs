using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using scr_Interfaces;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager Instance { get; private set; }
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    private string mostRecentProfile = "mostRecentProfile";
    public enum ProfileIds { A, B, C }

    public ProfileIds selectedProfileId;

    public string GetProfileId()
    {
        return selectedProfileId.ToString();
    }

    public void ChangeSelectedProfileId(ProfileIds selectedProfileId)
    {
        this.selectedProfileId = selectedProfileId;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;

        this.dataHandler = new FileDataHandler();

        HasGameData();
        
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        if(gameData != null)
        {
           LoadGame();
        }
    }

    public void OnSceneUnloaded(Scene scene)
    {
        if (gameData != null)
        {
            SaveGame();
        }
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void Continue()
    {
        selectedProfileId = (ProfileIds)PlayerPrefs.GetInt(mostRecentProfile, default);

        LoadGame();

        SceneManager.LoadScene(1);
        Debug.Log("Loaded most recent save: " + GetProfileId());
    }
    public void LoadGame()
    { 

        // load any saved data from a file using the data handler
        dataHandler.Load().TryGetValue(GetProfileId(), out this.gameData);
        // if no data can be loaded, initialize to a new game
        if(gameData == null)
        {
            return;
        }

        Debug.Log("Loaded Save File: " +  GetProfileId());
        Debug.Log("Player Spawn: " + gameData.playerSpawnPoint);

        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataObject in dataPersistenceObjects)
        {
            dataObject.LoadData(gameData);
        }
    }
    public void SaveGame()
    {
        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataObject in dataPersistenceObjects)
        {
            dataObject.SaveData(this.gameData);
        }

        // if we don't have any data to save, log a warning here
        if (this.gameData == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }
        Debug.Log("Saved game to: " + GetProfileId());

        PlayerPrefs.SetInt(mostRecentProfile, (int)selectedProfileId);
        // save that data to a file using the data handler
        dataHandler.Save(this.gameData);

    }
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public Dictionary<string, GameData> LoadProfiles()
    {
        return dataHandler.Load();
    }

    public bool HasGameData()
    {
        if (LoadProfiles().Count == 0)
        {
            return false;
        }

        else
        {
            return true;
        }
    }
}