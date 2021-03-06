using UnityEngine;
using Random = UnityEngine.Random;

public class GameBoard : MonoBehaviour
{

    private BoardTile[,] tiles;

    private const int dim = 20;
    
    [SerializeField] private BoardTile tilePrefab;

    private void Awake()
    {
        tiles = new BoardTile[dim, dim];
        for (int x = 0; x < dim; x++)
        {
            for (int y = 0; y < dim; y++)
            {
                tiles[x, y] = Instantiate(tilePrefab, transform);
                tiles[x, y].transform.localPosition = new Vector3(x, y, 0f);
            }
        }
        Restart();
    }
    
    private void Restart()
    {
        // all tiles become unocupied
        for (int x = 0; x < dim; x++)
        {
            for (int y = 0; y < dim; y++)
            {
                tiles[x, y].SetEmpty();
            }
        }
        
        // set border tiles to blocked
        for (int i = 0; i < dim; i++)
        {
            tiles[0, i].IsBlocked = true;
            tiles[i, 0].IsBlocked = true;
            tiles[i, dim - 1].IsBlocked = true;
            tiles[dim - 1, i].IsBlocked = true;
        }

        SpawnFood();
    }

    // return false if snake is dead
    public bool OnSnakeEnterTile(Vector2Int position, Snake snake, bool isReflected, Vector2Int lastTailPosition)
    {
        if (!IsPositionCorrect(position))
            return false;
        
        BoardTile tile = tiles[position.x, position.y];
        if (tile.IsBlocked)
        {
            snake.Kill();
            Restart();
            return false;
        }
        tile.IsBlocked = true;

        int reflectedInt = isReflected ? -1 : 1;
        bool eatenWrongFood = false;
        
        if (tile.ContainsFoodPoisoned)
        {
            tile.ContainsFoodPoisoned = false;
            snake.ChangeLength(-3);
        }
        else if (tile.ContainsFoodStandard)
        {
            tile.ContainsFoodStandard = false;
            SpawnFoodStandard();
            snake.ChangeLength(reflectedInt);
            eatenWrongFood = reflectedInt == -1;
        }
        else if (tile.ContainsFoodReflected)
        {
            tile.ContainsFoodReflected = false;
            SpawnFoodReflected();
            snake.ChangeLength(-reflectedInt);
            eatenWrongFood = reflectedInt == 1;
        }
        
        if(eatenWrongFood)
            SpawnFoodPoisoned(lastTailPosition);

        return true;
    }

    public void OnSnakeLeaveTile(Vector2Int position)
    {
        if (!IsPositionCorrect(position))
            return;
        
        Vector2Int reflectedPos = GetReflected(position.x, position.y);

        tiles[position.x, position.y].IsBlocked = false;
        tiles[reflectedPos.x, reflectedPos.y].IsBlocked = false;
    }
    
    
    public static Vector2Int GetReflected(Vector2Int position)
    {
        return GetReflected(position.x, position.y);
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

    private void SpawnFood()
    {
        SpawnFoodStandard();
        SpawnFoodReflected();
    }

    private void SpawnFoodStandard()
    {
        BoardTile randomTile = GetRandomEmptyTile();
        randomTile.ContainsFoodStandard = true;
    }
    
    private void SpawnFoodReflected()
    {
        BoardTile randomTile = GetRandomEmptyTile();
        randomTile.ContainsFoodReflected = true;
    }

    private void SpawnFoodPoisoned(Vector2Int position)
    {
        if (position.x < 0 || position.x >= dim || position.y < 0 || position.y >= dim)
            return;
        BoardTile tile = tiles[position.x, position.y];
        
        // make sure that poisoned food doesn't block 
        if (tile.ContainsFoodStandard)
        {
            tile.ContainsFoodStandard = false;
            SpawnFoodStandard();
        }
        
        if (tile.ContainsFoodReflected)
        {
            tile.ContainsFoodReflected = false;
            SpawnFoodReflected();
        }
        
        tiles[position.x, position.y].ContainsFoodPoisoned = true;
    }
    

    private BoardTile GetRandomEmptyTile()
    {
        BoardTile tile = tiles[0, 0];
        while(!tile.IsEmpty)
            tile = tiles[Random.Range(1, dim - 1), Random.Range(1, dim - 1)];
        return tile;
    }
}
