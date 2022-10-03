using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    [SerializeField] private ControlSystem _controlSystem;
    void Update()
    {
        if (Input.GetAxis("Horizontal") > 0.1  || Input.GetAxis("Horizontal") < -0.1) 
        {
            _controlSystem.MoveHorizontally(Input.GetAxis("Horizontal"));
        }
        if (Input.GetAxis("Vertical") > 0.1 || Input.GetAxis("Vertical") < -0.1)
        {
            _controlSystem.MoveVertically(Input.GetAxis("Vertical"));
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _controlSystem.ChangePlayableBody();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            _controlSystem.MakeInteraction();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            _controlSystem.InteractWithButton();
        }
    }
}
