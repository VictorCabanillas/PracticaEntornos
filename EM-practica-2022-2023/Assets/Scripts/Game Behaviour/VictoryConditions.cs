using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Movement.Components;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Threading;

public class VictoryConditions : NetworkBehaviour
{

    public NetworkVariable<int> alivePlayersRemaining = new NetworkVariable<int>(); //Jugadores que quedan vivos (Protegida)
<<<<<<< Updated upstream
    
    public NetworkVariable<bool> temporizadorEnMarcha = new NetworkVariable<bool>(true); //Variable para cuando se acabe el temporizador salte la pantalla de victoria correspondiente
=======


    public NetworkVariable<bool> temporizadorEnMarcha = new NetworkVariable<bool>(false); //Variable para cuando se acabe el temporizador salte la pantalla de victoria correspondiente
>>>>>>> Stashed changes

    int playersInGame = 0; //Variables para almacenar los jugadores que se han conectado
    public GameObject healthBar; //Para activar y desacticvar las barras de vida (Referencia)

    [SerializeField] GameObject victoryPanel; //Referencia hacia el panel de victoria
    [SerializeField] GameObject timerPanel;
    [SerializeField] int roundsNumber;

<<<<<<< Updated upstream
    public List<GameObject> playerObjectsList = new List<GameObject>();
=======
>>>>>>> Stashed changes




