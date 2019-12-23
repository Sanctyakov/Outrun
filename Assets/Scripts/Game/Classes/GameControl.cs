using UnityEngine;

public static class GameControl //A static class that communicates with the Game Controller script, so that each instantiated object doesn't need to find the script by tag.
{
	public static GameController gameController;

	public static void SetGameController(GameController gC)
	{
		gameController = gC; //The Game Controller script is set.
	}

	public static Transform[] Lanes
	{
		get { return gameController.lanes; } //The lanes are provided via game object reference in the editor.
	}

	public static float Speed
	{
		get { return gameController.speed; } //The speed at which the world will travel backwards (it peaks at maxSpeed).
	}

	public static float MaxSpeed
	{
		get { return gameController.maxSpeed; } //The maximum speed the world will accelerate to.
	}

	public static void UpdateGold() //Public methods.
	{
		gameController.UpdateGold();
	}

	public static void GameOver()
	{
		gameController.GameOver();
	}
}
