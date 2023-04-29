using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	private static MenuManager _instance;

	public float DelayBeforeCutToBlack = 2f;

	public Image EndingBG;

	public float DelayBeforeCreditsFadeIn = 1f;

	public float CreditsFadeInTime = 2f;

	public Image CreditsTitle;

	public TextMeshProUGUI[] TextCredits;

	public float DelayBeforeButtonFadeIn = 1f;

	public Button[] CreditButtons;

	public Image[] ButtonBGs;

	public TextMeshProUGUI[] ButtonTexts;

	public Image TitleImg;

	public float TitleHoldTime = 2f;

	public ClickableObject FirstTape;

	public static MenuManager Instance => _instance;

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

	public void Start()
	{
		StartCoroutine(TitleFade());
	}

	private IEnumerator TitleFade()
	{
		yield return new WaitForSeconds(1f);
		float percentage2 = 0f;
		while (TitleImg.color != Color.white)
		{
			percentage2 += Time.deltaTime;
			TitleImg.color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, percentage2 / CreditsFadeInTime);
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForSeconds(TitleHoldTime);
		percentage2 = 0f;
		while (TitleImg.color != new Color(1f, 1f, 1f, 0f))
		{
			percentage2 += Time.deltaTime;
			TitleImg.color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), percentage2 / CreditsFadeInTime);
			yield return new WaitForEndOfFrame();
		}
		FirstTape.SetClickable(canClick: true);
	}

	public void StartEnding()
	{
		StartCoroutine(EndingRoutine());
	}

	public IEnumerator EndingRoutine()
	{
		yield return new WaitForSeconds(DelayBeforeCutToBlack);
		EndingBG.gameObject.SetActive(value: true);
		yield return new WaitForSeconds(DelayBeforeCreditsFadeIn);
		AudioManager.Instance.PlayMainTheme();
		CreditsTitle.gameObject.SetActive(value: true);
		for (int i = 0; i < TextCredits.Length; i++)
		{
			TextCredits[i].gameObject.SetActive(value: true);
		}
		float percentage2 = 0f;
		CreditsTitle.color = Color.clear;
		while (CreditsTitle.color != Color.white)
		{
			percentage2 += Time.deltaTime;
			CreditsTitle.color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, percentage2 / CreditsFadeInTime);
			for (int j = 0; j < TextCredits.Length; j++)
			{
				TextCredits[j].color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, percentage2 / CreditsFadeInTime);
			}
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForSeconds(DelayBeforeButtonFadeIn);
		for (int k = 0; k < CreditButtons.Length; k++)
		{
			CreditButtons[k].gameObject.SetActive(value: true);
		}
		percentage2 = 0f;
		ButtonBGs[0].color = Color.clear;
		while (ButtonBGs[0].color != Color.white)
		{
			percentage2 += Time.deltaTime;
			for (int l = 0; l < ButtonBGs.Length; l++)
			{
				ButtonBGs[l].color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, percentage2 / CreditsFadeInTime);
				ButtonTexts[l].color = Color.Lerp(new Color(0f, 0f, 0f, 0f), Color.black, percentage2 / CreditsFadeInTime);
			}
			yield return new WaitForEndOfFrame();
		}
		for (int m = 0; m < CreditButtons.Length; m++)
		{
			CreditButtons[m].enabled = true;
		}
		CursorManager.Instance.SetCursorVisible(visible: true);
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(0);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
