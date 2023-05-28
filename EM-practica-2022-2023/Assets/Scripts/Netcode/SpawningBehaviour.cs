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

    public NetworkVariable<FixedString64Bytes> playerName = new NetworkVariable<FixedString64Bytes>(default,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);

    private void Start()
    {
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += sceneLoaded;
        playerName.OnValueChanged += cambiarNombre;
    }
    void cambiarNombre(FixedString64Bytes previous, FixedString64Bytes current) 
    {
        transform.GetChild(0).GetComponent<selectorPlayerBehaviour>().parentReady();
    }

    void sceneLoaded(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut) 
    {
        if (!IsOwner) return;
        if (sceneName == "SelectorPersonaje") 
        {
            Debug.Log("Vamosa instanciar");
            InstantiateSelectorServerRpc(OwnerClientId);
        }
        if (sceneName == "JuegoPrincipal") 
        {
            InstantiateCharacterServerRpc(OwnerClientId);
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Debug.Log(OwnerClientId);
            Debug.Log("Cliente entra a network Spawn");
            playerName.Value = PlayerPrefs.GetString("playerName");
            if (NetworkManager.Singleton.IsClient)
            {
                Debug.Log("Cliente instancia");
                InstantiateSelectorServerRpc(OwnerClientId);
            }
        }
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
        Debug.Log("Instanciamos");
        //GetComponent<selectorPlayerBehaviour>().enabled = true;
        GameObject characterGameObject = Instantiate(selectorPrefab,transform);
        characterGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(id);
        characterGameObject.transform.SetParent(transform, false); //Esto ocurre despues
        characterGameObject.GetComponent<selectorPlayerBehaviour>().parentReady();
    }

}
