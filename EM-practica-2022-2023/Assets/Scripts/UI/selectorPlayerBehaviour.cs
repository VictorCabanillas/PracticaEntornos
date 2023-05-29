using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UI;

public class selectorPlayerBehaviour : NetworkBehaviour
{
    UiManager UImanager;
    public GameObject selectorInfo;
    public GameObject parent;

    NetworkVariable<bool> ready = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    NetworkVariable<int> selectedCharacter = new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);

    PlayerSelectorButtons selectorButtons;

    public GameObject[] playersPrefabs;

    private void Awake()
    {
        NetworkManager.Singleton.SceneManager.OnUnload += onSceneUnload;
        selectedCharacter.OnValueChanged += selectedCharacterChanged;
        ready.OnValueChanged += playerReady;
    }

    public void selectedCharacterChanged(int previous, int current)
    {
        selectorInfo.GetComponent<PlayerSelectorInfo>().image.sprite = selectorInfo.GetComponent<PlayerSelectorInfo>().playerSprites[current];
    }

    public void playerReady(bool previous, bool current) 
    {
        Debug.Log("Readiness changed");
        selectorInfo.GetComponent<PlayerSelectorInfo>().playerReady.text = "Ready";
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            UImanager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UiManager>();
            selectorInfo = UImanager?.CrearBarras((int)OwnerClientId);
        }
        if (!IsOwner)
        {
            if (transform.parent != null)
            {
                parentReady();
            }
        }
        if (IsOwner) 
        {
            selectorButtons = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).GetComponent<PlayerSelectorButtons>();
            selectorButtons?.huntressButton.onClick.AddListener(() => { selectedCharacter.Value = 0; transform.parent.GetComponent<SpawningBehaviour>().characterPrefab = playersPrefabs[0]; });
            selectorButtons?.akaiKazeButton.onClick.AddListener(() => { selectedCharacter.Value = 1; transform.parent.GetComponent<SpawningBehaviour>().characterPrefab = playersPrefabs[1]; });
            selectorButtons?.oniButton.onClick.AddListener(() => { selectedCharacter.Value = 2; transform.parent.GetComponent<SpawningBehaviour>().characterPrefab = playersPrefabs[2]; });
            selectorButtons?.readyButton.onClick.AddListener(()=> { Debug.Log("readyButton pressed"); ready.Value = true; });

            //NetworkManager.Singleton.GetComponent<>();
        }
    }

    public void parentReady() 
    {
        parent = transform.parent.gameObject;
        string text = parent.GetComponent<SpawningBehaviour>().playerName.Value.ToString();
        if (selectorInfo != null)
        {
            selectorInfo.GetComponent<PlayerSelectorInfo>().playerName.text = text;
        }
    }

    private void onSceneUnload(ulong clientId, string sceneName, AsyncOperation asyncOperation) 
    {
        Destroy(gameObject);
    }


    public override void OnNetworkDespawn()
    {
        //If ready was true decrease player count;
        Destroy(selectorInfo);
    }
}
