using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System;
using TMPro;

public class GameManager : NetworkBehaviour
{

    public NetworkVariable<int> ganador  = new NetworkVariable<int>();
    public GameObject[] jugadores;
    private int nJugador;

    void Start(){
        ganador.OnValueChanged+=FinalizarPartida;
        jugadores = GameObject.FindGameObjectsWithTag("Personaje");
    }

    public void AñadirJugador(){
        nJugador++;
    }
    
    public void EliminarJugador(){
        nJugador--;
        if(nJugador==1){
            ganador.Value = 0;
        }
    }

    private void FinalizarPartida(int previous, int next){
        //EJECUTAR AQUI LO QUE QUERAMOS CUANDO ALGUIEN GANE UNA PARTIDA 
        Debug.Log("Se terminó la partida");
    }

    public void FinalizarPartidaTiempo(){
        int aux = 0;
        for(int i = 1; i < jugadores.Length; i++){
            if(jugadores[i].GetComponent<PlayerHealth>().GetVida()>= jugadores[aux].GetComponent<PlayerHealth>().GetVida()){
                    aux = i;
            }       
        }
        ganador.Value = aux;
    }
}

