using System;
using System.Collections.Generic;
using scr_Interfaces;
using scr_NPCs.scr_Enemies;
using UnityEngine;

namespace scr_Environment
{
    public class RoomController : MonoBehaviour
    {
        public List<Enemy> enemies = new List<Enemy>();
        public List<Door> doors = new List<Door>();

        private void Update()
        {
            if (enemies[0] == null)
            {
                foreach (var door in doors)
                {
                    door.GetComponent<IUnlockable>().UnlockDoor();
                }
            }
        }
    }
}
