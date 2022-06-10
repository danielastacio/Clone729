using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace scr_Utilities
{
    public class MenuButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
