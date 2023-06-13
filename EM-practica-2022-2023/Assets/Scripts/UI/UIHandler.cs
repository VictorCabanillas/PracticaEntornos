using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIHandler : NetworkBehaviour
    {
        public GameObject Buttons;
        public GameObject nameSelector;
        public Button hostButton;
        public Button clientButton;
        public Button serverButton;
        public Button backButton;
        public Button confirmButton;
        public TextMeshProUGUI IP;
        public TextMeshProUGUI Port;
        public TextMeshProUGUI Name;

        bool clientStart=true;

        string IpSelected;
        string PortSelected;


        private void Start()
        {
            hostButton.onClick.AddListener(OnHostButtonClicked);
            clientButton.onClick.AddListener(OnClientButtonClicked);
            serverButton.onClick.AddListener(OnServerButtonClicked);

            hostButton.onClick.AddListener(storeAdress);
            clientButton.onClick.AddListener(storeAdress);
            serverButton.onClick.AddListener(storeAdress);

            backButton.onClick.AddListener(OnBackButtonClicked);
            confirmButton.onClick.AddListener(OnConfirmButtonClicked);

            NetworkManager.Singleton.OnServerStarted+=loadScene;
        }

        private void storeAdress() 
        {
            IpSelected = IP.text;
            PortSelected = Port.text;
            PortSelected = PortSelected.Remove(PortSelected.Length - 1, 1);
            IpSelected = IpSelected.Remove(IpSelected.Length - 1, 1);

            Debug.Log(IpSelected.Length + " " +PortSelected.Length);
            if(!string.IsNullOrWhiteSpace(IpSelected)) NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = IpSelected;
            if(!string.IsNullOrWhiteSpace(PortSelected)) NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port = ushort.Parse(PortSelected);
            NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.ServerListenAddress = "0.0.0.0";

           
            
        }

        private void OnHostButtonClicked()
        {
            clientStart = false;
            Buttons.SetActive(false);
            nameSelector.SetActive(true);
        }

        private void OnClientButtonClicked()
        {
            clientStart = true;
            Buttons.SetActive(false);
            nameSelector.SetActive(true);
        }
        private void OnServerButtonClicked()
        {
            PlayerPrefs.SetInt("playingServer", 1);
            NetworkManager.Singleton.StartServer();
        }

        private void OnBackButtonClicked()
        {
            Buttons.SetActive(true);
            nameSelector.SetActive(false);
        }
        private void OnConfirmButtonClicked()
        {
            string name = Name.text;
            PlayerPrefs.SetString("playerName",name);

            if (clientStart)
            {
                NetworkManager.Singleton.StartClient();
            }
            else
            {
                PlayerPrefs.SetInt("playingServer", 0);
                NetworkManager.Singleton.StartHost();
            }
        }

        private void loadScene() 
        {
            if (NetworkManager.Singleton.IsServer || IsHost)
            {
                NetworkManager.Singleton.SceneManager.LoadScene("SelectorPersonaje", LoadSceneMode.Single);
            }
            
        }
        private void OnDisable()
        {
            if(NetworkManager.Singleton != null) NetworkManager.Singleton.OnServerStarted -= loadScene;

        }
    }
}