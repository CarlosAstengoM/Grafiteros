using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BoxHolder : MonoBehaviour
{
    public LayerMask ShelfLayerMask;
    public Transform BoxPositionHolder;
    [HideInInspector] public GameObject StoredBox;
}
