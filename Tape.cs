using UnityEngine;

[CreateAssetMenu(fileName = "Tape", menuName = "Tape", order = 1)]
public class Tape : ScriptableObject
{
	public TapeScene[] Scenes;

	public AnimationManager.QueueableAnimations PreZoomAnim;

	public AnimationManager.QueueableAnimations PostZoomAnim;
}
