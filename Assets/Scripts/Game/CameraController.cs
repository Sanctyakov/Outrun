using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    public Transform offset;

    void Update() //Follows the player with an offset so the camera doesn't get too close.
    {
        transform.position = new Vector3 (player.transform.position.x, transform.position.y, transform.position.z);

        transform.LookAt(player.transform.position + offset.position);
    }
}