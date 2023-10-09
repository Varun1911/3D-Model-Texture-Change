using Cinemachine;
using UnityEngine;

public class ModelPart : MonoBehaviour
{
    [SerializeField] private Material partSelectedMaterial;

    private MeshRenderer meshRenderer;
    private CinemachineVirtualCamera zoomCamera;
    private Material currMaterial;


    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        zoomCamera  = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    public void PartSelected()
    {
        currMaterial = meshRenderer.material;
        meshRenderer.material = partSelectedMaterial;
        zoomCamera.Priority = 11;
    }

    public void DeselectPart()
    {
        zoomCamera.Priority = 1;
        meshRenderer.material = currMaterial;
    }


    public void SetMaterial(Material mat)
    {
        meshRenderer.material = mat;
        currMaterial = mat;
    }
}
