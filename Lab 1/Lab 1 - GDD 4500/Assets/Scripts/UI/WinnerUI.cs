using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A script to manage the winner UI display
/// </summary>
public class WinnerUI : MonoBehaviour
{
    // Reference to the TextMeshProUGUI component to display the winner's name
    private TextMeshProUGUI winnerText = null;

    // Store the original template text with {winner} placeholder
    private string templateText;


    /// <summary>
    /// Called when the script instance is being loaded
    /// </summary>
    void Awake()
    {
        // Get reference to TextMeshProUGUI component if not assigned
        if (winnerText == null)
        {
            winnerText = GetComponent<TextMeshProUGUI>();
        }

        // Store the original text with {winner}
        templateText = winnerText.text;
    }

    /// <summary>
    /// Called before the first frame update
    /// </summary>
    void Start()
    {
        // Update the winner text at start
        UpdateWinnerText();
    }

    /// <summary>
    /// Update the winner text with the current winner's name and color
    /// </summary>
    public void UpdateWinnerText()
    {
        // If no winner info, display nothing
        if (GameManager.Instance.WinnerName == null || GameManager.Instance.WinnerColor == null)
        {
            winnerText.text = templateText.Replace("{winner}", " ");
            return;
        }

        // Replace {winner} with the winner's name in their color
        string coloredWinner = $"<color=#{GameManager.Instance.WinnerColor}>{GameManager.Instance.WinnerName}</color>";
        winnerText.text = templateText.Replace("{winner}", coloredWinner);
    }
}
