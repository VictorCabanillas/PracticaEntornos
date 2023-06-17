using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Movement.Components;

public class Timer : NetworkBehaviour
{
    [SerializeField] int min, seg;
    [SerializeField] TextMeshProUGUI tiempo;

    public NetworkVariable<float> restante = new NetworkVariable<float>(); //Tiempo restante
    //public NetworkVariable<bool> enMarcha = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public bool enMarcha;

    private void Start()
    {
         this.GetComponent<CanvasGroup>().alpha = 0f;
         InstatiateClockServerRpc();


        restante.OnValueChanged += UpdateClock;
    }

    [ServerRpc]
    public void InstatiateClockServerRpc()
    {
        if(!GetComponent<NetworkObject>().IsSpawned){
        GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        }
    }

    public override void OnNetworkSpawn()
    {

        if(IsServer)
        {
            restante.Value = (min * 60) + seg;
        }

        if (IsClient)
        {
            //Debug.Log("VALOR: " + restante.Value.ToString());
        }

    }

    public void UpdateClock(float previous, float current)
    {
        int tempMin = Mathf.FloorToInt(restante.Value / 60);
        int tempSeg = Mathf.FloorToInt(restante.Value % 60);
        tiempo.text = string.Format("{00:00}:{01:00}", tempMin, tempSeg);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {


        if ((NetworkManager.Singleton.IsServer) && enMarcha)
        {

            //Debug.Log("ENTRANDO ACAAAAAAAAA");
            restante.Value -= Time.deltaTime;
            if (restante.Value < 1)
            {
                enMarcha = false;
                //MATAR JUGADOR
            }
        }
    }
}