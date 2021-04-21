using System;
using UnityEngine;

[System.Serializable]
public struct MainMenuSnakeMove
{
    public Vector2Int move;
    public int numberOfRepetitions;
    [NonSerialized] public int currentRepetition;

}
