namespace scr_Interfaces
{ 
 public interface IDataPersistence
    {
        public void LoadData(GameData data);

        public void SaveData(GameData data);
    }
}
