using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager_UIReuse : MonoBehaviour
{
    [Header("Script checking")]
    [SerializeField] private Inv_Player PlayerInventoryScript;

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

    private void Awake()
    {
        btn_ReturnToGame.onClick.AddListener(CloseInventory);
    }

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

    public void CloseInventory()
    {
        if (PlayerInventoryScript.isInventoryOpen)
        {
            PlayerInventoryScript.ToggleInventory();
        }
    }
}