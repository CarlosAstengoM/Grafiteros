using UnityEngine;

public class PlaybackComponent : MonoBehaviour
{
    public void PlayAction(AgentAction action)
    {
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
