using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text tasksText;

    [Header("Countdown Settings")]
    [SerializeField] private float timeRemaining = 180f; // 3 Minutes = 180 Seconds
    [SerializeField] private bool isTimerRunning = true;

    [Header("Task Settings")]
    [SerializeField] private int totalTasks = 5;
    private int currentTasksRemaining;

    void Start()
    {
        currentTasksRemaining = totalTasks;
        isTimerRunning = true;
        UpdateTaskUI();
        DisplayTime(timeRemaining);
    }

    void Update()
    {
        if (!isTimerRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        }
        else
        {
            timeRemaining = 0;
            isTimerRunning = false;
            DisplayTime(0);
            Debug.Log("Time is up!");
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        if (timerText == null) return;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
    }

    public void CompleteTask()
    {
        if (currentTasksRemaining > 0)
        {
            currentTasksRemaining--;
            UpdateTaskUI();
        }
    }

    private void UpdateTaskUI()
    {
        if (tasksText != null)
        {
            tasksText.text = "Tasks Remaining: " + currentTasksRemaining;
        }
    }
}