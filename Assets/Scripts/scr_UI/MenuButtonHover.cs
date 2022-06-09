using scr_Interfaces;
using scr_Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace scr_UI
{
    public class MenuButtonHover : Menu, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            gameObject.GetComponent<Button>().image.color = Colors.HighlightedMenuButtonColor;
        }
        
        
        public void OnPointerExit(PointerEventData eventData)
        {
            gameObject.GetComponent<Button>().image.color = Colors.DefaultMenuButtonColor;
        }
    }
}
