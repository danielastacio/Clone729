using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Skill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Assignables")]
    public string str_SkillName;
    [SerializeField] private string str_SkillDescription;
    [SerializeField] private int skillPointsRequired;
    [SerializeField] private int skillPointUpgradeMultiplier;

    [Header("Scripts")]
    [SerializeField] private UI_Tooltip TooltipScript;
    [SerializeField] private Inv_Player PlayerInventoryScript;
    [SerializeField] private GameObject par_Managers;

    //public but hidden variables
    [HideInInspector] public int skillTier;

    //private variables
    private Manager_UIReuse UIReuseScript;

    private void Awake()
    {
        UIReuseScript = par_Managers.GetComponent<Manager_UIReuse>();
    }

    //loads tooltip when hovering over this skill
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltipText();
    }
    //closes tooltip when no longer hovering over this skill
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipScript.showTooltipUI = false;
    }

    public void UpdateButtonStatus()
    {
        if (PlayerInventoryScript.skillpoints >= skillPointsRequired
            && skillTier < 3)
        {
            gameObject.GetComponent<Button>().interactable = true;
            gameObject.GetComponent<Button>().onClick.AddListener(UnlockOrUpgrade);
        }
    }
    public void UnlockOrUpgrade()
    {
        //increases this skill tier
        skillTier++;
        //removes skillpoints
        PlayerInventoryScript.skillpoints -= skillPointsRequired;
        //updates player skillpoints text
        UIReuseScript.txt_Skillpoints.text = PlayerInventoryScript.skillpoints.ToString();
        //increases skillpoint requirement
        if (skillTier != 3)
        {
            skillPointsRequired *= skillPointUpgradeMultiplier;
        }
        else
        {
            skillPointsRequired = 0;
        }

        UIReuseScript.UpdateSkillTreeButtons();
        StartCoroutine(UpdateTooltip());
    }

    //simple update for tooltip when selected ability data updates
    private IEnumerator UpdateTooltip()
    {
        TooltipScript.showTooltipUI = false;

        yield return new WaitForSecondsRealtime(0.1f);

        ShowTooltipText();
    }
    //shows the actual tooltip ui
    private void ShowTooltipText()
    {
        TooltipScript.showTooltipUI = true;

        string tooltipText = str_SkillName;

        tooltipText += "\n\n" + str_SkillDescription;

        string abilityCost = "";
        int cost = skillPointsRequired;

        if (cost != 0)
        {
            if (cost > PlayerInventoryScript.skillpoints)
            {
                abilityCost += "<color=red>" + cost.ToString() + "</color>";
            }
            else if (cost <= PlayerInventoryScript.skillpoints)
            {
                abilityCost += "<color=green>" + cost.ToString() + "</color>";
            }
        }
        else
        {
            abilityCost += "<color=green>Fully upgraded</color>";
        }

        tooltipText += "\n\n" + "Cost: " + abilityCost;

        tooltipText += "\n" + "Tier: " + skillTier;
        TooltipScript.SetText(tooltipText);
    }
}