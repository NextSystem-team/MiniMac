using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // Importando o DOTween

public class PetReaction : MonoBehaviour
{
    [SerializeField] private Text petResponseText;
    [SerializeField] private Animator petReaction;

    [SerializeField] private float textReavelTime = 2f;

    private string responseText;
    private string reaction;

    private string fullResponseText = "";
    private Tween textRevealTween;

    public void SetPetResponse(string response, AnimationClip animation)
    {
        reaction = animation.name;
        fullResponseText = response;
        petResponseText.text = "";
    }

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(fullResponseText)) return;

        textRevealTween?.Kill();

        petResponseText.text = "";

        textRevealTween = petResponseText.DOText(fullResponseText, textReavelTime).SetEase(Ease.Linear);

        if (!string.IsNullOrEmpty(reaction))
        {
            petReaction.Play(reaction);
        }
    }

    private void OnDisable()
    {
        textRevealTween?.Kill();
    }
}