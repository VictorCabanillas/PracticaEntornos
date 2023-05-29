using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class AllPlayerReady : NetworkBehaviour
{
    int playerReady = 0;


    [ServerRpc]
    public void playerIsReadyServerRpc() 
    {
        playerReady += 1;
        Debug.Log("Ready players: " + playerReady);
        Debug.Log(NetworkManager.Singleton.ConnectedClients.Count);
        if (playerReady == NetworkManager.Singleton.ConnectedClients.Count) 
        {
            Debug.Log("Activamos boton start");
            activateStartButtonClientRpc();
        }
    }

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

    [ClientRpc]
    public void activateStartButtonClientRpc() 
    {
        if (NetworkManager.Singleton.LocalClientId == 0) 
        {
            Button startButton = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).GetComponent<PlayerSelectorButtons>().startButton;
            startButton.gameObject.SetActive(true);
            startButton.onClick.AddListener(startMatchServerRpc);
        }
    
    }


    [ClientRpc]
    public void deactivateStartButtonClientRpc()
    {
        if (OwnerClientId == 0)
        {
            Button startButton = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).GetComponent<PlayerSelectorButtons>().startButton;
            startButton.gameObject.SetActive(false);
            startButton.onClick.RemoveAllListeners();
        }

    }


    [ServerRpc]
    public void startMatchServerRpc() 
    {
        DontDestroyOnLoad(gameObject);
        SceneEventProgressStatus status = NetworkManager.Singleton.SceneManager.LoadScene("JuegoPrincipal",LoadSceneMode.Single);
        Destroy(gameObject);
    }

    public override void OnNetworkDespawn()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("JuegoPrincipal", LoadSceneMode.Single);
    }
}
