using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BateryComponent : BaseActuator
{
    [SerializeField] private int _maxCharge;
    private int _currentCharge;
    private int _lastChargeDelta;

    private void Start()
    {
        _currentCharge = _maxCharge;
        PlaybackManager.Instance.OnReverseToggled += SelectChargeMethod;
    }

    private void OnEnable()
    {
        Agents agent = GetComponent<Agents>();
        agent.OnActionStarted += CalculateChargeUsage;
    }

    private void OnDisable()
    {
        Agents agent = GetComponent<Agents>();
        agent.OnActionStarted -= CalculateChargeUsage;
        PlaybackManager.Instance.OnReverseToggled -= SelectChargeMethod;
    }

    public override void ExecuteAction(GridPosition from, GridPosition to)
    {
        ChargeUp(SimulationParameters.Instance.ChargeUpAmount);
    }

    public override void UndoAction(GridPosition from, GridPosition to)
    {
        UseCharge(SimulationParameters.Instance.ChargeUpAmount);
    }

    private void CalculateChargeUsage(ActionType actionType)
    {
        int amount = 0;
        switch (actionType)
        {
            case ActionType.MOVE:
                amount = SimulationParameters.Instance.MoveEnergyConsumption;
                break;
            case ActionType.PICK:
            case ActionType.DROP:
                amount = SimulationParameters.Instance.PickDropEnergyConsumption;
                break;
        }
        _lastChargeDelta = amount;
        SelectChargeMethod();
    }

    private void SelectChargeMethod()
    {
        if (PlaybackManager.Instance.IsPositiveTimeScale)
        {
            UseCharge(_lastChargeDelta);
        }
        else
        {
            ChargeUp(_lastChargeDelta);
        }
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
        if (_currentCharge > _maxCharge)
        {
            _currentCharge = _maxCharge;
        }
    }
}
