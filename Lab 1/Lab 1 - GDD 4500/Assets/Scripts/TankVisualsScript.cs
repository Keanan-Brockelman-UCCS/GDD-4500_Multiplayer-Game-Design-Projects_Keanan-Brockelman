using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A simple script to set the tank color based on player index
/// </summary>
public class TankVisualsScript : MonoBehaviour
{
    // Reference to the Renderer component
    private Renderer rend;

    /// <summary>
    /// Called before the first frame update
    /// </summary>
    void Start()
    {
        // Get the Renderer component from the tank model child object
        rend = GetComponentInChildren<Renderer>();

        // Get the PlayerInput component to read index
        PlayerInput input = GetComponent<PlayerInput>();

        // Set color based on player index
        Color tankColor = Color.white;
        switch (input.playerIndex)
        {
            case 0: tankColor = Color.red; break;
            case 1: tankColor = Color.blue; break;
            case 2: tankColor = Color.green; break;
            case 3: tankColor = Color.yellow; break;
        }
        rend.material.color = tankColor;
    }
}
