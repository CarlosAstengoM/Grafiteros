using System;
using UnityEngine;

[RequireComponent(typeof(PlaybackComponent))]
public class Agents : MonoBehaviour
{
    public Action<ActionType> OnActionStarted;
    
    public GridPosition _gridPosition { get; protected set; }
    private PlaybackComponent _playbackComponent;

    private void Awake()
    {
        _playbackComponent = GetComponent<PlaybackComponent>();
    }

    protected virtual void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetUnitAtGridPosition(_gridPosition, this);
        transform.position = LevelGrid.Instance.GetWorldPosition(_gridPosition);
    }
    
    public void ExecuteStep(AgentAction agentAction)
    {
        OnActionStarted?.Invoke(agentAction.type);
        _playbackComponent.PlayAction(agentAction);
    }
}
