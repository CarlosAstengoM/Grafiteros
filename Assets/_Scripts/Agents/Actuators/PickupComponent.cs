using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupComponent : BaseActuator
{
    [SerializeField] private BoxHolder _boxHolder;

    public override void ExecuteAction(GridPosition from, GridPosition to)
    {
        Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(to);
        float offset = 5f;
        RaycastHit raycastHit;
        Physics.Raycast(worldPosition + Vector3.down * offset, Vector3.up, out raycastHit, offset * 2, _boxHolder.ShelfLayerMask);

        _boxHolder.StoredBox = raycastHit.transform.GetComponent<Shelf>().GiveBox();
        
        Vector3 position = _boxHolder.BoxPositionHolder.transform.position;
        _boxHolder.StoredBox.transform.position = new Vector3(position.x,0,position.z);
        
        _boxHolder.StoredBox.transform.parent = gameObject.transform;

        _boxHolder.StoredBox.GetComponent<MoveComponent>().enabled = false;
        LevelGrid.Instance.ClearBoxAtGridPosition(to);
    }

    public override void UndoAction(GridPosition from, GridPosition to)
    {
        Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(to);
        float offset = 5f;
        RaycastHit raycastHit;
        Physics.Raycast(worldPosition + Vector3.down * offset, Vector3.up, out raycastHit, offset * 2, _boxHolder.ShelfLayerMask);

        raycastHit.transform.GetComponent<Shelf>().TakeBox(_boxHolder.StoredBox,true);
        
        MoveComponent moveComponent = _boxHolder.StoredBox.GetComponent<MoveComponent>();
        
        moveComponent.enabled = true;
        moveComponent.PreventMovement();
        
        LevelGrid.Instance.SetBoxAtGridPosition(to, _boxHolder.StoredBox.GetComponent<Agents>());
        
        _boxHolder.StoredBox = null;
    }
}
