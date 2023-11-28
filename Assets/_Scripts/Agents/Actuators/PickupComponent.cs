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
        _boxHolder.StoredBox.transform.parent = gameObject.transform;
        Vector3 localPosition = _boxHolder.BoxPositionHolder.transform.localPosition;
        _boxHolder.StoredBox.transform.localPosition = new Vector3(localPosition.x,0,localPosition.z);
    }

    public override void UndoAction(GridPosition from, GridPosition to)
    {
        Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(to);
        float offset = 5f;
        RaycastHit raycastHit;
        Physics.Raycast(worldPosition + Vector3.down * offset, Vector3.up, out raycastHit, offset * 2, _boxHolder.ShelfLayerMask);

        raycastHit.transform.GetComponent<Shelf>().TakeBox(_boxHolder.StoredBox,true);
        _boxHolder.StoredBox = null;
    }
}
