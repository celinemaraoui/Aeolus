using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public enum GameStage
    {
        Stage1_Manifest,
        Stage2_ShowTools,
        Stage2_CollectTools,
        Stage2_WaitForDone,
        Stage3_DeliverBox,
        Stage3_ClimbLadder,
        Completed
    }

    [Header("Current Stage Tracking")]
    [SerializeField] private GameStage currentStage = GameStage.Stage1_Manifest;

    [Header("HUD References")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text tasksText;

    [Header("Spatial Panel UI References")]
    [SerializeField] private GameObject toolsListPanel;         // Pops up when "Show Tools" is pressed
    [SerializeField] private GameObject doneButton;             // Appears after all tools are collected
    [SerializeField] private GameObject takeBoxToDropOffPanel;  // Appears after "Done" is pressed
    [SerializeField] private GameObject dropOffZoneObject;      // The physical drop-off zone in the warehouse

    [Header("Countdown Settings")]
    [SerializeField] private float timeRemaining = 180f;
    [SerializeField] private bool isTimerRunning = true;

    [Header("Overall Milestone Tasks (Total = 5)")]
    [SerializeField] private int totalTasks = 5;
    private int currentTasksRemaining;

    [Header("Warehouse Tool Collection Settings")]
    [SerializeField] private int totalToolsRequired = 5; // Number of tools needed to finish Task 3
    private int toolsCollected = 0;

    void Start()
    {
        currentTasksRemaining = totalTasks;
        toolsCollected = 0;
        isTimerRunning = true;

        UpdateTaskUI();
        DisplayTime(timeRemaining);

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
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        if (timerText == null) return;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
    }

    // --- MILESTONE 1: Press "Manifest" in the Office ---
    public void OnManifestButtonPressed()
    {
        if (currentStage == GameStage.Stage1_Manifest)
        {
            currentStage = GameStage.Stage2_ShowTools;
            DecrementMainTask();
            Debug.Log("Task 1 Complete: Manifest checked.");
        }
    }

    // --- MILESTONE 2: Press "Tool Check" / "Show Tools" in the Warehouse ---
    public void OnShowToolsButtonPressed()
    {
        if (currentStage == GameStage.Stage2_ShowTools)
        {
            if (toolsListPanel != null) toolsListPanel.SetActive(true);
            currentStage = GameStage.Stage2_CollectTools;
            DecrementMainTask();
            Debug.Log("Task 2 Complete: Tool list checked.");
        }
    }

    // --- MILESTONE 3: Put ALL tools in the box (Called by BoxManager per tool) ---
    public void CompleteTask()
    {
        if (currentStage == GameStage.Stage2_CollectTools)
        {
            toolsCollected++;
            Debug.Log($"Tool collected: {toolsCollected}/{totalToolsRequired}");

            // Only decrement the main HUD task once ALL tools are inside the box
            if (toolsCollected >= totalToolsRequired)
            {
                currentStage = GameStage.Stage2_WaitForDone;
                DecrementMainTask();

                if (doneButton != null) doneButton.SetActive(true);
                Debug.Log("Task 3 Complete: All tools packed! 'Done' button unlocked.");
            }
        }
    }

    // --- Press "Done" button to reveal Drop-Off Zone (Does not decrement task counter) ---
    public void OnDoneButtonPressed()
    {
        if (currentStage == GameStage.Stage2_WaitForDone)
        {
            currentStage = GameStage.Stage3_DeliverBox;

            if (toolsListPanel != null) toolsListPanel.SetActive(false);
            if (doneButton != null) doneButton.SetActive(false);
            if (takeBoxToDropOffPanel != null) takeBoxToDropOffPanel.SetActive(true);
            if (dropOffZoneObject != null) dropOffZoneObject.SetActive(true);
        }
    }

    // --- MILESTONE 4: Box placed in the Drop-Off Zone ---
    public void OnBoxDelivered()
    {
        if (currentStage == GameStage.Stage3_DeliverBox)
        {
            currentStage = GameStage.Stage3_ClimbLadder;
            DecrementMainTask();
            Debug.Log("Task 4 Complete: Box delivered to drop-off zone.");
        }
    }

    // --- MILESTONE 5: Reached top of the Wind Turbine Ladder ---
    public void OnLadderClimbed()
    {
        if (currentStage == GameStage.Stage3_ClimbLadder)
        {
            currentStage = GameStage.Completed;
            DecrementMainTask();
            Debug.Log("Task 5 Complete: Climbed the wind turbine!");
        }
    }

    private void DecrementMainTask()
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