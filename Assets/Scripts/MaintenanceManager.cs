using System.Collections;
using UnityEngine;
using UnityEngine.UI; 

public class MaintenanceManager : MonoBehaviour
{
    [Header("Spatial Panel UI References")]
    [SerializeField] private Image panelBackgroundImage;    // The UI Image showing the screen graphic
    [SerializeField] private GameObject showListButtonObj;   // The interactive "Manifest" button object

    [Header("UI Sprites (Designs)")]
    [SerializeField] private Sprite normalStateSprite;        // Screen 1: System Status Normal
    [SerializeField] private Sprite alertStateSprite;        // Screen 2: Maintenance Identified + Manifest Button
    [SerializeField] private Sprite manifestStateSprite;     // Screen 3: Manifest List & Redeploying

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip alertSound;

    [Header("Teleport & Reset Settings")]
    [SerializeField] private Transform xrOriginTransform;
    [SerializeField] private Transform warehouseSpawnPoint;
    [SerializeField] private float teleportDelay = 2.0f;

    [Header("Timer Settings")]
    [SerializeField] private float startDelay = 2.0f;

    void Start()
    {
        // 1. Hide the button at start
        if (showListButtonObj != null)
            showListButtonObj.SetActive(false);

        // 2. Set initial normal state graphic
        SetPanelNormal();

        // 3. Start alert sequence timer
        StartCoroutine(TriggerAlertSequence());
    }

    void Update()
    {
        // Press the 'P' key anytime during play to instantly reset/teleport back to the warehouse spawn point
        if (Input.GetKeyDown(KeyCode.P))
        {
            ResetPlayerPosition();
        }
    }

    private void SetPanelNormal()
    {
        if (panelBackgroundImage != null && normalStateSprite != null)
        {
            panelBackgroundImage.sprite = normalStateSprite;
        }
    }

    private IEnumerator TriggerAlertSequence()
    {
        // Wait 2 seconds
        yield return new WaitForSeconds(startDelay);

        // 1. Switch graphic to the Alert state (with the button area visible on the design)
        if (panelBackgroundImage != null && alertStateSprite != null)
        {
            panelBackgroundImage.sprite = alertStateSprite;
        }

        // 2. Play warning sound
        if (audioSource != null && alertSound != null)
        {
            audioSource.PlayOneShot(alertSound);
        }

        // 3. Enable the interactive button object over the graphic's button area
        if (showListButtonObj != null)
        {
            showListButtonObj.SetActive(true);
        }
    }

    // Triggered when user clicks/interacts with the button on the panel
    public void OnShowListButtonClicked()
    {
        // 1. Switch graphic to the Manifest & Redeploying state
        if (panelBackgroundImage != null && manifestStateSprite != null)
        {
            panelBackgroundImage.sprite = manifestStateSprite;
        }

        // 2. Hide the button once clicked
        if (showListButtonObj != null)
        {
            showListButtonObj.SetActive(false);
        }

        // 3. Start teleport sequence to warehouse
        StartCoroutine(TeleportRoutine());
    }

    private IEnumerator TeleportRoutine()
    {
        yield return new WaitForSeconds(teleportDelay);
        ResetPlayerPosition();
    }

    // Handles the actual movement safely with the CharacterController
    public void ResetPlayerPosition()
    {
        if (xrOriginTransform != null && warehouseSpawnPoint != null)
        {
            CharacterController cc = xrOriginTransform.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            xrOriginTransform.position = warehouseSpawnPoint.position;
            xrOriginTransform.rotation = warehouseSpawnPoint.rotation;

            if (cc != null) cc.enabled = true;

            Debug.Log("Player position reset to warehouse spawn point.");
        }
        else
        {
            Debug.LogWarning("MaintenanceManager: XR Origin or Warehouse Spawn Point references are missing!");
        }
    }
}