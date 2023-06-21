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
    
    public bool enMarcha;


    
    private void Start()
    {
         this.GetComponent<CanvasGroup>().alpha = 0f;
         

         restante.OnValueChanged += UpdateClock;
    }

    //Para instanciar el reloj en el servidor y que sea este quien lo controle
    [ServerRpc]
    public void InstatiateClockServerRpc() 
    {
        if(!GetComponent<NetworkObject>().IsSpawned){
        GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        }
    }

    //Spawneamos en el servidor el temporizador con el tiempo asignado en el inspector
    public override void OnNetworkSpawn()
    {

        if(IsServer)
        {
            restante.Value = (min * 60) + seg;
        }

        if (IsClient)
        {
            
        }

    }

    //Para que el temporizador funcione de manera visual
    public void UpdateClock(float previous, float current)
    {
        int tempMin = Mathf.FloorToInt(restante.Value / 60);
        int tempSeg = Mathf.FloorToInt(restante.Value % 60);
        tiempo.text = string.Format("{00:00}:{01:00}", tempMin, tempSeg);
    }


    private void LateUpdate()
    {


        if ((NetworkManager.Singleton.IsServer) && enMarcha)
        {     
            restante.Value -= Time.deltaTime;
            if (restante.Value < 1)
            {
                enMarcha = false;
                
            }
        }
    }
}