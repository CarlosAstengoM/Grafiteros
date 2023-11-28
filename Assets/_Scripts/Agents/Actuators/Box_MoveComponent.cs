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

    protected override void Update()
    {
        Vector3 position = transform.position;
        position.y = 0;
        float remainingDistance = Vector3.Distance(position, _to);
        if (remainingDistance > _minDistance)
        {
            Vector3 distance =  _to - _from;
            Vector3 velocity = distance / SimulationParameters.Instance.TurnTime * (PlaybackManager.Instance.SimulationTimeScale * Time.deltaTime);
            transform.position += velocity;
        }
        else
        {
            
        }
    }

    public override void ExecuteAction(GridPosition from, GridPosition to)
    {
        Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(to);
        float offset = 5f;
        RaycastHit raycastHit;
        bool hit = Physics.Raycast(worldPosition + Vector3.down * offset, Vector3.up, out raycastHit, offset * 2, _shelfLayerMask);
        if(hit)
        {
            raycastHit.transform.GetComponent<Shelf>().TakeBox(gameObject,false);
        }
        base.ExecuteAction(from, to);
    }

    public override void UndoAction(GridPosition from, GridPosition to)
    {
        base.UndoAction(from, to);
    }

}
