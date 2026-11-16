using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public enum GameStage { Stage2_ShowTools, Stage2_CollectTools, Stage3_DeliverBox, Completed }

    [Header("Current Stage Tracking")]
    [SerializeField] private GameStage currentStage = GameStage.Stage2_ShowTools;

    [Header("HUD References")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text tasksText;

    [Header("Spatial Panel UI References")]
    [SerializeField] private GameObject toolsListPanel;         // Pops up when "Show Tools" is pressed
    [SerializeField] private GameObject doneButton;             // Appears after all tools are collected
    [SerializeField] private GameObject takeBoxToDropOffPanel;  // New UI panel on your spatial screen after pressing "Done"
    [SerializeField] private GameObject dropOffZoneObject;      // The physical drop-off zone in the warehouse

    [Header("Countdown Settings")]
    [SerializeField] private float timeRemaining = 180f; 
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

        // Initialize UI state for Stage 2 start
        if (toolsListPanel != null) toolsListPanel.SetActive(false);
        if (doneButton != null) doneButton.SetActive(false);
        if (takeBoxToDropOffPanel != null) takeBoxToDropOffPanel.SetActive(false);
        if (dropOffZoneObject != null) dropOffZoneObject.SetActive(false);
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

    // --- Flow Control Public Methods ---

    // 1. Press "Show Tools" button
    public void OnShowToolsButtonPressed()
    {
        if (currentStage == GameStage.Stage2_ShowTools)
        {
            if (toolsListPanel != null) toolsListPanel.SetActive(true);
            currentStage = GameStage.Stage2_CollectTools;
            Debug.Log("Tools list shown. Collect tools from the shelf and put them in the box.");
        }
    }

    // Called automatically by BoxManager when a tool is collected
    public void CompleteTask()
    {
        if (currentStage == GameStage.Stage2_CollectTools && currentTasksRemaining > 0)
        {
            currentTasksRemaining--;
            UpdateTaskUI();

            // Once all tools are collected, reveal the "Done" button
            if (currentTasksRemaining <= 0)
            {
                if (doneButton != null) doneButton.SetActive(true);
                Debug.Log("All tools collected! Press the 'Done' button.");
            }
        }
    }

    // 2. Press "Done" button after collecting tools
    public void OnDoneButtonPressed()
    {
        if (currentTasksRemaining <= 0 && currentStage == GameStage.Stage2_CollectTools)
        {
            currentStage = GameStage.Stage3_DeliverBox;

            // Hide the tools list and done button
            if (toolsListPanel != null) toolsListPanel.SetActive(false);
            if (doneButton != null) doneButton.SetActive(false);

            // Show the new instruction UI on your Spatial Panel and activate the drop-off zone
            if (takeBoxToDropOffPanel != null) takeBoxToDropOffPanel.SetActive(true);
            if (dropOffZoneObject != null) dropOffZoneObject.SetActive(true);

            Debug.Log("Drop-off zone and spatial panel prompt are now visible.");
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