using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    [SerializeField] private Transform player;


    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, Camera.main.transform.position.y,player.transform.position.z-5);
    }
}
