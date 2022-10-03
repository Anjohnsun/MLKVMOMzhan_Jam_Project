using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private List<Button> _buttons;

    [SerializeField] private GameObject _buttonPrefab;

    [SerializeField] private int _pressedButonNumber;
    [SerializeField] private int _requiredPressedButtonNumber;

    [SerializeField] private TaskManager _taskManager;

    public int PressedButonNumber { get => _pressedButonNumber; set => _pressedButonNumber = value; }
    public int RequiredPressedButtonNumber { get => _requiredPressedButtonNumber; set => _requiredPressedButtonNumber = value; }

    private void Start()
    {
        foreach(Button button in _buttons)
        {
            button.ButtonPressed.AddListener(ButtonPressed);
            button.ButtonUnpressed.AddListener(ButtonUnpressed);
        }
    }

    public void CreateButton(Vector2 position)
    {
        _buttons.Add((Instantiate(_buttonPrefab, position, new Quaternion())).GetComponent<Button>());
        _buttons[_buttons.Count - 1].ButtonPressed.AddListener(ButtonPressed);
        _buttons[_buttons.Count - 1].ButtonUnpressed.AddListener(ButtonUnpressed);
    }

    private void ButtonPressed()
    {
        _pressedButonNumber++;
        TaskManager.PressedButtonAmount++;
        
    }
    private void ButtonUnpressed()
    {
        _pressedButonNumber--;
        TaskManager.PressedButtonAmount--;
    }
}