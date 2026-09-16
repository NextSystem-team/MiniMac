using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MissionList : MonoBehaviour
{
    [SerializeField] private MissionStatus patMission;
    [SerializeField] private MissionStatus minigameMission;
    [SerializeField] private MissionStatus questionMission;

    [SerializeField] private TextMeshProUGUI text;

    [Header("Menu Positions")]
    [SerializeField] private float closedPosition = -70f;
    [SerializeField] private float openedPosition = -500f;
    [SerializeField] private float easeTime = 0.6f;

    [Header("Text Wave Effect")]
    [SerializeField] private float jumpHeight = 15f;
    [SerializeField] private float jumpTime = 0.25f;
    [SerializeField] private float delayBetweenLetters = 0.05f;

    private bool isOpen = false;
    
    private float[] characterHeights;
    private bool isAnimatingText = false;

    private RectTransform rect;

    void OnEnable()
    {
        GlobalEvents.ConcludePatMission += EndPatMission;
        GlobalEvents.ConcludeMinigameMission += EndMinigameMission;
        GlobalEvents.ConcludeQuestionMission += EndQuestionMission;
    }

    void OnDisable()
    {
        GlobalEvents.ConcludePatMission -= EndPatMission;
        GlobalEvents.ConcludeMinigameMission -= EndMinigameMission;
        GlobalEvents.ConcludeQuestionMission -= EndQuestionMission;
    }

    void Start()
    {
        rect = GetComponent<RectTransform>();

        StartCoroutine(CheckMissionStatus());
    }

    void Update()
    {
        if (!isAnimatingText || characterHeights == null) return;

        text.ForceMeshUpdate();
        var textInfo = text.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            
            if (!charInfo.isVisible) continue;

            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;
            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

            Vector3 offset = new Vector3(0, characterHeights[i], 0);

            vertices[vertexIndex + 0] += offset;
            vertices[vertexIndex + 1] += offset;
            vertices[vertexIndex + 2] += offset;
            vertices[vertexIndex + 3] += offset;
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            text.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }

    public void ToggleMissions()
    {
        if (isOpen)
        {
            CloseMissionList();
        }
        else
        {
            OpenMissionList();
        }
    }

    private void OpenMissionList()
    {
        rect.DOAnchorPosX(openedPosition, easeTime).SetEase(Ease.InOutBack);
        isOpen = true;
    }

    private void CloseMissionList()
    {
        rect.DOAnchorPosX(closedPosition, easeTime).SetEase(Ease.InOutBack);
        isOpen = false;
    }

    public void MissionConcludedEffect()
    {
        text.ForceMeshUpdate();
        int charCount = text.textInfo.characterCount;
        
        characterHeights = new float[charCount];
        isAnimatingText = true;

        DOTween.Kill("MissionTextWave");
        
        Sequence waveSequence = DOTween.Sequence().SetId("MissionTextWave");

        for (int i = 0; i < charCount; i++)
        {
            if (!text.textInfo.characterInfo[i].isVisible) continue;

            int index = i;
            characterHeights[index] = 0f; 

            Tween letterTween = DOTween.To(() => characterHeights[index], x => characterHeights[index] = x, jumpHeight, jumpTime)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);

            waveSequence.Insert(index * delayBetweenLetters, letterTween);
        }

        waveSequence.OnComplete(() => isAnimatingText = false);
    }

    private void EndPatMission()
    {
        MissionConcludedEffect();
        patMission.SwitchState(MissionState.Concluded);
    }

    private void EndMinigameMission()
    {
        MissionConcludedEffect();
        minigameMission.SwitchState(MissionState.Concluded);
    }

    private void EndQuestionMission()
    {
        MissionConcludedEffect();
        questionMission.SwitchState(MissionState.Concluded);
    }

    private IEnumerator CheckMissionStatus()
    {
        yield return new WaitForEndOfFrame();

        if (GameManager.Instance.hasPattedPet) EndPatMission();
        if (GameManager.Instance.hasPlayedMiniGame) EndMinigameMission();
        if (GameManager.Instance.hasAnsweredQuestion) EndQuestionMission();
    }
}