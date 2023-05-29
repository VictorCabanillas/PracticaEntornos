using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Movement.Components;

public class Timer : MonoBehaviour
{
    //[SerializeField] int min, seg;
    int min = 1, seg = 30;
    [SerializeField] TextMeshProUGUI tiempo;

    public NetworkVariable<float> restante = new NetworkVariable<float>(10,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server); //Tiempo restante
                                                                                                                                                          

    private void Start()
    {
        restante.Value = (min * 60) + seg;
        //Debug.Log(restante.Value);
        //enMarcaha = true;
        restante.OnValueChanged += UpdateClock;
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
       if(NetworkManager.Singleton.IsServer)
        {
            restante.Value -= Time.deltaTime;
            if(restante.Value < 1) 
            {
                //enMarcaha = false;
                //MATAR JUGADOR
            }
        }
    }
}
