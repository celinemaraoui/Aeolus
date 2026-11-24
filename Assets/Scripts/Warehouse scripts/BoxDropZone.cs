using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxDropZone : MonoBehaviour
{
    [Header("UI & Progression Reference")]
    [SerializeField] private WarehouseUIManager warehouseUIManager;

    [Header("Drop-Off Settings")]
    [SerializeField] private string boxTag = "Box";

    [Header("Next Scene Settings")]
    [SerializeField] private string VesselSceneName = "VesselScene";

    private bool hasTriggeredTransition = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggeredTransition) return;

        if (other.CompareTag(boxTag) || other.GetComponent<BoxManager>() != null)
        {
            hasTriggeredTransition = true;
            Debug.Log($"Box delivered! Loading scene: {VesselSceneName}");

            if (warehouseUIManager != null)
            {
                warehouseUIManager.OnBoxDelivered();
            }

            Destroy(other.gameObject);

            if (!string.IsNullOrWhiteSpace(VesselSceneName))
            {
                SceneManager.LoadScene(VesselSceneName);
            }
            else
            {
                Debug.LogWarning("BoxDropZone: Vessel Scene Name is empty!");
            }
        }
    }
}