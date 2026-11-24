/*using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolInfoPopupUI : MonoBehaviour
{
    [Header("Popup UI References (Keep this script on an ALWAYS-ACTIVE parent object)")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private ScrollRect scrollRect;

    private ToolInfo currentActiveTool;

    void Start()
    {
        HidePopup();
    }

    public void ShowToolInfo(ToolInfo sourceTool, string toolName)
    {
        currentActiveTool = sourceTool;

        if (titleText != null)
        {
            titleText.text = toolName;
        }

        if (popupPanel != null)
        {
            popupPanel.SetActive(true);
        }

        if (scrollRect != null)
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 1f;
        }

        Debug.Log($"[ToolInfoPopupUI] Showing popup for: {toolName} ({sourceTool?.name})");
    }

    public void OnToolPlacedInBox(ToolInfo tool)
    {
        Debug.Log($"[ToolInfoPopupUI] OnToolPlacedInBox called with: {tool?.name}, currentActiveTool: {currentActiveTool?.name}");

        if (currentActiveTool == null || tool == null || currentActiveTool == tool)
        {
            HidePopup();
        }
    }

    public void HidePopup()
    {
        currentActiveTool = null;

        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }

        Debug.Log("[ToolInfoPopupUI] Popup hidden.");
    }
}*/