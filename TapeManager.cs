using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TapeManager : MonoBehaviour
{
	public delegate void OnVideoEndEventHandler();

	private static TapeManager _instance;

	public TapeScene KnockScene;

	public VideoPlayer VideoPlayer;

	public VideoClip VHSBlueScreen;

	public AudioSource VideoAudio;

	public TypingInput TypingInput;

	public ClickInput ClickInput;

	public ClickableObject[] ClickableTapes;

	private List<TapeScene> completedScenes = new List<TapeScene>();

	private Coroutine videoPlayerRoutine;

	private int interactionFailures;

	private int clickedTapes;

	public static TapeManager Instance => _instance;

	public TapeScene CurrentScene { get; private set; }

	public Tape CurrentTape { get; private set; }

	public event OnVideoEndEventHandler OnVideoEndEvent;

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

	public void PlayVideo(VideoClip clip, bool loop = false)
	{
		VideoPlayer.isLooping = loop;
		if (videoPlayerRoutine != null)
		{
			Debug.LogError("Video is already playing");
		}
		else
		{
			videoPlayerRoutine = StartCoroutine(VideoRoutine(clip));
		}
	}

	private IEnumerator VideoRoutine(VideoClip clip)
	{
		VideoPlayer.clip = clip;
		VideoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
		VideoPlayer.controlledAudioTrackCount = 1;
		VideoPlayer.SetTargetAudioSource(0, VideoAudio);
		VideoPlayer.EnableAudioTrack(0, enabled: true);
		VideoPlayer.Prepare();
		while (!VideoPlayer.isPrepared)
		{
			yield return null;
		}
		VideoPlayer.Play();
		while (VideoPlayer.isPlaying)
		{
			yield return null;
		}
		videoPlayerRoutine = null;
		if (this.OnVideoEndEvent != null)
		{
			this.OnVideoEndEvent();
			this.OnVideoEndEvent = null;
		}
	}

	public void SetTape(Tape tapeToSet)
	{
		if (videoPlayerRoutine != null)
		{
			StopCoroutine(videoPlayerRoutine);
			videoPlayerRoutine = null;
		}
		completedScenes.Clear();
		CurrentTape = tapeToSet;
		AnimationManager.Instance.PlayAnimation(tapeToSet.PreZoomAnim);
		AnimationManager.Instance.ClearAnimationEvent();
		AnimationManager.Instance.OnAnimationEndEvent += StartTape;
	}

	private void StartTape()
	{
		CurrentScene = CurrentTape.Scenes[0];
		PlayVideo(CurrentScene.PreInteractionClip);
		OnVideoEndEvent += SetupSceneInteraction;
		AnimationManager.Instance.PlayAnimation(AnimationManager.QueueableAnimations.AnimCameraZoomIn);
		AnimationManager.Instance.ClearAnimationEvent();
	}

	private void SetupSceneInteraction()
	{
		if (CurrentScene.InteractMode == InteractionMode.Type)
		{
			TypingInput.InitalizeCorrectWord(CurrentScene.correctWord, CurrentScene.allowAnyInput, CurrentScene.overflow);
		}
		else if (CurrentScene.InteractMode == InteractionMode.Click)
		{
			ClickInput.InitalizeClickableScene(CurrentScene.clickables);
		}
		else
		{
			CompleteCurrentScene();
		}
	}

	public void FailCurrentScene()
	{
		this.OnVideoEndEvent = null;
		interactionFailures++;
		Debug.Log("Failed interaction " + interactionFailures);
		if (interactionFailures == 1)
		{
			PlayVideo(CurrentScene.InteractionFailure1);
			OnVideoEndEvent += SetupSceneInteraction;
		}
		if (interactionFailures == 2)
		{
			PlayVideo(CurrentScene.InteractionFailure2);
			OnVideoEndEvent += SetupSceneInteraction;
			if (CurrentScene.InteractMode == InteractionMode.Type)
			{
				TypingInput.ForceCorrectInput = true;
			}
			else
			{
				ClickInput.HideIncorrectOptions();
			}
		}
	}

	public void CompleteCurrentScene()
	{
		if (CurrentScene == null)
		{
			Debug.LogError("There's no scene set on the tape manager!");
			return;
		}
		completedScenes.Add(CurrentScene);
		if (CurrentScene == KnockScene)
		{
			AudioManager.Instance.PlayKnockSFX();
		}
		if (completedScenes.Count == CurrentTape.Scenes.Length)
		{
			CompleteCurrentTape();
			return;
		}
		for (int i = 0; i < CurrentTape.Scenes.Length; i++)
		{
			if (!completedScenes.Contains(CurrentTape.Scenes[i]))
			{
				CurrentScene = CurrentTape.Scenes[i];
				PlayVideo(CurrentScene.PreInteractionClip);
				OnVideoEndEvent += SetupSceneInteraction;
				break;
			}
		}
		ClickInput.DisableAllClickableInteractions();
		TypingInput.ForceCorrectInput = false;
		interactionFailures = 0;
	}

	public void CompleteCurrentTape()
	{
		AnimationManager.Instance.PlayAnimation(AnimationManager.QueueableAnimations.AnimCameraZoomOut);
		AnimationManager.Instance.ClearAnimationEvent();
		AnimationManager.Instance.OnAnimationEndEvent += EndTape;
		PlayVideo(VHSBlueScreen, loop: true);
		this.OnVideoEndEvent = null;
	}

	private void EndTape()
	{
		Debug.Log("This is the sequence end event!");
		AnimationManager.Instance.PlayAnimation(CurrentTape.PostZoomAnim);
		AnimationManager.Instance.ClearAnimationEvent();
		AnimationManager.Instance.OnAnimationEndEvent += PostZoomOut;
	}

	public void PostZoomOut()
	{
		Object.Destroy(ClickableTapes[clickedTapes].gameObject);
		CursorManager.Instance.SetCursorVisible(visible: true);
		clickedTapes++;
		ClickableTapes[clickedTapes].SetClickable(canClick: true);
	}
}
