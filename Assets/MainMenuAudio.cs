using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    public AK.Wwise.Event menuEnter, menuReturn, menuStartGame;
    public AK.Wwise.Bank mainMenuBank;
    // Start is called before the first frame update
    private void Awake()
    {
        mainMenuBank.Load();
    }
    void Start()
    {
        UI_MainMenu.Instance.BTN_StartNewGame.onClick.AddListener(PlayStartGame);
        UI_MainMenu.Instance.BTN_LoadGame.onClick.AddListener(PlayMenuEnter);
        UI_MainMenu.Instance.BTN_ShowLoadUI.onClick.AddListener(PlayMenuEnter);
        UI_MainMenu.Instance.BTN_ShowCreditsUI.onClick.AddListener(PlayMenuEnter);
        UI_MainMenu.Instance.BTN_ReturnToMainMenu.onClick.AddListener(PlayMenuEnter);
        UI_MainMenu.Instance.BTN_QuitGame.onClick.AddListener(PlayMenuReturn);

       // PlayStartGame();
    }

    void PlayMenuEnter()
    {
        menuEnter.Post(gameObject);
    }

    void PlayMenuReturn()
    {
        menuReturn.Post(gameObject);
    }

    void PlayStartGame()
    {
        menuStartGame.Post(gameObject);
    }
}

