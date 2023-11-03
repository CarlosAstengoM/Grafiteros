using System;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    [SerializeField] private float _minDistance;
        
    private Vector3 _from;
    private Vector3 _to;

    private void Start()
    {
        MoveTo(new GridPosition(0,0), new GridPosition(0,1));
    }

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

    public void MoveTo(GridPosition from, GridPosition to)
    {
        _from = LevelGrid.Instance.GetWorldPosition(from);
        _to = LevelGrid.Instance.GetWorldPosition(to);
    }
}
