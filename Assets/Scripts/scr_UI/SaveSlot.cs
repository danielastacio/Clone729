using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SaveSlot : MonoBehaviour
{
    [SerializeField] private DataPersistenceManager.ProfileIds profileId;

    private GameData gameData;
    private Button slotButton;
    [SerializeField] private bool isGameLoading;
    // Use this for initialization
    void Awake()
    {
        slotButton = GetComponent<Button>();
        slotButton.onClick.AddListener(OnSlotButtonClicked);
    }

    void Start()
    {
        //LoadProfileData();

        //if (gameData == null)
        //{
        //    slotButton.interactable = false;
        //}

        //else
        //{
        //    slotButton.interactable = true;
        //}
    }    
    public void LoadProfileData()
    {
        DataPersistenceManager.Instance.LoadProfiles().TryGetValue(profileId.ToString(), out gameData);
    }

    private void OnSlotButtonClicked()
    {
        DataPersistenceManager.Instance.ChangeSelectedProfileId(profileId);

        if (isGameLoading)
        {
            DataPersistenceManager.Instance.LoadGame();
        }

        else
        {
            DataPersistenceManager.Instance.SaveGame();
        }
    }
}
