using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class MainMenuAudio : MonoBehaviour
{
    [SerializeField]
    private AK.Wwise.Event menuEnter, menuReturn, menuSelect, menuStartGame;

    [SerializeField]
    private AK.Wwise.Bank mainMenuBank;
    // Start is called before the first frame update
    private void Awake()
    {
        mainMenuBank.Load();
    }

    public void PlayMenuSelect()
    {
        menuSelect.Post(gameObject);
    }
    public void PlayMenuEnter()
    {
        menuEnter.Post(gameObject);
    }

    public void PlayMenuReturn()
    {
        menuReturn.Post(gameObject);
    }

    public void PlayStartGame()
    {
        menuStartGame.Post(gameObject);
    }
}

