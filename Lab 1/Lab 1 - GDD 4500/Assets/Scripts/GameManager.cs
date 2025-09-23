using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// A script to manage overall game state, player management, and crown mechanics
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Fields

    // Singleton instance
    public static GameManager Instance;

    // Holds the current active scene
    private Scene currentScene;

    // Holds a reference to the PlayerInputManager in the scene
    private PlayerInputManager playerInputManager;

    // The player who currently has the crown
    private GameObject crownedPlayer;

    // List to keep track of all players in the game
    private List<PlayerInput> players = new List<PlayerInput>();

    // Array of transforms for spawn points in spawn
    [SerializeField] Transform[] spawnPoints; // assigned in inspector
    private int nextSpawnIndex = 0; // To track next spawn point index

    #endregion

    #region Properties

    /// <summary>
    /// Public property to access the crowned player
    /// </summary>
    public GameObject CrownedPlayer { get { return crownedPlayer; } set { crownedPlayer = value; } }

    /// <summary>
    /// Public property to access the list of players
    /// </summary>
    public List<PlayerInput> Players { get { return players; } }

    #endregion

    #region Unity Methods

    void Awake()
    {
        // Ensure only one instance of GameManager exists
        Instance = this;

        // Get the PlayerInputManager in the scene
        playerInputManager = GetComponent<PlayerInputManager>();

        // Don't destroy this object on scene load
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// OnEnable is called when the object becomes enabled and active
    /// </summary>
    private void OnEnable()
    {
        // Subscribe to the player joined event
        playerInputManager.onPlayerJoined += OnPlayerJoined;
    }

    /// <summary>
    /// OnDisable is called when the behaviour becomes disabled or inactive
    /// </summary>
    private void OnDisable()
    {
        // Unsubscribe from the player joined event
        playerInputManager.onPlayerJoined -= OnPlayerJoined;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Called when transferring the crown to a new player
    /// </summary>
    /// <param name="newOwner"></param>
    public void TransferCrown(GameObject newOwner)
    {
        crownedPlayer = newOwner;
        Debug.Log(newOwner.name + " has the crown!");
        // TODO: Add crown visual above newOwner
    }

    public void WinGame(PlayerInput winner)
    {
        
    }

    #endregion

    #region Events

    /// <summary>
    /// Handles the event when a new player joins the game
    /// </summary>
    /// <param name="playerInput"></param>
    void OnPlayerJoined(PlayerInput playerInput)
    {
        // Get current scene name
        currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        // Only allow players to join in MainMenu scene
        if (sceneName == "MainMenu" && spawnPoints != null)
        {
            Debug.Log("Player joined: " + playerInput.playerIndex);

            players.Add(playerInput);
            Debug.Log(players);

            // Assign spawn point
            Transform spawnPoint = spawnPoints[nextSpawnIndex % spawnPoints.Length];
            playerInput.transform.position = spawnPoint.position;
            playerInput.transform.rotation = spawnPoint.rotation;

            nextSpawnIndex++;
        }
        else
        {
            // If not in MainMenu, remove the player immediately
            Debug.Log("Player tried to join outside MainMenu. Removing player.");
            Destroy(playerInput.gameObject);
        }
    }

    #endregion
}
