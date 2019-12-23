using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Car c; //Every enemy has a car.

    public float minSwitchLaneTime, maxSwitchLaneTime;

    void Start()
    {
        c = GetComponent<Car>(); //Adds our new car as a Unity Component.

        c.GetCurrentLane(); //Get the car's current lane as soon as it spawns.

        StartCoroutine(RandomSwitchLanes()); //Enemies will switch lanes at random directions and random intervals.
    }

    void Update()
    {
        if (GameControl.Speed == GameControl.MaxSpeed) //Enemies will stop switching lanes if the world is speeding up or slowing down.
        {
            if (c.moving) //Transform won't be constantly checked.
            {
                transform.position = c.Translate();
                transform.rotation = c.Rotate();

                if (c.LaneReached()) //Stops checking Transform.
                {
                    c.moving = false;
                    c.currentLane = c.targetLane;
                }
            }

            if (transform.rotation != Quaternion.identity) //Slowly returns rotation to identity.
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * c.rotationSpeed * 2);
            }
        }
    }

    private IEnumerator RandomSwitchLanes()
    {
        while (GameControl.Speed == GameControl.MaxSpeed)
        {
            while (GameControl.Speed != GameControl.MaxSpeed) //Stop switching lanes if the world is speeding up or slowing down.
            {
                yield return null;
            }

            c.moving = true;

            float switchLaneTime = Random.Range(minSwitchLaneTime, maxSwitchLaneTime);

            if (c.currentLane == 0) //Enemies won't bump into the road's borders.
            {
                c.targetLane = 1;
            }
            else if (c.currentLane == GameControl.Lanes.Length - 1)
            {
                c.targetLane = 3;
            }
            else //Enemies will randomly go left or right.
            {
                int randomLeftOrRight = 0;

                randomLeftOrRight = Random.Range(0, 2);

                if (randomLeftOrRight == 0)
                {
                    c.targetLane = c.currentLane - 1;
                }
                else
                {
                    c.targetLane = c.currentLane + 1;
                }
            }

            yield return new WaitForSeconds(switchLaneTime);
        }
    }
}
