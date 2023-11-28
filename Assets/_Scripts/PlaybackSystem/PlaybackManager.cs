using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TurnBasedStrategy.Grid;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlaybackManager : MonoBehaviour
{
    public static PlaybackManager Instance {get; private set;}

    [SerializeField] private List<SimulationStep> _stepList = new List<SimulationStep>();

    private float _simulationTimeStamp;
    public float SimulationTimeScale {get; private set;} = 1.0f;
    public bool IsPositiveTimeScale {get; private set;} = true;
    
    private int _currentIndex = 0;
    private bool _isRunning = false;
    [NonSerialized] public bool InDebugMode = true;

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
    
    private void Update()
    {
        //Input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (InDebugMode)
            {
                GridPositionsVisualizeSystem.Instance.HideAllGridPositions();
            }
            else
            {
                GridPositionsVisualizeSystem.Instance.ShowAllGridPositions();
            }

            InDebugMode = !InDebugMode;
        }
        
        
        if(!_isRunning) return;
        HandleSimulation();
    }

    private void HandleSimulation()
    {
        if (_currentIndex <= _stepList.Count && _currentIndex >= 0)
        {
            if (IsPositiveTimeScale && _simulationTimeStamp >= _currentIndex * SimulationParameters.Instance.TurnTime)
            {
                PlayNextStep();
                StartCoroutine(GetStep(_stepList.Count));
            }
            else if(!IsPositiveTimeScale && _simulationTimeStamp < _currentIndex * SimulationParameters.Instance.TurnTime)
            {
                PlayPreviousStep();
            }
        }
        _simulationTimeStamp += Time.deltaTime * SimulationTimeScale * (IsPositiveTimeScale ? 1 : -1);
        _simulationTimeStamp = Mathf.Clamp(_simulationTimeStamp, 0.0f, _stepList.Count * SimulationParameters.Instance.TurnTime);
    }

    public void PlayNextStep()
    {
        List<AgentAction> actions = _stepList[_currentIndex].agent_actions;

        foreach (AgentAction action in actions)
        {
            // Spawn box
            if (action.type == ActionType.SPAWN)
            {
                Instantiate(SimulationParameters.Instance.BoxPrefab, LevelGrid.Instance.GetWorldPosition(action.from), Quaternion.identity);
                continue;
            }
            
            Agents agent = LevelGrid.Instance.GetUnitAtGridPosition(action.from);
            agent.ExecuteStep(action);
        }
        
        foreach (AgentAction action in actions)
        {
            // Ignore spawned box
            if (action.type == ActionType.SPAWN)
            {
                continue;
            }
            
            Agents agent = LevelGrid.Instance.GetUnitAtGridPosition(action.from);
            agent.GetComponent<MoveComponent>().UpdatePositionInGrid(action.from,action.to);
        }
        
        _currentIndex++;
    }
    
    public void PlayPreviousStep()
    {
        List<AgentAction> actions = _stepList[_currentIndex-1].agent_actions;

        foreach (AgentAction action in actions)
        {
            // Spawn box
            if (action.type == ActionType.SPAWN)
            {
                Destroy(LevelGrid.Instance.GetUnitAtGridPosition(action.from).gameObject);
                continue;
            }
            
            Agents agent = LevelGrid.Instance.GetUnitAtGridPosition(action.to);
            agent.ExecuteStep(action);
        }
        foreach (AgentAction action in actions)
        {
            // Ignore spawned box
            if (action.type == ActionType.SPAWN)
            {
                continue;
            }
            
            Agents agent = LevelGrid.Instance.GetUnitAtGridPosition(action.to);
            agent.GetComponent<MoveComponent>().UpdatePositionInGrid(action.to,action.from);
        }
        _currentIndex--;
    }

    public void TryStartSimulationPython()
    {
        if(_isRunning) return;
        StartCoroutine(StartSimulationPython());
    }

    public void ToggleSimulation()
    {
        if (_isRunning)
        {
            _isRunning = false;
            Time.timeScale = 0;
        }
        else
        {
            _isRunning = true;
            Time.timeScale = 1;
        }
    }

    private void ToggleTimeScaleSign()
    {
        if (!IsPositiveTimeScale)
        {
            IsPositiveTimeScale = true;
            PlayNextStep();
        }
        else
        {
            IsPositiveTimeScale = false;
            PlayPreviousStep();
        }
    }
    
    public void IncreasePlayBackSeed()
    {
        //Turn it to positive time scale
        if (SimulationTimeScale == 1.0f && !IsPositiveTimeScale)
        {
            ToggleTimeScaleSign();
        }
        else
        {
            if (IsPositiveTimeScale)
            {
                ChangePlaybackSpeed(SimulationTimeScale + 0.5f);
            }
            else
            {
                ChangePlaybackSpeed(SimulationTimeScale - 0.5f);
            }
        }
    }
    
    public void DecreasePlayBackSeed()
    {
        //Turn it to positive time scale
        if (SimulationTimeScale == 1.0f && IsPositiveTimeScale)
        {
            ToggleTimeScaleSign();
        }
        else
        {
            if (IsPositiveTimeScale)
            {
                ChangePlaybackSpeed(SimulationTimeScale - 0.5f);
            }
            else
            {
                ChangePlaybackSpeed(SimulationTimeScale + 0.5f);
            }
        }
    }

    public void ChangePlaybackSpeed(float newSpeed)
    {
        SimulationTimeScale = Mathf.Clamp(newSpeed,1.0f,2.5f);
    }
    
    private IEnumerator StartSimulationPython()
    {
        UnityWebRequest request = new UnityWebRequest(SimulationParameters.Instance.ServerURL + "/start-simulation", "Get");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            //Get First One
            yield return StartCoroutine(GetStep(0));
            //Get Second One
            yield return StartCoroutine(GetStep(1));

            _isRunning = true;
        }
        else
        {
            Debug.LogError(request.error);
        }
    }

    private IEnumerator GetStep(int step)
    {
        UnityWebRequest request = UnityWebRequest.Get(SimulationParameters.Instance.ServerURL + "/simulation-step/" + (step + 1).ToString());
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string receivedMessage = request.downloadHandler.text;
            SimulationStep myData = JsonConvert.DeserializeObject<SimulationStep>(receivedMessage);

            _stepList.Add(myData);
        }
        else
        {
            Debug.LogError(request.error);
        }
    }
}
