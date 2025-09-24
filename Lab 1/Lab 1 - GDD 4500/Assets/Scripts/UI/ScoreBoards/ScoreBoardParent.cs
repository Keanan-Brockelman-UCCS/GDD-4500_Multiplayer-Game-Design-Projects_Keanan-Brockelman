using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

/// <summary>
/// A parent class for scoreboards to inherit from
/// </summary>
public class ScoreBoardParent : MonoBehaviour
{
    // The player's current score
    public string playerScore;
    // The player's previous score to detect changes
    public string previousScore;
    // Template text for displaying the score, with a placeholder for the score value
    public string templateText;
    // Reference to the TextMeshProUGUI component to display the score
    public TextMeshProUGUI scoreText;

    /// <summary>
    /// Start is called once before the first frame of Update
    /// </summary>
    public void Awake()
    {
        // Get reference to TextMeshProUGUI component if not assigned
        scoreText = GetComponent<TextMeshProUGUI>();

        // Store the original text with {score} placeholder
        templateText = scoreText.text;

        // Initialize scores
        UpdateScoreBoard();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    public void Update()
    {
        playerScore = GetPlayerScore();

        // Check if the player's score has changed
        if (playerScore != previousScore)
        {
            // If the score has changed, update the scoreboard display
            UpdateScoreBoard();

            // Update the previous score to the current score
            previousScore = playerScore;
        }
    }

    /// <summary>
    /// Update the scoreboard display with the current player's score
    /// </summary>
    public virtual void UpdateScoreBoard()
    {
        scoreText.text = templateText.Replace("{score}", playerScore);
    }

    /// <summary>
    /// Get the player's current score
    /// </summary>
    /// <returns></returns>
    public virtual string GetPlayerScore()
    {
        return "" + playerScore;
    }
}
