using System.Collections;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
	public delegate void OnSequenceEndEventHandler();

	public enum QueueableAnimations
	{
		AnimCameraZoomIn = 0,
		AnimCameraZoomOut = 1,
		AnimTape1Insert = 2,
		AnimTape1Eject = 3,
		AnimTape2Insert = 4,
		AnimTape2Eject = 5,
		AnimTape3Insert = 6,
		AnimTape3Eject = 7
	}

	private static AnimationManager _instance;

	[SerializeField]
	private AnimatorAnim AnimCameraZoomIn;

	[SerializeField]
	private AnimatorAnim AnimCameraZoomOut;

	[SerializeField]
	private AnimatorAnim AnimTape1Insert;

	[SerializeField]
	private AnimatorAnim AnimTape1Eject;

	[SerializeField]
	private AnimatorAnim AnimTape2Insert;

	[SerializeField]
	private AnimatorAnim AnimTape2Eject;

	[SerializeField]
	private AnimatorAnim AnimTape3Insert;

	[SerializeField]
	private AnimatorAnim AnimTape3Eject;

	private Coroutine currentAnimationRoutine;

	public static AnimationManager Instance => _instance;

	public event OnSequenceEndEventHandler OnAnimationEndEvent;

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

	public void ClearAnimationEvent()
	{
		this.OnAnimationEndEvent = null;
	}

	public void PlayAnimation(QueueableAnimations queuedAnimationEnums)
	{
		if (currentAnimationRoutine != null)
		{
			Debug.LogError("An animation is already playing!");
			return;
		}
		AnimatorAnim animationFromEnum = GetAnimationFromEnum(queuedAnimationEnums);
		if (animationFromEnum.Animator == null || string.IsNullOrEmpty(animationFromEnum.AnimationStateName))
		{
			Debug.LogError("Animation " + queuedAnimationEnums.ToString() + " isn't set on the Animation Manager!", base.gameObject);
		}
		else
		{
			currentAnimationRoutine = StartCoroutine(AnimationRoutine(animationFromEnum));
		}
	}

	private AnimatorAnim GetAnimationFromEnum(QueueableAnimations anim)
	{
		switch (anim)
		{
		case QueueableAnimations.AnimCameraZoomIn:
			return AnimCameraZoomIn;
		case QueueableAnimations.AnimCameraZoomOut:
			return AnimCameraZoomOut;
		case QueueableAnimations.AnimTape1Eject:
			return AnimTape1Eject;
		case QueueableAnimations.AnimTape1Insert:
			return AnimTape1Insert;
		case QueueableAnimations.AnimTape2Eject:
			return AnimTape2Eject;
		case QueueableAnimations.AnimTape2Insert:
			return AnimTape2Insert;
		case QueueableAnimations.AnimTape3Eject:
			return AnimTape3Eject;
		case QueueableAnimations.AnimTape3Insert:
			return AnimTape3Insert;
		default:
			Debug.LogError("Unexpected Queueable Animation");
			return null;
		}
	}

	private IEnumerator AnimationRoutine(AnimatorAnim animatorAnim)
	{
		animatorAnim.Animator.Play(animatorAnim.AnimationStateName);
		yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(animatorAnim.Animator.GetCurrentAnimatorStateInfo(0).length);
		currentAnimationRoutine = null;
		if (this.OnAnimationEndEvent != null)
		{
			this.OnAnimationEndEvent();
		}
	}
}
