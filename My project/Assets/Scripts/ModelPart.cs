using Cinemachine;
using UnityEngine;

public class ModelPart : MonoBehaviour
{
    [SerializeField] private Material partSelectedMaterial;
    [SerializeField] private Material partBaseMaterial;

    private MeshRenderer meshRenderer;
    private CinemachineVirtualCamera zoomCamera;


    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        zoomCamera  = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    public void PartSelected()
    {
        meshRenderer.material = partSelectedMaterial;
        zoomCamera.Priority = 11;
    }

    public void DeselectPart()
    {
        zoomCamera.Priority = 1;

        if(meshRenderer.material.name == partSelectedMaterial.name + " (Instance)")
        {
            meshRenderer.material = partBaseMaterial;
        }
    }
}
