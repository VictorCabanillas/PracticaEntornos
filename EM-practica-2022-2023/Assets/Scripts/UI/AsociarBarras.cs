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

    public override void OnNetworkSpawn()
    {
        if (IsClient && !IsOwner)
        {
<<<<<<< Updated upstream
            healthBar = UImanager.CrearBarras((int)OwnerClientId);
            GetComponent<PlayerHealth>().healthBar = healthBar;
        }
    }
=======
            crearBarras();
        }
    }

    public void crearBarras() 
    {
        Debug.Log("Vamos a crear las barras de vida");
        UImanager.playingServer = transform.parent.GetComponent<SpawningBehaviour>().playingServer;
        healthBar = UImanager.CrearBarras((int)OwnerClientId);
        GetComponent<PlayerHealth>().healthBar = healthBar;
    }
>>>>>>> Stashed changes
}
