using UnityEngine;

public class BoxDropZone : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string itemTag = "ToolItem"; // The tag assigned to your items

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the box is a tool item
        if (other.CompareTag(itemTag))
        {
            // Optional: Play a success sound or visual particle effect here

            // Make the item disappear
            Destroy(other.gameObject);

            Debug.Log("Item successfully placed in the box!");
        }
    }
}