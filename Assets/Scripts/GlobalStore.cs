using System;
using UnityEngine;
using UnityEngine.InputSystem;

public static class GlobalStore
{
    public static GameState GameState = GameState.Died;
    public static Vector3 DefaultObstacleVelocity = new Vector3(-5, 0, 0);
}

public enum GameState 
{
    Running,
    Died
}
