using UnityEngine;

public class PlaybackComponent : MonoBehaviour
{
    private AgentAction _currentAction;
    
    private void Start()
    {
        PlaybackManager.Instance.OnReverseToggled += ReverseAction;
    }

    private void OnDisable()
    {
        PlaybackManager.Instance.OnReverseToggled -= ReverseAction;
    }

    private void ReverseAction()
    {
        PlayAction(_currentAction);
        if (PlaybackManager.Instance.IsPositiveTimeScale)
        {
            GetComponent<MoveComponent>().UpdatePositionInGrid(_currentAction.from, _currentAction.to);
        }
        else
        {
            GetComponent<MoveComponent>().UpdatePositionInGrid(_currentAction.to, _currentAction.from);
        }
    }
    
    public void PlayAction(AgentAction action)
    {
        _currentAction = action;
        BaseActuator actuator = null;
        
        switch (action.type)
        {
            case ActionType.MOVE:
                actuator = GetComponent<MoveComponent>();
                break;
            case ActionType.PICK:
                actuator = GetComponent<PickupComponent>();
                break;
            case ActionType.DROP:
                actuator = GetComponent<DropComponent>();
                break;
            case ActionType.CHARGE:
                actuator = GetComponent<BateryComponent>();
                break;
        }
        
        //Check for null actuator meaning it send the wrong order to the wrong agent
        if (PlaybackManager.Instance.IsPositiveTimeScale)
        {
            actuator.ExecuteAction(action.from, action.to);
        }
        else
        { 
            actuator.UndoAction(action.from, action.to);
        }
    }
}
