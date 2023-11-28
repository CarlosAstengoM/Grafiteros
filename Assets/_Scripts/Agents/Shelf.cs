using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Shelf : MonoBehaviour
{
    [SerializeField] public Transform boxPosition;
    private GameObject stored;
    
    void Start()
    {
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        transform.position = LevelGrid.Instance.GetWorldPosition(gridPosition);
    }

    public virtual void TakeBox(GameObject box, bool updatePosition)
    {
        stored = box;
        box.transform.parent = null;
        if (updatePosition)
        {
            Vector3 position = boxPosition.position;
            stored.transform.position = new Vector3(position.x,0,position.z);
        }
    }

    public virtual GameObject GiveBox()
    {
        GameObject temp = stored;
        stored = null;
        return temp;
    }
}
