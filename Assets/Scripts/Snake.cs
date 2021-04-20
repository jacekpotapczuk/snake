using System;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private GameBoard gameBoard;
    [SerializeField] private Transform snakeReflection;
    [SerializeField] private TrailRenderer tailRenderer;
    [SerializeField] private TrailRenderer tailRendererReflected;

    
    [Tooltip("How long does it take to move one square.")]
    [SerializeField, Range(0.05f, 1f)] private float moveTime = 0.125f;
    private float timeTillNextMove;
    
    private Vector2Int posOnBoard;
    private Vector2Int reflectedPosOnBoard;
    private Vector2Int moveDirection = Vector2Int.right;
    private Vector2Int desiredMoveDirection = Vector2Int.right;

    private int tailLength;
    private const int startingLength = 4;

    private List<Vector2Int> tailBoardPositions;
    private List<Vector2Int> tailReflectedBoardPositons;
   
    
    private void Awake()
    {
        Restart();
        UpdateTailRenderer();
    }

    private void UpdateTailRenderer()
    {
        tailRenderer.time = moveTime * (tailLength - 1);
        tailRendererReflected.time = moveTime * (tailLength - 1);
    }

    private void Update()
    {

        HandleInput();
        HandleMovement();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && moveDirection != Vector2Int.down)
        {
            desiredMoveDirection = Vector2Int.up;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && moveDirection != Vector2Int.up)
        {
            desiredMoveDirection = Vector2Int.down;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && moveDirection != Vector2Int.left)
        {
            desiredMoveDirection = Vector2Int.right;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && moveDirection != Vector2Int.right)
        {
            desiredMoveDirection = Vector2Int.left;
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
            
            gameBoard.OnSnakeMove(posOnBoard + moveDirection, this);
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

        while (tailBoardPositions.Count > tailLength)
        {
            gameBoard.OnSnakeLeave(tailBoardPositions[0]);
            tailBoardPositions.RemoveAt(0);
        }
        
        tailReflectedBoardPositons.Add(GameBoard.GetReflected(boardPos));
        
        while (tailReflectedBoardPositons.Count > tailLength)
        {
            tailReflectedBoardPositons.RemoveAt(0);
        }
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
        tailBoardPositions.Add(new Vector2Int(3, 9));
        tailBoardPositions.Add(new Vector2Int(2, 9));
        tailBoardPositions.Add(new Vector2Int(1, 9));

        tailReflectedBoardPositons = new List<Vector2Int>();
        tailReflectedBoardPositons.Add(GameBoard.GetReflected(4, 9));
        tailReflectedBoardPositons.Add(GameBoard.GetReflected(3, 9));
        tailReflectedBoardPositons.Add(GameBoard.GetReflected(2, 9));
        tailReflectedBoardPositons.Add(GameBoard.GetReflected(1, 9));
        


        // TODO: znalezc sposob na reset renderera
        tailRenderer.time = -1f;
        tailRendererReflected.time = -1f;

        reflectedPosOnBoard = GameBoard.GetReflected(posOnBoard);
        moveDirection = Vector2Int.right;
        timeTillNextMove = moveTime;
        desiredMoveDirection = Vector2Int.right;
        tailLength = startingLength;

        UpdateTailRenderer();
    }

    public void IncreaseLength()
    {
        tailLength += 1;
        UpdateTailRenderer();
    }

    public void DecreaseLength(bool reflected)
    {
        tailLength -= 1;
        if (tailLength < startingLength)
            Debug.Log("GAME OVER LENGHT");
        
        if(reflected)
            gameBoard.SpawnPoisonedFood(tailReflectedBoardPositons[0]);
        else
            gameBoard.SpawnPoisonedFood(tailBoardPositions[0]);
        UpdateTailRenderer();
        
    }

}
