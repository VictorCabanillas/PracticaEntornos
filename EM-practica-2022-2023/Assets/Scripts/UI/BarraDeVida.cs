using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class BarraDeVida : MonoBehaviour
{
    
     [SerializeField] private float vidaMaxima;
     [SerializeField] GameObject relleno;
     [SerializeField] GameObject icono;
     [SerializeField] GameObject nombre;

    public void CambiarBarra(int current)
    {
        //vidaActual = player.GetComponent<>().GetVida();
        GetComponent<Image>().fillAmount = current / vidaMaxima;
        Debug.Log("Cambiar valor barra");
        relleno.GetComponent<Image>().fillAmount = current / vidaMaxima;
    }

    public void SetNombre(String n)
    {
        nombre.GetComponent<TMP_Text>().text = n;
    }

    public void SetIcono(Sprite s)
    {
        icono.GetComponent<Image>().sprite = s;
    }
}

