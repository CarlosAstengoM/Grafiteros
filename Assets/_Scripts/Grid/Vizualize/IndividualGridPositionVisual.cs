using UnityEngine;

public class IndividualGridPositionVisual : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    public void Hide()
    {
        _meshRenderer.enabled = false;
    }

    public void Show()
    {
        _meshRenderer.enabled = true;
    }
}
