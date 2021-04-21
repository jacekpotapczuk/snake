using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private GameBoard gameBoard;


    [SerializeField] private SnakeHead head;
    [SerializeField] private SnakeHead headReflected;
    
    [SerializeField] private SnakeTail tail;
    [SerializeField] private SnakeTail tailReflected;
    
    [SerializeField] private ScoreManager scoreManager;

    [SerializeField] private GameStateController gameStateController;
    
    [Tooltip("How long does it take to move one square.")]
    [SerializeField, Range(0.05f, 1f)] private float moveTime = 0.125f;
    private float timeTillNextMove;
    
    private Vector2Int moveDirection;
    private Vector2Int desiredMoveDirection;
    
    private bool playerClicked;
    private bool isDead;
    
    private readonly Vector2Int headStartingPos = new Vector2Int(4, 9);
    
    private void Start()
    {
        Restart();
    }
    
    public void Restart()
    {
        AudioManager.Instance.Play("bg");
        isDead = false;
            
        head.Position = headStartingPos;
        headReflected.Position = GameBoard.GetReflected(headStartingPos);
        
        tail.Restart(head.Position, moveTime);
        tailReflected.Restart(headReflected.Position, moveTime);
        
        moveDirection = Vector2Int.right;
        desiredMoveDirection = Vector2Int.right;
        
        timeTillNextMove = moveTime;

        scoreManager.Restart();
    }

    
    private void Update()
    {
        if (isDead)
            return;
        
        
        HandleInput();
        
        // move only when players clicked something
        //if (!playerClicked)
        //    return;
            
        HandleMovement();
    }

    private void HandleInput()
    {
        bool anyKeyDown = false;
        if (Input.GetKeyDown(KeyCode.UpArrow) && moveDirection != Vector2Int.down)
        {
            desiredMoveDirection = Vector2Int.up;
            playerClicked = true;
            anyKeyDown = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && moveDirection != Vector2Int.up)
        {
            desiredMoveDirection = Vector2Int.down;
            playerClicked = true;
            anyKeyDown = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && moveDirection != Vector2Int.left)
        {
            desiredMoveDirection = Vector2Int.right;
            playerClicked = true;
            anyKeyDown = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && moveDirection != Vector2Int.right)
        {
            desiredMoveDirection = Vector2Int.left;
            playerClicked = true;
            anyKeyDown = true;
        }
        if(anyKeyDown)
            AudioManager.Instance.Play("input");
    }

    private void HandleMovement()
    {
        timeTillNextMove -= Time.deltaTime;
        if (timeTillNextMove <= 0) // move between tiles
        {
            head.Position += moveDirection;
            headReflected.Position -= moveDirection;
            
            moveDirection = desiredMoveDirection;
            timeTillNextMove = moveTime + timeTillNextMove;
            
            bool isAlive = gameBoard.OnSnakeEnterTile(head.Position + moveDirection, this, false, tail.GetLastPosition());
            // only move with the reflected part if the first one is not dead to avoid calling Kill multiple times
            if(isAlive)
                gameBoard.OnSnakeEnterTile(headReflected.Position - moveDirection, this, true, tailReflected.GetLastPosition());
            else
            {
                return;
                Debug.Log("DOBRY ELESE");
            }
            tail.AddPositionToTail(head.Position);
            tailReflected.AddPositionToTail(headReflected.Position);
        }

        float t = (moveTime - timeTillNextMove) / moveTime;
        head.LerpInDirection(moveDirection, t);
        headReflected.LerpInDirection(-moveDirection, t);
    }
    
    
    public void OnTileLeave(Vector2Int position)
    {
        gameBoard.OnSnakeLeave(position);
    }

    public void ChangeLength(int amount)
    {
        if (amount > 0)
        {
            AudioManager.Instance.Play("good");
            scoreManager.AddToCurrentScore(amount);
        }
        else if (amount < 0)
            AudioManager.Instance.Play("bad");
        
        if (tail.Length + amount < 0)
        {
            Kill();
            return;
        }
            
        
        tail.Length += amount;
        tailReflected.Length += amount;

        scoreManager.UpdateCurrentLength(tail.Length);
    }

    public void Kill()
    {
        gameStateController.OnSnakeDeath(scoreManager.Score, scoreManager.BestScore);
        scoreManager.SaveScore();
        isDead = true;
    }

}
