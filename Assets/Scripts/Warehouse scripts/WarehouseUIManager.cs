using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarehouseUIManager : MonoBehaviour
{
    public enum WarehouseStage
    {
        Stage2_ShowTools,
        Stage2_CollectTools,
        Stage2_WaitForDone,
        Stage3_DeliverBox,
        Completed
    }

    [Header("Current Stage Tracking")]
    [SerializeField] private WarehouseStage currentStage = WarehouseStage.Stage2_ShowTools;

    [Header("Spatial Panel UI References")]
    [SerializeField] private GameObject toolsListPanel;
    [SerializeField] private GameObject doneButton;
    [SerializeField] private GameObject takeBoxToDropOffPanel;
    [SerializeField] private GameObject dropOffZoneObject;

    [Header("Warehouse Tool Collection Settings")]
    [SerializeField] private int totalToolsRequired = 5;
    private int toolsCollected = 0;

    [Header("Next Scene Transition (Optional Fallback)")]
    [SerializeField] private bool loadOceanSceneOnDelivery = false;
    [SerializeField] private string oceanCutsceneSceneName = "Vessel";
    [SerializeField] private float sceneLoadDelay = 1.5f;

    void Start()
    {
        toolsCollected = 0;

        if (toolsListPanel != null) toolsListPanel.SetActive(false);
        if (doneButton != null) doneButton.SetActive(false);
        if (takeBoxToDropOffPanel != null) takeBoxToDropOffPanel.SetActive(false);
        if (dropOffZoneObject != null) dropOffZoneObject.SetActive(false);
    }

    public void OnShowToolsButtonPressed()
    {
        if (currentStage == WarehouseStage.Stage2_ShowTools)
        {
            if (toolsListPanel != null) toolsListPanel.SetActive(true);
            currentStage = WarehouseStage.Stage2_CollectTools;

            if (GameProgressManager.Instance != null)
            {
                GameProgressManager.Instance.CompleteTask();
            }

            Debug.Log("Warehouse Task Complete: Tool list checked.");
        }
    }

    public void CompleteTask()
    {
        if (currentStage == WarehouseStage.Stage2_CollectTools)
        {
            toolsCollected++;
            Debug.Log($"Tool collected: {toolsCollected}/{totalToolsRequired}");

            if (toolsCollected >= totalToolsRequired)
            {
                currentStage = WarehouseStage.Stage2_WaitForDone;

                if (GameProgressManager.Instance != null)
                {
                    GameProgressManager.Instance.CompleteTask();
                }

                if (doneButton != null) doneButton.SetActive(true);
                Debug.Log("Warehouse Task Complete: All tools packed! 'Done' button unlocked.");
            }
        }
    }

    public void OnDoneButtonPressed()
    {
        if (currentStage == WarehouseStage.Stage2_WaitForDone)
        {
            currentStage = WarehouseStage.Stage3_DeliverBox;

            if (toolsListPanel != null) toolsListPanel.SetActive(false);
            if (doneButton != null) doneButton.SetActive(false);
            if (takeBoxToDropOffPanel != null) takeBoxToDropOffPanel.SetActive(true);
            if (dropOffZoneObject != null) dropOffZoneObject.SetActive(true);
        }
    }

    public void OnBoxDelivered()
    {
        if (currentStage == WarehouseStage.Stage3_DeliverBox)
        {
            currentStage = WarehouseStage.Completed;

            if (GameProgressManager.Instance != null)
            {
                GameProgressManager.Instance.CompleteTask();
            }

            Debug.Log("Warehouse Task Complete: Box delivered to drop-off zone.");

            if (loadOceanSceneOnDelivery && !string.IsNullOrWhiteSpace(oceanCutsceneSceneName))
            {
                StartCoroutine(LoadOceanSceneRoutine());
            }
        }
    }

    private IEnumerator LoadOceanSceneRoutine()
    {
        yield return new WaitForSeconds(sceneLoadDelay);
        SceneManager.LoadScene(oceanCutsceneSceneName);
    }
}