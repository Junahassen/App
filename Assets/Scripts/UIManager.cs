using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private Text scoreText;
    private Text healthText;
    private Text timerText;
    private Text messageText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        CreateUI();
    }

    private void CreateUI()
    {
        GameObject canvasObject = new GameObject("UI Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasObject.AddComponent<GraphicRaycaster>();

        CreateTextElement("Score Text", new Vector2(10, -10), new Vector2(0, 1), TextAnchor.UpperLeft, out scoreText);
        CreateTextElement("Health Text", new Vector2(10, -50), new Vector2(0, 1), TextAnchor.UpperLeft, out healthText);
        CreateTextElement("Timer Text", new Vector2(-10, -10), new Vector2(1, 1), TextAnchor.UpperRight, out timerText);
        CreateTextElement("Message Text", new Vector2(0, -80), new Vector2(0.5f, 1), TextAnchor.UpperCenter, out messageText);

        UpdateScore(0, 0);
        UpdateHealth(0, 0);
        UpdateTimer(0f);
        ShowMessage("Collect coins and win!");
    }

    private void CreateTextElement(string name, Vector2 anchoredPosition, Vector2 anchor, TextAnchor alignment, out Text text)
    {
        GameObject textObject = new GameObject(name);
        textObject.transform.SetParent(Instance.transform.GetChild(0), false);

        text = textObject.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 28;
        text.alignment = alignment;
        text.color = Color.white;
        text.raycastTarget = false;

        RectTransform rectTransform = text.GetComponent<RectTransform>();
        rectTransform.anchorMin = anchor;
        rectTransform.anchorMax = anchor;
        rectTransform.pivot = anchor;
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(800, 40);
    }

    public void UpdateScore(int score, int target)
    {
        scoreText.text = $"Score: {score}/{target}";
    }

    public void UpdateHealth(int health, int maxHealth)
    {
        healthText.text = $"Health: {health}/{maxHealth}";
    }

    public void UpdateTimer(float timeRemaining)
    {
        timerText.text = $"Time: {Mathf.CeilToInt(timeRemaining)}s";
    }

    public void ShowMessage(string message)
    {
        messageText.text = message;
    }
}
