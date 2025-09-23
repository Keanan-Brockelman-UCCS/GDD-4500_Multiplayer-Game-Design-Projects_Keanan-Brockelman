using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A game manager for arena scenes to handle player spawning and reset crowned player
/// </summary>
public class ArenaGameManager : MonoBehaviour
{
    // Array of transforms for spawn points in arena
    [SerializeField] Transform[] arenaSpawnPoints; // assigned in inspector

    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    private void Start()
    {
        GameManager.Instance.CrownedPlayer = null; // Reset crowned player on scene load

        // Position players at arena spawn points
        if (arenaSpawnPoints != null)
        {
            for (int i = 0; i < GameManager.Instance.Players.Count; i++)
            {
                GameManager.Instance.Players[i].transform.position = arenaSpawnPoints[i % arenaSpawnPoints.Length].position;
                GameManager.Instance.Players[i].transform.rotation = arenaSpawnPoints[i % arenaSpawnPoints.Length].rotation;
            }
        }
    }

    /// <summary>
    /// Respawn a player at their designated spawn point in the arena
    /// </summary>
    /// <param name="playerInput"></param>
    public void RespawnPlayer(PlayerInput playerInput)
    {
        int index = GameManager.Instance.Players.IndexOf(playerInput);
        if (index >= 0 && index < arenaSpawnPoints.Length)
        {
            playerInput.transform.position = arenaSpawnPoints[index].position;
            playerInput.transform.rotation = arenaSpawnPoints[index].rotation;
        }
    }
}
