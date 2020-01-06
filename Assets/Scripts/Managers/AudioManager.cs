using UnityEngine.Audio;
using UnityEngine;
using System;

[System.Serializable]
public class GameSound{
	public string name;
	public bool loop;
	public bool playOnAwake;
	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume;

	[Range(.1f, 3f)]
	public float pitch;

	[HideInInspector]
	public AudioSource source;
}

public class AudioManager : MonoBehaviour {

	public GameSound[] sounds;
	public GameSound buttonClickEffect;

	public static AudioManager instance;

	// Use this for initialization
	void Awake () {

		if(instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		foreach(GameSound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;

			if(s.playOnAwake)
			{
				Play(s.name);
			}
		}
		// foreach(GameSound s in sounds)
		// {
		// 	if(s.playOnAwake)
		// 	{
		// 		Play(s.name);
		// 	}
		// }
		if(buttonClickEffect != null)
		{
			buttonClickEffect.source = gameObject.AddComponent<AudioSource>();
			buttonClickEffect.source.clip = buttonClickEffect.clip;
			buttonClickEffect.source.volume = buttonClickEffect.volume;
			buttonClickEffect.source.pitch = buttonClickEffect.pitch;
			buttonClickEffect.source.loop = buttonClickEffect.loop;
		}
	}

	void OnDestroy()
	{
		Destroy(gameObject);
	}
	
	// Update is called once per frame
	public void Play(string name)
	{
		GameSound s = Array.Find(sounds, sound => sound.name == name);
		if(s == null)
		{
			Debug.LogWarning("Audio missing!!!!!");
			return;
		}
		s.source.Play();
	}

	public void PlayBtnClickSound()
	{
	}
}
