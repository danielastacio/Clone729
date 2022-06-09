using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using scr_Interfaces;
public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager Instance { get; private set; }
    private GameData gameData = new();
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    private readonly string mostRecentProfile = "mostRecentProfile";
    private enum ProfileIds
    {
        profile1,
        profile2,
        profile3
    }

    private ProfileIds selectedProfileId;
    public string GetProfileId()
    {
        return selectedProfileId.ToString();
    }

    private void SetProfileId(int selectedProfileId)
    {
        int totalProfiles = System.Enum.GetNames(typeof(ProfileIds)).Length;

        if (selectedProfileId >= 0 && selectedProfileId <= totalProfiles)
        {
            this.selectedProfileId = (ProfileIds)selectedProfileId;
        }
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
        //LoadGame((int)selectedProfileId);
    }

    private void OnApplicationQuit()
    {
        SaveGame((int)selectedProfileId);
    }
    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void Continue()
    {
        LoadGame(PlayerPrefs.GetInt(mostRecentProfile, default));
    }
    public void LoadGame(int selectedProfileId)
    { 
        //Get the selected profile Id before loading any saved data
        SetProfileId(selectedProfileId);

        // load any saved data from a file using the data handler
        this.gameData = dataHandler.Load();

        // if no data can be loaded, initialize to a new game
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataObject in dataPersistenceObjects)
        {
            dataObject.LoadData(gameData);
        }
    }
    public void SaveGame(int selectedProfileId)
    {
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
        SetProfileId(selectedProfileId);

        PlayerPrefs.SetInt(mostRecentProfile, selectedProfileId);
        // save that data to a file using the data handler
        dataHandler.Save(gameData);
    }
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}