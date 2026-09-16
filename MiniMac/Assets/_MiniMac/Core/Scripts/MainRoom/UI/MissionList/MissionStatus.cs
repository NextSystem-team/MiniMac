using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum MissionState
{
    NotConcluded,
    Concluded
}

public class MissionStatus : MonoBehaviour
{
    [SerializeField] private Sprite concludedTaskImage;
    [SerializeField] private Sprite notConcludedTaskImage;

    [SerializeField] private TextMeshProUGUI missionName;
    [SerializeField] private Image statusImage;

    private float effectStartScale = 1.5f;

    private RectTransform imageRect;

    public MissionState state = MissionState.NotConcluded;

    void Start()
    {
        imageRect = statusImage.GetComponent<RectTransform>();
    }

    public void SwitchState(MissionState state)
    {
        switch (state)
        {
            case MissionState.Concluded:
                StateChangerEffect(concludedTaskImage);
                state = MissionState.Concluded;
                break;
            case MissionState.NotConcluded:
                StateChangerEffect(notConcludedTaskImage);
                state = MissionState.NotConcluded;
                break;
        }
    }

    public void StateChangerEffect(Sprite newStatusSprite)
    {
        statusImage.sprite = newStatusSprite;
        imageRect.localScale = new(effectStartScale, effectStartScale, effectStartScale);

        imageRect.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBack);
    }
}