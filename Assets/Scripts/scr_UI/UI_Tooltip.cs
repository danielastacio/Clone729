using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Tooltip : MonoBehaviour
{
    [Header("Assignables")]
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private RectTransform backgroundRect;
    [SerializeField] private RectTransform canvasRect;

    //public but hidden variables
    [HideInInspector] public bool showTooltipUI;

    //private variables
    private bool calledUICloseOnce;
    private Color32 transparent = new(0, 0, 0, 0);
    private Color32 black = new(0, 0, 0, 200);
    private RectTransform parRect;

    private void Awake()
    {
        parRect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (showTooltipUI)
        {
            if (calledUICloseOnce)
            {
                calledUICloseOnce = false;
            }

            if (backgroundRect.GetComponent<RawImage>().color == transparent)
            {
                backgroundRect.GetComponent<RawImage>().color = black;
            }

            //get the cursor position
            Vector2 anchoredPosition = (Input.mousePosition + new Vector3(15, 0, 0)) / canvasRect.localScale.x;

            //check if tooltip fits on screen
            //if cursor is close to edge or out of screen
            //pushes tooltip to left side
            if (anchoredPosition.x + backgroundRect.rect.width > canvasRect.rect.width)
            {
                anchoredPosition.x = canvasRect.rect.width - backgroundRect.rect.width;
            }
            //pushes tooltip down
            if (anchoredPosition.y + backgroundRect.rect.height > canvasRect.rect.height)
            {
                anchoredPosition.y = canvasRect.rect.height - backgroundRect.rect.height;
            }
            //pushes tooltip to right side
            if (anchoredPosition.x + backgroundRect.rect.width < 380)
            {
                anchoredPosition.x = 0;
            }
            //pushes tooltip up
            if (anchoredPosition.y + backgroundRect.rect.height < 50)
            {
                anchoredPosition.y = 0;
            }

            //moves the tooltip parent to the position of the cursor
            parRect.anchoredPosition = anchoredPosition;
        }
        else if (!showTooltipUI
                 && !calledUICloseOnce)
        {
            SetText("");
            backgroundRect.GetComponent<RawImage>().color = transparent;

            calledUICloseOnce = true;
        }
    }

    public void SetText(string text)
    {
        //set the tmpro text
        textComponent.SetText(text);
        //force the text mesh to scale properly
        textComponent.ForceMeshUpdate();

        //get text scale
        Vector2 textSize = textComponent.GetRenderedValues(false);
        //extra space at the top and right
        Vector2 padding = new(8, 8);

        //update the background scale
        backgroundRect.sizeDelta = textSize + padding;
    }
}