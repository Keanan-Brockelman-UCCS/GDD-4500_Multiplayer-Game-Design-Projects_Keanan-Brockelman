using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// A script to manage the start pad where players gather before starting the game
/// </summary>
public class StartPadScript : MonoBehaviour
{
    // List to track players currently on the pad
    private List<PlayerInput> playersOnPad = new List<PlayerInput>();

    /// <summary>
    /// OnTriggerEnter is called when another collider enters the trigger collider attached to this object
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        // Add player to the list if they enter the pad
        PlayerInput player = other.GetComponent<PlayerInput>();
        if (player != null)
        {
            playersOnPad.Add(player);

            // Check if all players are on the pad
            CheckAllPlayersOnPad();
        }
    }

    /// <summary>
    /// OnTriggerExit is called when another collider exits the trigger collider attached to this object
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        // Remove player from the list if they leave the pad
        PlayerInput player = other.GetComponent<PlayerInput>();
        if (player != null)
        {
            playersOnPad.Remove(player);
        }
    }

    /// <summary>
    /// Checks if all players are on the pad and starts the game if so
    /// </summary>
    void CheckAllPlayersOnPad()
    {
        // Ensure GameManager instance exists
        if (GameManager.Instance == null) return;

        // Checks if all players are on the pad by comparing counts of the players in the game and those on the pad
        if (playersOnPad.Count == GameManager.Instance.Players.Count)
        {
            //TODO: If you expand, add a countdown or multiple arneas or a UI transition here

            SceneManager.LoadScene("ArenaScene");
        }
    }
}
