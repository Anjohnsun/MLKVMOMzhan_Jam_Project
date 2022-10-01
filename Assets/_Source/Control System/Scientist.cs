using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientist : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _antidoteAmount;

    [SerializeField] private bool _isInfected;

    [SerializeField] private List<GameObject> _pickableAntidoteFlasks;

    public float Speed { get => _speed;}
    public int AntidoteAmount { get => _antidoteAmount;}
    public List<GameObject> CloseAntidoteFlasks { get => _pickableAntidoteFlasks; set => _pickableAntidoteFlasks = value; }

    public void InfectScientist()
    {
        _isInfected = true;
        //Описать последствия заражения
    }

    public void TakeAntidoteFlask()
    {
        foreach(GameObject antidote in CloseAntidoteFlasks)
        {
            Destroy(antidote);
            _antidoteAmount++;
        }
    }
}
