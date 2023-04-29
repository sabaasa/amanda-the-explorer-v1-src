using UnityEngine;

public class ClickInput : MonoBehaviour
{
	public ClickInteraction Tape1StoreClickables;

	public ClickInteraction Tape2ButcherClickables;

	public ClickInteraction Tape3HouseClickables;

	public ClickInteraction Tape3DoorClickables;

	private ClickInteraction currentClickable;

	private void Start()
	{
		DisableAllClickableInteractions();
	}

	public void InitalizeClickableScene(TapeSceneClickables clickables)
	{
		CursorManager.Instance.SetCursorVisible(visible: true);
		DisableAllClickableInteractions();
		switch (clickables)
		{
		case TapeSceneClickables.Tape1Store:
			currentClickable = Tape1StoreClickables;
			break;
		case TapeSceneClickables.Tape2Butcher:
			currentClickable = Tape2ButcherClickables;
			break;
		case TapeSceneClickables.Tape3Door:
			currentClickable = Tape3DoorClickables;
			break;
		case TapeSceneClickables.Tape3House:
			currentClickable = Tape3HouseClickables;
			break;
		default:
			Debug.LogError("Unexpected clickable");
			break;
		}
		currentClickable.gameObject.SetActive(value: true);
	}

	public void OptionClicked(bool correctOption)
	{
		if (correctOption)
		{
			TapeManager.Instance.CompleteCurrentScene();
		}
		else
		{
			TapeManager.Instance.FailCurrentScene();
		}
		currentClickable.gameObject.SetActive(value: false);
		CursorManager.Instance.SetCursorVisible(visible: false);
	}

	public void HideIncorrectOptions()
	{
		for (int i = 0; i < currentClickable.incorrectObjects.Length; i++)
		{
			currentClickable.incorrectObjects[i].gameObject.SetActive(value: false);
		}
	}

	public void DisableAllClickableInteractions()
	{
		Tape1StoreClickables.gameObject.SetActive(value: false);
		Tape2ButcherClickables.gameObject.SetActive(value: false);
		Tape3HouseClickables.gameObject.SetActive(value: false);
		Tape3DoorClickables.gameObject.SetActive(value: false);
	}
}
