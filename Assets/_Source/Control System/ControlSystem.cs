using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSystem : MonoBehaviour
{
    [SerializeField] private Scientist _scientist;
    [SerializeField] private Transform _movingBody;

    [SerializeField] private float _raycastDistance;
    [SerializeField] private string _raycastlayerMask;

    [SerializeField] private List<Scientist> _playableScientists;
    [SerializeField] private int _numberOfControledBody;

    public Transform MovingBody { get => _movingBody; }

    [SerializeField] private TaskManager _taskManager;

    private void Start()
    {
        foreach (Scientist scientist in _playableScientists)
        {
            scientist.ScientistInfected.AddListener(MakeScientistUnavailable);
        }
    }

    public void MoveHorizontally(float vector)
    {
        if (_scientist.CanMove)
        {
            if (vector > 0.1)
            {
                if (!Physics2D.Raycast(_movingBody.position, Vector2.right, _raycastDistance, LayerMask.GetMask(_raycastlayerMask)))
                {
                    _movingBody.position = new Vector2(_movingBody.position.x + vector * _scientist.Speed,
                        _movingBody.position.y);
                }
            }
            else if (vector < -0.1)
            {
                if (!Physics2D.Raycast(_movingBody.position, Vector2.left, _raycastDistance, LayerMask.GetMask(_raycastlayerMask)))
                {
                    _movingBody.position = new Vector2(_movingBody.position.x + vector * _scientist.Speed,
                        _movingBody.position.y);
                }
            }
        }
    }

    public void MoveVertically(float vector)
    {
        if (_scientist.CanMove)
        {
            if (vector > 0.1)
            {
                if (!Physics2D.Raycast(MovingBody.position, Vector2.up, _raycastDistance, LayerMask.GetMask(_raycastlayerMask)))
                {
                    _movingBody.position = new Vector2(MovingBody.position.x,
                        _movingBody.position.y + vector * _scientist.Speed);
                }
            }

            else if (vector < -0.1)
            {
                if (!Physics2D.Raycast(MovingBody.position, Vector2.down, _raycastDistance, LayerMask.GetMask(_raycastlayerMask)))
                {
                    _movingBody.position = new Vector2(MovingBody.position.x,
                        _movingBody.position.y + vector * _scientist.Speed);
                }
            }

            _scientist.ScientistSpriteRenderer.sortingOrder = Mathf.RoundToInt(_movingBody.position.y * 4);
        }
    }

    public void MakeInteraction()
    {
        if (_scientist.RescueEnema != null && _scientist.AntidoteAmount > 0)
        {
            _scientist.FillRescueEnema();
        }
        else
        {
            foreach (GameObject antidoteFlusk in _scientist.PicableAntidoteFlasks)
            {
                Destroy(antidoteFlusk);
                _scientist.AntidoteAmount++;
            }
        }
    }

    public void InteractWithButton()
    {
        if (_scientist.AccessableButton != null)
        {
            if (_scientist.AccessableButton.IsPressed)
            {
                _scientist.AccessableButton.ButtonUnpressed.Invoke();
                _scientist.AccessableButton.IsPressed = false;
                _scientist.CanMove = true;
            }
            else
            {
                _scientist.AccessableButton.ButtonPressed.Invoke();
                _scientist.AccessableButton.IsPressed = true;
                _scientist.CanMove = false;
            }
        }
    }

    public void ChangePlayableBody()
    {
        if (_playableScientists.Count > 0)
        {
            MovingBody.tag = "Untagged";
            if (_numberOfControledBody + 1 < _playableScientists.Count)
            {
                _numberOfControledBody++;
            }
            else
            {
                _numberOfControledBody = 0;
            }

            _scientist = _playableScientists[_numberOfControledBody];
            _movingBody = _playableScientists[_numberOfControledBody].transform;
            _movingBody.tag = "ControlledScientist";
        }
    }

    public void MakeScientistUnavailable(Scientist scientist)
    {
        if (scientist.Equals(_scientist))
        {
            _playableScientists.Remove(scientist);
            _numberOfControledBody = 0;
        }
        _playableScientists.Remove(scientist);

        TaskManager.AliveScientistsAmount = _playableScientists.Count;
        _taskManager.RefreshPresentation();
    }
}