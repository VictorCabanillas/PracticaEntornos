using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UI;

public class AsociarBarras : NetworkBehaviour
{
    UiManager UImanager;
    GameObject healthBar;

    private void Awake()
    {
        UImanager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UiManager>();
    }

    public void crearBarras() 
    {
        Debug.Log("Vamos a crear las barras de vida");
        healthBar = UImanager.CrearBarras(transform.parent.GetComponent<SpawningBehaviour>().playerId.Value - 1);
        healthBar.GetComponent<BarraDeVida>().SetNombre(transform.parent.GetComponent<SpawningBehaviour>().playerName.Value.ToString());
        GetComponent<PlayerHealth>().healthBar = healthBar;
    }
}
