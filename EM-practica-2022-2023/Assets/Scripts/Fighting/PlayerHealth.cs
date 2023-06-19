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

    bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {

        victoryCondition = FindObjectOfType<VictoryConditions>();

    }

    public override void OnNetworkSpawn()
    {
        Health.OnValueChanged += updateHealth;
    }

    public override void OnNetworkDespawn()
    {
        //Health.Value = 0;
        //healthBar.GetComponent<BarraDeVida>().CambiarBarra(Health.Value);
        //victoryCondition.alivePlayersRemaining.Value -= 1;
        //isAlive = false;
        if (IsOwner)
        {
            Debug.Log("Aviso me marcho");
            victoryCondition.playerDisconnectedServerRpc();
            Debug.Log("Ya hice la rpc");
        }
        if (healthBar != null)
        {
            healthBar?.GetComponent<BarraDeVida>()?.CambiarBarra(0);
        }
        Health.OnValueChanged -= updateHealth;
    }

    

    void updateHealth(int previous,int current) 
    {
        if(healthBar != null ) 
        {
            healthBar?.GetComponent<BarraDeVida>().CambiarBarra(Health.Value);
        }
        
        if (Health.Value <= 0 && (IsServer) && isAlive) 
        {
            isAlive = false;
            FighterMovement movement = GetComponent<FighterMovement>();
            movement.speed = 0;
            movement.jumpAmount = 0;
            movement.Die();
            //MIRAR AQUI PARA DESACTIVAR BARRA
            //healthBar.gameObject.SetActive(false);
            
            victoryCondition.alivePlayersRemaining.Value -= 1;
            Debug.Log("Jugadores restantes: " + victoryCondition.alivePlayersRemaining.Value);
        }
    }

    public void DecreaseHealth(int amount) 
    {
        Health.Value -= amount;
    }

    public int GetVida(){
        return Health.Value;
    }

    /*private void OnApplicationQuit()
    {
        Health.Value = 0;
        isAlive = false;
        healthBar.GetComponent<BarraDeVida>().CambiarBarra(Health.Value);
        victoryCondition.alivePlayersRemaining.Value -= 1;
        Health.OnValueChanged -= updateHealth;
    }*/
}
