using System;
using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;
using System.Collections;

public enum ButtonStates
{
    pressed,
    notPressed
}

public class SodaChangerButtonBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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

    [SerializeField] private Text txt;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
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

        buttonImage.materialForRendering?.SetFloat("_BubbleSize", 0.196f);
        buttonImage.materialForRendering?.SetFloat("_BubbleDensity", 32f);
    }

    void Update()
    {
        if (currentMaterial == null) return;
        currentOffset += currentSpeed * Time.deltaTime;

        buttonImage.materialForRendering?.SetFloat(offsetReference, currentOffset);
    }

    public void SwitchState(ButtonStates state)
    {
        switch (state)
        {
            case ButtonStates.pressed:
                speedTween?.Kill();
                colorTween?.Kill();
                speedTween = DOTween.To(() => currentSpeed, x => currentSpeed = x, acceleratedSpeed, accelerationTime)
                    .SetEase(accelerationEase);
                colorTween = buttonImage.DOColor(pressedColor, easeTime)
                    .SetEase(accelerationEase);
                txt.fontStyle = FontStyle.Normal;
                txt.fontSize = 52;
                button.interactable = false;
                break;
            case ButtonStates.notPressed:
                speedTween?.Kill();
                colorTween?.Kill();
                speedTween = DOTween.To(() => currentSpeed, x => currentSpeed = x, defaultSpeed, accelerationTime)
                    .SetEase(Ease.InOutSine);
                colorTween = buttonImage.DOColor(defaultColor, easeTime)
                    .SetEase(Ease.InOutSine);
                txt.fontStyle = FontStyle.Bold;
                txt.fontSize = 70;
                button.interactable = true;
                break;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!button.interactable) return;
        AudioManager.Instance.PlaySFX(SFXType.UI_Button_Click);

        sizeTween?.Kill();

        sizeTween = rect.DOScale(pressedScale, easeTime)
            .SetEase(accelerationEase);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!button.interactable) return;
        sizeTween?.Kill();

        sizeTween = rect.DOScale(defaultScale, easeTime)
            .SetEase(Ease.OutBack);
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
