using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager_GameSaving : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private GameObject par_Managers;

    //public but hidden variables
    [HideInInspector] public string path;

    //private variables
    private readonly List<Button> saves = new();
    private string saveTime;
    private string saveLoc;

    private Manager_UIReuse UIReuseScript;

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

        UIReuseScript = par_Managers.GetComponent<Manager_UIReuse>();

        ClearSaveData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveGame();
        }
    }

    public void SaveGame()
    {
        par_Managers.GetComponent<GameManager>().GetScene();
        if (par_Managers.GetComponent<GameManager>().scene != 0)
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
        par_Managers.GetComponent<GameManager>().GetScene();
        if (par_Managers.GetComponent<GameManager>().scene != 0
            && par_Managers.GetComponent<UI_PauseMenu>().isGamePaused)
        {
            par_Managers.GetComponent<UI_PauseMenu>().par_PauseMenuContent.SetActive(false);
        }
        UIReuseScript.par_LoadMenuUI.SetActive(true);

        //clear out previous buttons
        foreach (Transform child in UIReuseScript.par_SaveListContent.transform)
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
    public void AddDataToList()
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
                        Button btn_saveFileButton = Instantiate(UIReuseScript.btn_SaveFileButtonTemplate);
                        //change new button parent
                        btn_saveFileButton.transform.SetParent(UIReuseScript.par_SaveListContent.transform, false);

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
    public void GetSaveData(string saveName)
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
        UIReuseScript.btn_LoadGame.onClick.AddListener(delegate { LoadGame(saveName); });
        //enable button interaction
        UIReuseScript.btn_LoadGame.interactable = true;

        //list all separators
        char[] extraSeparators = new char[] { 'X', '0' };
        //remove unwanted separators and split line into separate strings
        string[] extraLines = saveName.Split(extraSeparators, StringSplitOptions.RemoveEmptyEntries);
        //new text
        string txt = extraLines[0] + " " + extraLines[1];

        //assign save info
        UIReuseScript.txt_GameSaveName.text = txt;
        UIReuseScript.txt_SaveTime.text = saveTime;
        UIReuseScript.txt_SaveLoc.text = saveLoc;
    }
    public void ClearSaveData()
    {
        //remove button events
        UIReuseScript.btn_LoadGame.onClick.RemoveAllListeners();
        //disable button interaction
        UIReuseScript.btn_LoadGame.interactable = false;

        //clear save info
        UIReuseScript.txt_GameSaveName.text = "";
        UIReuseScript.txt_SaveTime.text = "";
        UIReuseScript.txt_SaveLoc.text = "";
    }

    public void LoadGame(string saveName)
    {
        Debug.Log("Loading data from " + saveName + ".");
    }
}