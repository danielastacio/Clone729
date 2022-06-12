using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ProfileSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private DataPersistenceManager.ProfileIds profileId;

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI PlayerSpawnText;
    [SerializeField] private TextMeshProUGUI PlayerHealthText;

    private Button slotButton;

    private void Awake()
    {
        slotButton = GetComponent<Button>();
       
    }
    private void Update()
    {
        //if (MainMenu.isStartingNewGame)
        //{
        //    slotButton.interactable = true;
        //}
        
        //if (MainMenu.isLoadingSavedGame && gameData == null)
        //{
        //    slotButton.interactable = false;
        //}

        if (Input.GetKeyDown(KeyCode.Equals))
        {
      

        }

    }
    public void SetData(GameData gameData)
    {
        // there's no data for this profileId
        if (gameData == null)
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        // there is data for this profileId
        else
        {
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);

            PlayerSpawnText.text = "Spawn Point: " + gameData.playerSpawnPoint;
            PlayerHealthText.text = "Player Health: " + gameData.playerCurrentHp;
        }
    }

    public string GetProfileId()
    {
        return profileId.ToString();
    }
    public void SetInteractable(bool interactable)
    {
        slotButton.interactable = interactable;
    }

    public void OnSlotButtonClicked()
    {
        DataPersistenceManager.Instance.ChangeSelectedProfileId(profileId);

        if (MainMenu.isLoadingSavedGame)
        {
            DataPersistenceManager.Instance.LoadGame();
            SceneManager.LoadSceneAsync(1);
        }


        else if (MainMenu.isStartingNewGame)
        {
            DataPersistenceManager.Instance.NewGame();
            DataPersistenceManager.Instance.SaveGame();
            SceneManager.LoadSceneAsync(1);
        }

    }
}
