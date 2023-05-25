using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public GameObject _ui;
    Vector3[] positions;

    void Start(){
        Transform transform = GetComponent<Transform>();
        positions = new Vector3[4];
        positions[0] = new Vector3(-440 + transform.position.x, 200 + transform.position.y, 0);
        positions[1] = new Vector3(440 + transform.position.x, 200 + transform.position.y, 0);
        positions[2] = new Vector3(-440 + transform.position.x, -200 + transform.position.y, 0);
        positions[3] = new Vector3(440 + transform.position.x, -200 + transform.position.y, 0);
        CrearBarras(3);
    }

    public void CrearBarras(int nPlayer)
    {
        switch (nPlayer){
            case 1:
            Instantiate(_ui, positions[0], _ui.transform.rotation, transform);
            break;
            case 2:
            Instantiate(_ui, positions[0], _ui.transform.rotation, transform);
            Instantiate(_ui, positions[1], _ui.transform.rotation, transform);
            
            break;
            case 3:
            Instantiate(_ui, positions[0], _ui.transform.rotation, transform);
            Instantiate(_ui, positions[1], _ui.transform.rotation, transform);
            Instantiate(_ui, positions[2], _ui.transform.rotation, transform);
            break;
            case 4:
            Debug.Log("Hola");
            Instantiate(_ui, positions[0], _ui.transform.rotation, transform);
            Instantiate(_ui, positions[1], _ui.transform.rotation, transform);
            Instantiate(_ui, positions[2], _ui.transform.rotation, transform);
            Instantiate(_ui, positions[3], _ui.transform.rotation, transform);
            break;
        }
    }
}