    public override void OnNetworkSpawn()
    {
        if (IsHost || IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadComplete += addPlayer;
            NetworkManager.Singleton.OnClientDisconnectCallback += removePlayer;

            temporizadorEnMarcha.Value = false;

            alivePlayersRemaining.OnValueChanged += CheckNumberOfAlivePlayers;
            temporizadorEnMarcha.OnValueChanged += CheckTemporizador;

        }
    }

<<<<<<< Updated upstream
    //Funci�n en la que a�adimos el n�mero de personajes para pdoer trabajar con ello
    private void addPlayer(ulong id)
    {
        playersInGame += 1;
        //TODO VER CUANDO ESTEN TODOS
        if(playersInGame == 1) //En cuanto todos los personajes est�n AQUI PASAMOS EL NUMERO DE JUGADORES QUE HAY EN EL JUEGO
=======
    //Función en la que añadimos el número de personajes para pdoer trabajar con ello
    private void addPlayer(ulong id, string sceneName, LoadSceneMode mode)
    {
        playersInGame += 1;
        Debug.Log("Add player se ha ejecutado" + playersInGame);
        //TODO VER CUANDO ESTEN TODOS
        if (playersInGame == NetworkManager.Singleton.ConnectedClients.Count) //En cuanto todos los personajes estén AQUI PASAMOS EL NUMERO DE JUGADORES QUE HAY EN EL JUEGO
>>>>>>> Stashed changes
        {
            Debug.Log("Suficientes para comenzar");
            timerPanel.GetComponent<Timer>().enMarcha = true;
            ActivateTimePanelClientRpc(); //Activamos el temporizador

            alivePlayersRemaining.Value = playersInGame; //Asignamos el numero de jugador cogidos a la variable alive players
            var fighterMovementOfPlayer = FindObjectsOfType<FighterMovement>(); //Buscamos el script de todos los personajes que se encarga de manejar el movimiento
            foreach (FighterMovement fighterMovement in fighterMovementOfPlayer) //Lo activamos ya que por defecto se encuentra desactivado para evitar que se puedan mover antes de que se hayan conectado todos los jugadores
            {
                fighterMovement.speed = 3;
                fighterMovement.jumpAmount = 1.2f;
            }

            //emepzar a moverse

        }
    }

<<<<<<< Updated upstream
    //M�todo para quitar personajes (a la variable)
=======
    //Método para quitar personajes (a la variable)
>>>>>>> Stashed changes
    private void removePlayer(ulong id)
    {
        playersInGame -= 1;
    }

<<<<<<< Updated upstream
    //En caso de desconexi�n actualizamos la variables correspondiente llamando a los m�todos necesarios
=======
    //En caso de desconexión actualizamos la variables correspondiente llamando a los métodos necesarios
>>>>>>> Stashed changes
    public override void OnNetworkDespawn()
    {
        if (IsHost)
        {
            NetworkManager.Singleton.SceneManager.OnLoadComplete -= addPlayer;
            NetworkManager.Singleton.OnClientDisconnectCallback -= removePlayer;

            alivePlayersRemaining.OnValueChanged -= CheckNumberOfAlivePlayers;
        }

    }

<<<<<<< Updated upstream
    //Aqu� comprobamos cuantos jugadores quedan vivo/en partida, en caso de quedar uno se activa la condici�n de victoria
    void CheckNumberOfAlivePlayers(int oldValue, int newValue)
    {
        if(newValue == 0) //AQU� VAMOS PASANDO EL PARANMETRO DE COMPROBAR CUANTOS QUEDAN VIVO
=======
    //Aquí comprobamos cuantos jugadores quedan vivo/en partida, en caso de quedar uno se activa la condición de victoria
    void CheckNumberOfAlivePlayers(int oldValue, int newValue)
    {
        if (newValue == 1) //AQUÍ VAMOS PASANDO EL PARANMETRO DE COMPROBAR CUANTOS QUEDAN VIVO
>>>>>>> Stashed changes
        {
            timerPanel.GetComponent<Timer>().enMarcha = false;
            ActivateEndGameCanvasClientRpc();
        }
    }


    void CheckTemporizador(bool oldValue, bool newValue)
    {
        Debug.Log("VIEJO: " + oldValue);
        Debug.Log("Nuevo: " + newValue);
<<<<<<< Updated upstream
        if (newValue == false) //AQU� VAMOS PASANDO EL PARANMETRO DE COMPROBAR CUANTOS QUEDAN VIVO
=======
        if (newValue == false) //AQUÍ VAMOS PASANDO EL PARANMETRO DE COMPROBAR CUANTOS QUEDAN VIVO
>>>>>>> Stashed changes
        {

            ActivateEndGameCanvasClientRpc();
            var fighterMovementOfPlayer = FindObjectsOfType<FighterMovement>(); //Buscamos el script de todos los personajes que se encarga de manejar el movimiento
            foreach (FighterMovement fighterMovement in fighterMovementOfPlayer) //Lo activamos ya que por defecto se encuentra desactivado para evitar que se puedan mover antes de que se hayan conectado todos los jugadores
            {
                Debug.Log("Parando jugadores");
                fighterMovement.speed = 0;
                fighterMovement.jumpAmount = 0f;
            }

        }
    }


    [ClientRpc]
    void ActivateTimePanelClientRpc()
    {
        timerPanel.GetComponent<CanvasGroup>().alpha = 1;

    }

    [ClientRpc]
    void ActivateEndGameCanvasClientRpc()
    {
        victoryPanel.SetActive(true);
<<<<<<< Updated upstream
        timerPanel.SetActive(false); //Desactivamos la ceunta atr�s ya que no nos interesa

        var fighterMovementOfPlayer = FindObjectsOfType<FighterMovement>(); //Buscamos el script de todos los personajes que se encarga de manejar el movimiento
        foreach (FighterMovement fighterMovement in fighterMovementOfPlayer) //Lo activamos ya que por defecto se encuentra desactivado para evitar que se puedan mover antes de que se hayan conectado todos los jugadores
        {
            fighterMovement.speed = 0;
            fighterMovement.jumpAmount = 0f;
        }
        StartCoroutine(ReloadScene());
=======
        timerPanel.GetComponent<CanvasGroup>().alpha = 0f; //Desactivamos la ceunta atrás ya que no nos interesa

>>>>>>> Stashed changes
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        temporizadorEnMarcha.Value = timerPanel.GetComponent<Timer>().enMarcha;


    }
<<<<<<< Updated upstream

    public void AddPlayerObject(GameObject player){
        Debug.Log("Voy a añadir al player: " + player);
        playerObjectsList.Add(player);
    }

    public void RemovePlayerObject(GameObject player){
        
    }

    IEnumerator ReloadScene(){
    yield return new WaitForSeconds(4);
    foreach (GameObject player in playerObjectsList) //Lo activamos ya que por defecto se encuentra desactivado para evitar que se puedan mover antes de que se hayan conectado todos los jugadores
    {
        //AQUI HAY QUE ELIMINAR AL PLAYER, DA ERROR//Destroy(player.transform.GetChild(0).gameObject);
    }
    if(roundsNumber==0){
        //SceneManager.LoadScene('SelectorPersonaje');
    }
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
=======
}
>>>>>>> Stashed changes
