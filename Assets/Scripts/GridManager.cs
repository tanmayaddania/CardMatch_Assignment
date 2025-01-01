using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [Header("Grid Settings")]
    [SerializeField] private Vector2 startPosition = new Vector2(-2f, 2f);
    [SerializeField] private float cardSpacing = 2f;
    [SerializeField] private float dropHeight = 5f;
    [SerializeField] private float dropDuration = 0.5f;
    [SerializeField] private int gridSize;

    public float DropHeight => dropHeight;
    public float DropDuration => dropDuration;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Vector3 GetGridPosition(int index)
    {
        int row = index / gridSize;
        int col = index % gridSize;

        return new Vector3(
            startPosition.x + (col * cardSpacing),
            startPosition.y - (row * cardSpacing),
            0
        );
    }
}