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

        playerCount.OnValueChanged += ProcessPlayerIdServerRpc;
        
    }


    [ServerRpc(RequireOwnership = false)]
    private void ProcessPlayerIdServerRpc(int previous, int current)
    {
        Debug.Log("hola");
        Debug.Log(listaSelectorPlayer.Count);
        foreach (var player in listaSelectorPlayer)
        {
            Debug.Log(current);
            Debug.Log("Entrando al foreach");
            if(player != null && player.playerId.Value > current)
            {
                Debug.Log("ENTRANDO AL IF");
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

    [ServerRpc(RequireOwnership = false)]
    public void PlayerDisconectServerRpc()
    {
        playerCount.Value = playerCount.Value - 1;
    }

    public void PlayerConect()
    {
        PlayerConectServerRpc();
    }

    public void PlayerDisconect()
    {
        PlayerDisconectServerRpc();
    }

    private void Update()
    {
        
        
    }

}
