using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace scr_UI
{
    public class MenuButtonHover : MainMenuButtons, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            gameObject.GetComponent<Button>().image.color = SelectedColor;
        }
        
        
        public void OnPointerExit(PointerEventData eventData)
        {
            gameObject.GetComponent<Button>().image.color = DefaultColor;
        }
    }
}
