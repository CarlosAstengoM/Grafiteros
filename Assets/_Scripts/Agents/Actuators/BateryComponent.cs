using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BateryComponent : MonoBehaviour
{
    [SerializeField] private int _maxCharge;
    private int _currentCharge;

    private void Start()
    {
        _currentCharge = _maxCharge;
    }

    private void OnEnable()
    {
        Agents agent = GetComponent<Agents>();
        agent.OnActionStarted += UseCharge;
    }

    private void OnDisable()
    {
        Agents agent = GetComponent<Agents>();
        agent.OnActionStarted -= UseCharge;
    }

    private void UseCharge(ActionType actionType)
    {
        int amount = 0;
        switch (actionType)
        {
            case ActionType.Move:
                amount = SimulationParameters.Instance.MoveEnergyConsumption;
                break;
        }
        
        UseCharge(amount);
    }

    private void UseCharge(int amount)
    {
        _currentCharge -= amount;
        if (_currentCharge < 0)
        {
            _currentCharge = 0;
        }
    }

    private void ChargeUp(int amount)
    {
        _currentCharge += amount;
        if (_currentCharge < _maxCharge)
        {
            _currentCharge = _maxCharge;
        }
    }
}
