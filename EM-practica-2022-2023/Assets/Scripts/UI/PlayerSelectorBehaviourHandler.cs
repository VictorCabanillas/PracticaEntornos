using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerSelectorBehaviourHandler : NetworkBehaviour
{
    NetworkVariable<int> playerCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    
    public List<SpawningBehaviour> listaSelectorPlayer = new List<SpawningBehaviour>();

   
    //para actualizar el id de los jugadores en caso de desconexión (si se va el 2 y hay 3 y 4, se actualizan solo sus ids y no el del 1)
    private void ProcessPlayerId(int playerDisconnectedID)
    {
        foreach (var player in listaSelectorPlayer)
        {
            if(player != null && player.playerId.Value>playerDisconnectedID)
            {
                
                player.playerId.Value = player.playerId.Value - 1;
            }
        }
    }

    //Contador de cuantos jugadores hay
    [ServerRpc(RequireOwnership =false)]
    public void PlayerConectServerRpc()
    {
        playerCount.Value = playerCount.Value + 1;
    }

    //Si se desconecta un jugador disminuye en 1 el numero
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


}
