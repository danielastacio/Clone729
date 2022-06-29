using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace scr_UI.scr_Utilities
{
    public class MenuButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void SetHighlighted()
        {
            gameObject.GetComponent<Button>().image.color = Colors.HighlightedMenuButtonColor;
        }

        public void SetDefault()
        {
            gameObject.GetComponent<Button>().image.color = Colors.DefaultMenuButtonColor;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetHighlighted();
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            SetDefault();
        }
    }
}
