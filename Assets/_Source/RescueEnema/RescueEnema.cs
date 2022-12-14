using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RescueEnema : MonoBehaviour
{
    [SerializeField] private GameObject _textForFilling;
    [SerializeField] private Slider _fullnesSlider;

    [SerializeField] private float _fullness;
    [SerializeField] private float _antidoteFlaskValue;

    private Scientist _scientist;

    [SerializeField] private TaskManager _taskManager;

    public float Fullness { get => _fullness; set => _fullness = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.TryGetComponent(out _scientist);
        if(_scientist != null)
        {
            _scientist.RescueEnema = this;
            _textForFilling.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.TryGetComponent(out _scientist);
        if (_scientist != null)
        {
            _scientist.RescueEnema = null;
            _textForFilling.SetActive(false);
        }
    }

    public void LoadAntidoteFlask(int numberOfAnidoteFlasks)
    {
        _fullness += _antidoteFlaskValue * numberOfAnidoteFlasks;
        TaskManager.ResqueEnemaFullness = _fullness;
        StartCoroutine(ChangeFullnessView());
    }

    private IEnumerator ChangeFullnessView()
    {
        while (_fullnesSlider.value < Fullness)
        {
            yield return new WaitForSeconds(0.1f);
            _fullnesSlider.value += 0.25f * _antidoteFlaskValue;
        }
    }
}
