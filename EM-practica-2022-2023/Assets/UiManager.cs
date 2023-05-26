using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UiManager : MonoBehaviour
    {
        public GameObject _ui;
        Vector3[] positions;

        void Start()
        {
            Transform transform = GetComponent<Transform>();
            positions = new Vector3[4];
            positions[0] = new Vector3(-220 + transform.position.x, 200 + transform.position.y, 0);
            positions[1] = new Vector3(220 + transform.position.x, 200 + transform.position.y, 0);
            positions[2] = new Vector3(-220 + transform.position.x, -200 + transform.position.y, 0);
            positions[3] = new Vector3(220 + transform.position.x, -200 + transform.position.y, 0);
            //CrearBarras(3);
        }

        public GameObject CrearBarras(int nPlayer)
        {
            return Instantiate(_ui, positions[nPlayer], _ui.transform.rotation, transform);
        }
    }
}
