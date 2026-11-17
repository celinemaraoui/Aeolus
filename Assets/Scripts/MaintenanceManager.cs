using System.Collections;
using UnityEngine;
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

    [Header("Teleport & Reset Settings")]
    [SerializeField] private Transform xrOriginTransform;
    [SerializeField] private Transform warehouseSpawnPoint;
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
        ResetPlayerPosition();
    }

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