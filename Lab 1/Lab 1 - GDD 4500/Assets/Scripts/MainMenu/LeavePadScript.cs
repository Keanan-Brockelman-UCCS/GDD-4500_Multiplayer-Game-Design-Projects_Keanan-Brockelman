using UnityEngine;
using UnityEngine.InputSystem;

public class LeavePadScript : MonoBehaviour
{
    /// <summary>
    /// OnTriggerEnter is called when another collider enters the trigger collider attached to this object
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // Destroy player object when it enters the leave pad trigger
        if (other.CompareTag("Player"))
        {
            // Remove player from GameManager's player list
            GameManager.Instance.RemovePlayer(other.GetComponent<PlayerInput>());

            // Destroy the player game object
            Destroy(other.gameObject);
        }
    }
}
