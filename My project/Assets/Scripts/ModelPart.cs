using Cinemachine;
using UnityEngine;

public class ModelPart : MonoBehaviour
{
    [SerializeField] private Material partSelectedMaterial;
    [SerializeField] private Material[] textures;

    private MeshRenderer meshRenderer;
    private CinemachineVirtualCamera zoomCamera;
    private Material currMaterial;


    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        zoomCamera  = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        string currMaterialName = PlayerPrefs.GetString(gameObject.name, "Base Material");
        for (int i = 0; i < textures.Length; i++)
        {
            if (textures[i].name == currMaterialName)
            {
                meshRenderer.material = textures[i];
            }
        }
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
        PlayerPrefs.SetString(gameObject.name, mat.name);
    }
}
