using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager_UIReuse : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private Inv_Player PlayerInventoryScript;
    [SerializeField] private GameObject par_Managers;

    [Header("Player UI")]
    public Slider PlayerHealthBar;
     
    [Header("Inventory content")]
    public TMP_Text txt_ItemName;
    public TMP_Text txt_ItemDescription;
    public TMP_Text txt_ItemValue;
    public TMP_Text txt_ItemValueValue;
    public TMP_Text txt_ItemDamage;
    public TMP_Text txt_ItemDamageValue;
    public TMP_Text txt_ItemRemainder;
    public TMP_Text txt_ItemRemainderValue;
    public TMP_Text txt_PlayerMoney;
    public Button btn_InventoryButtonTemplate;
    public Transform inventoryContent;
    public GameObject par_Inventory;
    [HideInInspector] public List<Button> inventoryButtons;

    [Header("Inventory buttons")]
    public Button btn_Inv1;
    public Button btn_Inv2;
    public Button btn_Inv3;
    public Button btn_ReturnToGame;

    [Header("Skill tree content")]
    public TMP_Text txt_Skillpoints;
    public GameObject par_SkillTree;
    public GameObject par_SkillTreeButtons;
    [HideInInspector] public List<Button> skillTreeButtons;

    [Header("Loading UI content")]
    public TMP_Text txt_GameSaveName;
    public TMP_Text txt_SaveTime;
    public TMP_Text txt_SaveLoc;
    public Button btn_LoadGame;
    public Button btn_SaveFileButtonTemplate;
    public GameObject par_SaveListContent;
    public GameObject par_LoadMenuUI;

    //clears inventory data
    public void ClearInventoryData()
    {
        txt_ItemName.text = "";
        txt_ItemDescription.text = "";

        txt_ItemValueValue.text = "";
        txt_ItemValue.gameObject.SetActive(false);
        txt_ItemValueValue.gameObject.SetActive(false);

        txt_ItemDamageValue.text = "";
        txt_ItemDamage.gameObject.SetActive(false);
        txt_ItemDamageValue.gameObject.SetActive(false);

        txt_ItemRemainderValue.text = "";
        txt_ItemRemainder.gameObject.SetActive(false);
        txt_ItemRemainderValue.gameObject.SetActive(false);

        btn_Inv1.onClick.RemoveAllListeners();
        btn_Inv1.GetComponentInChildren<TMP_Text>().text = "";
        btn_Inv1.interactable = false;
        btn_Inv1.gameObject.SetActive(false);

        btn_Inv2.onClick.RemoveAllListeners();
        btn_Inv2.GetComponentInChildren<TMP_Text>().text = "";
        btn_Inv2.interactable = false;
        btn_Inv2.gameObject.SetActive(false);

        btn_Inv3.onClick.RemoveAllListeners();
        btn_Inv3.GetComponentInChildren<TMP_Text>().text = "";
        btn_Inv3.interactable = false;
        btn_Inv3.gameObject.SetActive(false);
    }
    //clears inventory buttons
    public void ClearInventoryList()
    {
        if (inventoryButtons.Count > 0)
        {
            foreach (Transform button in inventoryContent)
            {
                Destroy(button.gameObject);
            }
            inventoryButtons.Clear();
        }
    }

    public void UpdateSkillTreeButtons()
    {
        foreach (Button button in skillTreeButtons)
        {
            button.onClick.RemoveAllListeners();
            button.interactable = false;

            button.GetComponent<UI_Skill>().UpdateButtonStatus();
        }
    }

    public void EnableReturnButton()
    {
        btn_ReturnToGame.gameObject.SetActive(true);
        btn_ReturnToGame.interactable = true;

        if (PlayerInventoryScript.isInventoryOpen)
        {
            btn_ReturnToGame.onClick.AddListener(PlayerInventoryScript.CloseUI);
        }
        else if (par_Managers.GetComponent<UI_SkillTree>().isSkillTreeUIOpen)
        {
            btn_ReturnToGame.onClick.AddListener(par_Managers.GetComponent<UI_SkillTree>().CloseUI);
        }
    }
    public void DisableReturnButton()
    {
        btn_ReturnToGame.onClick.RemoveAllListeners();
        btn_ReturnToGame.interactable = false;
        btn_ReturnToGame.gameObject.SetActive(false);
    }

    public void UpdatePlayerHealthUI(float currentHealth, float maxHealth)
    {
        PlayerHealthBar.value = currentHealth;
        PlayerHealthBar.maxValue = maxHealth;
    }
}