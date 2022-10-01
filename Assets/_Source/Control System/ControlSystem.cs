using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSystem : MonoBehaviour
{
    [SerializeField] private bool _canMove;
    [SerializeField] private Scientist _scientist;
    [SerializeField] private Transform _movingBody;

    [SerializeField] private float _raycastDistance;

    [SerializeField] private Scientist[] _playableBodies;
    [SerializeField] private bool[] _bodiesAvailability;
    [SerializeField] private int _numberOfControledBody;

    public void MoveHorizontally(float vector)
    {

        if (vector > 0.1)
        {
            if (!Physics.Raycast(_movingBody.position, Vector2.right, _raycastDistance))
            {
                _movingBody.position = new Vector2(_movingBody.position.x + vector * _scientist.Speed,
                    _movingBody.position.y);
            }
        }
        else if (vector < -0.1)
        {
            if (!Physics.Raycast(_movingBody.position, Vector2.left, _raycastDistance))
            {
                _movingBody.position = new Vector2(_movingBody.position.x + vector * _scientist.Speed,
                    _movingBody.position.y);
            }
        }
    }

    public void MoveVertically(float vector)
    {
        if (vector > 0.1)
        {
            if (!Physics.Raycast(_movingBody.position, Vector2.up, _raycastDistance))
            {
                _movingBody.position = new Vector2(_movingBody.position.x,
                    _movingBody.position.y + vector * _scientist.Speed);
            }
        }
        else if (vector < -0.1)
        {
            if (!Physics.Raycast(_movingBody.position, Vector2.down, _raycastDistance))
            {
                _movingBody.position = new Vector2(_movingBody.position.x,
                    _movingBody.position.y + vector * _scientist.Speed);
            }
        }
    }

    public void ChangePlayableBody()
    {
        _movingBody.tag = "Untagged";
        if (_numberOfControledBody + 1 < _playableBodies.Length)
        {
            _numberOfControledBody++;
        } else
        {
            _numberOfControledBody = 0;
        }
        _scientist = _playableBodies[_numberOfControledBody];
        _movingBody = _playableBodies[_numberOfControledBody].transform;
        _movingBody.tag = "ControlledScientist";
    }
}
