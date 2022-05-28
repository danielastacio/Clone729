using System;
using UnityEngine;

namespace scr_Consumables
{
    public class HealthConsumable : Consumable
    {
        [SerializeField] private float recoveryAmount;

        private void Start()
        {
            GetComponentInChildren<ParticleSystem>().Play();
        }

        public override float ConsumeItem()
        {
            return recoveryAmount;
        }
    }
}
