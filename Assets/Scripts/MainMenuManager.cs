using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string gameSceneName = "Aeolus";

    [Header("UI References")]
    [SerializeField] private GameObject menuContentPanel; // Drag your inner panel/buttons container here
    [SerializeField] private GameObject settingsPanel;     // Drag your Settings panel here

    void Start()
    {
        // Ensure the main menu is visible and settings is hidden when the game starts
        if (menuContentPanel != null)
        {
            menuContentPanel.SetActive(true);
        }

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    public void StartGame()
    {
        Debug.Log("Loading gameplay scene: " + gameSceneName);

        if (menuContentPanel != null)
        {
            menuContentPanel.SetActive(false);
        }

        SceneManager.LoadScene(gameSceneName);
    }

    // Called when clicking Settings
    public void OpenSettings()
    {
        if (menuContentPanel != null) menuContentPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
        Debug.Log("Opening settings panel.");
    }

    // Called when clicking Back from Settings
    public void ReturnToMainMenu()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (menuContentPanel != null) menuContentPanel.SetActive(true);
        Debug.Log("Returning to main menu.");
    }

    // Called when clicking Quit
    public void QuitGame()
    {
        Debug.Log("Quitting application...");
        Application.Quit();
    }
}