using scr_DataPersistence;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace scr_UI.Scr_MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Menu Navigation")]
        [SerializeField] private SaveSlotsMenu saveSlotsMenu;

        [Header("Menu Buttons")]
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button continueGameButton;
        [SerializeField] private Button loadGameButton;

        private void Start() 
        {
            if (!DataPersistenceManager.Instance.HasGameData()) 
            {
                continueGameButton.interactable = false;
                loadGameButton.interactable = false;
            }
        }

        public void OnNewGameClicked() 
        {
            SceneManager.LoadSceneAsync(1);

            /*if (DataPersistenceManager.instance.HasGameData())
            {
                saveSlotsMenu.ActivateMenu(false);
                this.DeactivateMenu();
            }

            else
            {
                DataPersistenceManager.instance.ChangeSelectedProfileId("A");
                DataPersistenceManager.instance.NewGame();
                SceneManager.LoadSceneAsync(1);
            }*/
        }

        public void OnLoadGameClicked() 
        {
            saveSlotsMenu.ActivateMenu(true);
            this.DeactivateMenu();
        }

        public void OnContinueGameClicked() 
        {
            DisableMenuButtons();
            // load the next scene - which will in turn load the game because of 
            // OnSceneLoaded() in the DataPersistenceManager
            SceneManager.LoadSceneAsync(1);
        }

        private void DisableMenuButtons() 
        {
            newGameButton.interactable = false;
            continueGameButton.interactable = false;
        }

        public void ActivateMenu() 
        {
            this.gameObject.SetActive(true);
        }

        public void DeactivateMenu() 
        {
            this.gameObject.SetActive(false);
        }
    }
}
