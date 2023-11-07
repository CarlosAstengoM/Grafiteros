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
        actuator.ExecuteAction(action.From, action.To);
    }
}
