using UnityEngine;
using UnityEngine.InputSystem;

public class TankVisualsScript : MonoBehaviour
{
    private Renderer rend;

    void Start()
    {
        rend = GetComponentInChildren<Renderer>();

        // Get the PlayerInput component to read index
        var input = GetComponent<PlayerInput>();

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
