using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string gameSceneName = "Aeolus";

    [Header("UI References")]
    [SerializeField] private GameObject menuContentPanel; // Drag your inner panel/buttons container here (NOT the whole Canvas)

    void Start()
    {
        // Ensure the menu is visible when the game starts
        if (menuContentPanel != null)
        {
            menuContentPanel.SetActive(true);
        }
    }

    public void StartGame()
    {
        Debug.Log("Loading gameplay scene: " + gameSceneName);

        // Hide only the menu content panel instantly so it doesn't flicker or come back
        if (menuContentPanel != null)
        {
            menuContentPanel.SetActive(false);
        }

        // Load the gameplay scene
        SceneManager.LoadScene(gameSceneName);
    }

    // Called when clicking Settings
    /*public void OpenSettings()
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
        }*/
}