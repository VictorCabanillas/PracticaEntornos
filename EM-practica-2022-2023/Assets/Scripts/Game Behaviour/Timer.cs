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

    public NetworkVariable<float> restante = new NetworkVariable<float>(10,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server); //Tiempo restante
    //public NetworkVariable<bool> enMarcha = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public bool enMarcha = true;

    private void Start()
    {
        
        restante.OnValueChanged += UpdateClock;
        restante.Value = (min * 60) + seg;
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkObject.Spawn(this);
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
       if((NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsClient) && enMarcha)
        {
            
            restante.Value -= Time.deltaTime;
            if(restante.Value < 1) 
            {
                enMarcha = false;
                //MATAR JUGADOR
            }
        }
    }
}
