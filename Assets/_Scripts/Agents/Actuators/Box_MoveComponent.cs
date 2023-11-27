using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Box_MoveComponent : MoveComponent
{
    [SerializeField] private LayerMask _shelfLayerMask;
    public override void ExecuteAction(GridPosition from, GridPosition to)
    {
        Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(to);
        float offset = 5f;
        bool hit = Physics.Raycast(worldPosition + Vector3.down * offset, Vector3.up, offset*2, _shelfLayerMask);
        if(hit)
        {
            
        }
        base.ExecuteAction(from, to);
    }

    public override void UndoAction(GridPosition from, GridPosition to)
    {
        base.UndoAction(from, to);
    }
    
    public override void OnActionReversed()
    {
        base.OnActionReversed();
    }

}
