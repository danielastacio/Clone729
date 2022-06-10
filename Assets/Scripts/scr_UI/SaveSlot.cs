using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SaveSlot : MonoBehaviour
{
    DataPersistenceManager dataManager;
    [SerializeField] private DataPersistenceManager.ProfileIds profileId;
    private GameData gameData;
    private Button slotButton;
    // Use this for initialization
    void Awake()
    {        
        slotButton = GetComponent<Button>();
        
    }

    void Start()
    {
        LoadProfileData();

        if (gameData == null)
        {
            slotButton.interactable = false;
        }

        else
        {
            slotButton.interactable = true;
        }
    }
    public void LoadProfileData()
    { 
        DataPersistenceManager.Instance.GetAllProfiles().TryGetValue(profileId.ToString(), out gameData);
    }
}
