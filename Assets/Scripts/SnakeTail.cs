using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(TrailRenderer))]
public class SnakeTail : MonoBehaviour
{
    
    [SerializeField] private Snake snake;
    
    private TrailRenderer trailRenderer;
    private List<Vector2Int> positions;
    
    public int Length
    {
        get
        {
            return length;
        }
        set
        {
            length = value;
            UpdateTrailRenderer();
        }
    }
    
    private int length;
    private float moveTime;

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        positions = new List<Vector2Int>(); // todo: upewnic sie czy potrzebne

    }
    
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        foreach (Vector2Int pos in positions)
        {
            Gizmos.DrawSphere(new Vector3(pos.x, pos.y, 0), 0.5f);
        }
    }

    public void Restart(Vector2Int startingPos, float moveTime)
    {
        positions = new List<Vector2Int>
        {
            startingPos
        };
        
        this.moveTime = moveTime;
        length = 0;

        // TODO: znalezc sposob na reset renderera, aby nie bylo glitchy
        trailRenderer.time = -1f;

        UpdateTrailRenderer();
    }

    
    public void AddPositionToTail(Vector2Int position)
    {
        positions.Add(position);

        while (positions.Count > length && positions.Count > 0)
        {
            snake.OnTileLeave(positions[0]);
            positions.RemoveAt(0);
        }
        UpdateTrailRenderer();
    }
    
    public Vector2Int GetLastPosition()
    {
        return positions.Count > 0 ? positions[0] : new Vector2Int(-1, -1);
    }
    
    private void UpdateTrailRenderer()
    {
        trailRenderer.time = moveTime * length;
    }
}
