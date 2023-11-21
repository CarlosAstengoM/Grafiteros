using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseActuator : MonoBehaviour
{
    private void Start()
    {
        PlaybackManager.Instance.OnReverseToggled += OnActionReversed;
    }

    private void OnDisable()
    {
        PlaybackManager.Instance.OnReverseToggled -= OnActionReversed;
    }

    public abstract void ExecuteAction(GridPosition from, GridPosition to);
    public abstract void UndoAction(GridPosition from, GridPosition to);
    
    public abstract void OnActionReversed();

}
