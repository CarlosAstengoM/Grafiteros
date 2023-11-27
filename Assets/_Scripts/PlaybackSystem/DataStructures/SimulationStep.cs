using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

[System.Serializable]
public class SimulationStep
{
    [FormerlySerializedAs("agentActions")] public List<AgentAction> agent_actions;
    [FormerlySerializedAs("OutBoxesNeeded")] public int out_boxes_needed;

    public SimulationStep(List<AgentAction> agentActions, int outBoxesNeeded)
    {
        agent_actions = agentActions;
        out_boxes_needed = outBoxesNeeded;
    }
}
