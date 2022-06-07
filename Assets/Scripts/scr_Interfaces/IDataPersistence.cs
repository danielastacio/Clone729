using System;
using System.Collections.Generic;
using UnityEngine;

namespace scr_Interface
{ 
 interface IDataPersistence
    {
        public void LoadData(GameData data);

        public void SaveData(GameData data);
    }
}
