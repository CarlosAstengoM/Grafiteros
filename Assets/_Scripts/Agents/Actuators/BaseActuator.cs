using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseActuator : MonoBehaviour
{
    public abstract void ExecuteAction(GridPosition from, GridPosition to);
    public abstract void UndoAction(GridPosition from, GridPosition to);
}
