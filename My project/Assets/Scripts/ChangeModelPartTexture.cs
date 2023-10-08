using System;
using UnityEngine;

public class ChangeModelPartTexture : MonoBehaviour
{
    public static Action OnPartSelected;

    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began )
            {
                Ray touchRayCast = Camera.main.ScreenPointToRay(touch.position);

                if(Physics.Raycast(touchRayCast, out RaycastHit hitInfo, float.MaxValue, LayerMask.GetMask("Selectable")))
                {
                    hitInfo.transform.gameObject.GetComponent<ModelPart>().PartSelected();
                    OnPartSelected?.Invoke();
                }
            }
        }
    }
}
