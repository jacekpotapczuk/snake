using System;
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    public Vector2Int Position
    {
        get
        {
            return position;
        }
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

    // public Vector2Int MoveDirection
    // {
    //     get => moveDirection;
    //     set
    //     {
    //         bool isValid = value == Vector2Int.up || value == Vector2Int.down || value == Vector2Int.left ||
    //                        value == Vector2Int.right;
    //         if (!isValid)
    //         {
    //             Debug.LogError("Given move direction is not valid.");
    //             return;
    //         }
    //
    //         moveDirection = value;
    //     }
    // }

    // private Vector2Int moveDirection;

    // public void SetPosition(Vector2Int newPos)
    // {
    //     Position = newPos;
    //     SetPosition(new Vector3(newPos.x, newPos.y, 0f));
    // }
    //
    // private void SetPosition(Vector3 position)
    // {
    //     transform.localPosition = position;
    // }


}
