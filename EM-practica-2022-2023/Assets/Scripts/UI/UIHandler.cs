using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIHandler : MonoBehaviour
    {
        public GameObject debugPanel;
        public Button hostButton;
        public Button clientButton;
        public Button serverButton;
        public TextMeshProUGUI IPText;

        private void Start()
        {
            hostButton.onClick.AddListener(OnHostButtonClicked);
            clientButton.onClick.AddListener(OnClientButtonClicked);
            serverButton.onClick.AddListener(OnServerButtonClicked);
        }

        private void OnHostButtonClicked()
        {
            NetworkManager.Singleton.StartHost();
            HidePanel();
        }

        private void OnClientButtonClicked()
        {
            NetworkManager.Singleton.StartClient();
            HidePanel();
        }

        //Método para cuando clicamos el boton de server 
        private void OnServerButtonClicked()
        {
            NetworkManager.Singleton.StartServer();
            HidePanel();
        }
        //Esconder paneles del boton
        public void HidePanel()
        {
            debugPanel.SetActive(false);
        }

        //Mostrar boton
        public void DisplayPanel()
        {
            debugPanel.SetActive(true);
        }
    }
}