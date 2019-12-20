using UnityEngine;

public static class GameControl //A static class that communicates with the Game Controller script, so that each instantiated object doesn't need to find the script by tag.
{
	public static GameController gameController;

	public static void SetGameController(GameController gC)
	{
		gameController = gC;
	}

	public static Transform[] Lanes
	{
		get { return gameController.lanes; }
	}

	public static float Speed
	{
		get { return gameController.Speed; }
	}

	public static float MaxSpeed
	{
		get { return gameController.MaxSpeed; }
	}

	public static void UpdateGold()
	{
		gameController.UpdateGold();
	}

	public static void GameOver()
	{
		gameController.GameOver();
	}
}
