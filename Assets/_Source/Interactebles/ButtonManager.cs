using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private List<Button> _buttons;

    [SerializeField] private int _pressedButonNumber;
    [SerializeField] private int _requiredPressedButtonNumber;

    [SerializeField] private TaskManager _taskManager;

    public int PressedButonNumber { get => _pressedButonNumber; set => _pressedButonNumber = value; }
    public int RequiredPressedButtonNumber { get => _requiredPressedButtonNumber; set => _requiredPressedButtonNumber = value; }

    public void RegistrateButtons(List<Button> generatedButtons)
    {
        foreach (Button button in generatedButtons)
        {
            _buttons.Add(button);
        }
        foreach (Button button in _buttons)
        {
            button.ButtonPressed.AddListener(ButtonPressed);
            button.ButtonUnpressed.AddListener(ButtonUnpressed);
        }
        TaskManager.RequiredPressedButtonAmount = _requiredPressedButtonNumber;
    }

    private void ButtonPressed()
    {
        _pressedButonNumber++;
        TaskManager.PressedButtonAmount++;
        _taskManager.RefreshPresentation();

    }
    private void ButtonUnpressed()
    {
        _pressedButonNumber--;
        TaskManager.PressedButtonAmount--;
        _taskManager.RefreshPresentation();
    }
}
