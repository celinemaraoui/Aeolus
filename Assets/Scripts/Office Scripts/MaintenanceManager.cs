using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MaintenanceManager : MonoBehaviour
{
    [Header("Spatial Panel UI References")]
    [SerializeField] private Image panelBackgroundImage;
    [SerializeField] private GameObject showListButtonObj;

    [Header("UI Sprites (Designs)")]
    [SerializeField] private Sprite normalStateSprite;
    [SerializeField] private Sprite alertStateSprite;
    [SerializeField] private Sprite manifestStateSprite;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip alertSound;

    [Header("Scene Transition Settings")]
    [SerializeField] private string WarehouseSceneName = "WarehouseScene";
    [SerializeField] private float teleportDelay = 2.0f;

    [Header("Timer Settings")]
    [SerializeField] private float startDelay = 2.0f;

    void Start()
    {
        if (showListButtonObj != null)
            showListButtonObj.SetActive(false);

        SetPanelNormal();

        StartCoroutine(TriggerAlertSequence());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadWarehouseScene();
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
        yield return new WaitForSeconds(startDelay);

        if (panelBackgroundImage != null && alertStateSprite != null)
        {
            panelBackgroundImage.sprite = alertStateSprite;
        }

        if (audioSource != null && alertSound != null)
        {
            audioSource.PlayOneShot(alertSound);
        }

        if (showListButtonObj != null)
        {
            showListButtonObj.SetActive(true);
        }
    }

    public void OnShowListButtonClicked()
    {
        if (panelBackgroundImage != null && manifestStateSprite != null)
        {
            panelBackgroundImage.sprite = manifestStateSprite;
        }

        if (showListButtonObj != null)
        {
            showListButtonObj.SetActive(false);
        }

        StartCoroutine(TeleportRoutine());
    }

    private IEnumerator TeleportRoutine()
    {
        yield return new WaitForSeconds(teleportDelay);
        LoadWarehouseScene();
    }

    public void LoadWarehouseScene()
    {
        if (!string.IsNullOrWhiteSpace(WarehouseSceneName))
        {
            Debug.Log($"Loading warehouse scene: {WarehouseSceneName}");
            SceneManager.LoadScene(WarehouseSceneName);
        }
        else
        {
            Debug.LogWarning("MaintenanceManager: Warehouse Scene Name is empty!");
        }
    }
}