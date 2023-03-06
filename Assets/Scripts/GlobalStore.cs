using System;
using UnityEngine;
using UnityEngine.InputSystem;

public static class GlobalStore
{
    public static ChangeNotified<GameState> State = new ChangeNotified<GameState>(GameState.Loading);
    public static ChangeNotified<int> Score = new ChangeNotified<int>(0);
    public static bool IsDashing = false;

    public static ChangeNotified<Vector3> ObstacleVelocity = new ChangeNotified<Vector3>(PLAYER_STARTUP_SPEED);
    public static Vector3 PLAYER_STARTUP_SPEED => new Vector3(-6, 0, 0);
    public static int HighestScore = 0;
}

public enum GameState
{
    Running,
    Died,
    Loading
}

public class ChangeNotified<T>
{
    public EventHandler<T> Onchange;
    private T _value;
    public T Value
    {
        get => _value;
        set
        {
            _value = value;
            Onchange?.Invoke(this, _value);
        }
    }

    public ChangeNotified(T starting)
    {
        Value = starting;
    }
}