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

        public GameObject CrearBarras(int nPlayer)
        {
            //if (playingServer) { nPlayer = nPlayer - 1; }
            Debug.Log(nPlayer);
            Vector3 position = positions[nPlayer] + transform.position;
            GameObject objeto = Instantiate(_ui, position, _ui.transform.rotation);
            objeto.transform.SetParent(transform);
            barras.Add(objeto);
            DesplazarBarras();
            return objeto;
        }

        public void DesplazarBarras()
        {
           for(int i = 0; i < barras.Count; i++)
            {
                barras[i].transform.localPosition = positions[i];
                //Debug.Log(positions[i]);
            }
        }

        public void EliminarBarra(GameObject objeto)
        {
            barras.Remove(objeto);
        }
    }
}
