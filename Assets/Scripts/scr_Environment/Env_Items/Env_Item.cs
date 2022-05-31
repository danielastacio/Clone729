using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Env_Item : MonoBehaviour
{
    [Header("Assignables")]
    public bool isProtected;
    public bool isStackable;
    public string str_ItemName;
    public string str_ItemDescription;
    public string str_ItemValue;

    [Header("Scripts")]
    [SerializeField] private Inv_Player PlayerInventoryScript;
    [SerializeField] private GameObject par_Managers;
    
    //public but hidden variables
    [HideInInspector] public bool isInPlayerInventory;
    [HideInInspector] public bool isInContainer;
    [HideInInspector] public bool isInTraderShop;
    [HideInInspector] public string str_FakeItemName;

    //private variables
    Manager_UIReuse UIReuseScript;

    private void Awake()
    {
        UIReuseScript = par_Managers.GetComponent<Manager_UIReuse>();

        //fake item name does not use any underscores
        str_FakeItemName = str_ItemName.Replace('_', ' ');
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            isInPlayerInventory = true;
            collision.gameObject.GetComponent<Inv_Player>().inventory.Add(gameObject);
        }
    }
    */

    public void ShowItemData()
    {
        UIReuseScript.ClearInventoryData();

        UIReuseScript.txt_ItemName.text = str_ItemName;
        UIReuseScript.txt_ItemDescription.text = str_ItemDescription;

        UIReuseScript.txt_ItemValue.gameObject.SetActive(true);
        UIReuseScript.txt_ItemValueValue.gameObject.SetActive(true);
        UIReuseScript.txt_ItemValueValue.text = str_ItemValue;

        UIReuseScript.btn_Inv2.gameObject.SetActive(true);
        UIReuseScript.btn_Inv2.GetComponentInChildren<TMP_Text>().text = "Drop";

        UIReuseScript.btn_Inv3.gameObject.SetActive(true);
        UIReuseScript.btn_Inv3.GetComponentInChildren<TMP_Text>().text = "Destroy";

        if (!isProtected)
        {
            UIReuseScript.btn_Inv2.interactable = true;
            UIReuseScript.btn_Inv2.onClick.AddListener(Drop);

            UIReuseScript.btn_Inv3.interactable = true;
            UIReuseScript.btn_Inv3.onClick.AddListener(Destroy);
        }
    }

    private void Drop()
    {
        Debug.Log("Dropped " + str_FakeItemName + "!");
    }
    private void Destroy()
    {
        Debug.Log("Destroyed " + str_FakeItemName + "!");
    }
}