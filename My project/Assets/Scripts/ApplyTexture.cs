using UnityEngine;
using UnityEngine.EventSystems;

public class ApplyTexture : MonoBehaviour
{
    [SerializeField] private Material[] textures;

    private ModelPart currentPart;
    
    private void OnEnable()
    {
        ChangeModelPartTexture.OnPartSelected += SetCurrentPart;
    }

    private void OnDisable()
    {
        ChangeModelPartTexture.OnPartSelected -= SetCurrentPart;
    }


    public void ApplyTextureToSelectedPart()
    {
        int pressedButtonNum = int.Parse(EventSystem.current.currentSelectedGameObject.transform.parent.name.Replace("Texture " , string.Empty));
        currentPart.SetMaterial(textures[pressedButtonNum - 1]);
    }


    private void SetCurrentPart(ModelPart part)
    {
        currentPart = part;
    }
}
