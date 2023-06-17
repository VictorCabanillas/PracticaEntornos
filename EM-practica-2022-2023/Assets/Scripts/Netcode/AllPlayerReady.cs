using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class AllPlayerReady : NetworkBehaviour
{
    int playerReady = 0;


    public void playerIsReady() 
    {
        playerReady += 1;
        
        if (playerReady == NetworkManager.Singleton.ConnectedClients.Count && NetworkManager.Singleton.ConnectedClients.Count > 1) 
        {
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
        selectorPlayerBehaviour[] playersIDS = FindObjectsOfType<selectorPlayerBehaviour>();
        foreach (selectorPlayerBehaviour p in playersIDS)
        {
            Debug.Log(p.playerId.Value);
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
        if (OwnerClientId == 0)
        {
            Button startButton = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).GetComponent<PlayerSelectorButtons>().startButton;
            startButton.gameObject.SetActive(false);
            startButton.onClick.RemoveAllListeners();
        }

    }


    [ServerRpc(RequireOwnership =false)]
    public void startMatchServerRpc() 
    {
        DontDestroyOnLoad(gameObject);
        SceneEventProgressStatus status = NetworkManager.Singleton.SceneManager.LoadScene("JuegoPrincipal",LoadSceneMode.Single);
        Destroy(gameObject);
    }

    public override void OnNetworkDespawn()
    {
        //NetworkManager.Singleton.SceneManager.LoadScene("JuegoPrincipal", LoadSceneMode.Single);
    }
}
