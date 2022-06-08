using System;
using scr_Player;
using TMPro;
using UnityEngine;

namespace scr_UI
{
    public class HPText : MonoBehaviour
    {
        private TextMeshProUGUI _hpText;
        private float _playerCurrentHp;
        private float _playerMaxHp;

        private void Awake()
        {
            _hpText = GetComponent<TextMeshProUGUI>();
        }

        public void OnEnable()
        {
            _playerCurrentHp = PlayerController.Instance.currentHp;
            _playerMaxHp = PlayerController.Instance.maxHp;
            _hpText.text = "HP: " + _playerCurrentHp + "/" + _playerMaxHp;
        }
    }
}
