using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerSelectorBehaviourHandler : NetworkBehaviour
{
    NetworkVariable<int> playerCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
    public List<selectorPlayerBehaviour> listaSelectorPlayer = new List<selectorPlayerBehaviour>();

    private void Awake()
    {

        playerCount.OnValueChanged += ProcessPlayerId;
        
    }

    
    private void ProcessPlayerId(int previous, int current)
    {
        foreach (var player in listaSelectorPlayer)
        {
            if(player!= null && player.playerId.Value > current)
            {
                player.playerId.Value--;
            }
        }
    }

    private void Update()
    {
        if(IsServer || IsHost) { playerCount.Value = NetworkManager.Singleton.ConnectedClients.Count; }
        
    }

}
