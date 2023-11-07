using System;
using System.Collections.Generic;

struct SimulationStep
{
    public List<AgentAction> Actions;

    public SimulationStep(List<AgentAction> actions)
    {
        Actions = actions;
    }
}
