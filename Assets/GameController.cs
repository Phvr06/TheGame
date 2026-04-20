using System;
using UnityEngine;

public static class GameController
{
    public static event Action StateChanged;
    public static event Action<bool, string> GameFinished;

    private static int collectedCount;
    private static int totalCollectibles;
    private static int playerLives;
    private static float remainingTime;
    private static float elapsedTime;
    private static float lostTimeToEnemies;
    private static bool gameOverState;
    private static string finishMessage = string.Empty;

    public static bool GameOver => gameOverState;
    public static bool CanPlayerMove => !gameOverState;
    public static int Score => collectedCount;
    public static int TotalCollectibles => totalCollectibles;
    public static int NextCollectibleOrder => Mathf.Clamp(collectedCount + 1, 1, Mathf.Max(1, totalCollectibles));
    public static int Lives => playerLives;
    public static float RemainingTime => Mathf.Max(0f, remainingTime);
    public static float ElapsedTime => Mathf.Max(0f, elapsedTime);
    public static float LostTimeToEnemies => Mathf.Max(0f, lostTimeToEnemies);
    public static bool ExitUnlocked => collectedCount >= totalCollectibles;

    public static string ObjectiveText
    {
        get
        {
            if (GameOver)
            {
                return finishMessage;
            }

            if (ExitUnlocked)
            {
                return "Saida liberada! Alcance o portal.";
            }

            return "Colete as moedas para abrir a saida.";
        }
    }

    public static void Init(int collectibles, int lives, float levelTime)
    {
        totalCollectibles = Mathf.Max(1, collectibles);
        playerLives = Mathf.Max(1, lives);
        remainingTime = Mathf.Max(15f, levelTime);
        elapsedTime = 0f;
        lostTimeToEnemies = 0f;
        collectedCount = 0;
        gameOverState = false;
        finishMessage = string.Empty;
        NotifyStateChanged();
    }

    public static void Tick(float deltaTime)
    {
        if (gameOverState)
        {
            return;
        }

        elapsedTime += Mathf.Max(0f, deltaTime);
        remainingTime -= Mathf.Max(0f, deltaTime);
        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            EndGame(false, "O tempo acabou.");
            return;
        }

        NotifyStateChanged();
    }

    public static void Collect(int scoreValue, float timeBonus)
    {
        if (gameOverState)
        {
            return;
        }

        collectedCount = Mathf.Clamp(collectedCount + Mathf.Max(1, scoreValue), 0, totalCollectibles);
        remainingTime += Mathf.Max(0f, timeBonus);
        NotifyStateChanged();
    }

    public static bool CanCollectOrder(int orderIndex)
    {
        if (gameOverState)
        {
            return false;
        }

        return Mathf.Max(1, orderIndex) == NextCollectibleOrder;
    }

    public static void DamagePlayer(int damage, float timePenalty)
    {
        if (gameOverState)
        {
            return;
        }

        float appliedPenalty = Mathf.Max(0f, timePenalty);

        playerLives -= Mathf.Max(1, damage);
        remainingTime = Mathf.Max(0f, remainingTime - appliedPenalty);
        lostTimeToEnemies += appliedPenalty;

        if (playerLives <= 0)
        {
            playerLives = 0;
            EndGame(false, "Voce ficou sem vidas.");
            return;
        }

        if (remainingTime <= 0f)
        {
            EndGame(false, "Voce perdeu todo o tempo.");
            return;
        }

        NotifyStateChanged();
    }

    public static bool TryFinishLevel()
    {
        if (gameOverState || !ExitUnlocked)
        {
            return false;
        }

        EndGame(true, $"Fuga concluida em {ElapsedTime:0.0}s.");
        return true;
    }

    private static void EndGame(bool didWin, string message)
    {
        gameOverState = true;
        finishMessage = message;
        NotifyStateChanged();
        GameFinished?.Invoke(didWin, message);
    }

    private static void NotifyStateChanged()
    {
        StateChanged?.Invoke();
    }
}
