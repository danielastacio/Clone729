using System;
using UnityEngine;
using scr_Interfaces;

namespace scr_Consumables
{
    public class Consumable : MonoBehaviour, IConsumable
    {
        public virtual float ConsumeItem()
        {
            return 0f;
        }
    }
}
