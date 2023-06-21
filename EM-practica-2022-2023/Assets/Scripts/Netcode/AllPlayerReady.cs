using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class AllPlayerReady : NetworkBehaviour
{
    int playerReady = 0;

    //Para activar o desactivar el bot�n de comenzar la partida una vez los jugadores esten listos
    public void playerIsReady() 
    {
        playerReady += 1;
        if (playerReady == NetworkManager.Singleton.ConnectedClients.Count && NetworkManager.Singleton.ConnectedClients.Count > 1) 
        {
            activateStartButtonClientRpc();
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += playerUnready;
        }
    }

    //Si alg�n jugador no est� listo porque cambia de decisi�n, comprobamos el numero de jugadores y actiamos el bot�n en funci�n de la condici�n
    //[ServerRpc (RequireOwnership =false)]
    public void playerUnready(ulong id) 
    {
        selectorPlayerBehaviour[] players = FindObjectsOfType<selectorPlayerBehaviour>();
        foreach (selectorPlayerBehaviour p in players)
        {
            if (p.OwnerClientId == id)
            {
                if (p.ready.Value) 
                {
                    Debug.Log("Alguien ya no esta ready");
                    playerReady -= 1;
                }
            }
        }
        
        if (playerReady == NetworkManager.Singleton.ConnectedClients.Count)
        {
            activateStartButtonClientRpc();
        }
        else 
        {
            deactivateStartButtonClientRpc();
        }
    }

    //Para activar el bot�n de start solo al primer jugador en entrar
    [ClientRpc]
    public void activateStartButtonClientRpc() 
    {
        SpawningBehaviour[] playersIDS = FindObjectsOfType<SpawningBehaviour>();
        foreach (SpawningBehaviour p in playersIDS)
        {
            if (p.IsOwner && p.playerId.Value == 1)
            {
                Button startButton = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).GetComponent<PlayerSelectorButtons>().startButton;
                startButton.gameObject.SetActive(true);
                startButton.onClick.AddListener(startMatchServerRpc);
            }
        }
        
    
    }


    [ClientRpc]
    public void deactivateStartButtonClientRpc()
    {
        SpawningBehaviour[] playersIDS = FindObjectsOfType<SpawningBehaviour>();
        foreach (SpawningBehaviour p in playersIDS)
        {
            if (p.IsOwner && p.playerId.Value == 1)
            {
                Button startButton = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).GetComponent<PlayerSelectorButtons>().startButton;
                startButton.gameObject.SetActive(false);
                startButton.onClick.RemoveAllListeners();
            }
        }
    }

    //Funcionalidad del bot�n de start
    [ServerRpc(RequireOwnership =false)]
    public void startMatchServerRpc() 
    {
        DontDestroyOnLoad(gameObject);
        SceneEventProgressStatus status = NetworkManager.Singleton.SceneManager.LoadScene("JuegoPrincipal",LoadSceneMode.Single);
        Destroy(gameObject);
    }


    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= playerUnready;
        }
    }

    private void OnDestroy()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback -= playerUnready;
    }
}
