using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{

    private bool[,] tiles;

    private const int dim = 20;

    [SerializeField] private Transform foodStandard;
    [SerializeField] private Transform foodReflected;
    
    [SerializeField] private Transform foodPoisonedPrefab;

    private List<Vector2Int> poisonedFoodBoardPositions;
    
    private void Awake()
    {
        tiles = new bool[dim, dim];
        Restart();

    }

    public bool IsTileBlocked(Vector2Int position)
    {
        if (!IsPositionCorrect(position))
            return true;
        return tiles[position.x, position.y];
    }

    // pick up food if on the tile, returns true if there is food, false otherwise
    // public bool PickUpFoodFrom(Vector2Int position)
    // {
    //     if (!IsPositionCorrect(position))
    //         return false;
    //     if (!tiles[position.x, position.y].containsFood && !tiles[position.x, position.y].containsreflectedFood)
    //         return false;
    //     SpawnFood();
    //     
    //     return true;
    // }

    public void OnSnakeMove(Vector2Int position, Snake snake)
    {
        if (!IsPositionCorrect(position))
            return;

        if (tiles[position.x, position.y])
        {
            snake.Restart();
            Restart();
        }
        
        Vector2Int reflectedPos = GameBoard.GetReflected(position.x, position.y);

        tiles[position.x, position.y] = true;
        tiles[reflectedPos.x, reflectedPos.y] = true;

        if (position.x == foodStandard.localPosition.x && position.y == foodStandard.localPosition.y)
        {
            SpawnFood(foodStandard, foodReflected);
            snake.IncreaseLength();
        }
        else if (position.x == foodReflected.localPosition.x && position.y == foodReflected.localPosition.y)
        {
            SpawnFood(foodReflected, foodStandard);
            snake.DecreaseLength(false);
        }
        else if (reflectedPos.x == foodReflected.localPosition.x && reflectedPos.y == foodReflected.localPosition.y)
        {
            SpawnFood(foodReflected, foodStandard);
            snake.IncreaseLength();
        }
        else if (reflectedPos.x == foodStandard.localPosition.x && reflectedPos.y == foodStandard.localPosition.y)
        {
            SpawnFood(foodStandard, foodReflected);
            snake.DecreaseLength(true);
        }
            
    }

    public void OnSnakeLeave(Vector2Int position)
    {
        if (!IsPositionCorrect(position))
            return;
        
        Vector2Int reflectedPos = GameBoard.GetReflected(position.x, position.y);

        tiles[position.x, position.y] = false;
        tiles[reflectedPos.x, reflectedPos.y] = false;
    }
    
    
    public static Vector2Int GetReflected(Vector2Int position)
    {
        return GameBoard.GetReflected(position.x, position.y);
    }
    
    public static Vector2Int GetReflected(int x, int y)
    {
        Vector2Int posReflected = new Vector2Int();
        posReflected.x = dim - 1 - x;
        posReflected.y = dim - 1 - y;
        
        return posReflected;
    }

    private bool IsPositionCorrect(Vector2Int position)
    {
        if (position.x < 0 || position.x >= dim || position.y < 0 || position.y >= dim)
        {
            Debug.LogError("Given position is outside of the GameBoard.");
            return false;   
        }
        return true;
    }

    private void Restart()
    {
        // all tiles become unocupied
        for (int x = 0; x < dim; x++)
        {
            for (int y = 0; y < dim; y++)
            {
                tiles[x, y] = false;
            }
        }
        
        // set border tiles to occupied
        for (int i = 0; i < dim; i++)
        {
            tiles[0, i] = true;
            tiles[i, 0] = true;
            tiles[i, dim - 1] = true;
            tiles[dim - 1, i] = true;
        }

        poisonedFoodBoardPositions = new List<Vector2Int>();

        SpawnFood();
    }
    
    private void SpawnFood()
    {

        SpawnFood(foodStandard, foodReflected);
        SpawnFood(foodReflected, foodStandard);
    }

    private void SpawnFood(Transform current, Transform other)
    {
        
        Vector3 pos = other.localPosition;
        while (pos == other.localPosition)
            pos = new Vector3(Random.Range(1, dim - 1), Random.Range(1, dim - 1), 0);
        
        current.localPosition = pos;
    }

    public void SpawnPoisonedFood(Vector2Int position)
    {
        poisonedFoodBoardPositions.Add(position);
        Transform t = Instantiate(foodPoisonedPrefab, transform);
        t.localPosition = new Vector3(position.x, position.y, 0f);
    }
}
