using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace UI
{
    public class UiManager : MonoBehaviour
    {
        public GameObject _ui;
        public Vector3[] positions;
        public bool playingServer=false;
        public GameObject CrearBarras(int nPlayer)
        {
            if (playingServer) { nPlayer = nPlayer - 1; }
            Vector3 position = positions[nPlayer] + transform.position;
            GameObject objeto = Instantiate(_ui, position, _ui.transform.rotation);
            objeto.transform.SetParent(transform);
            return objeto;
        }
    }
}
