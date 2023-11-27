using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropComponent : BaseActuator
{
    [SerializeField] private GameObject boxVisual;
    public override void ExecuteAction(GridPosition from, GridPosition to)
    {
        boxVisual.SetActive(false);
    }

    public override void UndoAction(GridPosition from, GridPosition to)
    {
        boxVisual.SetActive(true);
    }

    public override void OnActionReversed()
    {
        boxVisual.SetActive(true);
    }
}
