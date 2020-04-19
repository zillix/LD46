using UnityEngine;
using System.Collections;

public class Sounds : MonoBehaviour
{
	[SerializeField]
	private AudioSource player;

	public AudioClip ambientTheme;

	public AudioClip advanceText;
	public AudioClip textType;
	public AudioClip footstep;
	public AudioClip noteDiscovered;
	public AudioClip questionChosen;
	public AudioClip startGame;
	public AudioClip openNotes;

	public static Sounds instance;

	public static void PlayOneShot(AudioClip clip, float volume = 1.0f)
	{
		instance.PlaySfx(clip, volume);
	}

	public void PlaySfx(AudioClip clip, float volume = 1.0f)
	{
		if (clip == null)
		{
			return;
		}
		player.PlayOneShot(clip, volume);
	}

	// Use this for initialization
	void Start()
	{
		instance = this;
		//player = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
	{

	}
}
