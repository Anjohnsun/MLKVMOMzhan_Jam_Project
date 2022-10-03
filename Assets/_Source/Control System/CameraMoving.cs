using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    [SerializeField] private ControlSystem _controlSystem;
    void Update()
    {
        transform.position = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, -10), _controlSystem.MovingBody.position, 0.01f);
    }
}
