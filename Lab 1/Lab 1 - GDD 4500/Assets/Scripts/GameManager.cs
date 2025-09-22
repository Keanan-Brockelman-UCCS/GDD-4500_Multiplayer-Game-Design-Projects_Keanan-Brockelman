using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameObject crownedPlayer;

    void Awake()
    {
        Instance = this;
    }

    public GameObject GetCrownedPlayer()
    {
        return crownedPlayer;
    }

    public void TransferCrown(GameObject newOwner)
    {
        crownedPlayer = newOwner;
        Debug.Log(newOwner.name + " has the crown!");
        // TODO: Add crown visual above newOwner
    }

    [SerializeField] Transform[] spawnPoints; // assign in inspector
    private int nextSpawnIndex = 0;

    void OnEnable()
    {
        var pim = GetComponent<PlayerInputManager>();
        pim.onPlayerJoined += OnPlayerJoined;
    }

    void OnDisable()
    {
        var pim = GetComponent<PlayerInputManager>();
        pim.onPlayerJoined -= OnPlayerJoined;
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player joined: " + playerInput.playerIndex);

        // Assign spawn point
        Transform spawnPoint = spawnPoints[nextSpawnIndex % spawnPoints.Length];
        playerInput.transform.position = spawnPoint.position;
        playerInput.transform.rotation = spawnPoint.rotation;

        nextSpawnIndex++;
    }
}
