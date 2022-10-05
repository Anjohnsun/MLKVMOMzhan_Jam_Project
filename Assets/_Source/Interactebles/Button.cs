using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    [SerializeField] private bool _isPressed;

    [SerializeField] GameObject _buttonText;
    [SerializeField] SpriteRenderer _buttonSpriteForMiniMapSpriteRenderer;

    [SerializeField] private Scientist _closestScientist;

    public UnityEvent ButtonPressed = new UnityEvent();
    public UnityEvent ButtonUnpressed = new UnityEvent();


    public bool IsPressed
    {
        get => _isPressed; set
        {
            if (value == false)
            {
                _buttonSpriteForMiniMapSpriteRenderer.color = new Color(0.9f, 0.74f, 0, 1);
                _isPressed = false;
            } else
            {
                _buttonSpriteForMiniMapSpriteRenderer.color = new Color(0.38f, 0.9f, 0, 1);
                _isPressed = true;
            }
        }
    }

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