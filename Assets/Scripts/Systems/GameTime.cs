using UnityEngine;

public static class GameTime
{
    public static bool Pause
    { 
        get
        {
            return _pause;
        }
        set
        {
            _pause = value;
            Time.timeScale = TimeScale;
        }
    }
    private static bool _pause = false;
    public static float TimeScale => _pause ? 0 : 1;
    public static float DeltaTime => Time.deltaTime;
}
