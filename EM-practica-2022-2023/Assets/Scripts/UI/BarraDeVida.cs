using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{
    private GameObject player;

    private float vidaActual;

    [SerializeField] private float vidaMaxima;
    // Start is called before the first frame update
    void Start(){
        vidaActual = vidaMaxima;
    }
    private void Update()
    {
        CambiarBarra();
    }

    // Update is called once per frame
    public void CambiarBarra()
    {
        //vidaActual = player.GetComponent<>().GetVida();
        GetComponent<Image>().fillAmount = vidaActual / vidaMaxima;
    }

    public void SetPlayer(GameObject p)
    {
        //vidaActual = player.GetComponent<>().GetVida();
        player = p;
    }
}
