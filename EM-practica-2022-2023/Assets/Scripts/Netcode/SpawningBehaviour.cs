using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.Collections;

public class SpawningBehaviour : NetworkBehaviour
{
    public GameObject characterPrefab;
    public GameObject selectorPrefab;

    public NetworkVariable<FixedString64Bytes> playerName = new NetworkVariable<FixedString64Bytes>();

    private void Start()
    {
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += sceneLoaded;
    }

    void sceneLoaded(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut) 
    {
        if (!IsOwner) return;
        if (sceneName == "SelectorPersonaje") 
        {
            playerName.Value = PlayerPrefs.GetString("playerName");
            InstantiateSelectorServerRpc(OwnerClientId);
        }
        if (sceneName == "JuegoPrincipal") 
        {
            InstantiateCharacterServerRpc(OwnerClientId);
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner) return;
        //InstantiateSelectorServerRpc(OwnerClientId);
    }


    [ServerRpc]
    public void InstantiateCharacterServerRpc(ulong id)
    {
        GameObject characterGameObject = Instantiate(characterPrefab);
        characterGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(id);
        characterGameObject.transform.SetParent(transform, false);
        characterGameObject.GetComponent<PlayerHealth>().Health.Value = 100;
    }

    [ServerRpc]
    public void InstantiateSelectorServerRpc(ulong id)
    {

        GetComponent<selectorPlayerBehaviour>().enabled = true;
        //GameObject characterGameObject = Instantiate(selectorPrefab,transform);
        //characterGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(id);
        //characterGameObject.transform.SetParent(transform, false);
    }
}
