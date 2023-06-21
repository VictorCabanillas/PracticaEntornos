using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UI;
using UnityEngine.SceneManagement;
using System.Threading;

public class selectorPlayerBehaviour : NetworkBehaviour
{
    UiManager UImanager;
    public GameObject selectorInfo;
    public GameObject parent;

    NetworkVariable<bool> ready = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    NetworkVariable<int> selectedCharacter = new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);

    PlayerSelectorButtons selectorButtons;

    public GameObject[] playersPrefabs;

    bool once = true;
    bool isSceneUnloading = false;
    bool spawnOneBar = true;


    private void Awake()
    {
        
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += onSceneLoad;
        NetworkManager.Singleton.SceneManager.OnUnload += onSceneUnload;
        
    }

    //Para cuandoi un jugador seleccione otro personaje, cambie el sprite donde se indica cual tiene seleccionado
    public void selectedCharacterChanged(int previous, int current)
    {
        if (selectorInfo == null) return;
        PlayerSelectorInfo player = selectorInfo.GetComponent<PlayerSelectorInfo>();
        if (player != null) { player.image.sprite = player.playerSprites[current]; } 
    }

    //Para cambiar la información (texto) de si está listo o no
    public void playerReady(bool previous, bool current) 
    {
        if (IsClient)
        {
            selectorInfo.GetComponent<PlayerSelectorInfo>().playerReady.text = "Ready";
        }
    }

    //Para actualizar los IDValue en función vayan entrando
    public void ChangePlayerIDValue()
    { 
        transform.parent.GetComponent<SpawningBehaviour>().playerId.Value = FindObjectsOfType<selectorPlayerBehaviour>().Length;
    }

    //Para cuando se conecte un jugador nuevo, le salga bien la informaciónd elos jugadores anteriores
    public override void OnNetworkSpawn()
    {
        selectedCharacter.OnValueChanged += selectedCharacterChanged;
        ready.OnValueChanged += playerReady;

        if (IsOwner) 
        {
            newPlayerConnectedServerRpc();
            FindObjectOfType<PlayerSelectorBehaviourHandler>().PlayerConect();
        }
        
        UImanager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UiManager>();
        
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
            selectorButtons?.huntressButton.onClick.AddListener(() => { selectedCharacter.Value = 0; transform.parent.GetComponent<SpawningBehaviour>().characterPrefab = 0; });
            selectorButtons?.akaiKazeButton.onClick.AddListener(() => { selectedCharacter.Value = 1; transform.parent.GetComponent<SpawningBehaviour>().characterPrefab = 1; });
            selectorButtons?.oniButton.onClick.AddListener(() => { selectedCharacter.Value = 2; transform.parent.GetComponent<SpawningBehaviour>().characterPrefab = 2; });
            selectorButtons?.readyButton.onClick.AddListener(()=> { ready.Value = true; if (once) { once = false; playerReadyServerRpc(); } });
        }
    }

    //Ver si están todos listos
    [ServerRpc]
    public void playerReadyServerRpc() 
    {
        GameObject.FindGameObjectWithTag("AllPlayerReady").GetComponent<AllPlayerReady>().playerIsReady();
    }

    //Si un jugador nuevo se conecta, desactivamos el boton de start
    [ServerRpc]
    public void newPlayerConnectedServerRpc()
    {
        GameObject.FindGameObjectWithTag("AllPlayerReady").GetComponent<AllPlayerReady>().deactivateStartButtonClientRpc();
    }

    public void parentReady() 
    {
        parent = transform.parent.gameObject;
        //UImanager.playingServer = transform.parent.GetComponent<SpawningBehaviour>().playingServer;
        FindObjectOfType<PlayerSelectorBehaviourHandler>().listaSelectorPlayer.Add(transform.parent.GetComponent<SpawningBehaviour>()); //MIRAR AQUIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII
        if (IsClient && spawnOneBar)
        {
            spawnOneBar = false;
            Debug.Log(transform.parent.GetComponent<SpawningBehaviour>().playerId.Value);
            selectorInfo = UImanager?.CrearBarras(transform.parent.GetComponent<SpawningBehaviour>().playerId.Value);
        }
        
        string text = parent.GetComponent<SpawningBehaviour>().playerName.Value.ToString();

        if (selectorInfo != null)
        {
            selectorInfo.GetComponent<PlayerSelectorInfo>().playerName.text = text;
            PlayerSelectorInfo player = selectorInfo.GetComponent<PlayerSelectorInfo>();
            if (player != null) { player.image.sprite = player.playerSprites[selectedCharacter.Value]; }
            if (ready.Value) { selectorInfo.GetComponent<PlayerSelectorInfo>().playerReady.text = "Ready"; }
        }
    }

    private void onSceneLoad(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut) 
    {
        if (sceneName == "JuegoPrincipal")
        {
            if (gameObject != null)
            {
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= onSceneLoad;
                selectedCharacter.OnValueChanged -= selectedCharacterChanged;
                ready.OnValueChanged -= playerReady;
                Destroy(gameObject);
            }
        }
    }

    private void onSceneUnload(ulong clientId, string sceneName, AsyncOperation asyncOperation) 
    {
        isSceneUnloading = true;
    }


    public override void OnNetworkDespawn()
    {
        //If ready was true decrease player count;
        if (!isSceneUnloading)
        {
            if (ready.Value)
            {
                GameObject.FindGameObjectWithTag("AllPlayerReady").GetComponent<AllPlayerReady>().playerUnreadyServerRpc();
            }           
            UImanager.EliminarBarra(selectorInfo);
            UImanager.DesplazarBarras();
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= onSceneLoad;
            selectedCharacter.OnValueChanged -= selectedCharacterChanged;
            ready.OnValueChanged -= playerReady;
            Destroy(selectorInfo);
        }


    }
}
