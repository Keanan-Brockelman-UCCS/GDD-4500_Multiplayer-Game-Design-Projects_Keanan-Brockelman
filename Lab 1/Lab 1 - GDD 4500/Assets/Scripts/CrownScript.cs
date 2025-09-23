using UnityEngine;

public class CrownScript : MonoBehaviour
{
    // The transform (tank) that is currently holding the crown
    private Transform holder;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // If the crown has a holder, follow the holder's position and rotation
        if (holder != null)
        {
            transform.position = holder.position + new Vector3(0, 2f, 0); // floats above tank
            transform.rotation = holder.rotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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
        holder = tank.transform;
        rb.isKinematic = true;
        rb.useGravity = false;
        tank.HasCrown = true;
        tank.AttachCrown(this);
    }

    /// <summary>
    /// Drop the crown, detaching it from its current holder
    /// </summary>
    public void Drop()
    {
        holder = null;
        transform.SetParent(null);
        rb.useGravity = true;
        rb.isKinematic = false;
    }
}
