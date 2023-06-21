using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerData : NetworkBehaviour
{

    public NetworkVariable<string> Name = new NetworkVariable<string>();
    public GameObject characterPrefab;
    public GameObject selectorPrefab;

    

  
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Name.OnValueChanged += (string previous, string current) => { Name.Value = current; };
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "CharacterSelector")
        {
            if (!IsOwner) return;
            InstantiateSelectorServerRpc(OwnerClientId);
        }

        if (scene.name == "JuegoPrincipal")
        {
            if (!IsOwner) return;
            InstantiateCharacterServerRpc(OwnerClientId);
        }      
    }

    

    //Para la ejecución de la partida (personajes)
    [ServerRpc]
    public void InstantiateCharacterServerRpc(ulong id)
    {
        GameObject characterGameObject = Instantiate(characterPrefab);
        characterGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(id);
        characterGameObject.transform.SetParent(transform, false);
        characterGameObject.GetComponent<PlayerHealth>().Health.Value = 100;
    }

    //Para el selector de personajes
    [ServerRpc]
    public void InstantiateSelectorServerRpc(ulong id)
    {
        GameObject characterGameObject = Instantiate(selectorPrefab);
        characterGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(id);
        characterGameObject.transform.SetParent(transform, false);
        
    }

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            Object.DontDestroyOnLoad(gameObject);
            Name.Value = PlayerPrefs.GetString("NombreJugador");
            
        }
    }
}
