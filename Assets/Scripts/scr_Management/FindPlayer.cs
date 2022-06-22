using Cinemachine;
using scr_Player;
using UnityEngine;

namespace scr_Management
{
    public class FindPlayer : MonoBehaviour
    {
        private PlayerController _playerController;

        private void Start()
        {
            _playerController = PlayerController.Instance;
            GetComponent<CinemachineVirtualCamera>().Follow = _playerController.transform;
        }
    }
}
