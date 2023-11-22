using UnityEngine.Serialization;

[System.Serializable]
public class AgentAction
{
    public GridPosition from;
    public GridPosition to;
    public ActionType type;

    public AgentAction(GridPosition from, GridPosition to, ActionType type)
    {
        this.from = from;
        this.to = to;
        this.type = type;
    }
}
