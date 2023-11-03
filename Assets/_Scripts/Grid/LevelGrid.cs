using UnityEngine;

//? This class depends on changing the script execution order to run before the Default Time
public class LevelGrid : MonoBehaviour
{

    public static LevelGrid Instance {get; private set;}

    [SerializeField] private GameObject _prefab;
    
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private float _cellSize;


    private GridSystem<GridObject> _gridSystem;

    private void Awake()
    {
        if(Instance != null)
        {
            UnityEngine.Debug.LogError("More than one Level Grid singleton detected");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _gridSystem = new GridSystem<GridObject>(_width, _height, _cellSize, (GridSystem<GridObject> grid, GridPosition gridPosition) => new GridObject(grid, gridPosition));
        _gridSystem.CreateDebugObjects(_prefab);
    }

    public void SetUnitAtGridPosition(GridPosition gridPosition, Agents agents)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.SetUnit(agents);
    }

    public Agents GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

    public void ClearUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.SetUnit(null);
    }

    public void UpdateUnitGridPosition(Agents agents, GridPosition previousPosition, GridPosition newPosition)
    {
        ClearUnitAtGridPosition(previousPosition);
        SetUnitAtGridPosition(newPosition,agents);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => _gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => _gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => _gridSystem.IsValidGridPosition(gridPosition);

    public int GetWidth() => _gridSystem.GetWidth();

    public int GetHeight() => _gridSystem.GetHeight();

    public bool HasUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.HasUnit();
    }
}
