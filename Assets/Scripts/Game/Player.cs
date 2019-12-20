using UnityEngine;

public class Player : MonoBehaviour
{
    private Car c; //Every player has a car.

    private Rigidbody rb; //Player will be affected by physics.

    private bool crash; //True if the player crashes into another car.

    public float crashForce; //The increment at which the player will be thrown off into the air.

    void Start()
    {
        c = gameObject.AddComponent<Car>() as Car; //Adds our new car as a Unity Component.

        c.SetUp(); //Set up the car's parameters.

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!crash) //Player cannot move if they have crashed.
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

    public void LeftButton() //Called by user input.
    {
        if (!c.moving) //Does nothing if player is still moving.
        {
            if (c.currentLane > 0) //Player cannot bump into the road's borders (nor exceed array's size).
            {
                c.moving = true;

                c.targetLane = c.currentLane - 1;
            }
        }
    }

    public void RightButton() //Called by user input.
    {
        if (!c.moving) //Does nothing if player is still moving.
        {
            if (c.currentLane < GameControl.Lanes.Length - 1) //Player cannot bump into the road's borders (nor exceed array's size).
            {
                c.moving = true;

                c.targetLane = c.currentLane + 1;
            }
        }
    }

    private void OnTriggerEnter(Collider other) //Checks if gold is obtained and calls the corresponding update method.
    {
        if (other.tag == "Gold")
        {
            GameControl.UpdateGold();
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision) //If a car is crashed into, add a force, a torque, and end the game.
    {
        if (collision.collider.tag == "Car")
        {
            rb.AddForce(transform.up * crashForce);
            rb.AddTorque(transform.forward * crashForce);

            GameControl.GameOver();

            crash = true;
        }
    }
}