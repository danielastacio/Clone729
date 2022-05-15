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
    [Header("Assignables")]
    [SerializeField] private GameObject par_MainMenuContent;
    [SerializeField] private GameObject par_LoadContent;
    [SerializeField] private GameObject par_CreditsContent;
    [SerializeField] private GameObject par_SaveListContent;

    [Header("Buttons")]
    [SerializeField] private Button btn_StartNewGame;
    [SerializeField] private Button btn_ShowLoadUI;
    [SerializeField] private Button btn_LoadGame;
    [SerializeField] private Button btn_ShowCreditsUI;
    [SerializeField] private Button btn_QuitGame;
    [SerializeField] private Button btn_ReturnToMainMenu;
    [SerializeField] private Button btn_SaveFileButtonTemplate;

    [Header("Load data")]
    [SerializeField] private TMP_Text txt_GameSaveName;

    //public but hidden variables
    [HideInInspector] public string path;

    //private variables
    private readonly List<string> fileNames = new();
    private readonly List<Button> saves = new();

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

                AddSaveDataToList();
            }
            else
            {
                AddSaveDataToList();
            }
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
                        fileNames.Add(line);
                    }
                }
            }
        }

        foreach (string file in fileNames)
        {
            //create a new button
            Button btn_saveFileButton = Instantiate(btn_SaveFileButtonTemplate);
            //change new button parent
            btn_saveFileButton.transform.SetParent(par_SaveListContent.transform, false);

            //change button text
            btn_saveFileButton.GetComponentInChildren<TMP_Text>().text = file;
            //add event to button
            btn_saveFileButton.onClick.AddListener(delegate { ShowSaveData(file); });

            //save new button to list
            saves.Add(btn_saveFileButton);
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

        //add save name
        txt_GameSaveName.text = saveName;
        //add button event
        btn_LoadGame.onClick.AddListener(delegate { LoadGame(saveName); });
        //add button text
        btn_LoadGame.GetComponentInChildren<TMP_Text>().text = saveName;
    }
    private void ClearSaveData()
    {
        //clear save name
        txt_GameSaveName.text = "";
        //remove button events
        btn_LoadGame.onClick.RemoveAllListeners();
        //remove button text
        btn_LoadGame.GetComponentInChildren<TMP_Text>().text = "";
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}