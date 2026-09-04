using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum PetState
{
    Idle,
    Happy,
    Curious,
    Listening
}

public class PetController : MonoBehaviour
{
    public PetState CurrentState { get; private set; } = PetState.Idle;

    [Header("References")]
    [SerializeField] private PoseListSO poseListSO;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer hatRenderer;
    private PetQuestionManager petQuestionManager;
    [SerializeField] private GameObject petGameObject;
    [SerializeField] private Button questionButton;

    [SerializeField] private StreakCanva streakCanva;

    void Awake()
    {
        spriteRenderer = petGameObject.GetComponent<SpriteRenderer>();
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
                spriteRenderer.sprite = poseListSO.GetPoseByName(PetPoses.Neutral);
                break;
            case PetState.Happy:
                print("Pet is happy!");
                break;
            case PetState.Curious:
                print("Pet is curious!");
                spriteRenderer.sprite = poseListSO.GetPoseByName(PetPoses.Happy);
                questionButton.gameObject.SetActive(true);
                break;
            case PetState.Listening:
                spriteRenderer.sprite = poseListSO.GetPoseByName(PetPoses.Neutral);
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
            spriteRenderer.sprite = poseListSO.GetPoseByName(PetPoses.Happy);

            if (!GameManager.Instance.hasPattedPet)
            {
                GameManager.Instance.hasPattedPet = true;
                GameManager.Instance.playerScore += 10; 
                streakCanva.OpenStreakScreen();
            }else
            {
                GameManager.Instance.playerScore += 1; 
            }

            StartCoroutine(ResetToIdleAfterDelay(2f));
        }
    }

    private IEnumerator ResetToIdleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeState(PetState.Idle);
        petQuestionManager.IncrementPetQuestionChance(15f);
    }
}
