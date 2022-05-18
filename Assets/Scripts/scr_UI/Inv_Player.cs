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
    [SerializeField] private GameObject par_Managers;

    //public but hidden variables
    [HideInInspector] public bool isInventoryOpen;
    [HideInInspector] public int playerMoney;
    [HideInInspector] public List<GameObject> inventory;

    //private variables
    private Manager_UIReuse UIReuseManager;

    private void Awake()
    {
        UIReuseManager = par_Managers.GetComponent<Manager_UIReuse>();

        isInventoryOpen = true;
    }

    private void Update()
    {
        if (!par_Managers.GetComponent<UI_PauseMenu>().isGamePaused
            && Input.GetKeyDown(KeyCode.Tab))
        {
            isInventoryOpen = !isInventoryOpen;

            ToggleInventory();
        }

        if (!par_Managers.GetComponent<UI_PauseMenu>().isGamePaused
            && Input.GetKeyDown(KeyCode.X))
        {
            testDuplicateTemplate.GetComponent<Env_Item>().PickUpTest();
        }
    }

    public void ToggleInventory()
    {
        if (!isInventoryOpen)
        {
            //pauses game timer
            Time.timeScale = 0;

            UIReuseManager.par_Inventory.SetActive(true);
            RebuildPlayerInventory();

            UIReuseManager.txt_PlayerMoney.text = playerMoney.ToString();
        }
        else
        {
            UIReuseManager.ClearInventoryData();
            UIReuseManager.ClearInventoryList();
            UIReuseManager.par_Inventory.SetActive(false);

            //continues game timer
            Time.timeScale = 1;
        }
    }

    public void RebuildPlayerInventory()
    {
        UIReuseManager.ClearInventoryData();
        UIReuseManager.ClearInventoryList();

        foreach (GameObject item in inventory)
        {
            //spawn a new button
            Button itemButton = Instantiate(UIReuseManager.btn_InventoryButtonTemplate,
                                            UIReuseManager.inventoryContent.transform,
                                            true);
            //add button to buttons list
            UIReuseManager.inventoryButtons.Add(itemButton);
            //add name to button
            itemButton.GetComponentInChildren<TMP_Text>().text = item.GetComponent<Env_Item>().str_FakeItemName;
            //add function to button
            itemButton.onClick.AddListener(item.GetComponent<Env_Item>().ShowItemData);
        }
    }
}