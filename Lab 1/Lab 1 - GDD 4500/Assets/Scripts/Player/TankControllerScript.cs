using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// A script to control tank movement and shooting
/// </summary>
public class TankControllerScript : MonoBehaviour
{
    // Shooting parameters
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;

    // Movement parameters
    private float moveSpeed = 15f;
    public float moveMultiplier = 0.5f; // Can be adjusted for speed boosts or slowdowns
    private float rotationSpeed = 200f;

    // Movement input
    private Vector2 moveInput;
    private PlayerInput playerInput;

    // Shooting cooldown
    private bool cooldown = false;
    private int timer = 0;

    // Crown mechanics
    public bool hasCrown = false;
    public float crownTime = 0f;
    private CrownScript crownRef;

    /// <summary>
    /// A public property to check if the tank has the crown
    /// </summary>
    public bool HasCrown { get { return hasCrown; } set { hasCrown = value; } }

    /// <summary>
    /// Awake is called when the script instance is being loaded
    /// </summary>
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// OnMove is called by the Input System when the move action is performed
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// OnShoot is called by the Input System when the shoot action is performed
    /// </summary>
    /// <param name="context"></param>
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Shoot();
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // Forward/back movement (Y axis of input)
        float move = moveInput.y * moveSpeed * Time.deltaTime;
        transform.Translate(Vector3.forward * move * moveMultiplier);

        // Rotation (X axis of input)
        float rotate = moveInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * rotate);


        //Ensure the crown time resets when returning to main menu from arena
        Scene currentScene = SceneManager.GetActiveScene();
        bool resetComplete = false;

        if (currentScene.name == "ArenaScene" && resetComplete == false)
        {
            if (crownTime > 0f)
            {
                crownTime = 0;
                resetComplete = true;
            }
        }

        if (currentScene.name == "MainMenu" && resetComplete == true)
        {
            resetComplete = false;
        }
    }

    /// <summary>
    /// FixedUpdate is called at a fixed interval and is independent of frame rate
    /// </summary>
    private void FixedUpdate()
    {
        // Cooldown timer for shooting
        if (cooldown == true)
        {
            // Increment timer
            timer++;

            // Reset cooldown after 30 frames (~0.5 seconds)
            if (timer >= 30)
            {
                cooldown = false;
                timer = 0;
            }
        }

        // Timer if holding crown
        if (hasCrown)
        {
            // Increment crown time
            crownTime += Time.deltaTime;

            GameManager.Instance.UpdateScoreBoardTimers(playerInput, crownTime);

            // If crown time reaches 30 seconds, trigger win condition
            if (crownTime >= 15f * GameManager.Instance.Players.Count)
            {
                // Win method activation
                GameManager.Instance.WinGame(gameObject, playerInput);

                crownTime = 0f;

                // Drop the crown
                LoseCrown();

                // Load the main menu scene
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    /// <summary>
    /// Attack method to shoot a bullet from the tank
    /// </summary>
    void Shoot()
    {
        if (bulletPrefab && firePoint && cooldown == false)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // Set bullet color to match tank
            Color tankColor = GetComponentInChildren<Renderer>().material.color;
            bullet.GetComponent<BulletControllerScript>().SetColor(tankColor);

            cooldown = true;
        }
    }

    /// <summary>
    /// Attach the crown to this tank
    /// </summary>
    /// <param name="crown"></param>
    public void AttachCrown(CrownScript crown)
    {
        crownRef = crown;
        hasCrown = true;
        GameManager.Instance.CrownedPlayer = playerInput;
    }

    /// <summary>
    /// Detach the crown from this tank
    /// </summary>
    public void LoseCrown()
    {
        if (hasCrown && crownRef != null)
        {
            hasCrown = false;
            GameManager.Instance.CrownedPlayer = null;
            crownRef.Drop();
        }
    }

    /// <summary>
    /// When the tank dies, drop the crown and respawn at the arena spawn point
    /// </summary>
    public void DieAndRespawn()
    {
        // Drop crown if holding it
        LoseCrown();

        // Find arena manager
        ArenaGameManager arena = FindFirstObjectByType<ArenaGameManager>();
        if (arena != null)
        {
            // Respawn at arena spawn point
            PlayerInput input = GetComponent<PlayerInput>();
            arena.RespawnPlayer(input);
        }
    }
}
