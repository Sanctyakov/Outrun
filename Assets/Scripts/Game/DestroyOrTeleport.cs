using UnityEngine;

public class DestroyOrTeleport : MonoBehaviour
{
	public Transform teleportTransform; //The location at which terrain blocks will be teleported.
	void OnTriggerExit(Collider other) //Destroys or teleports objects that pass through the boundary depending on what they are.
	{
		if (other.tag == "Terrain")
		{
			other.gameObject.transform.position = teleportTransform.position + other.transform.position;
			other.gameObject.transform.rotation = Quaternion.identity;
		}
		else if (other.tag == "Car" || other.tag == "Gold")
		{
			Destroy(other.gameObject);
		}
	}
}