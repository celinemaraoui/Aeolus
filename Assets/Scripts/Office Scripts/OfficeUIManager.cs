using UnityEngine;

public class OfficeUIManager : MonoBehaviour
{
    public enum OfficeStage
    {
        Stage1_Manifest,
        Completed
    }

    [Header("Current Stage Tracking")]
    [SerializeField] private OfficeStage currentStage = OfficeStage.Stage1_Manifest;

    public void OnManifestButtonPressed()
    {
        if (currentStage == OfficeStage.Stage1_Manifest)
        {
            currentStage = OfficeStage.Completed;

            if (GameProgressManager.Instance != null)
            {
                GameProgressManager.Instance.CompleteTask();
            }

            Debug.Log("Office Task Complete: Manifest checked.");
        }
    }
}