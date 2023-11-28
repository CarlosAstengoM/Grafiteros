using System;
using UnityEngine;

public class MoveComponent : BaseActuator
{
    [SerializeField] protected float _minDistance;
        
    protected Vector3 _from;
    protected Vector3 _to;

    protected Agents _agent;

    private void Awake()
    {
        _agent = GetComponent<Agents>();
    }

    protected virtual void Update()
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

    public virtual void UpdatePositionInGrid(GridPosition previous, GridPosition current)
    {
        if(previous == current) return;
        LevelGrid.Instance.UpdateUnitGridPosition(_agent,previous,current);
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
}
