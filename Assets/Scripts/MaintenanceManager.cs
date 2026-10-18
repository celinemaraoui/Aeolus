using System.Collections;
using UnityEngine;
using TMPro;

public class MaintenanceManager : MonoBehaviour
{
    [Header("Wall Alert Canvas")]
    [SerializeField] private GameObject wallAlertPanel;
    [SerializeField] private TMP_Text wallAlertText;

    [Header("Camera HUD List (Appears on Grab)")]
    [SerializeField] private GameObject hudListPanel;
    [SerializeField] private TMP_Text hudListText;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip alertSound;

    [Header("Timing Settings")]
    [SerializeField] private float startDelay = 2.0f;
    [SerializeField] private float displayDuration = 3.0f;

    void Start()
    {
        if (wallAlertPanel != null) wallAlertPanel.SetActive(false);
        if (hudListPanel != null) hudListPanel.SetActive(false); // Hide HUD list on start

        StartCoroutine(AlertSequenceRoutine());
    }

    private IEnumerator AlertSequenceRoutine()
    {
        yield return new WaitForSeconds(startDelay);

        if (wallAlertPanel != null) wallAlertPanel.SetActive(true);
        if (wallAlertText != null)
        {
            wallAlertText.text = "ALERT: MAINTENANCE IDENTIFIED \n\nProceed to the work table and retrieve your task specification sheet.";
        }

        if (audioSource != null && alertSound != null)
        {
            audioSource.PlayOneShot(alertSound);
        }

        yield return new WaitForSeconds(displayDuration);

        if (wallAlertPanel != null) wallAlertPanel.SetActive(false);
    }

    // Call this event from the XR Grab Interactable on the 3D Clipboard
    public void OnTaskListGrabbed()
    {
        // Show the list attached directly in front of the VR camera
        if (hudListPanel != null)
        {
            hudListPanel.SetActive(true);
        }

        if (hudListText != null)
        {
            hudListText.text = "TASK DESCRIPTION:\n" +
                                 "• Inspect Gearbox in Turbine #3\n\n" +
                                 "VESSEL LOADING MANIFEST:\n" +
                                 "Gearbox Replacement Unit\n" +
                                 "Synthetic Lubricant (10L)\n" +
                                 "Calibration Wrench\n" +
                                 "Safety Harness";
        }
    }
}