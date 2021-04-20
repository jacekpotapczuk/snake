using System;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private GameBoard gameBoard;
    
    [Tooltip("How long does it take to move one square.")]
    [SerializeField, Range(0.05f, 1f)] private float moveTime = 0.125f;
    private float timeTillNextMove;
    
    private Vector2Int posOnBoard;
    private Vector2Int moveDirection = Vector2Int.right;
    private Vector2Int desiredMoveDirection = Vector2Int.right;
    
    private Queue<Vector2Int> tailPositions;
    private int tileLength;
    
    private void Awake()
    {
        posOnBoard = new Vector2Int(2, 9);
        timeTillNextMove = moveTime;
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
            moveDirection = desiredMoveDirection;
            
            if (gameBoard.IsTileOccupied(posOnBoard + moveDirection))
            {
                Debug.Log("Game over");
                posOnBoard = new Vector2Int(2, 9);
                moveDirection = Vector2Int.right;
                timeTillNextMove = moveTime;
                desiredMoveDirection = Vector2Int.right;
                return;
            }
                
            timeTillNextMove = moveTime + timeTillNextMove;
        }

        float movePercent = (moveTime - timeTillNextMove) / moveTime;
        transform.localPosition = new Vector3(posOnBoard.x, posOnBoard.y, 0f) + movePercent * new Vector3(moveDirection.x, moveDirection.y, 0f);
    }
}
