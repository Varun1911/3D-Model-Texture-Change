using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeModelPartTexture : MonoBehaviour
{
    public static Action<ModelPart> OnPartSelected;

    private bool isPartSelected = false;


    private void OnEnable()
    {
        TextureUI.OnPartDeselect += PartDeselected;
    }

    private void OnDisable()
    {
        TextureUI.OnPartDeselect -= PartDeselected;
    }

    void Update()
    {
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.touchCount > 0 && !isPartSelected)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began )
            {
                Ray touchRayCast = Camera.main.ScreenPointToRay(touch.position);

                if(Physics.Raycast(touchRayCast, out RaycastHit hitInfo, float.MaxValue, LayerMask.GetMask("Selectable")))
                {
                    ModelPart selectedPart = hitInfo.transform.gameObject.GetComponent<ModelPart>();
                    selectedPart.PartSelected();
                    OnPartSelected?.Invoke(selectedPart);
                    isPartSelected = true;
                }
            }
        }
    }


    private void PartDeselected()
    {
        isPartSelected = false;
    }
}
