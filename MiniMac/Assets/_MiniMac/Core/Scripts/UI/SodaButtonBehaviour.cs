using System;
using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;
using System.Collections;

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
    float easeTime = 0.1f;

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

            StartCoroutine(FixAspectRatio());
        }

        currentSpeed = defaultSpeed;

        defaultScale = rect.localScale;
        pressedScale = defaultScale * 0.95f;
    }

    void Update()
    {
        if (currentMaterial == null) return;
        currentOffset += currentSpeed * Time.deltaTime;

        buttonImage.materialForRendering?.SetFloat(offsetReference, currentOffset);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(SFXType.UI_Button_Click);

        speedTween?.Kill();
        sizeTween?.Kill();
        colorTween?.Kill();

        speedTween = DOTween.To(() => currentSpeed, x => currentSpeed = x, acceleratedSpeed, accelerationTime)
            .SetEase(accelerationEase).SetUpdate(true);

        sizeTween = rect.DOScale(pressedScale, easeTime)
            .SetEase(accelerationEase).SetUpdate(true);

        if (buttonImage == null) return;
        colorTween = buttonImage.DOColor(pressedColor, easeTime)
            .SetEase(accelerationEase).SetUpdate(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        speedTween?.Kill();
        sizeTween?.Kill();
        colorTween?.Kill();

        speedTween = DOTween.To(() => currentSpeed, x => currentSpeed = x, defaultSpeed, accelerationTime)
            .SetEase(Ease.InOutSine).SetUpdate(true);

        sizeTween = rect.DOScale(defaultScale, easeTime)
            .SetEase(Ease.OutBack).SetUpdate(true);

        if (buttonImage == null) return;
        colorTween = buttonImage.DOColor(defaultColor, easeTime)
            .SetEase(Ease.InOutSine).SetUpdate(true);
    }

    private IEnumerator FixAspectRatio()
    {
        yield return new WaitForEndOfFrame();

        if (rect.rect.height > 0)
        {
            Vector2 proportion = new(rect.rect.width / rect.rect.height, 1f);
            currentMaterial.SetVector("_AspectRatio", proportion);

            buttonImage.materialForRendering?.SetVector("_AspectRatio", proportion);
        }
    }
}
