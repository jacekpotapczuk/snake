using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    public Vector2Int Position
    {
        get => position;
        set
        {
            position = value;
            transform.localPosition = new Vector3(value.x, value.y, 0f);
        }
    }

    private Vector2Int position;

    public void LerpInDirection(Vector2Int moveDirection, float t)
    {
        Vector3 a = new Vector3(position.x, position.y, 0f);
        Vector3 dir = new Vector3(moveDirection.x, moveDirection.y, 0f);

        transform.localPosition =  a + t * dir;
    }
}
