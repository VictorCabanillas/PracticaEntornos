using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerSelectorBehaviourHandler : NetworkBehaviour
{
    NetworkVariable<int> playerCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    
    public List<selectorPlayerBehaviour> listaSelectorPlayer = new List<selectorPlayerBehaviour>();

    private void Awake()
    {
        
    }

    private void ProcessPlayerId(int playerDisconnectedID)
    {
        foreach (var player in listaSelectorPlayer)
        {
            if(player != null && player.playerId.Value>playerDisconnectedID)
            {
                //player.playerId.Value--;
                player.playerId.Value = player.playerId.Value - 1;
            }
        }
    }

    [ServerRpc(RequireOwnership =false)]
    public void PlayerConectServerRpc()
    {
        playerCount.Value = playerCount.Value + 1;
    }

    public void PlayerDisconect(int playerDisconectedID)
    {
        if (IsServer)
        {
            playerCount.Value = playerCount.Value - 1;
            ProcessPlayerId(playerDisconectedID);
        }
    }

    public void PlayerConect()
    {
        PlayerConectServerRpc();
    }


    private void Update()
    {
        
        
    }

}
