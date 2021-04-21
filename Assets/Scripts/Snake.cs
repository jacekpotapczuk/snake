using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private GameBoard gameBoard;
    [SerializeField] private Transform snakeReflection;
    [SerializeField] private TrailRenderer tailRenderer;
    [SerializeField] private TrailRenderer tailRendererReflected;

    [SerializeField] private TextMeshProUGUI scoreText;
    
    [Tooltip("How long does it take to move one square.")]
    [SerializeField, Range(0.05f, 1f)] private float moveTime = 0.125f;
    private float timeTillNextMove;
    
    private Vector2Int posOnBoard;
    private Vector2Int reflectedPosOnBoard;
    private Vector2Int moveDirection = Vector2Int.right;
    private Vector2Int desiredMoveDirection = Vector2Int.right;

    private int tailLength;
    private const int startingTailLength = 0;
    private const int minTailLength = 0;

    private List<Vector2Int> tailBoardPositions;
    private List<Vector2Int> tailReflectedBoardPositons;

    private bool playerClicked;
    
    private void Awake()
    {
        Restart();
    }

    private void UpdateTailRenderer()
    {
        tailRenderer.time = moveTime * (tailLength);
        tailRendererReflected.time = moveTime * (tailLength);
    }

    private void Update()
    {

        HandleInput();

        if (!playerClicked)
            return;
            
        HandleMovement();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && moveDirection != Vector2Int.down)
        {
            desiredMoveDirection = Vector2Int.up;
            playerClicked = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && moveDirection != Vector2Int.up)
        {
            desiredMoveDirection = Vector2Int.down;
            playerClicked = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && moveDirection != Vector2Int.left)
        {
            desiredMoveDirection = Vector2Int.right;
            playerClicked = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && moveDirection != Vector2Int.right)
        {
            desiredMoveDirection = Vector2Int.left;
            playerClicked = true;
        }
    }

    private void HandleMovement()
    {
        timeTillNextMove -= Time.deltaTime;
        if (timeTillNextMove <= 0)
        {
            posOnBoard += moveDirection;
            reflectedPosOnBoard -= moveDirection;
            moveDirection = desiredMoveDirection;
            
            gameBoard.OnSnakeEnter(posOnBoard + moveDirection, this);
            AddTailBoardPosition(posOnBoard);
   
            timeTillNextMove = moveTime + timeTillNextMove;
        }

        float movePercent = (moveTime - timeTillNextMove) / moveTime;
        transform.localPosition = new Vector3(posOnBoard.x, posOnBoard.y, 0f) + movePercent * new Vector3(moveDirection.x, moveDirection.y, 0f);
        
        snakeReflection.localPosition = new Vector3(reflectedPosOnBoard.x, reflectedPosOnBoard.y, 0f) - movePercent * new Vector3(moveDirection.x, moveDirection.y, 0f);
        

    }

    private void AddTailBoardPosition(Vector2Int boardPos)
    {
        tailBoardPositions.Add(boardPos);

        while (tailBoardPositions.Count > tailLength && tailBoardPositions.Count > 0)
        {
            gameBoard.OnSnakeLeave(tailBoardPositions[0]);
            tailBoardPositions.RemoveAt(0);
        }
        
        tailReflectedBoardPositons.Add(GameBoard.GetReflected(boardPos));
        
        while (tailReflectedBoardPositons.Count > tailLength  && tailReflectedBoardPositons.Count > 0)
        {
            tailReflectedBoardPositons.RemoveAt(0);
        }

        UpdateTailRenderer();
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            for (int i = 0; i < tailBoardPositions.Count; i++)
            {
                Gizmos.DrawSphere(new Vector3(tailBoardPositions[i].x, tailBoardPositions[i].y, 0) + new Vector3(0.5f, 0.5f, 0f), 0.5f);
            }
            
            for (int i = 0; i < tailReflectedBoardPositons.Count; i++)
            {
                Gizmos.DrawSphere(new Vector3(tailReflectedBoardPositons[i].x, tailReflectedBoardPositons[i].y, 0) + new Vector3(0.5f, 0.5f, 0f), 0.5f);
            }
        }

    }

    public void Restart()
    {
        posOnBoard = new Vector2Int(4, 9);
        
        tailBoardPositions = new List<Vector2Int>();
        tailBoardPositions.Add(new Vector2Int(4, 9));
        //tailBoardPositions.Add(new Vector2Int(3, 9));
        //tailBoardPositions.Add(new Vector2Int(2, 9));
        //tailBoardPositions.Add(new Vector2Int(1, 9));

        tailReflectedBoardPositons = new List<Vector2Int>();
        tailReflectedBoardPositons.Add(GameBoard.GetReflected(4, 9));
        //tailReflectedBoardPositons.Add(GameBoard.GetReflected(3, 9));
        //tailReflectedBoardPositons.Add(GameBoard.GetReflected(2, 9));
        //tailReflectedBoardPositons.Add(GameBoard.GetReflected(1, 9));
        
        // TODO: znalezc sposob na reset renderera
        tailRenderer.time = -1f;
        tailRendererReflected.time = -1f;

        reflectedPosOnBoard = GameBoard.GetReflected(posOnBoard);
        moveDirection = Vector2Int.right;
        timeTillNextMove = moveTime;
        desiredMoveDirection = Vector2Int.right;
        tailLength = startingTailLength;
        scoreText.text = tailLength.ToString();

        UpdateTailRenderer();
    }

    public void IncreaseLength()
    {
        tailLength += 1;
        UpdateTailRenderer();
        scoreText.text = tailLength.ToString();

    }

    public void DecreaseLength(int amount)
    {
        tailLength -= amount;
        if (tailLength < minTailLength)
            Debug.Log("GAME OVER LENGHT");
        
        UpdateTailRenderer();
        scoreText.text = tailLength.ToString();
    }

    public Vector2Int GetLastTailBoardPosition(bool reflectedTail)
    {
        if (tailBoardPositions.Count == 0 || tailReflectedBoardPositons.Count == 0)
            return new Vector2Int(-1, -1);
        return reflectedTail ? tailReflectedBoardPositons[0] : tailBoardPositions[0];
    }

}
