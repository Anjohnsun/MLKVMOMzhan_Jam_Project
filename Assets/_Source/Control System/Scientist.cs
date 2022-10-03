using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Scientist : MonoBehaviour
{
    [SerializeField] private bool _canMove;
    [SerializeField] private float _speed;
    [SerializeField] private int _antidoteAmount;

    [SerializeField] private float _scientistInfection;
    [SerializeField] private bool _isInfected;  //Возможно не пригодится

    [SerializeField] private List<GameObject> _pickableAntidoteFlasks;
    [SerializeField] private SpriteRenderer _scientistSpriteRenderer;

    [SerializeField] private RescueEnema _rescueEnema;

    [SerializeField] private Button _accessableButton;

    public UnityEvent<Scientist> ScientistInfected = new UnityEvent<Scientist>();

    public bool CanMove { get => _canMove; set => _canMove = value; }
    public float Speed { get => _speed;}
    public int AntidoteAmount { get => _antidoteAmount; set => _antidoteAmount = value; }
    public List<GameObject> PicableAntidoteFlasks { get => _pickableAntidoteFlasks; set => _pickableAntidoteFlasks = value; }
    public SpriteRenderer ScientistSpriteRenderer { get => _scientistSpriteRenderer; set => _scientistSpriteRenderer = value; }
    public float ScientistInfection { get => _scientistInfection; set => _scientistInfection = value; }
    public RescueEnema RescueEnema { get => _rescueEnema; set => _rescueEnema = value; }
    public Button AccessableButton { get => _accessableButton; set => _accessableButton = value; }

    public void InfectScientist()
    {
        _isInfected = true;
        _canMove = false;
        ScientistInfected.Invoke(this);
        //Описать последствия заражения
    }

    public void TakeAntidoteFlask()
    {
        for(int i = 0; i < PicableAntidoteFlasks.Count; i++)
        {
            Destroy(PicableAntidoteFlasks[i]);
            _antidoteAmount++;
        }
    }

    public void RemoveAntidoteFlaskFromPickable(GameObject antidoteFlask)
    {
        _pickableAntidoteFlasks.Remove(antidoteFlask);
    }

    public void FillRescueEnema()
    {
        RescueEnema.LoadAntidoteFlask(_antidoteAmount);
        _antidoteAmount = 0;
    }
}
