using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class PlaybackManager : MonoBehaviour
{
    public static PlaybackManager Instance {get; private set;}

    public Action OnReverseToggled;

    private List<SimulationStep> _stepList = new List<SimulationStep>();

    private float _simulationTimeStamp;
    public float SimulationTimeScale {get; private set;} = 1.0f;
    public bool IsPositiveTimeScale {get; private set;} = true;
    
    private int _currentIndex = 0;
    private bool _isRunning = true;

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
        for (int i = 1; i < LevelGrid.Instance.GetWidth(); i++)
        {
            List<AgentAction> a = new List<AgentAction>();
            a.Add(new AgentAction(new GridPosition(0,i-1),new GridPosition(0,i),ActionType.Move));
            SimulationStep x = new SimulationStep(a);
            _stepList.Add(x);
        }
    }
    
    private void Update()
    {
        if(!_isRunning) return;
        HandleSimulation();
    }

    private void HandleSimulation()
    {
        if (_currentIndex < _stepList.Count && _currentIndex >= 0)
        {
            if (IsPositiveTimeScale && _simulationTimeStamp >= _currentIndex * SimulationParameters.Instance.TurnTime)
            {
                PlayNextStep();
                //StartCoroutine(GetStep(_currentIndex));
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
        List<AgentAction> actions = _stepList[_currentIndex].Actions;

        foreach (AgentAction action in actions)
        {
            Debug.Log(action.From.ToString() + " " + action.To.ToString());
            Agents agent = LevelGrid.Instance.GetUnitAtGridPosition(action.From);
            agent.ExecuteStep(action);
        }
        _currentIndex++;
    }
    
    public void PlayPreviousStep()
    {
        List<AgentAction> actions = _stepList[_currentIndex-1].Actions;

        foreach (AgentAction action in actions)
        {
            Agents agent = LevelGrid.Instance.GetUnitAtGridPosition(action.To);
            agent.ExecuteStep(action);
        }
        _currentIndex--;
    }

    public void TryStartSimulationPython()
    {
        if(_isRunning) return;
        StartCoroutine(StartSimulationPython());
    }

    [ContextMenu("TogglePlay")]
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

    [ContextMenu("TEMP -- REVERSE TIME")]
    public void ToggleTimeScaleSign()
    {
        if (!IsPositiveTimeScale)
        {
            IsPositiveTimeScale = true;
            _currentIndex++;
        }
        else
        {
            IsPositiveTimeScale = false;
            _currentIndex--;
        }
        OnReverseToggled?.Invoke();
    }
    
    [ContextMenu("TEMP -- INCREASE SPEED")]
    public void IncreasePlayBackSeed()
    {
        ChangePlaybackSpeed(2.0f);
    }

    public void ChangePlaybackSpeed(float newSpeed)
    {
        SimulationTimeScale = newSpeed;
    }
    
    private IEnumerator StartSimulationPython()
    {
        string bodyJsonString = "true";
        var request = new UnityWebRequest(SimulationParameters.Instance.ServerURL, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        
        if (request.result == UnityWebRequest.Result.Success)
        {
            _isRunning = true;
        }
        else
        {
            Debug.LogError(request.error);
        }
    }

    private IEnumerator GetStep(int step)
    {
        UnityWebRequest request = UnityWebRequest.Get(SimulationParameters.Instance.ServerURL + "'\'" + step.ToString());
        
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string receivedMessage = request.downloadHandler.text;
            SimulationStep myData = JsonUtility.FromJson<SimulationStep>(receivedMessage);

            _stepList.Add(myData);
            PlayNextStep();
        }
        else
        {
            Debug.LogError(request.error);
        }
    }
}
