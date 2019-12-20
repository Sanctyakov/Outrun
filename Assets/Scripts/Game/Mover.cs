using UnityEngine;

public class Mover : MonoBehaviour
{
	void Update () //Game Objects with this component will move backwards at world speed (to be destroyed or teleported upon crossing the boundary). 
	{
		transform.position += Vector3.forward * -GameControl.Speed * Time.deltaTime;
	}
}