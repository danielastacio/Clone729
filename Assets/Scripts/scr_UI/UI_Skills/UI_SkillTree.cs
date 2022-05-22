using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillTree : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private Inv_Player PlayerInventoryScript;
    [SerializeField] private GameObject par_Managers;

    //public but hidden variables
    [HideInInspector] public bool isSkillTreeUIOpen;

    //private variables
    private Manager_UIReuse UIReuseScript;

    private void Awake()
    {
        UIReuseScript = par_Managers.GetComponent<Manager_UIReuse>();

        foreach (Transform btn in UIReuseScript.par_SkillTreeButtons.transform)
        {
            UIReuseScript.skillTreeButtons.Add(btn.GetComponent<Button>());
        }
    }

    private void Update()
    {
        if (!par_Managers.GetComponent<UI_PauseMenu>().isGamePaused
            && !PlayerInventoryScript.isInventoryOpen
            && Input.GetKeyDown(KeyCode.Z))
        {
            isSkillTreeUIOpen = !isSkillTreeUIOpen;

            if (!isSkillTreeUIOpen
                && UIReuseScript.par_SkillTree.activeInHierarchy)
            {
                CloseUI();
            }
            else if (isSkillTreeUIOpen
                     && !UIReuseScript.par_SkillTree.activeInHierarchy)
            {
                OpenUI();
            }
        }
    }

    private void OpenUI()
    {
        par_Managers.GetComponent<UI_PauseMenu>().PauseGame();

        UIReuseScript.par_SkillTree.SetActive(true);
        UIReuseScript.txt_Skillpoints.text = PlayerInventoryScript.skillpoints.ToString();
        UIReuseScript.UpdateSkillTreeButtons();

        UIReuseScript.EnableReturnButton();
    }
    public void CloseUI()
    {
        UIReuseScript.DisableReturnButton();

        UIReuseScript.par_SkillTree.SetActive(false);

        isSkillTreeUIOpen = false;
        par_Managers.GetComponent<UI_PauseMenu>().UnpauseGame();
    }
}