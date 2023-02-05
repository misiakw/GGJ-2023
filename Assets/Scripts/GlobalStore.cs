using System;
using UnityEngine;
using UnityEngine.InputSystem;

public static class GlobalStore
{
    public static GameState GameState = GameState.Died;
    public static int Score = 0;
    public static Vector3 ObstacleVelocity { get { return new Vector3(-5 + -5 * Score / 100, 0, 0); } }
}

public enum GameState 
{
    Running,
    Died
}
