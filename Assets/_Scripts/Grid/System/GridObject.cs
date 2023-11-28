using UnityEngine;
public class GridObject
{
    private GridSystem<GridObject> _parentGrid;
    private GridPosition _gridPosition;
    private Agents _agents;
    private Agents _box;

    public GridObject(GridSystem<GridObject> parentGrid, GridPosition gridPosition, Agents agents = null)
    {
        _parentGrid = parentGrid;
        _gridPosition = gridPosition;
        _agents = agents;
        _box = null;
    }

    public void SetUnit(Agents agents)
    {
        _agents = agents;
    }
    
    public void SetBox(Agents agents)
    {
        _box = agents;
    }

    public Agents GetUnit()
    {
        return _agents;
    }
    
    public Agents GetBox()
    {
        return _box;
    }

    public bool HasUnit()
    {
        return _agents != null;
    }
    
    public bool HasBox()
    {
        return _agents != null;
    }

    public override string ToString()
    {
        string hasUnit = _agents != null ? "Has Unit" : "";        
        string hasBox = _box != null ? "Has Box" : "";
        return _gridPosition.ToString() + "\n" + hasUnit + "\n" + hasBox;
    }
}
