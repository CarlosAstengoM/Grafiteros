using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DeliveryShelf : Shelf
{
    void Start()
    {
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        transform.position = LevelGrid.Instance.GetWorldPosition(gridPosition);
    }

    public override void TakeBox(GameObject box, bool updatePosition)
    {
        Destroy(box);
    }

    public override GameObject GiveBox()
    {
        return Instantiate(boxPosition.gameObject, boxPosition.position, boxPosition.rotation);
    }
}
