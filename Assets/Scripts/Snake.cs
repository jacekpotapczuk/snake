using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private GameBoard gameBoard;


    [SerializeField] private SnakeHead head;
    [SerializeField] private SnakeHead headReflected;
    
    [SerializeField] private SnakeTail tail;
    [SerializeField] private SnakeTail tailReflected;
    
    [SerializeField] private TextMeshProUGUI scoreText;
    
    [Tooltip("How long does it take to move one square.")]
    [SerializeField, Range(0.05f, 1f)] private float moveTime = 0.125f;
    private float timeTillNextMove;
    
    // private Vector2Int headPos;
    // private Vector2Int headPosReflected;
    
    private Vector2Int moveDirection = Vector2Int.right;
    private Vector2Int desiredMoveDirection = Vector2Int.right;
    

    private bool playerClicked;

    private readonly Vector2Int headStartingPos = new Vector2Int(4, 9);
    
    private void Awake()
    {
        Restart();
    }
    
    public void Restart()
    {
        head.Position = headStartingPos;
        headReflected.Position = GameBoard.GetReflected(headStartingPos);
        
        moveDirection = Vector2Int.right;
        desiredMoveDirection = Vector2Int.right;
        
        tail.Restart(head.Position, moveTime);
        tailReflected.Restart(headReflected.Position, moveTime);
        
        timeTillNextMove = moveTime;
        
        // todo: score do osobnego pliku?
        scoreText.text = tail.Length.ToString();
    }

    
    private void Update()
    {

        HandleInput();

        // move only when players clicked something
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
        if (timeTillNextMove <= 0) // move between tiles
        {
            head.Position += moveDirection;
            headReflected.Position -= moveDirection;
            
            moveDirection = desiredMoveDirection;
            timeTillNextMove = moveTime + timeTillNextMove;
            
            gameBoard.OnSnakeEnterTile(head.Position + moveDirection, this);
            
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

    public void IncreaseLength()
    {
        tail.Length += 1;
        tailReflected.Length += 1;
        scoreText.text = tail.Length.ToString();

    }

    public void DecreaseLength(int amount)
    {
        tail.Length -= amount;
        tailReflected.Length -= amount;
        
        if (tail.Length < 0)
            Debug.Log("GAME OVER LENGHT");
        
        scoreText.text = tail.Length.ToString();
    }

    public Vector2Int GetLastTailPos(bool reflectedTail)
    {
        return reflectedTail ? tailReflected.GetLastPosition() : tail.GetLastPosition();
    }

}
