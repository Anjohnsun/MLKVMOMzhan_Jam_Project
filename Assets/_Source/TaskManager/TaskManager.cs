using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private static int _aliveScientistsAmount;
    [SerializeField] private static int _pressedButtonAmount;
    [SerializeField] private static int _requiredPressedButtonAmount;
    [SerializeField] private static float _resqueEnemaFullness;

    [SerializeField] private TextMeshProUGUI _aliveScientistsText;
    [SerializeField] private TextMeshProUGUI _pressedButtonsText;
    [SerializeField] private TextMeshProUGUI _enemaFullnessText;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _miniMapCamera;

    public static int AliveScientistsAmount { get => _aliveScientistsAmount; set => _aliveScientistsAmount = value; }
    public static int PressedButtonAmount { get => _pressedButtonAmount; set => _pressedButtonAmount = value; }
    public static int RequiredPressedButtonAmount { get => _requiredPressedButtonAmount; set => _requiredPressedButtonAmount = value; }
    public static float ResqueEnemaFullness { get => _resqueEnemaFullness; set => _resqueEnemaFullness = value; }

    public void RefreshPresentation()
    {
        _aliveScientistsText.text = "Still alive: " + _aliveScientistsAmount;
        _pressedButtonsText.text = "Pressed buttons: " + _pressedButtonAmount + "/" + _requiredPressedButtonAmount;
        _enemaFullnessText.text = "The machine is " + Mathf.RoundToInt(_resqueEnemaFullness*100) + "% full";

        if (_resqueEnemaFullness >= 1 && _requiredPressedButtonAmount == _pressedButtonAmount)
        {
            _winPanel.SetActive(true);
            _miniMapCamera.SetActive(false);
            _requiredPressedButtonAmount = 0;
        }
        if (_aliveScientistsAmount < _requiredPressedButtonAmount)
        {
            _losePanel.SetActive(true);
            _miniMapCamera.SetActive(false);
        }
    }
}
