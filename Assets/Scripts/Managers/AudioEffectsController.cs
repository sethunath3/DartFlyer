using UnityEngine;
using System;

public class AudioEffectsController : MonoBehaviour {
	public GameSound[] sounds;

	// Use this for initialization
	void Start () {
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
		foreach(GameSound s in sounds)
		{
			if(s.playOnAwake)
			{
				Play(s.name);
			}
		}
	}
	
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
}
