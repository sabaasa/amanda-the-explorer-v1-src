using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ClickableObject : MonoBehaviour
{
	public delegate void OnClickEventHandler();

	public bool DisableOnClick = true;

	public bool CanClickDefaultValue = true;

	public bool HideCursorOnClick;

	private Outline outlineComponent;

	public bool CanClick { get; private set; }

	public event OnClickEventHandler OnClickEvent;

	private void Awake()
	{
		SetClickable(CanClickDefaultValue);
	}

	public void SetClickable(bool canClick)
	{
		CanClick = canClick;
		if (outlineComponent != null)
		{
			Object.Destroy(outlineComponent);
		}
	}

	private void OnMouseEnter()
	{
		if (CanClick)
		{
			CursorManager.Instance.SetClickCursor();
		}
	}

	private void OnMouseExit()
	{
		if (CanClick)
		{
			CursorManager.Instance.SetNormalCursor();
		}
	}

	private void OnMouseDown()
	{
		if (CanClick)
		{
			CursorManager.Instance.SetNormalCursor();
			if (this.OnClickEvent != null)
			{
				this.OnClickEvent();
			}
			else
			{
				Debug.LogError("OnClickEvent Null", base.gameObject);
			}
			if (DisableOnClick)
			{
				SetClickable(canClick: false);
			}
			if (HideCursorOnClick)
			{
				CursorManager.Instance.SetCursorVisible(visible: false);
			}
		}
	}
}
