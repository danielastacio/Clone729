using System.Collections.Generic;
using System.Linq;
using scr_Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace scr_DataPersistence
{
    public class DataPersistenceManager : MonoBehaviour
    {

        [Header("File Storage Config")]
        private readonly string fileName = "gamedata.json";
        [SerializeField] private bool useEncryption;

        private GameData _gameData;
        private List<IDataPersistence> _dataPersistenceObjects;
        private FileDataHandler _dataHandler;

        private string _selectedProfileId = "";

        public static DataPersistenceManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this.gameObject);



            this._dataHandler = new FileDataHandler();

            this._selectedProfileId = _dataHandler.GetMostRecentlyUpdatedProfileId();

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
            _dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
        }

        public void OnSceneUnloaded(Scene scene)
        {
            SaveGame();
        }

        public void ChangeSelectedProfileId(string newProfileId)
        {
            // update the profile to use for saving and loading
            this._selectedProfileId = newProfileId;
            // load the game, which will use that profile, updating our game data accordingly
            LoadGame();
        }

        public string GetProfileId()
        {
            return _selectedProfileId;
        }

        public void NewGame()
        {
            this._gameData = new GameData();
        }

        public void LoadGame()
        {
            // load any saved data from a file using the data handler
            this._gameData = _dataHandler.Load(_selectedProfileId);

            // if no data can be loaded, don't continue
            if (this._gameData == null)
            {
                Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
                return;
            }

            // push the loaded data to all other scripts that need it
            foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(_gameData);
            }
        }

        public void SaveGame()
        {
            // if we don't have any data to save, log a warning here
            if (this._gameData == null)
            {
                Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
                return;
            }

            // pass the data to other scripts so they can update it
            foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
            {
                dataPersistenceObj.SaveData(_gameData);
            }

            // timestamp the data so we know when it was last saved
            _gameData.lastUpdated = System.DateTime.Now.ToBinary();
            _gameData.lastUpdatedString = System.DateTime.Now.ToString();

            // save that data to a file using the data handler
            _dataHandler.Save(_gameData, _selectedProfileId);
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }

        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
                .OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);
        }

        public bool HasGameData()
        {
            return _gameData != null;
        }

        public Dictionary<string, GameData> GetAllProfilesGameData()
        {
            return _dataHandler.LoadAllProfiles();
        }
    }
}
