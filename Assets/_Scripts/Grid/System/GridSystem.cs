using System;
using UnityEngine;

public class GridSystem<T_GridObject>
{
    private int _width;
    private int _height;
    private float _cellSize;
    private T_GridObject[,] _gridObjects;

    //* Passed a Func to create a instance of the required GridObject since it does not allow to use costume constructor when using generics
    public GridSystem(int width, int height, float size, Func< GridSystem<T_GridObject>, GridPosition, T_GridObject> gridObjectCreator)
    {
        _width = width;
        _height = height;
        _cellSize = size;
        _gridObjects = new T_GridObject[width, height];

        for(int x=0; x < _width; x++)
        {
            for(int z=0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x,z);
                _gridObjects[x,z] = gridObjectCreator(this,gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition position)
    {
        return new Vector3(position.z,0,position.x*-1) * _cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.z/_cellSize) * -1;
        int z = Mathf.RoundToInt(worldPosition.x/_cellSize);
        return new GridPosition(x,z);
    }

    public T_GridObject GetGridObject(GridPosition gridPosition)
    {
        return _gridObjects[gridPosition.x,gridPosition.z];
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 &&
               gridPosition.z >= 0 &&
               gridPosition.x < _width &&
               gridPosition.z < _height;
    }

    public int GetWidth()
    {
        return _width;
    }

    public int GetHeight()
    {
        return _height;
    }

    //* Just for visualizing and debugging purposes
    public void CreateDebugObjects(GameObject textPrefab)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                GameObject debugText = GameObject.Instantiate(textPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugText gridDebugText = debugText.GetComponent<GridDebugText>();
                gridDebugText.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }
}
