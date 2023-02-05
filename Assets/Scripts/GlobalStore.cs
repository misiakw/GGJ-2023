using System;
using UnityEngine;
using UnityEngine.InputSystem;

public static class GlobalStore
{
    public static GameState GameState = GameState.Loading;

    public static bool ShouldScrollScreen() => GameState == GameState.Running;
    public static int Score = 0;
    public static Vector3 ObstacleVelocity { get { return new Vector3(-6f + -5 * Score / 100, 0, 0); } }
    public static int HighestScore = 0;
}

public enum GameState 
{
    Running,
    Died,
    Loading
}
