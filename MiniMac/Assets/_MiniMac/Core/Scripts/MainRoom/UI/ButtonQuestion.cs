using UnityEngine;
using DG.Tweening;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class ButtonQuestion : MonoBehaviour
{
    [Header("Configurações de Flutuação")]
    [SerializeField] private float distanciaFlutuacao = 2f; // Quantos pixels na tela ele sobe
    [SerializeField] private float tempoFlutuacao = 1.5f;

    private float posicaoOriginalY;
    private RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        
        posicaoOriginalY = rect.anchoredPosition.y;
    }

    void OnEnable()
    {
        StartCoroutine(EnableEffect());
    }

    void OnDisable()
    {
        rect.DOKill();
    }

    private void IniciarFlutuacao()
    {
        rect.DOAnchorPosY(posicaoOriginalY + distanciaFlutuacao, tempoFlutuacao)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private IEnumerator EnableEffect()
    {
        yield return new WaitForEndOfFrame();

        rect.localScale = Vector3.one*0.4f;
        rect.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack, 2.5f);

        IniciarFlutuacao();
    }
}