using UnityEngine;
using UnityEngine.InputSystem;

public class BulletControllerScript : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;

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
            // Tell GameManager crown changes
            GameManager.Instance.TransferCrown(other.gameObject);
            Destroy(gameObject);
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
