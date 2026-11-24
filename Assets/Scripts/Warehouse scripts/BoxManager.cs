using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BoxManager : MonoBehaviour
{
    [Header("Quest Settings")]
    [Tooltip("Total number of tools required before the box can be picked up.")]
    public int totalToolsRequired = 5;
    private int currentToolsCollected = 0;

    [Header("References")]
    [SerializeField] private XRGrabInteractable grabInteractable;
    [SerializeField] private GameObject glowingOutlineVisual;
    [SerializeField] private WarehouseUIManager warehouseUIManager;

    [Header("Detection Settings")]
    [SerializeField] private string toolTag = "Tool";

    void Start()
    {
        if (grabInteractable == null)
            grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
            grabInteractable.enabled = false;

        if (glowingOutlineVisual != null)
            glowingOutlineVisual.SetActive(false);

        if (warehouseUIManager == null)
            warehouseUIManager = FindFirstObjectByType<WarehouseUIManager>(FindObjectsInactive.Include);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == gameObject) return;

        IndividualToolUI individualToolUI = other.GetComponentInParent<IndividualToolUI>();

        if (other.CompareTag(toolTag) || individualToolUI != null)
        {
            if (individualToolUI != null)
                individualToolUI.ForceHidePopup();

            GameObject objectToDestroy = individualToolUI != null ? individualToolUI.gameObject : other.gameObject;
            Destroy(objectToDestroy);
            ToolCollected();
        }
    }

    public void ToolCollected()
    {
        currentToolsCollected++;
        Debug.Log($"Tools inside box: {currentToolsCollected} / {totalToolsRequired}");

        if (warehouseUIManager == null)
            warehouseUIManager = FindFirstObjectByType<WarehouseUIManager>(FindObjectsInactive.Include);

        if (warehouseUIManager != null)
        {
            warehouseUIManager.CompleteTask();
        }

        if (currentToolsCollected >= totalToolsRequired)
        {
            UnlockBoxPickup();
        }
    }

    private void UnlockBoxPickup()
    {
        if (grabInteractable != null)
        {
            grabInteractable.enabled = true;
        }

        if (glowingOutlineVisual != null)
            glowingOutlineVisual.SetActive(true);

        Debug.Log("All tools collected! Box is now unlocked and ready to pick up.");
    }
}