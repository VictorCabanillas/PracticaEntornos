using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;

namespace UI
{
    public class UiManager : MonoBehaviour
    {
        public GameObject _ui;
        public Vector3[] positions;
        public bool playingServer = false;
        

        private List<GameObject> barras = new List<GameObject>();

        //Crear las barras del selector
        public GameObject CrearBarras(int nPlayer)
        {
            
            Debug.Log(nPlayer);
            Vector3 position = positions[nPlayer] + transform.position;
            
            GameObject objeto = Instantiate(_ui, position, _ui.transform.rotation);
            objeto.transform.SetParent(transform);
            barras.Add(objeto);
            DesplazarBarras();
            return objeto;
        }

        //Mover las barras por si algún jugador se desconecta, poder reorganizarlas
        public void DesplazarBarras()
        {
           for(int i = 0; i < barras.Count; i++)
            {
                barras[i].transform.localPosition = positions[i];
               
            }
        }

        //Eliminar las barras en caso de desconexión
        public void EliminarBarra(GameObject objeto)
        {
            barras.Remove(objeto);
        }
    }
}
