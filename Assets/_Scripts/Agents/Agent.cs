using UnityEngine;

public class Agent : MonoBehaviour
{
    private GridPosition _gridPosition;

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetUnitAtGridPosition(_gridPosition, this);
        transform.position = LevelGrid.Instance.GetWorldPosition(_gridPosition);
    }

    private void Update()
    {
        GridPosition currentGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(_gridPosition != currentGridPosition)
        {
            LevelGrid.Instance.UpdateUnitGridPosition(this,_gridPosition,currentGridPosition);
            _gridPosition = currentGridPosition;
        }
    }

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }
}
