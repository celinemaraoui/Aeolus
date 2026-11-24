using UnityEngine;

public class LadderTopTrigger : MonoBehaviour
{
    public WarehouseUIManager warehouseUIManager;
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && (other.CompareTag("Player") || other.GetComponentInParent<Unity.XR.CoreUtils.XROrigin>() != null))
        {
            triggered = true;
            Debug.Log("Reached top of ladder.");

            if (warehouseUIManager != null)
            {
                warehouseUIManager.OnBoxDelivered();
            }
        }
    }
}