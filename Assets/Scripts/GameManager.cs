using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private Card cardPrefab;
    [SerializeField] private int pairsCount = 4;
    [SerializeField] private float matchCheckDelay = 0.5f;

    [Header("Card Sprites")]
    [SerializeField] private Sprite cardBackSprite;
    [SerializeField] private List<CardData> cardFrontSprites;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI movesText;

    private List<Card> cards = new List<Card>();
    private Dictionary<CardType, Sprite> frontSpriteMap;
    private Card firstRevealed;
    private Card secondRevealed;
    public bool isChecking { get; private set; }
    private int score;
    private int moves;

    public Sprite CardBackSprite => cardBackSprite;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeSpriteMap();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartGame();
    }

    private void InitializeSpriteMap()
    {
        frontSpriteMap = new Dictionary<CardType, Sprite>();
        foreach (var data in cardFrontSprites)
        {
            frontSpriteMap[data.type] = data.frontSprite;
        }
    }

    public void StartGame()
    {
        score = 0;
        moves = 0;
        isChecking = false;

        // Clear existing cards
        foreach (var card in cards)
        {
            if (card != null)
                Destroy(card.gameObject);
        }
        cards.Clear();

        // Generate and spawn new cards
        List<CardType> cardTypes = GenerateCardTypes();
        for (int i = 0; i < pairsCount * 2; i++)
        {
            Card card = Instantiate(cardPrefab);
            card.Initialize(cardTypes[i]);
            cards.Add(card);
            card.SpawnWithAnimation(GridManager.Instance.GetGridPosition(i), i);
        }

        UpdateUI();
    }

    private List<CardType> GenerateCardTypes()
    {
        List<CardType> types = new List<CardType>();

        // Add pairs
        for (int i = 0; i < pairsCount; i++)
        {
            CardType type = (CardType)i;
            types.Add(type);
            types.Add(type);
        }

        // shuffle
        for (int i = types.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            CardType temp = types[i];
            types[i] = types[j];
            types[j] = temp;
        }

        return types;
    }

    public Sprite GetSpriteForType(CardType type)
    {
        if (frontSpriteMap.TryGetValue(type, out Sprite sprite))
        {
            return sprite;
        }
        Debug.LogError($"No sprite found: {type}");
        return null;
    }

    public void CardRevealed(Card card)
    {
        if (isChecking) return;

        if (firstRevealed == null)
        {
            firstRevealed = card;
        }
        else
        {
            secondRevealed = card;
            moves++;
            StartCoroutine(CheckMatchRoutine());
        }

        UpdateUI();
    }

    private IEnumerator CheckMatchRoutine()
    {
        isChecking = true;
        yield return new WaitForSeconds(matchCheckDelay);

        if (firstRevealed.CardType == secondRevealed.CardType)
        {
            firstRevealed.SetMatched();
            secondRevealed.SetMatched();
            score++;

            if (score >= pairsCount)
            {
                GameOver();
            }
        }
        else
        {
            firstRevealed.Hide();
            secondRevealed.Hide();
        }

        firstRevealed = null;
        secondRevealed = null;
        isChecking = false;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
        if (movesText != null)
            movesText.text = $"Moves: {moves}";
    }

    private void GameOver()
    {
        Debug.Log($"Game Over! You won in {moves} moves!");
    }
}