using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace scr_NPCs.scr_Enemies.scr_EnemyUtilities
{
    public class EnemyDropTable : MonoBehaviour
    {
        [System.Serializable] public class Items
        {
            public GameObject Item;
            public int DropChance;
        }

        public List<Items> DropTable = new List<Items>();

        public void CalculateDropChance(int dropChance)
        {
            int calcDropChance = Random.Range(0, 100);

            if (calcDropChance > dropChance)
            {
                return;
            }
            else
            {
                DropItem();
            }
        }

        public void DropItem()
        {
            int totalDropChance = 0;

            for (int i = 0; i < DropTable.Count; i++)
            {
                totalDropChance += DropTable[i].DropChance;
            }

            int randValue = Random.Range(0, totalDropChance);

            for (int i = 0; i < DropTable.Count; i++)
            {
                if (randValue < DropTable[i].DropChance)
                {
                    Instantiate(DropTable[i].Item, transform.position, quaternion.identity);
                    return;
                }
                else
                {
                    totalDropChance -= DropTable[i].DropChance;
                }
            }
        }
    }
}
