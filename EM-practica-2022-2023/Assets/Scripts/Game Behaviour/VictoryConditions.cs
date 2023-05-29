using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Movement.Components;
using Unity.VisualScripting;
using System.Threading;

public class VictoryConditions : NetworkBehaviour
{

    public NetworkVariable<int> alivePlayersRemaining = new NetworkVariable<int>(); //Jugadores que quedan vivos (Protegida)
    

    public NetworkVariable<bool> temporizadorEnMarcha = new NetworkVariable<bool>(false); //Variable para cuando se acabe el temporizador salte la pantalla de victoria correspondiente

    int playersInGame = 0; //Variables para almacenar los jugadores que se han conectado
    //public GameObject healthBar; //Para activar y desacticvar las barras de vida (Referencia)

    [SerializeField] GameObject victoryPanel; //Referencia hacia el panel de victoria
    [SerializeField] GameObject timerPanel;

   

    

    public override void OnNetworkSpawn()
    {
        if (IsHost || IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += addPlayer;
            NetworkManager.Singleton.OnClientDisconnectCallback += removePlayer;

            temporizadorEnMarcha.Value = false;

            alivePlayersRemaining.OnValueChanged += CheckNumberOfAlivePlayers; 
            temporizadorEnMarcha.OnValueChanged += CheckTemporizador;

        }       
    }

    //Función en la que añadimos el número de personajes para pdoer trabajar con ello
    private void addPlayer(ulong id)
    {
        playersInGame += 1;

        //TODO VER CUANDO ESTEN TODOS
        if(playersInGame == 2) //En cuanto todos los personajes estén AQUI PASAMOS EL NUMERO DE JUGADORES QUE HAY EN EL JUEGO
        {
            Debug.Log("Suficientes para comenzar");
            timerPanel.GetComponent<Timer>().enMarcha = true;
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
        if (IsHost )
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= addPlayer;
            NetworkManager.Singleton.OnClientDisconnectCallback -= removePlayer;

            alivePlayersRemaining.OnValueChanged -= CheckNumberOfAlivePlayers;
        }

    }

    //Aquí comprobamos cuantos jugadores quedan vivo/en partida, en caso de quedar uno se activa la condición de victoria
    void CheckNumberOfAlivePlayers(int oldValue, int newValue)
    {
        if(newValue == 1) //AQUÍ VAMOS PASANDO EL PARANMETRO DE COMPROBAR CUANTOS QUEDAN VIVO
        {
            timerPanel.GetComponent<Timer>().enMarcha = false;
            ActivateEndGameCanvasClientRpc();
        }
    }


    void CheckTemporizador(bool oldValue, bool newValue)
    {
        Debug.Log("VIEJO: " + oldValue);
        Debug.Log("Nuevo: " + newValue);
        if (newValue == false) //AQUÍ VAMOS PASANDO EL PARANMETRO DE COMPROBAR CUANTOS QUEDAN VIVO
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
        timerPanel.GetComponent<CanvasGroup>().alpha = 0f; //Desactivamos la ceunta atrás ya que no nos interesa
       
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
}
