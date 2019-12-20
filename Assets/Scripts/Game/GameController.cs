using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;

//This script will be the first amongst all our custom scripts to be executed, as defined by project settings.

public class GameController : MonoBehaviour
{
    #region Variable declarations

    public float acceleration, maxSpeed, goldSpawnTimeMin, goldSpawnTimeMax, carSpawnTimeMin, carSpawnTimeMax; //Variables we can set from the inspector to vary the game's difficulty

    private float speed; //The speed at which the world will travel backwards (it peaks at maxSpeed).

    public float Speed //Public properties that can only be read, never written.
    {
        get { return speed; }
    }

    public float MaxSpeed
    {
        get { return maxSpeed; }

    }

    public Text speedText, goldCounterText, bestCounterText, newBestText, newBestCounterText; //UI texts.

    public Transform goldY, carY; //Offsets for spawning either gold or cars.
    public Transform[] lanes; //An array of the road's lanes, at which things will be instantiated and from which cars will switch between.

    public Button[] directionalButtons;

    public GameObject IGM, gold; //GameObjects to instantiate and In-Game Menu to activate / deactivate.
    public GameObject[] cars;

    private string bestPath = Path.Combine(Application.streamingAssetsPath, "best.txt"); //Location of save file (a simple txt file will do).

    private int goldCounter, bestCounter; //Score counters.
    private enum GameStates //The game will switch between these states.
    {
        GameStart,
        //Pause,
        Round,
        //RoundEnd,
        GameOver
    }

    private GameStates gameState = GameStates.GameStart; //State in which the game will start.

    #endregion

    #region Methods

    void Start()
    {
        GameControl.SetGameController(this); //The static class will communicate with this script.

        speed = 0; //Set everything to its starting value.
        goldCounterText.text = "0";

        if (File.Exists(bestPath)) //If the file exists, read the best score from it and set it.
        {
            StreamReader reader = new StreamReader(bestPath);
            bestCounterText.text = reader.ReadLine();
            reader.Close();
        }
        else
        {
            bestCounterText.text = "0";
        }

        newBestText.text = "";
        newBestCounterText.text = "";

        foreach (Button b in directionalButtons) //Player can't move while the game is yet to be started.
        {
            b.interactable = false;
        }
    }

    private void Update()
    {
        UpdateSpeed();
    }

    private void UpdateSpeed() //Displays current world-speed.
    {
        switch (gameState)
        {
            case GameStates.Round:
                if (speed < maxSpeed)
                {
                    speed += acceleration * Time.deltaTime;
                    speedText.text = Mathf.RoundToInt(speed) + " km/h";
                }
                else
                {
                    speed = maxSpeed;
                    speedText.text = speed + " km/h";
                }
                break;
            case GameStates.GameOver:
                if (speed > 0)
                {
                    speed -= acceleration * Time.deltaTime;
                    speedText.text = Mathf.RoundToInt(speed) + " km/h";
                }
                else
                {
                    speedText.text = 0 + " km/h";
                }
                break;
            default:
                break;
        }
    }

    public void RoundStart() //Accessed via the "play" button in the In-Game Menu.
    {
        ChangeState(GameStates.Round);
        StartCoroutine(SpawnGold());
        StartCoroutine(SpawnCars());

        foreach (Button b in directionalButtons)
        {
            b.interactable = true; //Player can now be moved.
        }
    }

    private void ChangeState(GameStates state)
    {
        gameState = state;
    }

    private IEnumerator SpawnGold() //Spawns gold at random intervals.
    {
        while (gameState == GameStates.Round)
        {
            while (gameState == GameStates.GameOver /*|| gameState == GameStates.Pause*/) //Stop spawning if game over.
            {
                yield return null;
            }

            float goldSpawnTime = Random.Range(goldSpawnTimeMin, goldSpawnTimeMax);

            Vector3 spawnPosition = lanes[Random.Range(0, lanes.Length)].position + goldY.position;
            Quaternion spawnRotation = Quaternion.identity;

            Instantiate(gold, spawnPosition, spawnRotation);

            yield return new WaitForSeconds(goldSpawnTime);
        }
    }

    private IEnumerator SpawnCars() //Spawns random cars at random intervals.
    {
        while (gameState == GameStates.Round)
        {
            while (gameState == GameStates.GameOver /*|| gameState == GameStates.Pause*/) //Stop spawning if game over.
            {
                yield return null;
            }

            float carSpawnTime = Random.Range(carSpawnTimeMin, carSpawnTimeMax);

            int random = Random.Range(0, lanes.Length);

            Vector3 spawnPosition = lanes[random].position + carY.position;
            Quaternion spawnRotation = Quaternion.identity;

            Instantiate(cars[Random.Range(0,cars.Length)], spawnPosition, spawnRotation);

            yield return new WaitForSeconds(carSpawnTime);
        }
    }

    public void UpdateGold() //Called each time gold is obtained, updates the UI counter.
    {
        goldCounter++;

        goldCounterText.text = goldCounter.ToString();

        if (goldCounter > bestCounter)
        {
            bestCounter = goldCounter;
            bestCounterText.text = bestCounter.ToString();

            newBestText.text = "NEW BEST!";
            newBestCounterText.text = bestCounter.ToString();
        }
    }

    public void GameOver() //Called if the player crashes into another car.
    {
        ChangeState(GameStates.GameOver);

        IGM.SetActive(true);

        foreach (Button b in directionalButtons) //Player cannot move while crashed.
        {
            b.interactable = false;
        }
    }

    public void Restart() //Reloads the scene upon "restart" button press.
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnApplicationQuit() //Saves the current best score if it is higher than a previous one.
    {
        if (bestCounter > 0)
        {
            if (File.Exists(bestPath))
            {
                StreamWriter writer = new StreamWriter(bestPath);
                writer.WriteLine(bestCounter);
                writer.Close();
            }
        }
    }

    #endregion
}