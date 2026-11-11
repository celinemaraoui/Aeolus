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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == gameObject) return;

        if (other.CompareTag(toolTag))
        {
            Destroy(other.gameObject);
            ToolCollected();
        }
    }

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
            grabInteractable.enabled = true;
        }

        if (glowingOutlineVisual != null)
            glowingOutlineVisual.SetActive(true);

        Debug.Log("All tools collected! Box is now unlocked and ready to pick up.");
    }
}