using System.Runtime.CompilerServices;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb =>GetComponent<Rigidbody>();
    private void OnCollisionEnter(Collision collision)
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        // Add logic for what happens when the bullet collides with something
        
    }
}
