using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "TapeScene", menuName = "TapeScene", order = 1)]
public class TapeScene : ScriptableObject
{
	public VideoClip PreInteractionClip;

	public VideoClip InteractionFailure1;

	public VideoClip InteractionFailure2;

	public InteractionMode InteractMode;

	[HideInInspector]
	public string correctWord;

	[HideInInspector]
	public bool allowAnyInput;

	[HideInInspector]
	public bool overflow;

	[HideInInspector]
	public TapeSceneClickables clickables;
}
