using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class PlaybackManager : MonoBehaviour
{
    public static PlaybackManager Instance {get; private set;}

    private List<SimulationStep> _stepList = new List<SimulationStep>();

    private float _simulationTimeStamp;
    private float _simulationTimeScale;
    private bool _isPositiveTimeScale = true;
    
    private int _currentIndex = 0;
    private bool _isRunning = false;

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
        if(!_isRunning) return;
        HandleSimulation();

    }

    private void HandleSimulation()
    {
        if (_currentIndex < _stepList.Count && _currentIndex >= 0)
        {
            if (_isPositiveTimeScale && _simulationTimeStamp >= _currentIndex * SimulationParameters.Instance.TurnTime)
            {
                StartCoroutine(GetStep(_currentIndex));
            }
            else if(!_isPositiveTimeScale && _simulationTimeStamp < _currentIndex -1 * SimulationParameters.Instance.TurnTime)
            {
                PlayPreviousStep();
            }
            else
            {
                _simulationTimeStamp += Time.deltaTime * _simulationTimeScale * (_isPositiveTimeScale ? 1 : -1);
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
    
    public void PlayPreviousStep()
    {
        List<AgentAction> actions = _stepList[_currentIndex-1].Actions;

        foreach (AgentAction action in actions)
        {
            Agents agent = LevelGrid.Instance.GetUnitAtGridPosition(action.From);
            agent.ExecuteStep(action);
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

    public void ChangePlaybackSpeed(float newSpeed)
    {
        _simulationTimeScale = newSpeed;
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
