using UnityEngine;

public class GameBoard : MonoBehaviour
{
    
    private bool[,] tiles;

    private const int dim = 20;

    private void Awake()
    {
        tiles = new bool[dim, dim];
        
        // set border tiles to occupied
        for (int i = 0; i < dim; i++)
        {
            tiles[0, i] = true;
            tiles[i, 0] = true;
            tiles[i, dim - 1] = true;
            tiles[dim - 1, i] = true;
        }
        
        
    }

    public bool IsTileOccupied(Vector2Int position)
    {
        if (position.x < 0 || position.x >= dim || position.y < 0 || position.y >= dim)
        {
            Debug.LogError("Given position is outside of the GameBoard.");
            return true;   
        }
        return tiles[position.x, position.y];
    }
    
}
