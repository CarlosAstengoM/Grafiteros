using UnityEngine;

public class PlaybackComponent : MonoBehaviour
{
    public void PlayAction(AgentAction action)
    {
        BaseActuator actuator = null;
        
        switch (action.Type)
        {
            case ActionType.Move:
                actuator = GetComponent<MoveComponent>();
                break;
        }
        
        //Check for null actuator meaning it send the wrong order to the wrong agent
        if (PlaybackManager.Instance.IsPositiveTimeScale)
        {
            actuator.ExecuteAction(action.From, action.To);
        }
        else
        { 
            actuator.UndoAction(action.From, action.To);
        }
    }
}
