using System;
using System.Collections.Generic;

[System.Serializable]
struct SimulationStep
{
    public List<AgentAction> Actions;

    public SimulationStep(List<AgentAction> actions)
    {
        Actions = actions;
    }
}
