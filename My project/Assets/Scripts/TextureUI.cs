using System;
using System.Collections;
using UnityEngine;


public class TextureUI : MonoBehaviour
{
    [SerializeField] private RectTransform panelRectTransform;

    [Header("LeanTween")]
    [SerializeField] private RectTransform scrollArea;
    [SerializeField] private RectTransform leftButton;
    [SerializeField] private RectTransform RightButton;
    [SerializeField] private RectTransform crossButton;

    [Header("Values")]
    [SerializeField] private float buttonScrollAmount = 20f;
    [SerializeField] private float buttonScrollLerpDuration = 0.2f;


    private ModelPart currentPart;
    public static Action OnPartDeselect;

    private void Start()
    {
        ChangeModelPartTexture.OnPartSelected += EnableUI;
    }


    private void OnDestroy()
    {
        ChangeModelPartTexture.OnPartSelected -= EnableUI;
    }


    private void EnableUI(ModelPart part)
    {
        //tweening
        float tweenTime = 1f;
        LeanTween.move(scrollArea, new Vector2(0f, 100f), tweenTime).setEase(LeanTweenType.easeOutCubic);
        LeanTween.move(scrollArea, new Vector2(0f, 50f), tweenTime / 3f).setDelay(tweenTime).setEase(LeanTweenType.easeInCubic);
        LeanTween.move(leftButton, new Vector2(60f, 100f), tweenTime).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.move(leftButton, new Vector2(30f, 100f), tweenTime / 3f).setDelay(tweenTime).setEase(LeanTweenType.easeInCubic);
        LeanTween.move(RightButton, new Vector2(-60f, 100f), tweenTime).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.move(RightButton, new Vector2(-30f, 100f), tweenTime / 3f).setDelay(tweenTime).setEase(LeanTweenType.easeInCubic);
        LeanTween.move(crossButton, new Vector2(80f, -50f), tweenTime).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.move(crossButton, new Vector2(50f, -50f), tweenTime / 3f).setDelay(tweenTime).setEase(LeanTweenType.easeInCubic);


        currentPart = part;
    }


    public void DisableUI()
    {
        //tweening
        float tweenTime = 1f;
        LeanTween.move(scrollArea, new Vector2(0f, 100f), tweenTime / 3f).setEase(LeanTweenType.easeOutCubic);
        LeanTween.move(scrollArea, new Vector2(0f, -280f), tweenTime).setDelay(tweenTime /3f).setEase(LeanTweenType.easeInCubic); 
        LeanTween.move(leftButton, new Vector2(60f, 100f), tweenTime / 3f).setEase(LeanTweenType.easeOutCubic);
        LeanTween.move(leftButton, new Vector2(-130f, 100f), tweenTime).setDelay(tweenTime /3f).setEase(LeanTweenType.easeInCubic);        
        LeanTween.move(RightButton, new Vector2(-60f, 100f), tweenTime / 3f).setEase(LeanTweenType.easeOutCubic);
        LeanTween.move(RightButton, new Vector2(130f, 100f), tweenTime).setDelay(tweenTime /3f).setEase(LeanTweenType.easeInCubic);
        LeanTween.move(crossButton, new Vector2(80f, -50f), tweenTime / 3f).setEase(LeanTweenType.easeOutCubic);
        LeanTween.move(crossButton, new Vector2(-130f, -50f), tweenTime).setDelay(tweenTime /3f).setEase(LeanTweenType.easeInCubic);


        currentPart.DeselectPart();
        OnPartDeselect?.Invoke();
        panelRectTransform.anchoredPosition = Vector2.zero;   //resetting position of scroll rect
    }


    public void RightScrollButton()
    {
        float scrollRectWidth = panelRectTransform.parent.GetComponent<RectTransform>().rect.width;

        //to prevent scrolling if already at the edge
        if (Mathf.Abs(panelRectTransform.anchoredPosition.x - buttonScrollAmount) < panelRectTransform.rect.width - scrollRectWidth)
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
        //to prevent scrolling if already at the edge
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
