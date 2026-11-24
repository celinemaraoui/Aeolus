using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class IndividualToolUI : MonoBehaviour
{
    [Header("This Tool's Own UI")]
    [SerializeField] private GameObject toolPopupPanel;
    [SerializeField] private ScrollRect scrollRect;

    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Start()
    {
        if (toolPopupPanel != null)
            toolPopupPanel.SetActive(false);
    }

    void OnEnable()
    {
        if (grabInteractable != null)
            grabInteractable.selectEntered.AddListener(HandleGrabbed);
    }

    void OnDisable()
    {
        if (grabInteractable != null)
            grabInteractable.selectEntered.RemoveListener(HandleGrabbed);
    }

    private void HandleGrabbed(SelectEnterEventArgs args)
    {
        ShowPopup();
    }

    public void ShowPopup()
    {
        if (toolPopupPanel == null) return;

        toolPopupPanel.SetActive(true);

        if (scrollRect != null)
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 1f;
        }
    }

    public void HidePopup()
    {
        if (grabInteractable != null && grabInteractable.isSelected)
            return;

        ForceHidePopup();
    }

    public void ForceHidePopup()
    {
        if (toolPopupPanel != null)
            toolPopupPanel.SetActive(false);
    }
}