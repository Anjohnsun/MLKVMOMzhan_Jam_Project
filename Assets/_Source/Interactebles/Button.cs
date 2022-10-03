using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    [SerializeField] private bool _isPressed;

    public UnityEvent ButtonPressed = new UnityEvent();
    public UnityEvent ButtonUnpressed = new UnityEvent();

    [SerializeField] GameObject _buttonText;

    [SerializeField] private Scientist _closestScientist;

    public bool IsPressed { get => _isPressed; set => _isPressed = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ControlledScientist") && !_isPressed)
        {
            collision.gameObject.TryGetComponent(out _closestScientist);
            _closestScientist.AccessableButton = this;

        }
        _buttonText.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ControlledScientist") && !_isPressed)
        {
            collision.gameObject.TryGetComponent(out _closestScientist);
            _closestScientist.AccessableButton = null;

        }
        _buttonText.SetActive(false);
    }
}