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
            Vector3 velocity = distance / SimulationParameters.Instance.TurnTime * Time.deltaTime;
            transform.position += velocity;
        }
    }

    public override void ExecuteAction(GridPosition from, GridPosition to)
    {
        _from = LevelGrid.Instance.GetWorldPosition(from);
        _to = LevelGrid.Instance.GetWorldPosition(to);
    }
}
