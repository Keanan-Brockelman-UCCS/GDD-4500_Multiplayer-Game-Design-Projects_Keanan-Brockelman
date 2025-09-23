using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A script to control tank movement and shooting
/// </summary>
public class TankControllerScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 200f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;

    private Vector2 moveInput;
    private PlayerInput playerInput;
    private bool cooldown = false;
    private int timer = 0;

    // Crown mechanics
    public bool hasCrown = false;
    public float crownTime = 0f;
    private CrownScript crown;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        DontDestroyOnLoad(gameObject);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Shoot();
        }
    }

    void Update()
    {
        // Forward/back movement (Y axis of input)
        float move = moveInput.y * moveSpeed * Time.deltaTime;
        transform.Translate(Vector3.forward * move);

        // Rotation (X axis of input)
        float rotate = moveInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * rotate);
    }

    private void FixedUpdate()
    {
        if (cooldown == true)
        {
            timer++;
            if (timer >= 60)
            {
                cooldown = false;
                timer = 0;
            }
        }

        // Timer if holding crown
        if (hasCrown)
        {
            crownTime += Time.deltaTime;

            PlayerInput input = GetComponent<PlayerInput>();

            Debug.Log("Crown time: " + crownTime.ToString("F2") + " seconds" + input);

            if (crownTime >= 30f)
            {
                // Win condition
                GameManager.Instance.WinGame(playerInput);
            }
        }
    }

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

    public bool HasCrown { get { return hasCrown; } set {hasCrown = value;} }
    private CrownScript crownRef;

    public void AttachCrown(CrownScript crown)
    {
        crownRef = crown;
        HasCrown = true;
    }

    public void PickUpCrown(CrownScript newCrown)
    {
        hasCrown = true;
        GameManager.Instance.CrownedPlayer = gameObject;
        crown = newCrown;
        //crown.AttachTo(transform);
    }

    public void LoseCrown()
    {
        if (HasCrown && crownRef != null)
        {
            crownRef.Drop();
        }
    }

    public void DieAndRespawn()
    {
        // Drop crown if holding it
        LoseCrown();

        // Find arena manager
        ArenaGameManager arena = FindFirstObjectByType<ArenaGameManager>();
        if (arena != null)
        {
            PlayerInput input = GetComponent<PlayerInput>();
            arena.RespawnPlayer(input);
        }
    }
}
