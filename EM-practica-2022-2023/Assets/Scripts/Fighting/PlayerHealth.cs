using System.Collections;
using System.Collections.Generic;
using Systems;
using UnityEngine;
using Unity.Netcode;
using Movement.Components;

public class PlayerHealth : NetworkBehaviour
{
    public NetworkVariable<int> Health = new NetworkVariable<int>();
    public GameObject healthBar;
    private VictoryConditions victoryCondition;   

    // Start is called before the first frame update
    void Start()
    {

        victoryCondition = FindObjectOfType<VictoryConditions>();

    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            Health.Value = 100;
            Health.OnValueChanged += updateHealth;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer) 
        { 
            Health.Value = 0;
            Health.OnValueChanged -= updateHealth;
        }
    }
   
    void updateHealth(int previous,int current) 
    {
        Health.Value = current;
        
        

            UpdateHealthBarClientRpc();
        
        
        if (Health.Value <= 0 && (IsServer)) 
        {
            FighterMovement movement = GetComponent<FighterMovement>();
            movement.speed = 0;
            movement.jumpAmount = 0;
            movement.Die();
            //MIRAR AQUI PARA DESACTIVAR BARRA
            
            
            victoryCondition.alivePlayersRemaining.Value -= 1;
            
        }
    }

    public void DecreaseHealth(int amount) 
    {
        Health.Value -= amount;
    }

    [ClientRpc]
    public void UpdateHealthBarClientRpc() 
    {
        healthBar.GetComponentInChildren<BarraDeVida>().CambiarBarra(Health.Value);
        if(Health.Value <= 0)
        {
            healthBar.gameObject.SetActive(false);
        }
        
    }
}
