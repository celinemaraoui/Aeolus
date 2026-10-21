using System.Collections;
using UnityEngine;
using TMPro;

public class MaintenanceManager : MonoBehaviour
{
    [Header("Spatial Panel UI References")]
    [SerializeField] private TMP_Text panelTitleText;   // Screen Header
    [SerializeField] private TMP_Text panelBodyText;    // Screen Body
    [SerializeField] private GameObject showListButton; // "Show List" UI Button
    [SerializeField] private TMP_Text timerText;        // Countdown Timer Text

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip alertSound;

    [Header("Timer & Alert Settings")]
    [SerializeField] private float timeRemaining = 180f; // 3 minutes countdown
    [SerializeField] private float startDelay = 2.0f;     // Alert pops up after 2 seconds

    private bool isTimerRunning = true;

    void Start()
    {
        // Hide button on startup and set screen to normal status
        if (showListButton != null) showListButton.SetActive(false);
        SetPanelIdle();

        // Start countdown and alert sequence
        isTimerRunning = true;
        StartCoroutine(TriggerAlertSequence());
    }

    void Update()
    {
        // Run 3-minute countdown timer
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
        // Wait 2 seconds
        yield return new WaitForSeconds(startDelay);

        // 1. Update text to Red Alert
        if (panelTitleText != null)
        {
            panelTitleText.text = "<color=red>⚠️ MAINTENANCE IDENTIFIED ⚠️</color>";
        }

        if (panelBodyText != null)
        {
            panelBodyText.text = "Inspection required for Turbine #3 Gearbox.\nClick the button below to view the loading manifest.";
        }

        // 2. Play alert sound
        if (audioSource != null && alertSound != null)
        {
            audioSource.PlayOneShot(alertSound);
        }

        // 3. Show "Show List" button
        if (showListButton != null)
        {
            showListButton.SetActive(true);
        }
    }

    // Triggered when user clicks/touches the "Show List" button
    public void OnShowListButtonClicked()
    {
        // Show loading manifest directly on screen
        if (panelTitleText != null)
        {
            panelTitleText.text = "<color=yellow>VESSEL LOADING MANIFEST</color>";
        }

        if (panelBodyText != null)
        {
            panelBodyText.text = "• Gearbox Inspection Endoscope\n" +
                                 "• Synthetic Lubricant (10L Container)\n" +
                                 "• Calibrated Torque Wrench & Sockets\n" +
                                 "• Safety Harness & LOTO Kit";
        }

        // Hide button after clicking
        if (showListButton != null)
        {
            showListButton.SetActive(false);
        }
    }
}