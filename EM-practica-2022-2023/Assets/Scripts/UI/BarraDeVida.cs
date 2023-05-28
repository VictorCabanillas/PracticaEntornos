using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{
    [SerializeField] private float vidaMaxima;

    public void CambiarBarra(int current)
    {
        //vidaActual = player.GetComponent<>().GetVida();
        //Debug.Log("Cambiar valor barra");
        GetComponent<Image>().fillAmount = current / vidaMaxima;
    }
}
