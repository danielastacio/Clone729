using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace scr_UI
{
    public class MenuButtonHover : Menu, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            gameObject.GetComponent<Button>().image.color = HighlightedColor;
        }
        
        
        public void OnPointerExit(PointerEventData eventData)
        {
            gameObject.GetComponent<Button>().image.color = DefaultColor;
        }
    }
}
