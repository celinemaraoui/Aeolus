using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string gameSceneName = "Aeolus";

    [Header("UI References")]
    [SerializeField] private GameObject menuContentPanel; 
    [SerializeField] private GameObject settingsPanel;    

    void Start()
    {
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

    public void OpenSettings()
    {
        if (menuContentPanel != null) menuContentPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
        Debug.Log("Opening settings panel.");
    }

    public void ReturnToMainMenu()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (menuContentPanel != null) menuContentPanel.SetActive(true);
        Debug.Log("Returning to main menu.");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting application...");
        Application.Quit();
    }
}