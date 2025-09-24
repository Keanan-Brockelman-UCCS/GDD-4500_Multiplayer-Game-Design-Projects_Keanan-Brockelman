using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI.Table;

/// <summary>
/// A script to control bullet behavior
/// </summary>
public class BulletControllerScript : MonoBehaviour
{
    // Speed of the bullet
    private float speed = 15f;

    // Reference to the Renderer component for color changes
    private Renderer rend;

    /// <summary>
    /// Awake is called when the script instance is being loaded
    /// </summary>
    void Awake()
    {
        // Get the Renderer component from the bullet or its children
        rend = GetComponentInChildren<Renderer>();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // Move the bullet forward based on its speed and deltaTime
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        // Check for collision with player tanks
        if (other.CompareTag("Player"))
        {
            // Get the tank script from the other tank and call its DieAndRespawn method
            TankControllerScript tank = other.GetComponent<TankControllerScript>();
            tank.DieAndRespawn();

            // Destroy the bullet after hitting a player
            Destroy(gameObject);
        }

        // Check for collision with walls
        if (other.CompareTag("DestructableWall"))
        {
            // Destroy the wall upon collision
            Destroy(other);

            // Destroy the bullet upon hitting a wall
            Destroy(gameObject);
        }

        // Check for collision with walls
        if (other.CompareTag("Wall"))
        {
            // Destroy the bullet upon hitting a wall
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Set the color of the bullet
    /// </summary>
    /// <param name="color"></param>
    public void SetColor(Color color)
    {
        if (rend != null)
        {
            rend.material.color = color;
        }
    }
}
