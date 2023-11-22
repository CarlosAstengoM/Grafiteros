using System;
using System.Collections.Generic;

[System.Serializable]
public class SimulationStep
{
    public List<AgentAction> Actions;

    public SimulationStep(List<AgentAction> actions)
    {
        Actions = actions;
    }
}
