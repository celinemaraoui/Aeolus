using UnityEngine;

public class LadderTopTrigger : MonoBehaviour
{
    public UIManager uiManager;
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && (other.CompareTag("Player") || other.GetComponentInParent<Unity.XR.CoreUtils.XROrigin>() != null))
        {
            triggered = true;
            if (uiManager != null) uiManager.OnLadderClimbed();
        }
    }
}