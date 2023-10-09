using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        string selectedMaterialName = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite.name + " Material";
        for(int i = 0; i< textures.Length; i++)
        {
            if(textures[i].name == selectedMaterialName)
            {
                currentPart.SetMaterial(textures[i]);
            }
        }
    }


    private void SetCurrentPart(ModelPart part)
    {
        currentPart = part;
    }
}
