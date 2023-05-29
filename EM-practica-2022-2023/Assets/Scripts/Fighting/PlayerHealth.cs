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

    // Start is called before the first frame update
    void Start()
    {
        Health.OnValueChanged += updateHealth;
    }
    void updateHealth(int previous,int current) 
    {
        Health.Value = current;
        if (healthBar != null)
        {
            healthBar.GetComponentInChildren<BarraDeVida>().CambiarBarra(Health.Value);
        }
        if (Health.Value <= 0) 
        {
            FighterMovement movement = GetComponent<FighterMovement>();
            movement.speed = 0;
            movement.jumpAmount = 0;
            movement.Die();
        }
    }

    public void DecreaseHealth(int amount) 
    {
        Health.Value -= amount;
    }
}
