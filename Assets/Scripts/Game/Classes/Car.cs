using UnityEngine;

public class Car : MonoBehaviour
{
    public float speed, rotationSpeed;

    public bool moving;

    public int currentLane, targetLane;

    public void GetCurrentLane()
    {
        for (int i = 0; i < GameControl.Lanes.Length; i++)
        {
            if (transform.position.x == GameControl.Lanes[i].position.x)
            {
                currentLane = i;
                break;
            }
        }
    }

    public Vector3 Translate()
    {
        float step = speed * Time.deltaTime;

        Vector3 targetPosition = new Vector3(GameControl.Lanes[targetLane].position.x, transform.position.y, transform.position.z);

        return Vector3.MoveTowards(transform.position, targetPosition, step);
    }

    public Quaternion Rotate()
    {
        Vector3 targetPosition = new Vector3(GameControl.Lanes[targetLane].position.x, transform.position.y, transform.position.z);

        Quaternion targetRotation = Quaternion.LookRotation((transform.position - targetPosition) * -1);

        return Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public bool LaneReached()
    {
        if (transform.position.x == GameControl.Lanes[targetLane].position.x)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}