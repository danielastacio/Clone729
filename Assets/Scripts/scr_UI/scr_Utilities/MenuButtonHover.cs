<<<<<<<< HEAD:Assets/Scripts/scr_UI/scr_Utilities/MenuButtonHover.cs
using UnityEngine;
========
>>>>>>>> origin/dev:Assets/Scripts/scr_UI/MenuButtonHover.cs
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace scr_UI.scr_Utilities
{
<<<<<<<< HEAD:Assets/Scripts/scr_UI/scr_Utilities/MenuButtonHover.cs
    public class MenuButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
========
    public class MenuButtonHover : MainMenuButtons, IPointerEnterHandler, IPointerExitHandler
>>>>>>>> origin/dev:Assets/Scripts/scr_UI/MenuButtonHover.cs
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
