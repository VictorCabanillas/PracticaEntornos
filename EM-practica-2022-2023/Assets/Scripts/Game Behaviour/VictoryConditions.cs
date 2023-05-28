using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Movement.Components;


public class VictoryConditions : NetworkBehaviour
{

    public NetworkVariable<int> alivePlayersRemaining = new NetworkVariable<int>();
    int playersInGame = 0;
    public GameObject healthBar;
    [SerializeField] GameObject victoryPanel;

    public override void OnNetworkSpawn()
    {
        if (IsHost || IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += addPlayer;
            NetworkManager.Singleton.OnClientDisconnectCallback += removePlayer;

           
            alivePlayersRemaining.OnValueChanged += CheckNumberOfAlivePlayers;
        }       
    }

    private void addPlayer(ulong id)
    {
        playersInGame += 1;

        //TODO VER CUANDO ESTEN TODOS
        if(playersInGame == 3)
        {
            alivePlayersRemaining.Value = playersInGame;
            var fighterMovementOfPlayer = FindObjectsOfType<FighterMovement>();
            foreach(FighterMovement fighterMovement in fighterMovementOfPlayer)
            {
                fighterMovement.speed = 3;
                fighterMovement.jumpAmount = 1.2f;
            }
            //emepzar a moverse

        }
    }

    private void removePlayer(ulong id)
    {
        playersInGame -= 1;
    }

    public override void OnNetworkDespawn()
    {
        if (IsHost)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= addPlayer;
            NetworkManager.Singleton.OnClientDisconnectCallback -= removePlayer;

            alivePlayersRemaining.OnValueChanged -= CheckNumberOfAlivePlayers;
        }

    }

    void CheckNumberOfAlivePlayers(int oldValue, int newValue)
    {
        if(newValue == 1)
        {
            ActivateEndGameCanvasClientRpc();
        }
        else
        {

        }
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
