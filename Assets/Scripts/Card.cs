using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class Card : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float flipDuration = 0.3f;
    [SerializeField] private float matchedFadeTime = 0.5f;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private bool isRevealed;
    private bool isMatched;
    private Sprite cardFront;

    public CardType CardType { get; private set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnMouseDown()
    {
        if (!isRevealed && !isMatched && !GameManager.Instance.isChecking)
        {
            Reveal();
        }
    }

    public void Initialize(CardType type)
    {
        CardType = type;
        cardFront = GameManager.Instance.GetSpriteForType(type);
        spriteRenderer.sprite = GameManager.Instance.CardBackSprite;
        ResetCard();
    }

    private void ResetCard()
    {
        isRevealed = false;
        isMatched = false;
        transform.localScale = Vector3.one;
        spriteRenderer.color = Color.white;
        gameObject.SetActive(true);
        boxCollider.enabled = true;
    }

    public void SpawnWithAnimation(Vector3 targetPosition, int index)
    {
        Vector3 startPos = targetPosition + Vector3.up * GridManager.Instance.DropHeight;
        transform.position = startPos;

        DOTween.Sequence()
            .SetDelay(index * 0.1f)
            .Append(transform.DOMove(targetPosition, GridManager.Instance.DropDuration))
            .SetEase(Ease.OutBounce);
    }

    public void Reveal()
    {
        isRevealed = true;
        FlipCard(true);
        GameManager.Instance.CardRevealed(this);
    }

    public void Hide()
    {
        isRevealed = false;
        FlipCard(false);
    }

    private void FlipCard(bool faceUp)
    {
        Sequence flipSequence = DOTween.Sequence();
        flipSequence.Append(transform.DORotate(new Vector3(0, 90, 0), flipDuration / 2))
            .AppendCallback(() =>
            {
                spriteRenderer.sprite = faceUp ? cardFront : GameManager.Instance.CardBackSprite;
            })
            .Append(transform.DORotate(Vector3.zero, flipDuration / 2));
    }

    public void SetMatched()
    {
        isMatched = true;
        isRevealed = true;
        boxCollider.enabled = false;

        Sequence matchSequence = DOTween.Sequence();
        matchSequence.Append(transform.DOScale(Vector3.zero, matchedFadeTime))
            .Join(spriteRenderer.DOFade(0, matchedFadeTime))
            .SetEase(Ease.InBack)
            .OnComplete(() => gameObject.SetActive(false));
    }
}