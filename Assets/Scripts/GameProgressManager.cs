using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance { get; private set; }

    [Header("Global Task Settings")]
    [Tooltip("Total tasks across the entire game (e.g., Office manifest, Warehouse tool list, Pack tools, Deliver box, etc.).")]
    [SerializeField] private int totalGameTasks = 5;
    private int tasksRemaining;

    [Header("Global Timer Settings")]
    [SerializeField] private float timeRemaining = 180f;
    [SerializeField] private bool isTimerRunning = true;

    [Header("Optional Scene HUD References (Auto-found if left empty)")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text tasksText;

    [Header("HUD Object Names (Used when auto-finding across scenes)")]
    [SerializeField] private string timerObjectName = "Timer";
    [SerializeField] private string tasksObjectName = "Tasks";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        tasksRemaining = totalGameTasks;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    void Start()
    {
        BindSceneHUDReferences();
        RefreshHUD();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BindSceneHUDReferences();
        RefreshHUD();
    }

    void Update()
    {
        if (!isTimerRunning) return;

        if (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0f) timeRemaining = 0f;
            DisplayTime(timeRemaining);
        }
        else
        {
            isTimerRunning = false;
            DisplayTime(0f);
        }
    }

    public void CompleteTask(int amount = 1)
    {
        if (amount <= 0) return;

        tasksRemaining = Mathf.Max(0, tasksRemaining - amount);
        UpdateTaskUI();
        Debug.Log($"Global task completed! Tasks remaining across game: {tasksRemaining}");
    }

    public void SetTotalGameTasks(int newTotal)
    {
        totalGameTasks = Mathf.Max(0, newTotal);
        tasksRemaining = totalGameTasks;
        UpdateTaskUI();
    }

    public void SetTimerRunning(bool running)
    {
        isTimerRunning = running;
    }

    public int GetTasksRemaining() => tasksRemaining;
    public float GetTimeRemaining() => timeRemaining;

    private void RefreshHUD()
    {
        DisplayTime(timeRemaining);
        UpdateTaskUI();
    }

    private void DisplayTime(float timeToDisplay)
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timeToDisplay / 60f);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60f);
        timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
    }

    private void UpdateTaskUI()
    {
        if (tasksText == null) return;

        tasksText.text = "Tasks Remaining: " + tasksRemaining;
    }

    private void BindSceneHUDReferences()
    {
        if (timerText == null && !string.IsNullOrWhiteSpace(timerObjectName))
        {
            GameObject timerObj = GameObject.Find(timerObjectName);
            if (timerObj != null) timerText = timerObj.GetComponent<TMP_Text>();
        }

        if (tasksText == null && !string.IsNullOrWhiteSpace(tasksObjectName))
        {
            GameObject tasksObj = GameObject.Find(tasksObjectName);
            if (tasksObj != null) tasksText = tasksObj.GetComponent<TMP_Text>();
        }
    }
}