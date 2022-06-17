using UnityEngine;

namespace scr_UI.scr_Utilities
{
    public static class CanvasController
    {
        public static void ShowCanvas(Canvas canvas)
        {
            canvas.gameObject.SetActive(true);
        }

        public static void HideCanvas(Canvas canvas)
        {
            canvas.gameObject.SetActive(false);
        }
    }
}