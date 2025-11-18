using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private Material gotHitMaterial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            GetComponent<MeshRenderer>().material = gotHitMaterial;
        }
    }
}
