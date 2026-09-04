using UnityEngine;
using UnityEngine.UI;

public class PetReaction : MonoBehaviour
{
    [SerializeField] private Text petResponseText;
    [SerializeField] private Image petSprite;

    public void SetPetResponse(string response, Sprite pose)
    {
        petResponseText.text = response;
        petSprite.sprite = pose;
    }
}
