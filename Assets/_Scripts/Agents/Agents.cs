using System;
using UnityEngine;

[RequireComponent(typeof(PlaybackComponent))]
public class Agents : MonoBehaviour
{
    public Action<ActionType> OnActionStarted;
    
    public GridPosition _gridPosition { get; private set; }
    private PlaybackComponent _playbackComponent;

    private void Awake()
    {
        _playbackComponent = GetComponent<PlaybackComponent>();
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetUnitAtGridPosition(_gridPosition, this);
        transform.position = LevelGrid.Instance.GetWorldPosition(_gridPosition);
    }

    private void Update()
    {
        GridPosition currentGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(_gridPosition != currentGridPosition)
        {
            LevelGrid.Instance.UpdateUnitGridPosition(this,_gridPosition,currentGridPosition);
            _gridPosition = currentGridPosition;
        }
    }

    public void ExecuteStep(AgentAction agentAction)
    {
        OnActionStarted?.Invoke(agentAction.Type);
        _playbackComponent.PlayAction(agentAction);
    }
}
