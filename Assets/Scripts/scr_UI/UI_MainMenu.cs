using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [Header("Scripts")]
    [SerializeField] private GameObject par_Managers; 

    private void Awake()
    {
        btn_StartNewGame.onClick.AddListener(StartNewGame);
        btn_ShowLoadUI.onClick.AddListener(ShowLoadMenuUI);
        btn_ShowCreditsUI.onClick.AddListener(ShowCreditsUI);
        btn_QuitGame.onClick.AddListener(QuitGame);

        btn_ReturnToMainMenu.onClick.AddListener(ShowMainMenuUI);

        ShowMainMenuUI();
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

        par_MainMenuContent.SetActive(true);
    }
    public void ShowLoadMenuUI()
    {
        par_MainMenuContent.SetActive(false);

        par_Managers.GetComponent<Manager_GameSaving>().OpenLoadUI();

        btn_ReturnToMainMenu.gameObject.SetActive(true);
    }
    public void ShowCreditsUI()
    {
        par_MainMenuContent.SetActive(false);

        par_CreditsContent.SetActive(true);
        btn_ReturnToMainMenu.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}