using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileSlotsMenu : MonoBehaviour
{
    private ProfileSlot[] profileSlots;
    // Start is called before the first frame update
    void Awake()
    {
        profileSlots = GetComponentsInChildren<ProfileSlot>();
    }

    private void OnEnable()
    {
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.Instance.LoadProfiles();
        foreach (ProfileSlot profileSlot in profileSlots)
        {
            profilesGameData.TryGetValue(profileSlot.GetProfileId(), out GameData profileData);
            profileSlot.SetData(profileData);
            if(profileSlot == null && MainMenu.isLoadingSavedGame)
            {
                profileSlot.SetInteractable(false);
            }

            else
            {
                profileSlot.SetInteractable(true);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            DataPersistenceManager.Instance.LoadProfiles()
                .TryGetValue("A", out GameData profileData);
            Debug.Log(profileData.playerSpawnPoint);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            DataPersistenceManager.Instance.LoadProfiles()
                .TryGetValue("A", out GameData profileData);
            Debug.Log(profileData.playerSpawnPoint);
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            DataPersistenceManager.Instance.LoadProfiles()
                .TryGetValue("A", out GameData profileData);
            Debug.Log(profileData.playerSpawnPoint);
        }
    }
}
