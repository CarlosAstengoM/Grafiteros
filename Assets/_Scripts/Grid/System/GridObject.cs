using UnityEngine;
public class GridObject
{
    private GridSystem<GridObject> _parentGrid;
    private GridPosition _gridPosition;
    private Agent _agent;

    public GridObject(GridSystem<GridObject> parentGrid, GridPosition gridPosition, Agent agent = null)
    {
        _parentGrid = parentGrid;
        _gridPosition = gridPosition;
        _agent = agent;
    }

    public void SetUnit(Agent agent)
    {
        _agent = agent;
    }

    public Agent GetUnit()
    {
        return _agent;
    }

    public bool HasUnit()
    {
        return _agent != null;
    }

    public override string ToString()
    {
        string hasUnit = _agent != null ? "Has Unit" : "";
        return _gridPosition.ToString() + "\n" + hasUnit;
    }
}
