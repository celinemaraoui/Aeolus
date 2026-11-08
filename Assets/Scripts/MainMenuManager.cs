using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("The exact name of your main gameplay scene.")]
    [SerializeField] private string gameSceneName = "BasicScene";

    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuCanvasObject; // Drag your main menu Canvas/Panel here
    [SerializeField] private GameObject settingsPanel;

    // Called when clicking the Start Game button
    public void StartGame()
    {
        Debug.Log("Loading gameplay scene: " + gameSceneName);

        // Hide the main menu instantly
        if (mainMenuCanvasObject != null)
        {
            mainMenuCanvasObject.SetActive(false);
        }

        // Load the gameplay scene
        SceneManager.LoadScene(gameSceneName);
    }

    // Called when clicking Settings
    public void OpenSettings()
    {
        if (mainMenuCanvasObject != null) mainMenuCanvasObject.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
        Debug.Log("Opening settings panel.");
    }

    // Called when clicking Back from Settings
    public void ReturnToMainMenu()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainMenuCanvasObject != null) mainMenuCanvasObject.SetActive(true);
    }

    // Called when clicking Quit
    public void QuitGame()
    {
        Debug.Log("Quitting application...");
        Application.Quit();
    }
}