using UnityEngine;

public class Mouse : MonoBehaviour
{
    private static Mouse s_instance;

    //* To avoid spelling mistakes a const string was created, use this if ever necessary 
    //? Consider creating a class that holds all the const string names?
    private const string _movableLayerName = "MouseDetection";
    private int _movableLayerMask;

    private void Awake()
    {
        s_instance = this;
       _movableLayerMask = 1 << LayerMask.NameToLayer(_movableLayerName);
    }

    public static Vector3 GetPosition()
    {
        //* Unity now caches camera.main internally
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, s_instance._movableLayerMask);
        return hit.point;
    }
}
