using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR.Haptics;

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

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
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
    }

    void Shoot()
    {
        if (bulletPrefab && firePoint && cooldown == false)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            cooldown = true;
        }
    }
}
