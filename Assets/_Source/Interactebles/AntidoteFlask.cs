using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AntidoteFlask : MonoBehaviour
{
    [SerializeField] private Scientist _closestScientist;
    [SerializeField] private GameObject _textForPicking;
    [SerializeField] private UnityEvent<GameObject> AntidoteObjectDestroyed = new UnityEvent<GameObject>();
    private void OnDestroy()
    {
        AntidoteObjectDestroyed.Invoke(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ControlledScientist"))
        {
            collision.gameObject.TryGetComponent(out _closestScientist);
            _closestScientist.PicableAntidoteFlasks.Add(gameObject);

            AntidoteObjectDestroyed.AddListener(_closestScientist.RemoveAntidoteFlaskFromPickable);
        }
        _textForPicking.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ControlledScientist"))
        {
            collision.gameObject.TryGetComponent(out _closestScientist);
            _closestScientist.PicableAntidoteFlasks.Remove(gameObject);
        }
        _textForPicking.SetActive(false);

        AntidoteObjectDestroyed.RemoveListener(_closestScientist.RemoveAntidoteFlaskFromPickable);
    }

}
