using System;
using System.Collections.Generic;
using scr_Management.Management_Events;
using scr_NPCs.scr_Enemies;
using UnityEngine;

namespace scr_Environment
{
    public class RoomController : MonoBehaviour
    {
        public List<Enemy> enemies = new();
        public bool roomEntered;
        public bool roomCleared;
        [SerializeField] private List<string> interactableIds = new();
        private BoxCollider2D _bc2D;

        private void Awake()
        {
            _bc2D = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            CheckRemainingEnemies();
        }

        private void CheckRemainingEnemies()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null)
                {
                    enemies.Remove(enemies[i]);
                }
            }

            if (enemies.Count == 0 && !roomCleared)
            {
                foreach (var interactableID in interactableIds)
                {
                    Actions.OnDoorTriggered(interactableID);
                }
                roomCleared = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player") && !roomEntered)
            {
                Debug.Log("Player has entered room");
                Actions.OnDoorTriggered(interactableIds[0]);
                roomEntered = true;
                _bc2D.enabled = false;
            }
        }
    }
}
