using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusSite : MonoBehaviour
{
    [SerializeField] private float _virusIntensity;

    [SerializeField] private List<Scientist> _scientistsInVirusArea;
    private Scientist _movingScientist;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.TryGetComponent(out _movingScientist);
        _scientistsInVirusArea.Add(_movingScientist);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.TryGetComponent(out _movingScientist);
        _scientistsInVirusArea.Remove(_movingScientist);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        foreach(Scientist scientist in _scientistsInVirusArea)
        {
            scientist.ScientistInfection += _virusIntensity * Time.deltaTime;
            if (scientist.ScientistInfection >= 1)
                scientist.InfectScientist();
        }
    }
}
