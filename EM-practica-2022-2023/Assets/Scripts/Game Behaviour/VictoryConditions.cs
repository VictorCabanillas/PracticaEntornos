using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Movement.Components;


public class VictoryConditions : NetworkBehaviour
{

    public NetworkVariable<int> alivePlayersRemaining = new NetworkVariable<int>(); //Jugadores que quedan vivos (Protegida)
    int playersInGame = 0; //Variables para almacenar los jugadores que se han conectado
    public GameObject healthBar; //Para activar y desacticvar las barras de vida (Referencia)
    [SerializeField] GameObject victoryPanel; //Referencia hacia el panel de victoria

    [SerializeField] GameObject timerPanel;

    public override void OnNetworkSpawn()
    {
        if (IsHost || IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += addPlayer;
            NetworkManager.Singleton.OnClientDisconnectCallback += removePlayer;

           
            alivePlayersRemaining.OnValueChanged += CheckNumberOfAlivePlayers;

            
        }       
    }

    //Función en la que añadimos el número de personajes para pdoer trabajar con ello
    private void addPlayer(ulong id)
    {
        playersInGame += 1;

        //TODO VER CUANDO ESTEN TODOS
        if(playersInGame == 1) //En cuanto todos los personajes estén
        {
            ActivateTimePanelClientRpc(); //Activamos el temporizador

            alivePlayersRemaining.Value = playersInGame; //Asignamos el numero de jugador cogidos a la variable alive players
            var fighterMovementOfPlayer = FindObjectsOfType<FighterMovement>(); //Buscamos el script de todos los personajes que se encarga de manejar el movimiento
            foreach(FighterMovement fighterMovement in fighterMovementOfPlayer) //Lo activamos ya que por defecto se encuentra desactivado para evitar que se puedan mover antes de que se hayan conectado todos los jugadores
            {
                fighterMovement.speed = 3;
                fighterMovement.jumpAmount = 1.2f;
            }
            //emepzar a moverse

        }
    }

    //Método para quitar personajes (a la variable)
    private void removePlayer(ulong id)
    {
        playersInGame -= 1;
    }

    //En caso de desconexión actualizamos la variables correspondiente llamando a los métodos necesarios
    public override void OnNetworkDespawn()
    {
        if (IsHost)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= addPlayer;
            NetworkManager.Singleton.OnClientDisconnectCallback -= removePlayer;

            alivePlayersRemaining.OnValueChanged -= CheckNumberOfAlivePlayers;
        }

    }

    //Aquí comprobamos cuantos jugadores quedan vivo/en partida, en caso de quedar uno se activa la condición de victoria
    void CheckNumberOfAlivePlayers(int oldValue, int newValue)
    {
        if(newValue == 0)
        {
            ActivateEndGameCanvasClientRpc();
        }
        else
        {

        }
    }

    [ClientRpc]
    void ActivateTimePanelClientRpc()
    {
        timerPanel.SetActive(true);
    }

    [ClientRpc]
    void ActivateEndGameCanvasClientRpc()
    {
        victoryPanel.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
