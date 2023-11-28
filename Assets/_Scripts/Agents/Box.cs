using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Agents
{
    protected override void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetBoxAtGridPosition(_gridPosition, this);
        transform.position = LevelGrid.Instance.GetWorldPosition(_gridPosition);
    }
}
