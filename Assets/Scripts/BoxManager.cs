using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BoxManager : MonoBehaviour
{
    [Header("Quest Settings")]
    [Tooltip("Total number of tools required before the box can be picked up.")]
<<<<<<< Updated upstream
    public int totalToolsRequired = 4; // Public variable requested
=======
    public int totalToolsRequired = 5; // Public variable requested
>>>>>>> Stashed changes
    private int currentToolsCollected = 0;

    [Header("References")]
    [SerializeField] private XRGrabInteractable grabInteractable;
    [SerializeField] private GameObject glowingOutlineVisual; // Optional: visual indicator when ready

    void Start()
    {
        // Automatically grab reference if not assigned
        if (grabInteractable == null)
            grabInteractable = GetComponent<XRGrabInteractable>();

        // Lock the box so it cannot be picked up at the start
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }

        if (glowingOutlineVisual != null)
            glowingOutlineVisual.SetActive(false);
    }

    // Called by your tool drop zone when an item is placed inside
    public void ToolCollected()
    {
        currentToolsCollected++;
        Debug.Log($"Tools inside box: {currentToolsCollected} / {totalToolsRequired}");

        if (currentToolsCollected >= totalToolsRequired)
        {
            UnlockBoxPickup();
        }
    }

    private void UnlockBoxPickup()
    {
        if (grabInteractable != null)
        {
            grabInteractable.enabled = true; // Enables picking up the box!
        }

        if (glowingOutlineVisual != null)
            glowingOutlineVisual.SetActive(true);

        Debug.Log("All tools collected! Box is now unlocked and ready to pick up.");
    }

    // Detect when the box reaches the warehouse drop-off zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WarehouseDropOff"))
        {
            Debug.Log("Quest Complete! Box successfully delivered to the warehouse drop-off point.");
            // Add your win condition, level complete screen, or audio trigger here
            Destroy(gameObject); // Or disable box
        }
    }
}