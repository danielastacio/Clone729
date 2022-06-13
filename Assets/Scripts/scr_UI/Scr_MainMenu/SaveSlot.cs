using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    private enum ProfileId { A, B, C}
    [Header("Profile")]
    [SerializeField] private ProfileId profileId;

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI lastUpdatedText;
    [SerializeField] private TextMeshProUGUI playerHealthText;

    private Button saveSlotButton;

    private void Awake() 
    {
        saveSlotButton = this.GetComponent<Button>();
    }

    public void SetData(GameData data) 
    {
        // there's no data for this profileId
        if (data == null) 
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        // there is data for this profileId
        else 
        {
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);

            lastUpdatedText.text = data.lastUpdatedString + "";
            playerHealthText.text = "HP: " + data.playerCurrentHp;
        }
    }

    public string GetProfileId() 
    {
        return this.profileId.ToString();
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
    }
}
