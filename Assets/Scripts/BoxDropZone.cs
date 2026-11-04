using UnityEngine;

public class BoxDropZone : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BoxManager boxManager; // Drag your Box object here
    [SerializeField] private string itemTag = "ToolItem";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(itemTag))
        {
            // Destroy the tool item
            Destroy(other.gameObject);

            // Tell the box manager a tool was collected
            if (boxManager != null)
            {
                boxManager.ToolCollected();
            }
        }
    }
}