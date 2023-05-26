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
    private bool alive = true;

    // Start is called before the first frame update
    void Start()
    {
        Health.Value = 100;
        Health.OnValueChanged += updateHealth;
    }
    void updateHealth(int previous,int current) 
    {
        Health.Value = current;
        healthBar.GetComponentInChildren<BarraDeVida>().CambiarBarra(Health.Value);
        if (Health.Value <= 0 && alive) 
        {
            alive = false;
            Debug.Log(alive);
            InputSystem.Instance.Character = null;
            GetComponent<FighterMovement>().Die();
        }
    }

    public void DecreaseHealth(int amount) 
    {
        Health.Value -= amount;
    }
}
