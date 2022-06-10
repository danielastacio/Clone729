using scr_Player;
using TMPro;
using UnityEngine;

namespace scr_UI.scr_PauseMenu
{
    public class PauseMenuStats : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI hpText;
        private float _playerCurrentHp;
        private float _playerMaxHp;

        public void OnEnable()
        {
            CollectStats();
            DisplayStats();
        }

        private void CollectStats()
        {
            _playerCurrentHp = PlayerController.Instance.currentHp;
            _playerMaxHp = PlayerController.Instance.maxHp;
        }

        private void DisplayStats()
        {
            hpText.GetComponent<TextMeshProUGUI>().text = 
                "HP: " + _playerCurrentHp + "/" + _playerMaxHp;
        }
    }
}
