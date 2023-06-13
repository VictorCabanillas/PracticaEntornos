using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.Collections;

public class SpawningBehaviour : NetworkBehaviour
{
    public int characterPrefab;
    public GameObject[] characters;
    public GameObject selectorPrefab;

    public NetworkVariable<FixedString64Bytes> playerName = new NetworkVariable<FixedString64Bytes>(default,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);

    public bool playingServer = false;
    private void Start()
    {
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += sceneLoaded;
        playerName.OnValueChanged += cambiarNombre;
    }
    void cambiarNombre(FixedString64Bytes previous, FixedString64Bytes current) 
    {
        transform.GetChild(0).GetComponent<selectorPlayerBehaviour>()?.parentReady();
    }

    void sceneLoaded(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut) 
    {
        if (!IsOwner) return;
        if (sceneName == "SelectorPersonaje") 
        {
            InstantiateSelectorServerRpc(OwnerClientId);
        }
        if (sceneName == "JuegoPrincipal") 
        {
            InstantiateCharacterServerRpc(OwnerClientId,characterPrefab);
        }
    }

    public override void OnNetworkSpawn()
    {
        playingServer = PlayerPrefs.GetInt("playingServer") == 1 ? true : false;
        if (IsOwner)
        {
            playerName.Value = PlayerPrefs.GetString("playerName");
            if (NetworkManager.Singleton.IsClient && NetworkManager.Singleton.LocalClientId!=0)
            {
                InstantiateSelectorServerRpc(OwnerClientId);
            }
        }
        else
        {
            if (transform.childCount > 0)
            {
                transform.GetChild(0).GetComponent<selectorPlayerBehaviour>()?.parentReady();
            }
        }
    }


    [ServerRpc]
    public void InstantiateCharacterServerRpc(ulong id, int characterNum)
    {
        GameObject characterToSpawn = characters[characterNum];
        GameObject characterGameObject = Instantiate(characterToSpawn);
        characterGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(id);
        characterGameObject.transform.SetParent(transform, false);
        characterGameObject.GetComponent<PlayerHealth>().Health.Value = 100;
        CreateBarClientRpc();
        if (IsServer && !IsHost) { characterGameObject.GetComponent<SpriteRenderer>().enabled = false; }
    }

    [ServerRpc]
    public void InstantiateSelectorServerRpc(ulong id)
    {
        GameObject characterGameObject = Instantiate(selectorPrefab,transform);
        characterGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(id);
        characterGameObject.transform.SetParent(transform, false); //Esto ocurre despues
        UpdateNameClientRpc();
    }

    [ClientRpc]
    public void CreateBarClientRpc()
    {
        transform.GetChild(0).GetComponent<AsociarBarras>()?.crearBarras();
        transform.GetChild(1).GetComponent<AsociarBarras>()?.crearBarras();
    }

    [ClientRpc]
    public void UpdateNameClientRpc() 
    {
        transform.GetChild(0).GetComponent<selectorPlayerBehaviour>()?.parentReady();
    }
}
