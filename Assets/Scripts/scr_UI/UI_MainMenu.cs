using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [Header("Main menu UI")]
    [SerializeField] private GameObject par_MainMenuContent;
    [SerializeField] private GameObject par_LoadContent;
    [SerializeField] private GameObject par_CreditsContent;

    [Header("Buttons")]
    [SerializeField] private Button btn_StartNewGame;
    [SerializeField] private Button btn_ShowLoadUI;
    [SerializeField] private Button btn_LoadGame;
    [SerializeField] private Button btn_ShowCreditsUI;
    [SerializeField] private Button btn_QuitGame;
    [SerializeField] private Button btn_ReturnToMainMenu;

    [Header("Load menu UI")]
    [SerializeField] private TMP_Text txt_GameSaveName;
    [SerializeField] private TMP_Text txt_SaveTime;
    [SerializeField] private TMP_Text txt_SaveLoc;
    [SerializeField] private Button btn_SaveFileButtonTemplate;
    [SerializeField] private GameObject par_SaveListContent;

    //private variables
    private string path;
    private readonly List<Button> saves = new();
    private string saveTime;
    private string saveLoc;

    private void Awake()
    {
        btn_StartNewGame.onClick.AddListener(StartNewGame);
        btn_ShowLoadUI.onClick.AddListener(ShowLoadUI);
        btn_ShowCreditsUI.onClick.AddListener(ShowCreditsUI);
        btn_QuitGame.onClick.AddListener(QuitGame);

        btn_ReturnToMainMenu.onClick.AddListener(ShowMainMenuUI);

        ShowMainMenuUI();

        try
        {
            //get path to game saves folder
            path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\mvjam";

            if (!Directory.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
            }
            AddSaveDataToList();
        }
        catch (Exception e)
        {
            Debug.LogError("Error: Failed to create save folder at " + path + "! " + e.Message + ".");
        }
    }

    private void AddSaveDataToList()
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
                        //create a new button
                        Button btn_saveFileButton = Instantiate(btn_SaveFileButtonTemplate);
                        //change new button parent
                        btn_saveFileButton.transform.SetParent(par_SaveListContent.transform, false);

                        //list all separators
                        char[] extraSeparators = new char[] { 'X', '0' };
                        //remove unwanted separators and split line into separate strings
                        string[] extraLines = line.Split(extraSeparators, StringSplitOptions.RemoveEmptyEntries);
                        //new text
                        string txt = extraLines[0] + " " + extraLines[1];

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

    public void StartNewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowMainMenuUI()
    {
        par_LoadContent.SetActive(false);
        par_CreditsContent.SetActive(false);
        btn_ReturnToMainMenu.gameObject.SetActive(false);

        ClearSaveData();
        par_MainMenuContent.SetActive(true);
    }
    public void ShowLoadUI()
    {
        par_MainMenuContent.SetActive(false);

        par_LoadContent.SetActive(true);
        btn_ReturnToMainMenu.gameObject.SetActive(true);
    }
    public void ShowCreditsUI()
    {
        par_MainMenuContent.SetActive(false);

        par_CreditsContent.SetActive(true);
        btn_ReturnToMainMenu.gameObject.SetActive(true);
    }

    public void LoadGame(string saveName)
    {
        Debug.Log("Loading data from " + saveName + ".");
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

    public void QuitGame()
    {
        Application.Quit();
    }
}