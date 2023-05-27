using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UiManager : MonoBehaviour
    {
        public GameObject _ui;
        public Vector3[] positions;

        void Start()
        {
           
        }

        public GameObject CrearBarras(int nPlayer)
        {
            var playerUI = Instantiate(_ui);
            playerUI.transform.SetParent(transform, false);
            playerUI.transform.localPosition = positions[nPlayer];
            return playerUI;
        }
    }
}
