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

    public static int AliveScientistsAmount { get => _aliveScientistsAmount; set => _aliveScientistsAmount = value; }
    public static int PressedButtonAmount { get => _pressedButtonAmount; set => _pressedButtonAmount = value; }
    public static int RequiredPressedButtonAmount { get => _requiredPressedButtonAmount; set => _requiredPressedButtonAmount = value; }
    public static float ResqueEnemaFullness { get => _resqueEnemaFullness; set => _resqueEnemaFullness = value; }

    public void RefreshPresentation()
    {
        _aliveScientistsText.text = "Живы ещё " + _aliveScientistsAmount;
        _pressedButtonsText.text = "Нажато " + _pressedButtonAmount + "/" + _requiredPressedButtonAmount;
        _enemaFullnessText.text = "Машина заполнена на " + Mathf.RoundToInt(_resqueEnemaFullness);
    }
}
