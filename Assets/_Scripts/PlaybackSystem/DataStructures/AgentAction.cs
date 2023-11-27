using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[System.Serializable]
public class AgentAction
{
    public GridPosition from;
    public GridPosition to;
    [JsonConverter(typeof(StringEnumConverter))]
    public ActionType type;

    public AgentAction(GridPosition from, GridPosition to, ActionType type)
    {
        this.from = from;
        this.to = to;
        this.type = type;
    }
}
