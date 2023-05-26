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
    // Start is called before the first frame update
    void Start()
    {
        /*if (IsClient)
        {
            healthBar = UImanager.CrearBarras((int)OwnerClientId);
            GetComponent<PlayerHealth>().healthBar = healthBar;
        }*/
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            healthBar = UImanager.CrearBarras((int)OwnerClientId);
            GetComponent<PlayerHealth>().healthBar = healthBar;
        }
    }
}
