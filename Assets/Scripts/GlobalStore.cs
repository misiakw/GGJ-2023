public static class GlobalStore
{
    public static GameState GameState = GameState.Died;
}

public enum GameState 
{
    Running,
    Died
}
