using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System;
using TMPro;

public class Timer : NetworkBehaviour
{
    [SerializeField] float totalTime;
    [SerializeField] GameObject timerGameObject;
    TextMeshProUGUI timerText;
    public NetworkVariable<String> timerTextFinal;

    void Start()
    {
        timerTextFinal = new NetworkVariable<String>("");
        StartTimer();
        timerTextFinal.OnValueChanged += updateTimer;
    }

    public void StartTimer()
    {
        timerText = timerGameObject.GetComponent<TextMeshProUGUI>();
        if(IsServer) StartCoroutine(CalculateTime());
    }

    IEnumerator CalculateTime()
    {

        float counter = 0;
        int tMinutes = 0, tSeconds = 0;
        String sMinutes = "", sSeconds = "";

        while (counter > 0)
        {
            tMinutes = (int)counter / 60;
            tSeconds = (int)counter % 60;

            if (tMinutes < 10)
            {
                sMinutes = "0" + tMinutes;
            }
            else sMinutes = tMinutes.ToString();
            if (tSeconds < 10)
            {
                sSeconds = "0" + tSeconds;
            }
            else sSeconds = tSeconds.ToString();

            timerTextFinal = new NetworkVariable<String>(sMinutes + ":" + sSeconds);

            counter -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("END");
    }

    void updateTimer(String previous, String current)
    {
        timerTextFinal.Value = current;
        timerText.text = timerTextFinal.Value;
    }
}
