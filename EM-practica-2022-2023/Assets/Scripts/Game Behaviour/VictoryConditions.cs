using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Movement.Components;
using Unity.VisualScripting;
using System.Threading;
using TMPro;

public class VictoryConditions : NetworkBehaviour
{

    public NetworkVariable<int> alivePlayersRemaining = new NetworkVariable<int>(); //Jugadores que quedan vivos (Protegida)


    public NetworkVariable<bool> temporizadorEnMarcha = new NetworkVariable<bool>(false); //Variable para cuando se acabe el temporizador salte la pantalla de victoria correspondiente

    public int playersInGame = 0; //Variables para almacenar los jugadores que se han conectado
    

    [SerializeField] GameObject victoryPanel; //Referencia hacia el panel de victoria
    [SerializeField] GameObject timerPanel;
    [SerializeField] TextMeshProUGUI winningText;

    bool once = true;


    //Comenzamos haciendo que el server o host de la partida calcule cuantos jugadores hay en función de las conexiones o desconexiones
    public override void OnNetworkSpawn()
    {
        if (IsHost || IsServer)  //Solo el servidor (o el host) es quien lleva la cuenta de los jugadores presentes
        {
            NetworkManager.Singleton.SceneManager.OnLoadComplete += addPlayer; //Añadimos un jugador cuando se haya conectado
            NetworkManager.Singleton.OnClientDisconnectCallback += removePlayer; //En caso de desconexión del cliente, quitamos un jugador

            temporizadorEnMarcha.Value = false;
            temporizadorEnMarcha.OnValueChanged += CheckTemporizador;

            if(IsServer)
            {
                playersInGame--; //Restamos el servidor como jugador 
            }

        }
    }

    //Función en la que añadimos el número de personajes para pdoer trabajar con ello
    private void addPlayer(ulong id, string sceneName, LoadSceneMode mode)
    {
        playersInGame += 1;
        //TODO VER CUANDO ESTEN TODOS
        if (playersInGame == NetworkManager.Singleton.ConnectedClients.Count) //En cuanto todos los personajes estén (AQUI PASAMOS EL NUMERO DE JUGADORES QUE HAY EN EL JUEGO)
        {
            timerPanel.GetComponent<Timer>().enMarcha = true;
            ActivateTimePanelClientRpc(); //Activamos el temporizador

            alivePlayersRemaining.Value = playersInGame; //Asignamos el numero de jugador cogidos a la variable alive players
            alivePlayersRemaining.OnValueChanged += CheckNumberOfAlivePlayers; //Comprueba el numero de jugadores vivos (Para luego lanzar la condición de victoria)

            var fighterMovementOfPlayer = FindObjectsOfType<FighterMovement>(); //Buscamos el script de todos los personajes que se encarga de manejar el movimiento
            foreach (FighterMovement fighterMovement in fighterMovementOfPlayer) //Lo activamos ya que por defecto se encuentra desactivado para evitar que se puedan mover antes de que se hayan conectado todos los jugadores
            {
                fighterMovement.speed = 3;
                fighterMovement.jumpAmount = 1.2f;
            } 
        }
    }

    //Método para quitar personajes (a la variable)
    private void removePlayer(ulong id)
    {
        playersInGame -= 1;

        if(playersInGame == 1) //En caso de quedar solo un juagador (Para cuando se descontecten los jugadores)
        {
            PlayerHealth[] players = FindObjectsOfType<PlayerHealth>();
            foreach(PlayerHealth p in players)
            {
                if (p.OwnerClientId == id) 
                {
                    p.Health.Value = 0; //Ponemos su vida a 0 (el jugador que se marcha) para cuando se calcule quien gana, lo muestre correctamente
                }
            }
            ActivateEndGameCanvasClientRpc();
        }
    }

    //En caso de desconexión actualizamos la variables correspondiente llamando a los métodos necesarios
    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadComplete -= addPlayer;
            NetworkManager.Singleton.OnClientDisconnectCallback -= removePlayer;

            alivePlayersRemaining.OnValueChanged -= CheckNumberOfAlivePlayers;
        }

    }

    //Aquí comprobamos cuantos jugadores quedan vivo/en partida, en caso de quedar uno se activa la condición de victoria
    void CheckNumberOfAlivePlayers(int oldValue, int newValue)
    {
        Debug.Log(newValue);
        if (newValue == 1) //AQUÍ VAMOS PASANDO EL PARANMETRO DE COMPROBAR CUANTOS QUEDAN VIVO
        {
            timerPanel.GetComponent<Timer>().enMarcha = false;
            ActivateEndGameCanvasClientRpc(); //Activamos el canvas de victoria
        }
    }


    void CheckTemporizador(bool oldValue, bool newValue)
    {
        if (newValue == false) //AQUÍ VAMOS COMPROBANDO SI EL TEMPORIZADOS ESTÁ PARADO (YA HA TERMINADO)
        {

            ActivateEndGameCanvasClientRpc();
            var fighterMovementOfPlayer = FindObjectsOfType<FighterMovement>(); //Buscamos el script de todos los personajes que se encarga de manejar el movimiento
            foreach (FighterMovement fighterMovement in fighterMovementOfPlayer) //Lo activamos ya que por defecto se encuentra desactivado para evitar que se puedan mover antes de que se hayan conectado todos los jugadores
            {
                fighterMovement.speed = 0;
                fighterMovement.jumpAmount = 0f;
            }

        }
    }


    [ServerRpc]
    public void playerDisconnectedServerRpc()
    {
        alivePlayersRemaining.Value--;
    }


    [ClientRpc]
    void ActivateTimePanelClientRpc() //Para el panel del tiempo
    {
        timerPanel.GetComponent<CanvasGroup>().alpha = 1;

    }

    [ClientRpc]
    void ActivateEndGameCanvasClientRpc() //Para el canvas de victoria
    {
        SpawningBehaviour[] spawninBehaviourArray = FindObjectsOfType<SpawningBehaviour>();
        SpawningBehaviour lastPlayer = spawninBehaviourArray[0];
        foreach(var x in spawninBehaviourArray)
        {
            if (x != null)
            {
                if (x.transform.childCount > 0)
                {
                    int currentBestHealth;
                    if (lastPlayer.GetComponentInChildren<PlayerHealth>() == null) 
                    {
                        currentBestHealth = 0; 
                    }
                    else 
                    {
                        currentBestHealth = lastPlayer.GetComponentInChildren<PlayerHealth>().Health.Value;
                    }
                    
                    if (currentBestHealth < x.GetComponentInChildren<PlayerHealth>().Health.Value)
                    {
                        lastPlayer = x;
                    }
                }
            }
        }
        winningText.text = lastPlayer.playerName.Value.ToString() + " GANA!";
        victoryPanel.SetActive(true);
        timerPanel.GetComponent<CanvasGroup>().alpha = 0f; //Desactivamos la ceunta atrás ya que no nos interesa
        RematchServerRpc();
    }


    //Para volver a jugar una vez se haya terminado la partida anterior
    [ServerRpc(RequireOwnership = false)]
    public void RematchServerRpc()
    {
        DontDestroyOnLoad(gameObject);
        if (once)
        {
            SceneEventProgressStatus status = NetworkManager.Singleton.SceneManager.LoadScene("SelectorPersonaje", LoadSceneMode.Single);
        }
        Destroy(gameObject);
    }

   

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            temporizadorEnMarcha.Value = timerPanel.GetComponent<Timer>().enMarcha;
        }
    }
}