using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI.Table;

/// <summary>
/// A script to control bullet behavior
/// </summary>
public class BulletControllerScript : MonoBehaviour
{
    private float speed = 10f;
    private float lifeTime = 2f;

    private Renderer rend;

    void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TankControllerScript tank = other.GetComponent<TankControllerScript>();
            tank.DieAndRespawn();
        }

        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    public void SetColor(Color color)
    {
        if (rend != null)
        {
            rend.material.color = color;
        }
    }
}
