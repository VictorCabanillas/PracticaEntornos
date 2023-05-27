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
    public NetworkVariable<float> timerTextFinal  = new NetworkVariable<float>();
    public NetworkVariable<int> tSeconds = new NetworkVariable<int>(0);
    public NetworkVariable<int> tMinutes  = new NetworkVariable<int>(0);

    void Start()
    {
        timerTextFinal.OnValueChanged += updateTimer;
        StartTimer();
    }

    public void StartTimer()
    {
        timerText = timerGameObject.GetComponent<TextMeshProUGUI>();
        StartCoroutine(CalculateTime());
    }

    IEnumerator CalculateTime()
    {
        float counter = totalTime;
        while (counter > 0)
        {
            timerTextFinal.Value = counter;
            tMinutes.Value = (int) timerTextFinal.Value / 60;
            tSeconds.Value = (int) timerTextFinal.Value % 60;
            counter -= Time.deltaTime;
            yield return null;
        }
    }

    void updateTimer(float previous, float current)
    {
        Debug.Log(tMinutes.Value);
        String sMinutes = "", sSeconds = "";

            if (tMinutes.Value < 10)
            {
                sMinutes = "0" + tMinutes.Value;
            }
            else sMinutes = tMinutes.Value.ToString();
            if (tSeconds.Value < 10)
            {
                sSeconds = "0" + tSeconds.Value;
            }
            else sSeconds = tSeconds.Value.ToString();

        timerText.text = sMinutes + ":" + sSeconds;
    }
}
