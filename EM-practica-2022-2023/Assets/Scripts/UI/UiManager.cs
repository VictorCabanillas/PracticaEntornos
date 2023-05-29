using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UiManager : MonoBehaviour
    {
        public GameObject _ui;
        public Vector3[] positions;

        public GameObject CrearBarras(int nPlayer)
        {
            Vector3 position = positions[nPlayer] + transform.position;
            GameObject objeto = Instantiate(_ui, position, _ui.transform.rotation);
            objeto.transform.SetParent(transform);
            return objeto;
        }
    }
}
