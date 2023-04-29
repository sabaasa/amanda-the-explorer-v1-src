using UnityEngine;

public class ClickInteraction : MonoBehaviour
{
	public ClickableObject correctObject;

	public ClickableObject[] incorrectObjects;

	public ClickInput ClickInput;

	private void Start()
	{
		correctObject.OnClickEvent += Event_OnClickEventCorrect;
		for (int i = 0; i < incorrectObjects.Length; i++)
		{
			incorrectObjects[i].OnClickEvent += Event_OnClickEventIncorrect;
		}
	}

	public void Event_OnClickEventCorrect()
	{
		ClickInput.OptionClicked(correctOption: true);
	}

	public void Event_OnClickEventIncorrect()
	{
		ClickInput.OptionClicked(correctOption: false);
	}
}
