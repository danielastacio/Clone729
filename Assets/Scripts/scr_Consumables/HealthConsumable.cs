using UnityEngine;

namespace scr_Consumables
{
    public class HealthConsumable : Consumable
    {
        [SerializeField] private float recoveryAmount;

        public override float ConsumeItem()
        {
            return recoveryAmount;
        }
    }
}
