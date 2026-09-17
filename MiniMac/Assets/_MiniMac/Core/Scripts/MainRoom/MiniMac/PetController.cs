using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum PetState
{
    Idle,
    Happy,
    Curious,
    Listening,
    Angry,
    Sad
}

public class PetController : MonoBehaviour
{
    public PetState CurrentState { get; private set; } = PetState.Idle;

    [Header("References")]
    [SerializeField] private PoseListSO poseListSO;
    private Animator petAnimator;
    [SerializeField] private SpriteRenderer hatRenderer;
    private PetQuestionManager petQuestionManager;
    [SerializeField] private GameObject petGameObject;
    [SerializeField] private Button questionButton;

    [SerializeField] private StreakCanva streakCanva;

    void Awake()
    {
        petAnimator = petGameObject.GetComponent<Animator>();
        petQuestionManager = GetComponent<PetQuestionManager>();

        hatRenderer.sprite = GameManager.Instance.currentHat != null ? GameManager.Instance.currentHat.itemRendererSprite : null;
    }

    void Start()
    {
        ChangeState(PetState.Idle);
    }

    public void ChangeState(PetState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case PetState.Idle:
                // 3. TOCAMOS a animação pelo nome exato dela usando petAnimator.Play()
                petAnimator.Play(poseListSO.GetPoseByName(PetPoses.Neutral).poseName); 
                break;
                
            case PetState.Happy:
                print("Pet is happy!");
                petAnimator.Play(poseListSO.GetPoseByName(PetPoses.Happy).poseName);
                break;
            case PetState.Angry:
                print("Pet is angry!");
                petAnimator.Play(poseListSO.GetPoseByName(PetPoses.Angry).poseName);
                break;
            case PetState.Sad:
                print("Pet is sad!");
                petAnimator.Play(poseListSO.GetPoseByName(PetPoses.Sad).poseName);
                break;
            case PetState.Curious:
                print("Pet is curious!");
                petAnimator.Play(poseListSO.GetPoseByName(PetPoses.Curious).poseName);
                questionButton.gameObject.SetActive(true);
                break;
                
            case PetState.Listening:
                petAnimator.Play(poseListSO.GetPoseByName(PetPoses.Neutral).poseName);
                print("Pet is listening!");
                questionButton.gameObject.SetActive(false);
                break;
        }
    }

    public void ReactToPat()
    {
        if (CurrentState == PetState.Idle)
        {
            ChangeState(PetState.Happy);

            if (!GameManager.Instance.hasPattedPet)
            {
                GameManager.Instance.hasPattedPet = true;
                GlobalEvents.ConcludePatMission?.Invoke();
                AudioManager.Instance.PlaySFX(SFXType.Pet_Caress_Success);
                GameManager.Instance.playerScore += 10;
                streakCanva.OpenStreakScreen();
            }
            else
            {
                GameManager.Instance.playerScore += 1;
            }

            StartCoroutine(ResetToIdleAfterDelay(0.8f));
        }
    }

    private IEnumerator ResetToIdleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeState(PetState.Idle);
        petQuestionManager.IncrementPetQuestionChance(15f);
    }
}
