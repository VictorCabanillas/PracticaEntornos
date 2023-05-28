using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UI;
using TMPro;

public class SelectorPrefabBehaviour : NetworkBehaviour
{
    UiManager uiManager;

    public override void OnNetworkSpawn()
    {
        GameObject barra = uiManager.CrearBarras((int)OwnerClientId);
        var playerLobby = barra.GetComponent<PlayerLobby>();
        var playerData = GetPlayerData();
        playerLobby.SetData(playerData.Name.Value, "Eligiendo", null);
                  
    }

    PlayerData GetPlayerData()
    {
        return FindObjectOfType<PlayerData>();
    }

    void Awake()
    {
        uiManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UiManager>();
    }
}
