using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
struct GridPositionList
{
    public List<GridPosition> Positions;

    public GridPositionList(List<GridPosition> list)
    {
        Positions = list;
    }
}

public class AgentSetup : MonoBehaviour
{
    [SerializeField] private GameObject _agentPrefab;
    private bool _isActive = true;

    private List<GridPosition> _gridPositions = new List<GridPosition>();

    private void Update()
    {
        if(!_isActive) return;
        
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        Vector3 position = Mouse.GetPosition();
        if(position.x < 0) return;
        
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(position);
        
        if (Input.GetMouseButtonDown(0))
        {
            if(LevelGrid.Instance.GetUnitAtGridPosition(gridPosition) != null) return;
            Instantiate(_agentPrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
            _gridPositions.Add(gridPosition);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Agents agent = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
            if(agent == null) return;
            Destroy(agent.gameObject);
            _gridPositions.Remove(gridPosition);
        }
    }

    public void SendSimulationParameters()
    {
        StartCoroutine(nameof(SendData));
        _isActive = false;
    }
    
    private IEnumerator SendData()
    {
        GridPositionList gridPositionList = new GridPositionList(_gridPositions);
        string data = JsonUtility.ToJson(gridPositionList);
        Debug.Log(data);
        WWWForm form = new WWWForm();
        form.AddField("bundle", data);
        
        string url = SimulationParameters.Instance.ServerURL +  "/set_positions";

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                PlaybackManager.Instance.TryStartSimulationPython();
            }
            else
            {
                Debug.Log(www.error);
            }
        }
    }
}
