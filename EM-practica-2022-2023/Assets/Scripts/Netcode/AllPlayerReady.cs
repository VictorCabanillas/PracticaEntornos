using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class AllPlayerReady : NetworkBehaviour
{
    int playerReady = 0;

    //Para activar o desactivar el botón de comenzar la partida una vez los jugadores esten listos
    public void playerIsReady() 
    {
        playerReady += 1;
        if (playerReady == NetworkManager.Singleton.ConnectedClients.Count && NetworkManager.Singleton.ConnectedClients.Count > 1) 
        {
            activateStartButtonClientRpc();
        }
    }


    //Si algún jugador no está listo porque cambia de decisión, comprobamos el numero de jugadores y actiamos el botón en función de la condición
    [ServerRpc]
    public void playerUnreadyServerRpc() 
    {
        playerReady -= 1;
        if (playerReady == NetworkManager.Singleton.ConnectedClients.Count)
        {
            activateStartButtonClientRpc();
        }
        else 
        {
            deactivateStartButtonClientRpc();
        }
    }

    //Para activar el botón de start solo al primer jugador en entrar
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

    //Funcionalidad del botón de start
    [ServerRpc(RequireOwnership =false)]
    public void startMatchServerRpc() 
    {
        DontDestroyOnLoad(gameObject);
        SceneEventProgressStatus status = NetworkManager.Singleton.SceneManager.LoadScene("JuegoPrincipal",LoadSceneMode.Single);
        Destroy(gameObject);
    }

    
}
