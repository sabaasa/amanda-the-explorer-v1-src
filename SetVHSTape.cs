using UnityEngine;

[RequireComponent(typeof(ClickableObject))]
public class SetVHSTape : MonoBehaviour
{
	public Tape TapeToSet;

	private ClickableObject clickable;

	private void Awake()
	{
		clickable = GetComponent<ClickableObject>();
		clickable.OnClickEvent += Event_OnClick;
	}

	public void Event_OnClick()
	{
		TapeManager.Instance.SetTape(TapeToSet);
	}
}
