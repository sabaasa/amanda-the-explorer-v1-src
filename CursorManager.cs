using UnityEngine;

public class CursorManager : MonoBehaviour
{
	private static CursorManager _instance;

	public Texture2D NormalCursor;

	public Texture2D ClickCursor;

	public static CursorManager Instance => _instance;

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

	private void Start()
	{
		SetNormalCursor();
	}

	public void SetClickCursor()
	{
		Cursor.SetCursor(ClickCursor, Vector2.zero, CursorMode.ForceSoftware);
	}

	public void SetNormalCursor()
	{
		Cursor.SetCursor(NormalCursor, Vector2.zero, CursorMode.ForceSoftware);
	}

	public void SetCursorVisible(bool visible)
	{
		if (visible)
		{
			Cursor.SetCursor(NormalCursor, Vector2.zero, CursorMode.ForceSoftware);
		}
		else
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
		}
		Cursor.visible = visible;
	}
}
