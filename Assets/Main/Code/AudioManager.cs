using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public GameObject EffectsSource;

	public float LowPitchRange = .95f;
	public float HighPitchRange = 1.05f;

	public static AudioManager Instance = null;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	public void Play(AudioClip clip)
	{
		GameObject fx = Instantiate(EffectsSource, new Vector2(transform.position.x, transform.position.y), transform.rotation);
		fx.GetComponent<AudioSource>().clip = clip;

		float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

		fx.GetComponent<AudioSource>().pitch = randomPitch;

		fx.GetComponent<AudioSource>().Play();
	}

	public void RandomSoundEffect(params AudioClip[] clips)
	{
		GameObject fx = Instantiate(EffectsSource, new Vector2(transform.position.x, transform.position.y), transform.rotation);

		int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

		fx.GetComponent<AudioSource>().pitch = randomPitch;
		fx.GetComponent<AudioSource>().clip = clips[randomIndex];
		fx.GetComponent<AudioSource>().Play();
	}
}
