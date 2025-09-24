using UnityEngine;

public class CrownScript : MonoBehaviour
{
    // Reference to the current holder of the crown
    private Transform holder;

    // Reference to the Rigidbody component for physics interactions
    private Rigidbody rb;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        // Get the Rigidbody component attached to the crown
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        // If the crown has a holder, follow the holder's position and rotation
        if (holder != null)
        {
            transform.position = holder.position + new Vector3(0, 2f, 0); // floats above tank

            transform.Rotate(Vector3.up * 90 * Time.deltaTime); // spin effect
        }
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // If the crown is not held and a player collides with it, attach the crown to that player
        if (holder == null && other.CompareTag("Player"))
        {
            TankControllerScript tank = other.GetComponent<TankControllerScript>();
            if (tank != null)
            {
                AttachTo(tank);
            }
        }
    }

    /// <summary>
    /// Attach the crown to a new holder
    /// </summary>
    /// <param name="newHolder"></param>
    public void AttachTo(TankControllerScript tank)
    {
        // Detach from current holder if any
        holder = tank.transform;

        // Deactivate physics
        rb.isKinematic = true;
        rb.useGravity = false;

        // Parent the crown to the new holder
        tank.HasCrown = true;
        tank.AttachCrown(this);
    }

    /// <summary>
    /// Drop the crown, detaching it from its current holder
    /// </summary>
    public void Drop()
    {
        // Detach from holder
        holder = null;
        transform.SetParent(null);

        // Reactivate physics
        rb.useGravity = true;
        rb.isKinematic = false;
    }
}
