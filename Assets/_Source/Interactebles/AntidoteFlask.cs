using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntidoteFlask : MonoBehaviour
{
    [SerializeField] private Scientist _closestScientist;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ControlledScientist"))
        {
            collision.TryGetComponent<Scientist>(out _closestScientist);
            _closestScientist.CloseAntidoteFlasks.Add(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ControlledScientist"))
        {
            collision.TryGetComponent<Scientist>(out _closestScientist);
            _closestScientist.CloseAntidoteFlasks.Remove(gameObject);
        }
    }
}
