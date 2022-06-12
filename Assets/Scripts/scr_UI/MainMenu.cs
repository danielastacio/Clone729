using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }

    [Header("Menu Navigation")]
    [SerializeField] private GameObject profileSlotsMenu;
    [SerializeField] private GameObject titleScreenMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button backButton;

    public static bool isStartingNewGame { get; private set; }
    public static bool isLoadingSavedGame { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }
    private void Start() 
    {
        if (!DataPersistenceManager.Instance.HasGameData()) 
        {
            loadGameButton.gameObject.SetActive(false);
            continueGameButton.gameObject.SetActive(false);
        }
    }
    public void OnNewGameClicked()
    {
        if (!DataPersistenceManager.Instance.HasGameData())
        {
            DataPersistenceManager.Instance.NewGame();
            SceneManager.LoadSceneAsync(1);
        }
        else
        {

            profileSlotsMenu.SetActive(true);
            titleScreenMenu.SetActive(false);
            backButton.gameObject.SetActive(true);

            isStartingNewGame = true;
        }
    }

    public void OnLoadGameClicked() 
    {
        isLoadingSavedGame = true;

        backButton.gameObject.SetActive(true);
        profileSlotsMenu.SetActive(true);
        titleScreenMenu.SetActive(false);
    }

    public void OnBackButtonClicked()
    {
        profileSlotsMenu.SetActive(false);
        titleScreenMenu.SetActive(true);

        isStartingNewGame = false;
        isLoadingSavedGame = false;
        backButton.gameObject.SetActive(false);
    }
}
