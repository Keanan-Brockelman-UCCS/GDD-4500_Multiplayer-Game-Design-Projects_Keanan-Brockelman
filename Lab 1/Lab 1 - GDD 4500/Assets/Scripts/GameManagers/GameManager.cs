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
    private PlayerInput crownedPlayer;

    // List to keep track of all players in the game
    private List<PlayerInput> players = new List<PlayerInput>();

    // Array of transforms for spawn points in spawn
    [SerializeField] Transform[] spawnPoints; // assigned in inspector
    private int nextSpawnIndex = 0; // To track next spawn point index

    // Holds the UI strings for displaying the winner
    private string winnerName;
    private string winnerColor;

    #endregion

    #region Properties

    /// <summary>
    /// Public property to access the crowned player
    /// </summary>
    public PlayerInput CrownedPlayer { get { return crownedPlayer; } set { crownedPlayer = value; } }

    /// <summary>
    /// Public property to access the list of players
    /// </summary>
    public List<PlayerInput> Players { get { return players; } }

    /// <summary>
    /// Winner's name to be displayed in the UI
    /// </summary>
    public string WinnerName { get { return winnerName; } }

    /// <summary>
    /// Winner's color in hex format for UI display
    /// </summary>
    public string WinnerColor { get { return winnerColor; } }

    /// <summary>
    /// Player Ones scoreboard time for UI display
    /// </summary>
    public float PlayerOneScoreBoardTime { get; private set; } = 0f;

    /// <summary>
    /// Player Twos scoreboard time for UI display
    /// </summary>
    public float PlayerTwoScoreBoardTime { get; private set; } = 0f;

    /// <summary>
    /// Player Threes scoreboard time for UI display
    /// </summary>
    public float PlayerThreeScoreBoardTime { get; private set; } = 0f;

    /// <summary>
    /// Player Fours scoreboard time for UI display
    /// </summary>
    public float PlayerFourScoreBoardTime { get; private set; } = 0f;

    #endregion

    #region Unity Methods

    void Awake()
    {
        // Singleton pattern implementation
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // kill duplicate
            return;
        }

        // Cap frame rate to 60 FPS
        Application.targetFrameRate = 60; 

        // Ensure only one instance of GameManager exists
        Instance = this;

        // Get the PlayerInputManager in the scene
        playerInputManager = GetComponent<PlayerInputManager>();

        // Don't destroy this object on scene load
        DontDestroyOnLoad(gameObject);

        // Initialize winner UI to be empty at start
        WinGame(null, null); // This works because awake only gets called once
    }

    /// <summary>
    /// OnEnable is called when the object becomes enabled and active
    /// </summary>
    private void OnEnable()
    {
        // Subscribe to the player joined event
        playerInputManager.onPlayerJoined += OnPlayerJoined;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Activates once a player hits the win condition
    /// </summary>
    /// <param name="winner"></param>
    public void WinGame(GameObject winnerObject, PlayerInput winnerInput)
    {
        // Start with the winner being null
        winnerName = null;

        // As long as we have a valid winner object and input
        if (winnerObject != null && winnerInput != null)
        {
            // Determine winner color based on player index
            switch (winnerInput.playerIndex)
            {
                case 0: winnerName = "Red"; break;
                case 1: winnerName = "Blue"; break;
                case 2: winnerName = "Green"; break;
                case 3: winnerName = "Yellow"; break;
                default: winnerName = null; break;
            }

            // Grab player color and convert to hex string to change text color
            Color color = winnerObject.GetComponent<Renderer>().material.color;
            winnerColor = ColorUtility.ToHtmlStringRGB(color);
        }
        else 
        {
            // If no winner, reset values to null so that the UI displays nothing
            winnerName = null;
            winnerColor = null;
        }

        // Teleport everyone back to spawn points at the end of the game
        for (int i = 0; i < players.Count; i++)
        {
            Transform spawnPoint = spawnPoints[i % spawnPoints.Length];
            players[i].transform.position = spawnPoint.position;
            players[i].transform.rotation = spawnPoint.rotation;
        }
    }

    /// <summary>
    /// Updates public properties of scorboard times for UI to read and display
    /// </summary>
    /// <param name="playerTank"></param>
    /// <param name="crowntime"></param>
    public void UpdateScoreBoardTimers(PlayerInput playerTank, float crowntime)
    {
        // Determine winner color based on player index
        switch (playerTank.playerIndex)
        {
            case 0: PlayerOneScoreBoardTime = crowntime; break;
            case 1: PlayerTwoScoreBoardTime = crowntime; break;
            case 2: PlayerThreeScoreBoardTime = crowntime; break;
            case 3: PlayerFourScoreBoardTime = crowntime; break;
        }
    }

    /// <summary>
    /// A method to remove a player from the game
    /// </summary>
    /// <param name="player"></param>
    public void RemovePlayer(PlayerInput player)
    {
        if (players.Contains(player))
        {
            players.Remove(player);
        }
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
