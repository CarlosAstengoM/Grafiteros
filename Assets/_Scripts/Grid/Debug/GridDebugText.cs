using UnityEngine;
using TMPro;

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
        _text.text = _gridObject.ToString();
    }
}
