using UnityEngine;

public class AudioManager : MonoBehaviour
{
	private static AudioManager _instance;

	public AudioSource MonsterSource;

	public AudioSource ThemeSource;

	public AudioSource KnockSource;

	public AudioSource TapeInsertSource;

	public AudioSource TapeEjectSource;

	public AudioSource TapeClackSource;

	public static AudioManager Instance => _instance;

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			_instance = this;
		}
	}

	public void PlayMainTheme()
	{
		ThemeSource.Play();
	}

	public void PlayMonsterSound()
	{
		MonsterSource.Play();
	}

	public void PlayKnockSFX()
	{
		KnockSource.Play();
	}

	public void PlayTapeSFX()
	{
		TapeInsertSource.Play();
	}

	public void PlayTapeEjectSFX()
	{
		TapeEjectSource.Play();
	}

	public void PlayTapeClackSFX()
	{
		TapeClackSource.Play();
	}
}
