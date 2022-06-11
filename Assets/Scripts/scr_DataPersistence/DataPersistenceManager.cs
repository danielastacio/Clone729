using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using scr_Interfaces;
public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager Instance { get; private set; }
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    private string mostRecentProfile = "mostRecentProfile";
    public TextMeshProUGUI message;
    public enum ProfileIds { A, B, C }

    //private int totalProfiles = System.Enum.GetNames(typeof(ProfileIds)).Length;
    private ProfileIds selectedProfileId;

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
        }
        Instance = this;

    }
    private void Start()
    {
        this.dataHandler = new FileDataHandler();
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            Debug.Log(LoadProfiles().Count);
            Debug.Log(HasGameData());
            
        }
    }
    private void OnApplicationQuit()
    {
        //SaveGame((int)selectedProfileId);
    }
    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void Continue()
    {
        selectedProfileId = (ProfileIds)PlayerPrefs.GetInt(mostRecentProfile, default);

        LoadGame();
        message.text = "Loaded most recent save: " + GetProfileId();
    }
    SaveSlot slot;
    public void LoadGame()
    { 
        message.text = "Loaded Save File: " +  GetProfileId();

        // load any saved data from a file using the data handler
        dataHandler.Load().TryGetValue(GetProfileId(), out this.gameData);
        // if no data can be loaded, initialize to a new game

        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataObject in dataPersistenceObjects)
        {
            dataObject.LoadData(gameData);
        }
    }
    public void SaveGame()
    {
        message.text = "Saved game to: " + GetProfileId();
        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataObject in dataPersistenceObjects)
        {
            dataObject.SaveData(gameData);
        }

        // if we don't have any data to save, log a warning here
        if (this.gameData == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }

        //Get the selected profile Id before loading any saved data
        //SetProfileId(selectedProfileId);

        PlayerPrefs.SetInt(mostRecentProfile, (int)selectedProfileId);
        // save that data to a file using the data handler
        dataHandler.Save(gameData);

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
        return gameData != null;
    }
}