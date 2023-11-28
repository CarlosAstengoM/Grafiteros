using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Box_MoveComponent : MoveComponent
{
    [SerializeField] private LayerMask _shelfLayerMask;
    
    public override void UpdatePositionInGrid(GridPosition previous, GridPosition current)
    {
        if(previous == current) return;
        LevelGrid.Instance.UpdateBoxGridPosition(_agent,previous,current);
    }

    public override void ExecuteAction(GridPosition from, GridPosition to)
    {
        Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(to);
        float offset = 5f;
        RaycastHit raycastHit;
        bool hit = Physics.Raycast(worldPosition + Vector3.down * offset, Vector3.up, out raycastHit, offset * 2, _shelfLayerMask);
        if(hit)
        {
            Shelf shelf = raycastHit.transform.GetComponent<Shelf>();
            shelf.TakeBox(gameObject,false);
            _to = shelf.boxPosition.transform.position;
        }
        base.ExecuteAction(from, to);
    }

    public override void UndoAction(GridPosition from, GridPosition to)
    {
        base.UndoAction(from, to);
    }

}
