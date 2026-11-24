/*using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class ToolInfo : MonoBehaviour
{
    [Header("Tool UI Content")]
    [SerializeField] private string toolName = "Torque Wrench";

    [Header("Optional Popup Reference (Auto-found if empty)")]
    [SerializeField] private ToolInfoPopupUI popupUI;

    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (popupUI == null)
        {
            popupUI = FindFirstObjectByType<ToolInfoPopupUI>();
        }
    }

    void OnEnable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(HandleGrabbed);
        }
    }

    void OnDisable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(HandleGrabbed);
        }
    }

    private void HandleGrabbed(SelectEnterEventArgs args)
    {
        ShowInfo();
    }

    public void ShowInfo()
    {
        if (popupUI == null)
        {
            popupUI = FindFirstObjectByType<ToolInfoPopupUI>();
        }

        if (popupUI != null)
        {
            popupUI.ShowToolInfo(this, toolName);
        }
    }
}*/