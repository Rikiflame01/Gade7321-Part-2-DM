using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// The PlayerInputManager script captures player names through input fields,
/// assigns colours randomly to each player, and changes scenes upon successful submission.
/// It handles button click events to submit names and colour assignments. If a player name
/// is missing, it logs a reminder; otherwise, it loads the specified scene.
/// The script also manages event listeners for the submit button.
/// </summary>
public class PlayerInputManager : MonoBehaviour
{
    public PlayerInfo playerInfo;
    public GameStateData gameData;

    public TMP_InputField player1InputField;
    public TMP_InputField player2InputField;

    public Button submitButton;

    void Start()
    {
        if (submitButton != null)
        {
            submitButton.onClick.AddListener(SubmitNamesAndRandomizeColours);
        }
        else
        {
            Debug.LogError("Submit Button is not assigned.");
        }

        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "EasyScene" || currentSceneName == "Medium-MinimaxScene" ||
            currentSceneName == "MinimaxScene" || currentSceneName == "MultiplayerScene")
        {
            SetupEndGameButtons();
        }
    }

    private void SubmitNamesAndRandomizeColours()
    {
        if (player1InputField == null || player2InputField == null || playerInfo == null)
        {
            Debug.LogError("Input fields or playerInfo are not assigned.");
            return;
        }

        playerInfo.player1.playerName = player1InputField.text;
        playerInfo.player2.playerName = player2InputField.text;

        if (Random.value > 0.5f)
        {
            playerInfo.player1.playerColour = "Red";
            playerInfo.player2.playerColour = "Blue";
        }
        else
        {
            playerInfo.player1.playerColour = "Blue";
            playerInfo.player2.playerColour = "Red";
        }

        Debug.Log($"{playerInfo.player1.playerName} is {playerInfo.player1.playerColour}");
        Debug.Log($"{playerInfo.player2.playerName} is {playerInfo.player2.playerColour}");

        if (string.IsNullOrWhiteSpace(playerInfo.player1.playerName) || string.IsNullOrWhiteSpace(playerInfo.player2.playerName))
        {
            Debug.Log("Please enter a name for both players.");
            return;
        }
        else
        {
            SceneManager.LoadScene("MultiplayerScene");
        }
    }

    private void EnterPlayerInfoAI(string difficulty)
    {
        if (playerInfo == null)
        {
            Debug.LogError("PlayerInfo is not assigned.");
            return;
        }

        playerInfo.player1.playerName = difficulty + "AI";
        playerInfo.player2.playerName = "Player";

        if (Random.value > 0.5f)
        {
            playerInfo.player1.playerColour = "Red";
            playerInfo.player2.playerColour = "Blue";
        }
        else
        {
            playerInfo.player1.playerColour = "Blue";
            playerInfo.player2.playerColour = "Red";
        }
    }

    public void EnterEasy()
    {
        if (gameData != null)
        {
            gameData.difficulty = Difficulty.Easy;
            EnterPlayerInfoAI("Easy");
            SceneManager.LoadScene("EasyScene");
        }
        else
        {
            Debug.LogError("GameData is not assigned.");
        }
    }

    public void EnterMedium()
    {
        if (gameData != null)
        {
            gameData.difficulty = Difficulty.Medium;
            EnterPlayerInfoAI("Medium");
            SceneManager.LoadScene("Medium-MinimaxScene");
        }
        else
        {
            Debug.LogError("GameData is not assigned.");
        }
    }

    public void EnterDifficultScene()
    {
        if (gameData != null)
        {
            gameData.difficulty = Difficulty.Hard;
            EnterPlayerInfoAI("Hard");
            SceneManager.LoadScene("MinimaxScene");
        }
        else
        {
            Debug.LogError("GameData is not assigned.");
        }
    }

    void OnDestroy()
    {
        if (submitButton != null)
        {
            submitButton.onClick.RemoveListener(SubmitNamesAndRandomizeColours);
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetupEndGameButtons()
    {
        GameObject uiCanvas = GameObject.Find("UICanvas");

        if (uiCanvas == null)
        {
            Debug.LogError("UICanvas not found.");
            return;
        }

        GameObject endGamePrefab = uiCanvas.transform.Find("EndGame")?.gameObject;

        if (endGamePrefab == null)
        {
            Debug.LogError("EndGame prefab not found.");
            return;
        }

        // Temporarily enable the EndGame prefab to find the buttons
        bool wasActive = endGamePrefab.activeSelf;
        if (!wasActive)
        {
            endGamePrefab.SetActive(true);
        }

        Button returnHomeBtn = FindButtonWithTag(endGamePrefab, "ReturnHomeBtn");
        Button quitGameBtn = FindButtonWithTag(endGamePrefab, "QuitGameBtn");

        if (returnHomeBtn != null)
        {
            returnHomeBtn.onClick.AddListener(LoadMainMenu);
            Debug.Log("ReturnHomeBtn listener added.");
        }
        else
        {
            Debug.LogError("ReturnHomeBtn not found or missing Button component.");
        }

        if (quitGameBtn != null)
        {
            quitGameBtn.onClick.AddListener(QuitGame);
            Debug.Log("QuitGame listener added.");
        }
        else
        {
            Debug.LogError("QuitGame button not found or missing Button component.");
        }

        // Restore the original active state
        if (!wasActive)
        {
            endGamePrefab.SetActive(false);
        }
    }

    private Button FindButtonWithTag(GameObject parentObject, string tag)
    {
        Button[] buttons = parentObject.GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
        {
            if (button.CompareTag(tag))
            {
                return button;
            }
        }
        return null;
    }
}
