using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
public class UIManager : MonoBehaviour
{
    private readonly List<Image> heartIcons = new();

    [Header("Level Settings")]
    [Tooltip("Tempo inicial da fase, em segundos.")]
    [SerializeField] private float levelTime = 30f;
    [Tooltip("Quantidade maxima de vidas no inicio da fase.")]
    [SerializeField] private int startingLives = 2;

    [Header("HUD References")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private RectTransform livesContainer;
    [SerializeField] private Image heartIconTemplate;
    [SerializeField] private float heartSpacing = 8f;

    [Header("Result Panel References")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TextMeshProUGUI resultTitleText;
    [SerializeField] private TextMeshProUGUI resultBodyText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    private void Awake()
    {
        ConfigureCanvas();
        if (heartIconTemplate != null)
        {
            heartIconTemplate.gameObject.SetActive(false);
        }
        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }
        HookButtons();
    }

    private void Start()
    {
        int collectibleCount = GameObject.FindGameObjectsWithTag("Coletavel").Length;
        GameController.Init(collectibleCount, startingLives, levelTime);
        GameController.StateChanged += RefreshHud;
        GameController.GameFinished += HandleGameFinished;
        RefreshHud();
    }

    private void Update()
    {
        GameController.Tick(Time.deltaTime);
    }

    private void OnDestroy()
    {
        GameController.StateChanged -= RefreshHud;
        GameController.GameFinished -= HandleGameFinished;
    }

    private void ConfigureCanvas()
    {
        if (canvas == null)
        {
            return;
        }

        CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
        if (scaler != null)
        {
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(900f, 600f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
        }
    }

    private void HookButtons()
    {
        if (restartButton != null)
        {
            restartButton.onClick.RemoveListener(RestartLevel);
            restartButton.onClick.AddListener(RestartLevel);
        }

        if (menuButton != null)
        {
            menuButton.onClick.RemoveListener(ReturnToMenu);
            menuButton.onClick.AddListener(ReturnToMenu);
        }
    }

    private void RefreshHud()
    {
        if (scoreText == null || timeText == null || objectiveText == null)
        {
            return;
        }

        scoreText.text = $"Score: {GameController.Score}/{GameController.TotalCollectibles}";
        timeText.text = $"Tempo: {GameController.RemainingTime:0.0}s";
        objectiveText.text = GameController.ObjectiveText;
        RefreshLivesDisplay();
    }

    private void HandleGameFinished(bool didWin, string message)
    {
        if (resultPanel == null || resultTitleText == null || resultBodyText == null)
        {
            return;
        }

        resultPanel.SetActive(true);
        resultTitleText.text = didWin ? "Voce Escapou!" : "Fim de Jogo";
        resultBodyText.text = $"{message}\nScore final: {GameController.Score}/{GameController.TotalCollectibles}\nTempo gasto: {GameController.ElapsedTime:0.0}s\nTempo perdido para inimigos: {GameController.LostTimeToEnemies:0.0}s";

        if (didWin)
        {
            GameAudioManager.PlayWin();
        }
        else
        {
            GameAudioManager.PlayLose();
        }
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void RefreshLivesDisplay()
    {
        if (livesContainer == null || heartIconTemplate == null)
        {
            return;
        }

        int lives = Mathf.Max(0, GameController.Lives);
        EnsureHeartPoolSize(lives);

        float heartWidth = heartIconTemplate.rectTransform.sizeDelta.x;
        float totalWidth = lives > 0 ? (lives * heartWidth) + ((lives - 1) * heartSpacing) : 0f;
        float startX = -totalWidth * 0.5f + (heartWidth * 0.5f);

        for (int i = 0; i < heartIcons.Count; i++)
        {
            bool isVisible = i < lives;
            Image icon = heartIcons[i];
            icon.gameObject.SetActive(isVisible);
            if (!isVisible)
            {
                continue;
            }

            icon.rectTransform.anchoredPosition = new Vector2(startX + (i * (heartWidth + heartSpacing)), -12f);
        }
    }

    private void EnsureHeartPoolSize(int targetCount)
    {
        while (heartIcons.Count < targetCount)
        {
            Image heartIcon = Instantiate(heartIconTemplate, livesContainer);
            heartIcon.gameObject.name = $"Heart {heartIcons.Count + 1}";
            heartIcon.gameObject.SetActive(true);
            heartIcons.Add(heartIcon);
        }
    }
}
