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

    NetworkVariable<bool> ready = new NetworkVariable<bool>(false);

    private void Awake()
    {
        UImanager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UiManager>();
        NetworkManager.Singleton.SceneManager.OnUnload += onSceneUnload;
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            selectorInfo = UImanager.CrearBarras((int)OwnerClientId);

            parent = transform.parent.gameObject;
            string text = parent.GetComponent<SpawningBehaviour>().playerName.Value.ToString();
            selectorInfo.GetComponent<PlayerSelectorInfo>().playerName.text = text;
        }
    }

    private void onSceneUnload(ulong clientId, string sceneName, AsyncOperation asyncOperation) 
    {
        Destroy(gameObject);
    }
}
