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

    [SerializeField] private List<GameObject> _pickableAntidoteFlasks;
    [SerializeField] private SpriteRenderer _scientistSpriteRenderer;
    [SerializeField] private GameObject _scientistMiniMapPointer;

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
    public GameObject ScientistMiniMapPointer { get => _scientistMiniMapPointer; set => _scientistMiniMapPointer = value; }

    public void InfectScientist()
    {
        ScientistInfected.Invoke(this);
        if (!_canMove && _accessableButton != null)
        {
            _accessableButton.ButtonUnpressed.Invoke();
            _accessableButton = null;
        }
        _canMove = false;
        GetComponent<SpriteRenderer>().color = Color.green;
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
