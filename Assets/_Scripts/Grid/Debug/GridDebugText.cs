using UnityEngine;
using TMPro;
using TurnBasedStrategy.Grid;

public class GridDebugText : MonoBehaviour
{
    [SerializeField] protected TextMeshPro _text;
    private object _gridObject;

    public virtual void SetGridObject(object gridObject)
    {
        _gridObject = gridObject;
    }

    protected virtual void Update()
    {
        if (PlaybackManager.Instance.InDebugMode)
        {
            _text.text = _gridObject.ToString();
        }
        else
        {
            _text.text = "";
        }
    }
}
