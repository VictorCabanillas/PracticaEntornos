using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIHandler : NetworkBehaviour
    {
        public GameObject debugPanel;
        public Button hostButton;
        public Button clientButton;
        public Button serverButton;
        public Button confirmNameButton;
        private bool hostOrClient = false; //Booleano para determinar que acción se va a ejecutar tras la elección del nombre en función de si hemos elegido host o client
        public TextMeshProUGUI namePlayer;
        public TextMeshProUGUI IP;
        public TextMeshProUGUI Port;

        UnityTransport unityTransport; // Cambiar ip y puerto en función del introducido

        [SerializeField] GameObject nameSelectorPanel;

        private void Start()
        {
            hostButton.onClick.AddListener(OnHostButtonClicked);
            clientButton.onClick.AddListener(OnClientButtonClicked);
            serverButton.onClick.AddListener(OnServerButtonClicked);
            confirmNameButton.onClick.AddListener(ConfirmNameButtonClicked);

            unityTransport = NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();
        }

        //Host: Se ocultan los botones y se muestra el selector de nombre, además, se pone a true el booleano que en el metodo ConfirmNameButton() nos va a hacer el Start Host
        private void OnHostButtonClicked()
        {
            string ip = IP.text;
            string port = Port.text;

            port = port.Remove(port.Length - 1, 1); // Para quitar el extra character (hueco vacio adicional) que crea el string y poder parsearlo bien

            unityTransport.ConnectionData.Address = ip;
            unityTransport.ConnectionData.Port = ushort.Parse(port);

            hostOrClient = true;
            Hide();
            ShowNameSelector();
        }
        //Client: Se ocultan los botones y se muestra el selector de nombre, además, se pone a false el booleano que en el metodo ConfirmNameButton() nos va a hacer el Start Client
        private void OnClientButtonClicked()
        {
            string ip = IP.text;
            string port = Port.text;

            port = port.Remove(port.Length - 1, 1); // Para quitar el extra character (hueco vacio adicional) que crea el string y poder parsearlo bien

            unityTransport.ConnectionData.Address = ip;
            unityTransport.ConnectionData.Port = ushort.Parse(port);

            hostOrClient = true;
            Hide();
            ShowNameSelector();
        }

        private void OnServerButtonClicked()
        {
            string ip = IP.text;
            string port = Port.text;


            port  = port.Remove(port.Length-1, 1); // Para quitar el extra character (hueco vacio adicional) que crea el string y poder parsearlo bien

            unityTransport.ConnectionData.Address = ip;
            unityTransport.ConnectionData.Port = ushort.Parse(port);


            NetworkManager.Singleton.StartServer();
            Hide();
        }

        //Para esconder el botón
        public void Hide()
        {
            debugPanel.SetActive(false);
        }

        //Para mostrar el botón
        public void ShowNameSelector()
        {
            nameSelectorPanel.SetActive(true);
        }

        //Relacionado con el botón de confirmación a la hora de escribir el nombre en la casilla correspondiente
        public void ConfirmNameButtonClicked()
        {
            string nombre = namePlayer.text;

            PlayerPrefs.SetString("NombreJugador", nombre); //En las player preferences va a haber un apartado NombreJugador que se va a pasar entre escenas   

            if (hostOrClient)
            {
                
                NetworkManager.Singleton.StartHost(); 
            }
            else
            {
                
                NetworkManager.Singleton.StartClient();
            }

            NetworkManager.SceneManager.LoadScene("CharacterSelector",LoadSceneMode.Single);
            
            //SceneManager.LoadScene("CharacterSelector");

        }

    }
}