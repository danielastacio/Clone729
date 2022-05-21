using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inv_Player : MonoBehaviour
{
    [Header("Assignables")]
    [SerializeField] private GameObject testDuplicateTemplate;
    public Transform par_PlayerInventory;

    [Header("Scripts")]
    [SerializeField] private UI_SkillTree SkillTreeScript;
    [SerializeField] private GameObject par_Managers;

    //public but hidden variables
    [HideInInspector] public bool isInventoryOpen;
    [HideInInspector] public int playerMoney;
    [HideInInspector] public int skillpoints;
    [HideInInspector] public List<GameObject> inventory;

    //private variables
    private Manager_UIReuse UIReuseScript;

    private void Awake()
    {
        UIReuseScript = par_Managers.GetComponent<Manager_UIReuse>();
    }

    private void Update()
    {
        if (!par_Managers.GetComponent<UI_PauseMenu>().isGamePaused
            && !SkillTreeScript.isSkillTreeUIOpen
            && Input.GetKeyDown(KeyCode.Tab))
        {
            isInventoryOpen = !isInventoryOpen;

        }
        if (!isInventoryOpen
            && UIReuseScript.par_Inventory.activeInHierarchy)
        {
            CloseUI();
        }
        else if (isInventoryOpen
                 && !UIReuseScript.par_Inventory.activeInHierarchy)
        {
            OpenUI();
        }

        /*
        --------------------------------------------
        DEBUGGING FEATURE - REMOVE BEFORE RELEASE!!!
        
        adds a placeholder item into players inventory
        --------------------------------------------
        */
        if (!par_Managers.GetComponent<UI_PauseMenu>().isGamePaused
            && isInventoryOpen
            && Input.GetKeyDown(KeyCode.X))
        {
            testDuplicateTemplate.GetComponent<Env_Item>().PickUpTest();
        }
    }

    private void OpenUI()
    {
        par_Managers.GetComponent<UI_PauseMenu>().PauseGame();

        UIReuseScript.par_Inventory.SetActive(true);
        RebuildPlayerInventory();

        UIReuseScript.txt_PlayerMoney.text = playerMoney.ToString();

        UIReuseScript.EnableReturnButton();
    }
    public void CloseUI()
    {
        UIReuseScript.DisableReturnButton();

        UIReuseScript.ClearInventoryData();
        UIReuseScript.ClearInventoryList();
        UIReuseScript.par_Inventory.SetActive(false);

        par_Managers.GetComponent<UI_PauseMenu>().UnpauseGame();
    }

    public void RebuildPlayerInventory()
    {
        UIReuseScript.ClearInventoryData();
        UIReuseScript.ClearInventoryList();

        foreach (GameObject item in inventory)
        {
            //spawn a new button
            Button itemButton = Instantiate(UIReuseScript.btn_InventoryButtonTemplate,
                                            UIReuseScript.inventoryContent.transform,
                                            true);
            //add button to buttons list
            UIReuseScript.inventoryButtons.Add(itemButton);
            //add name to button
            itemButton.GetComponentInChildren<TMP_Text>().text = item.GetComponent<Env_Item>().str_FakeItemName;
            //add function to button
            itemButton.onClick.AddListener(item.GetComponent<Env_Item>().ShowItemData);
        }
    }
}