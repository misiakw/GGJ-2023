using System;
using UnityEngine;
using UnityEngine.InputSystem;

public static class GlobalStore
{
    public static ChangeNotified<GameState> State = new ChangeNotified<GameState>(GameState.Loading);
    public static int Score = 0;
    public static bool IsDashing = false;

    //public static ChangeNotified<Vector3>();
    public static Vector3 ObstacleVelocity { get { return new Vector3(-6f + (IsDashing ? -30 : 0) + -5 * Score / 100, 0, 0); } }
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
        _value = starting;
    }
}