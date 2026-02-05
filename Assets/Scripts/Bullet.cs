using System.Runtime.CompilerServices;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject bulletImapactFX;
    private Rigidbody rb =>GetComponent<Rigidbody>();
    private void OnCollisionEnter(Collision collision)
    {
        CreateImpactFx(collision);
        Destroy(gameObject);
        //rb.constraints = RigidbodyConstraints.FreezeAll;
        // Add logic for what happens when the bullet collides with something

    }

    private void CreateImpactFx(Collision collision)
    {
        if ((collision.contacts.Length > 0))
        {
            ContactPoint contact = collision.contacts[0];
            GameObject newImpactFx = Instantiate(bulletImapactFX, contact.point, Quaternion.LookRotation(contact.normal));
            Destroy(newImpactFx, 1f);
        }
    }
}
