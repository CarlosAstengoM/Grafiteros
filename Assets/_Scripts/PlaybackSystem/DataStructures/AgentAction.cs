[System.Serializable]
public struct AgentAction
{
    public GridPosition From;
    public GridPosition To;
    public ActionType Type;

    public AgentAction(GridPosition from, GridPosition to, ActionType type)
    {
        From = from;
        To = to;
        Type = type;
    }
}
