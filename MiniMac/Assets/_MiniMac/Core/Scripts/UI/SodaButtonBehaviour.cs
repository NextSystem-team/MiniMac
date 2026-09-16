using System;
using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

public class SodaButtonBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Image buttonImage;
    private Material currentMaterial;
    private RectTransform rect;

    [Header("Bubble Speed Settings")]
    [SerializeField] float defaultSpeed = 0.05f;
    [SerializeField] float acceleratedSpeed = 0.8f;

    private Vector3 defaultScale;
    private Vector3 pressedScale;
    private Vector3 currentScale;

    private Color32 defaultColor;
    [SerializeField] Color32 pressedColor = Color.red;
    private Color32 currentColor;

    [SerializeField] float accelerationTime = 0.2f;
    [SerializeField] float easeTime = 0.2f;

    [SerializeField] Ease accelerationEase = Ease.OutQuad;

    private float currentSpeed;
    private float currentOffset;
    private Tweener speedTween;
    private Tweener sizeTween;
    private Tweener colorTween;

    private readonly string offsetReference = "_BubbleOffset";

    void Start()
    {
        rect = GetComponent<RectTransform>();

        buttonImage = GetComponent<Image>();

        if (buttonImage != null && buttonImage.material != null)
        {
            currentMaterial = new(buttonImage.material);
            buttonImage.material = currentMaterial;
            defaultColor = buttonImage.color;

            Vector2 proportion = new(rect.rect.width / rect.rect.height, 1f);
            currentMaterial.SetVector("_AspectRatio", proportion);
        }

        currentSpeed = defaultSpeed;

        defaultScale = rect.localScale;
        pressedScale = defaultScale * 0.9f;
    }

    void Update()
    {
        if (currentMaterial == null) return;
        currentOffset += currentSpeed * Time.deltaTime;
        currentMaterial.SetFloat(offsetReference, currentOffset);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(SFXType.UI_Button_Click);

        speedTween?.Kill();
        sizeTween?.Kill();
        colorTween?.Kill();

        speedTween = DOTween.To(() => currentSpeed, x => currentSpeed = x, acceleratedSpeed, accelerationTime)
            .SetEase(accelerationEase);

        sizeTween = rect.DOScale(pressedScale, easeTime)
            .SetEase(accelerationEase);

        if (buttonImage == null) return;
        colorTween = buttonImage.DOColor(pressedColor, easeTime)
            .SetEase(accelerationEase);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        speedTween?.Kill();
        sizeTween?.Kill();
        colorTween?.Kill();

        speedTween = DOTween.To(() => currentSpeed, x => currentSpeed = x, defaultSpeed, accelerationTime)
            .SetEase(Ease.InOutSine);

        sizeTween = rect.DOScale(defaultScale, easeTime)
            .SetEase(Ease.OutBack);

        if (buttonImage == null) return;
        colorTween = buttonImage.DOColor(defaultColor, easeTime)
            .SetEase(Ease.InOutSine);
    }
}
