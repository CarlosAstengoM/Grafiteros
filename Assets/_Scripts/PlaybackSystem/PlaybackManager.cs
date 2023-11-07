using System;
using System.Collections.Generic;
using UnityEngine;

public class PlaybackManager : MonoBehaviour
{
    public static PlaybackManager Instance {get; private set;}

    private List<SimulationStep> _stepList = new List<SimulationStep>();

    private int _currentIndex = 0;

    private float _simulationTimeStamp;

    private void Awake()
    {
        if(Instance != null)
        {
            UnityEngine.Debug.LogError("More than one Playback Manager singleton detected");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        List<AgentAction> a = new List<AgentAction>();
        a.Add(new AgentAction(new GridPosition(0,0),new GridPosition(0,1),ActionType.Move));
        SimulationStep x = new SimulationStep(a);
        
        List<AgentAction> b = new List<AgentAction>();
        b.Add(new AgentAction(new GridPosition(0,1),new GridPosition(0,2),ActionType.Move));
        SimulationStep y = new SimulationStep(b);

        _stepList.Add(x);
        _stepList.Add(y);
    }

    private void Update()
    {
        if (_currentIndex < _stepList.Count)
        {
            if (_simulationTimeStamp >= _currentIndex * SimulationParameters.Instance.TurnTime)
            {
                PlayNextStep();
            }
            else
            {
                _simulationTimeStamp += Time.deltaTime;
            }
        }
    }

    public void PlayNextStep()
    {
        List<AgentAction> actions = _stepList[_currentIndex].Actions;

        foreach (AgentAction action in actions)
        {
            Agents agent = LevelGrid.Instance.GetUnitAtGridPosition(action.From);
            agent.ExecuteStep(action);
        }
        _currentIndex++;
    }
}
