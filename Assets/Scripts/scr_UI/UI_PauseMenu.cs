using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UI_PauseMenu : MonoBehaviour
{
    [Header("Script checking")]
    [SerializeField] private Inv_Player PlayerInventoryScript;
    [SerializeField] private GameObject par_Managers;

    [Header("Pause menu UI")]
    [SerializeField] private Button btn_ReturnToGame;
    [SerializeField] private Button btn_ReturnToPauseMenu;
    [SerializeField] private Button btn_SaveGame;
    [SerializeField] private Button btn_ShowLoadMenuUI; 
    [SerializeField] private Button btn_ReturnToMainMenu;
    [SerializeField] private Button btn_QuitGame;
    [SerializeField] private GameObject par_PauseMenuContent;
    [SerializeField] private GameObject par_PauseMenuUI;

    [Header("Load menu UI")]
    [SerializeField] private TMP_Text txt_GameSaveName;
    [SerializeField] private TMP_Text txt_SaveTime;
    [SerializeField] private TMP_Text txt_SaveLoc;
    [SerializeField] private Button btn_LoadGame;
    [SerializeField] private Button btn_SaveFileButtonTemplate;
    [SerializeField] private GameObject par_SaveListContent;
    [SerializeField] private GameObject par_LoadMenuUI;

    //public but hidden variables
    [HideInInspector] public bool isGamePaused;

    //private variables
    private string path;
    private readonly List<Button> saves = new();
    private string saveTime;
    private string saveLoc;

    private void Awake()
    {
        try
        {
            //get path to game saves folder
            path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\mvjam";

            if (!Directory.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error: Failed to create save folder at " + path + "! " + e.Message + ".");
        }

        ClearSaveData();
        ClosePauseMenuUI();

        btn_ReturnToGame.onClick.AddListener(ClosePauseMenuUI);
        btn_ReturnToPauseMenu.onClick.AddListener(OpenPauseMenuUI);
        btn_SaveGame.onClick.AddListener(SaveGame);
        btn_ShowLoadMenuUI.onClick.AddListener(OpenLoadUI);
        btn_ReturnToMainMenu.onClick.AddListener(ReturnToMainMenu);
        btn_QuitGame.onClick.AddListener(QuitGame);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGamePaused = !isGamePaused;

            if (!isGamePaused
                && par_PauseMenuUI.activeInHierarchy)
            {
                ClosePauseMenuUI();
            }
            else if (isGamePaused
                     && !par_PauseMenuUI.activeInHierarchy)
            {
                OpenPauseMenuUI();
            }
        }
    }

    public void PauseGame()
    {
        //pauses game timer
        Time.timeScale = 0;
    }
    public void UnpauseGame()
    {
        //continues game timer
        Time.timeScale = 1;
    }

    public void OpenPauseMenuUI()
    {
        PauseGame();

        par_PauseMenuUI.SetActive(true);
        par_PauseMenuContent.SetActive(true);
        par_LoadMenuUI.SetActive(false);
    }
    public void ClosePauseMenuUI()
    {
        par_PauseMenuContent.SetActive(false);
        par_PauseMenuUI.SetActive(false);

        if (!PlayerInventoryScript.isInventoryOpen
            && !par_Managers.GetComponent<UI_SkillTree>().isSkillTreeUIOpen)
        {
            UnpauseGame();
        }

        isGamePaused = false;
    }

    public void SaveGame()
    {
        try
        {
            string saveFilePath = path + @"\_SaveX";

            //list all files in path into new array
            string[] files = Directory.GetFiles(path);
            //check if new array actually found any files
            if (files.Length > 0)
            {
                //list of confirmed numbers
                List<int> numbers = new();

                //get all separators in line
                char[] separators = new char[] { 'X', '.' };

                foreach (string line in files)
                {
                    //remove unwanted separators and split line into separate strings
                    string[] values = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    //add all numbers to new list
                    foreach (string value in values)
                    {
                        bool isInt = int.TryParse(value, out _);
                        if (isInt)
                        {
                            numbers.Add(int.Parse(value));
                        }
                    }
                }

                //sort numbers list
                numbers.Sort();
                //find newest save number
                int newestSaveNumber = Mathf.Max(0, numbers[^1]);

                newestSaveNumber++;
                Debug.Log(newestSaveNumber);

                string addedSaveNumber = "";
                if (newestSaveNumber < 10)
                {
                    addedSaveNumber = "000" + newestSaveNumber.ToString();
                }
                else if (newestSaveNumber >= 10
                         && newestSaveNumber < 100)
                {
                    addedSaveNumber = "00" + newestSaveNumber.ToString();
                }
                else if (newestSaveNumber >= 100
                         && newestSaveNumber < 1000)
                {
                    addedSaveNumber = "0" + newestSaveNumber.ToString();
                }
                else if (newestSaveNumber >= 1000)
                {
                    addedSaveNumber = newestSaveNumber.ToString();
                }

                saveFilePath += addedSaveNumber;

                InsertDataToNewSave(saveFilePath + ".txt");
            }
            else
            {
                saveFilePath += "0001";

                InsertDataToNewSave(saveFilePath + ".txt");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error: Failed to create save file at " + path + "! " + e.Message + ".");
        }
    }
    private void InsertDataToNewSave(string saveFilePath)
    {
        //create a new file at path and insert data into it
        using StreamWriter saveFile = File.CreateText(saveFilePath);

        saveFile.WriteLine("Save file for mvjamgame");
        saveFile.WriteLine("");
        saveFile.WriteLine("WARNING: Invalid values will break the game - edit at your own risk!");
        saveFile.WriteLine("");

        saveFile.WriteLine("--- GLOBAL VALUES ---");
        saveFile.WriteLine("");

        //get current local time
        string time = DateTime.Now.ToString("T");
        saveFile.WriteLine("gv_saveTime = " + time);

        //random location name
        saveFile.WriteLine("gv_saveLoc = loc" + UnityEngine.Random.Range(1, 25));

        Debug.Log("Created new save file at path " + saveFilePath + "!");
    }

    public void OpenLoadUI()
    {
        par_PauseMenuContent.SetActive(false);
        par_LoadMenuUI.SetActive(true);

        //clear out previous buttons
        foreach (Transform child in par_SaveListContent.transform)
        {
            Destroy(child.gameObject);
        }
        ClearSaveData();

        try
        {
            AddDataToList();
        }
        catch (Exception e)
        {
            Debug.LogError("Error: Failed to read save folder at " + path + "! " + e.Message + ".");
        }
    }
    private void AddDataToList()
    {
        //list all files in path into new array
        string[] files = Directory.GetFiles(path);
        //check if new array actually found any files
        if (files.Length > 0)
        {
            //loop through all found files
            foreach (string file in files)
            {
                //list all separators
                char[] separators = new char[] { '_', '.', '-' };
                //remove unwanted separators and split line into separate strings
                string[] lines = file.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in lines)
                {
                    if (line.Contains("Save"))
                    {
                        //list all separators
                        char[] extraSeparators = new char[] { 'X', '0' };
                        //remove unwanted separators and split line into separate strings
                        string[] extraLines = line.Split(extraSeparators, StringSplitOptions.RemoveEmptyEntries);
                        //new text
                        string txt = extraLines[0] + " " + extraLines[1];

                        //create a new button
                        Button btn_saveFileButton = Instantiate(btn_SaveFileButtonTemplate);
                        //change new button parent
                        btn_saveFileButton.transform.SetParent(par_SaveListContent.transform, false);

                        //change button text
                        btn_saveFileButton.GetComponentInChildren<TMP_Text>().text = txt;
                        //add event to button
                        btn_saveFileButton.onClick.AddListener(delegate { ShowSaveData(line); });

                        //save new button to list
                        saves.Add(btn_saveFileButton);
                    }
                }
            }
        }
    }
    private void GetSaveData(string saveName)
    {
        //get full path to save  file
        string saveFilePath = path + @"\_" + saveName + ".txt";

        //read each line in save file
        foreach (string line in File.ReadLines(saveFilePath))
        {
            //get all separators in line
            char[] separators = new char[] { ' ', ',', '=', '(', ')', '_' };
            //remove unwanted separators and split line into separate strings
            string[] values = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            //read global values
            if (line.Contains("gv_"))
            {
                //find save time
                if (line.Contains("saveTime"))
                {
                    saveTime = values[2] + " " + values[3];
                }
                //find save location
                else if (line.Contains("saveLoc"))
                {
                    saveLoc = values[2];
                }
            }
        }
    }
    public void ShowSaveData(string saveName)
    {
        ClearSaveData();
        GetSaveData(saveName);

        //add button event
        btn_LoadGame.onClick.AddListener(delegate { LoadGame(saveName); });
        //enable button interaction
        btn_LoadGame.interactable = true;

        //list all separators
        char[] extraSeparators = new char[] { 'X', '0' };
        //remove unwanted separators and split line into separate strings
        string[] extraLines = saveName.Split(extraSeparators, StringSplitOptions.RemoveEmptyEntries);
        //new text
        string txt = extraLines[0] + " " + extraLines[1];

        //assign save info
        txt_GameSaveName.text = txt;
        txt_SaveTime.text = saveTime;
        txt_SaveLoc.text = saveLoc;
    }
    private void ClearSaveData()
    {
        //remove button events
        btn_LoadGame.onClick.RemoveAllListeners();
        //disable button interaction
        btn_LoadGame.interactable = false;

        //clear save info
        txt_GameSaveName.text = "";
        txt_SaveTime.text = "";
        txt_SaveLoc.text = "";
    }
    public void LoadGame(string saveName)
    {
        Debug.Log("Loading data from " + saveName + ".");
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}