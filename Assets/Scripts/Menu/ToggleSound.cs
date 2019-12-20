using UnityEngine;

public class ToggleSound : MonoBehaviour
{
	public AudioSource[] audioSources;
	private bool muted = false;

	public void Toggle() //Mutes / unmutes audiosource.
	{
		foreach (AudioSource audioSource in audioSources)
		{
			audioSource.mute = !muted;
		}

		muted = !muted;
	}
}
