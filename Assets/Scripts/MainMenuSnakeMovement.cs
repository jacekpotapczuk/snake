using UnityEngine;

public class MainMenuSnakeMovement : MonoBehaviour
{
    [SerializeField] private SnakeHead head;
    [SerializeField] private SnakeHead headReflected;
    
    [SerializeField] private SnakeTail tail;
    [SerializeField] private SnakeTail tailReflected;
    
    [Tooltip("How long does it take to move one square.")]
    [SerializeField, Range(0.05f, 1f)] private float moveTime = 0.125f;

    [SerializeField] private  Vector2Int headStartingPos;
    
    [SerializeField] private MainMenuSnakeMove[] moves;
    private int currentMoveIndex;
    
    private float timeTillNextMove;
    
    private Vector2Int moveDirection;

    private bool playerClicked;


    
    private void Start()
    {
        currentMoveIndex = 0;
        head.Position = headStartingPos;
        headReflected.Position = GameBoard.GetReflected(headStartingPos);
        
        tail.Restart(head.Position, moveTime);
        tail.Length = 10;
        tailReflected.Restart(headReflected.Position, moveTime);
        tailReflected.Length = 10;
        
        moveDirection = Vector2Int.zero;
    
        timeTillNextMove = moveTime;
    }

    
    private void Update()
    {
        HandleMovement();
    }
    

    private void HandleMovement()
    {
        timeTillNextMove -= Time.deltaTime;
        if (timeTillNextMove <= 0) // move between tiles
        {
            head.Position += moveDirection;
            headReflected.Position -= moveDirection;

            int index = (currentMoveIndex) % moves.Length;
            moveDirection = moves[index].move;
            moves[(currentMoveIndex) % moves.Length].currentRepetition += 1;
            
            bool shouldBeNextMove = moves[index].currentRepetition >= moves[index].numberOfRepetitions;
            //Debug.Log($"Srpawdzenie {moves[index].currentRepetition} i {moves[index].numberOfRepetitions}.");
            if (shouldBeNextMove)
            {
                currentMoveIndex += 1;
                moves[index].currentRepetition = 0;
            }
            
            timeTillNextMove = moveTime + timeTillNextMove;
            
            tail.AddPositionToTail(head.Position);
            tailReflected.AddPositionToTail(headReflected.Position);
        }

        float t = (moveTime - timeTillNextMove) / moveTime;
        head.LerpInDirection(moveDirection, t);
        headReflected.LerpInDirection(-moveDirection, t);
    }
    

}
