using System.Collections;
using UnityEngine;
using TMPro;

public class MaintenanceManager : MonoBehaviour
{
    [Header("Spatial Panel UI References")]
    [SerializeField] private TMP_Text panelTitleText;
    [SerializeField] private TMP_Text panelBodyText;
    [SerializeField] private GameObject showListButton;
    [SerializeField] private TMP_Text timerText;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip alertSound;

    [Header("Teleport Settings")]
    [SerializeField] private Transform xrOriginTransform;      // Drag your XR Origin (XR Rig) here
    [SerializeField] private Transform warehouseSpawnPoint;  // Drag your WarehouseSpawnPoint here
    [SerializeField] private float teleportDelay = 2.0f;     // Seconds to wait after clicking before teleporting

    [Header("Timer & Alert Settings")]
    [SerializeField] private float timeRemaining = 180f;
    [SerializeField] private float startDelay = 2.0f;

    private bool isTimerRunning = true;

    void Start()
    {
        if (showListButton != null) showListButton.SetActive(false);
        SetPanelIdle();
        isTimerRunning = true;
        StartCoroutine(TriggerAlertSequence());
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

    private void SetPanelIdle()
    {
        if (panelTitleText != null) panelTitleText.text = "SYSTEM STATUS: NORMAL";
        if (panelBodyText != null) panelBodyText.text = "All turbine systems operating within normal parameters.";
    }

    private IEnumerator TriggerAlertSequence()
    {
        yield return new WaitForSeconds(startDelay);

        if (panelTitleText != null)
        {
            panelTitleText.text = "<color=red>MAINTENANCE IDENTIFIED</color>";
        }

        if (panelBodyText != null)
        {
            panelBodyText.text = "Inspection required for Turbine #3 Gearbox.\nClick the button below to view manifest & deploy to Warehouse.";
        }

        if (audioSource != null && alertSound != null)
        {
            audioSource.PlayOneShot(alertSound);
        }

        if (showListButton != null)
        {
            showListButton.SetActive(true);
        }
    }

    // Triggered when user clicks the "Show Task List" button on the wall
    public void OnShowListButtonClicked()
    {
        // 1. Show the manifest on the wall screen
        if (panelTitleText != null)
        {
            panelTitleText.text = "<color=yellow>MANIFEST & REDEPLOYING...</color>";
        }

        if (panelBodyText != null)
        {
            panelBodyText.text = "• Gearbox Inspection Endoscope\n" +
                                 "• Synthetic Lubricant (10L)\n" +
                                 "• Calibrated Torque Wrench\n\n" +
                                 "<i>Teleporting to Warehouse...</i>";
        }

        // 2. Hide button
        if (showListButton != null)
        {
            showListButton.SetActive(false);
        }

        // 3. Start teleport sequence
        StartCoroutine(TeleportRoutine());
    }

    private IEnumerator TeleportRoutine()
    {
        // Wait a couple seconds so the user can read the manifest before moving
        yield return new WaitForSeconds(teleportDelay);

        if (xrOriginTransform != null && warehouseSpawnPoint != null)
        {
            CharacterController characterController = xrOriginTransform.GetComponent<CharacterController>();

            // Temporarily disable Character Controller to prevent physics snapping issues
            if (characterController != null)
            {
                characterController.enabled = false;
            }

            // Move player to warehouse spawn point
            xrOriginTransform.position = warehouseSpawnPoint.position;
            xrOriginTransform.rotation = warehouseSpawnPoint.rotation;

            // Re-enable Character Controller
            if (characterController != null)
            {
                characterController.enabled = true;
            }

            Debug.Log("Teleported player to the warehouse!");
        }
        else
        {
            Debug.LogWarning("XR Origin or Warehouse Spawn Point is not assigned in the Inspector!");
        }
    }
}