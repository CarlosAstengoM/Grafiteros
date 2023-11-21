using System;
using UnityEngine;

public class MoveComponent : BaseActuator
{
    [SerializeField] private float _minDistance;
        
    private Vector3 _from;
    private Vector3 _to;
    private void Update()
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
    }

    public override void ExecuteAction(GridPosition from, GridPosition to)
    {
        _from = LevelGrid.Instance.GetWorldPosition(from);
        _to = LevelGrid.Instance.GetWorldPosition(to);
    }

    public override void UndoAction(GridPosition from, GridPosition to)
    {
        _from = LevelGrid.Instance.GetWorldPosition(to);
        _to = LevelGrid.Instance.GetWorldPosition(from);
    }

    public override void OnActionReversed()
    {
        Vector3 temp = _from;
        _from = _to;
        _to = temp;
    }
}
