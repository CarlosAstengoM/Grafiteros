using UnityEngine;
public class GridObject
{
    private GridSystem<GridObject> _parentGrid;
    private GridPosition _gridPosition;
    private Agents _agents;

    public GridObject(GridSystem<GridObject> parentGrid, GridPosition gridPosition, Agents agents = null)
    {
        _parentGrid = parentGrid;
        _gridPosition = gridPosition;
        _agents = agents;
    }

    public void SetUnit(Agents agents)
    {
        _agents = agents;
    }

    public Agents GetUnit()
    {
        return _agents;
    }

    public bool HasUnit()
    {
        return _agents != null;
    }

    public override string ToString()
    {
        string hasUnit = _agents != null ? "Has Unit" : "";
        return _gridPosition.ToString() + "\n" + hasUnit;
    }
}
