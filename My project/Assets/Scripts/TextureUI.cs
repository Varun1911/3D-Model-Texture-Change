using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class TextureUI : MonoBehaviour
{
    [SerializeField] private RectTransform panelRectTransform;
    [SerializeField] private float buttonScrollAmount = 20f;
    [SerializeField] private float buttonScrollLerpDuration = 0.2f;


    private ModelPart currentPart;
    public static Action OnPartDeselect;

    private void Start()
    {
        ChangeModelPartTexture.OnPartSelected += EnableUI;
        gameObject.SetActive(false);
    }


    private void OnDestroy()
    {
        ChangeModelPartTexture.OnPartSelected -= EnableUI;
    }


    private void EnableUI(ModelPart part)
    {
        gameObject.SetActive(true);
        currentPart = part;
    }


    public void DisableUI()
    {
        gameObject.SetActive(false);
        currentPart.DeselectPart();
        OnPartDeselect?.Invoke();
        panelRectTransform.anchoredPosition = Vector2.zero;   //resetting position of scroll rect
    }


    public void RightScrollButton()
    {
        float scrollRectWidth = panelRectTransform.parent.GetComponent<RectTransform>().rect.width;

        if(Mathf.Abs(panelRectTransform.anchoredPosition.x - buttonScrollAmount) < panelRectTransform.rect.width - scrollRectWidth)
        {
            StartCoroutine(ButtonScrollLerp(panelRectTransform, -1, buttonScrollAmount));
        }

        else
        {
            StartCoroutine(ButtonScrollLerp(panelRectTransform, -1, panelRectTransform.rect.width - scrollRectWidth - Mathf.Abs(panelRectTransform.anchoredPosition.x)));
        }
    } 
    
    
    public void LeftScrollButton()
    {
        if(panelRectTransform.anchoredPosition.x + buttonScrollAmount < 0)
        {
            StartCoroutine(ButtonScrollLerp(panelRectTransform, 1, buttonScrollAmount));
        }

        else
        {
            StartCoroutine(ButtonScrollLerp(panelRectTransform, 1, Mathf.Abs(panelRectTransform.anchoredPosition.x)));
        }
    }


    //For smoothly scrolling with buttons
    public IEnumerator ButtonScrollLerp(RectTransform panelRect, int direction, float amount)
    {
        float elapsedTime = 0f;
        Vector2 startPosition = panelRect.anchoredPosition;

        while (elapsedTime < buttonScrollLerpDuration)
        {
            float percent = (elapsedTime / buttonScrollLerpDuration);
            elapsedTime += Time.deltaTime;
            panelRect.anchoredPosition = new Vector2(Mathf.Lerp(startPosition.x, startPosition.x + amount * direction, percent), 0);
            yield return null;
        }

        panelRect.anchoredPosition = new Vector2(startPosition.x + amount * direction, 0);
    }
}
