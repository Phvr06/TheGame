using UnityEngine;

public static class GameController
{
    private static int collectiblesCounter;

    public static bool gameOver
    {
        get{ return collectiblesCounter <= 0; }
    }

    public static void Init()
    {
        collectiblesCounter = 4;
    }

    public static void Collect()
    {
        collectiblesCounter--;
    }
}
