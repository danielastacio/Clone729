using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public GameObject par_PauseMenuContent;
    [SerializeField] private GameObject par_PauseMenuUI;

    //public but hidden variables
    [HideInInspector] public bool isGamePaused;

    //private variables
    private Manager_UIReuse UIReuseScript;

    private void Awake()
    {
        UIReuseScript = par_Managers.GetComponent<Manager_UIReuse>();

        ClosePauseMenuUI();

        btn_ReturnToGame.onClick.AddListener(ClosePauseMenuUI);
        btn_ReturnToPauseMenu.onClick.AddListener(OpenPauseMenuUI);
        btn_SaveGame.onClick.AddListener(par_Managers.GetComponent<Manager_GameSaving>().SaveGame);
        btn_ShowLoadMenuUI.onClick.AddListener(par_Managers.GetComponent<Manager_GameSaving>().OpenLoadUI);
        btn_ReturnToMainMenu.onClick.AddListener(ReturnToMainMenu);
        btn_QuitGame.onClick.AddListener(QuitGame);
    }

    private void Update()
    {
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
        UIReuseScript.par_LoadMenuUI.SetActive(false);
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