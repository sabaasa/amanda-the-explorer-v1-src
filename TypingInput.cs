using System.Collections;
using TMPro;
using UnityEngine;

public class TypingInput : MonoBehaviour
{
	public delegate void OnOverflowEndEventHandler();

	public TMP_InputField inputField;

	public TextMeshProUGUI InputText;

	public TextMeshProUGUI PlaceholderLines;

	public TextMeshProUGUI OverflowText;

	public ParticleSystem backgroundParticles;

	public string CorrectWord;

	public bool LimitInputSize = true;

	public bool ForceCorrectInput;

	public bool AllowAnyInput;

	public bool UseOverflow;

	public float textFadeTime = 0.5f;

	public Material TVReflectionMat;

	public MeshRenderer TVRenderer;

	private Coroutine textLerpRoutine;

	private float percentage;

	private bool overflowing;

	public float OverflowTypeTime = 0.05f;

	public event OnOverflowEndEventHandler OnOverflowEnd;

	private void Start()
	{
		ToggleTypingInputActive(active: false, quick: true);
		OnOverflowEnd += OverflowEnd;
	}

	public void ToggleTypingInputActive(bool active, bool quick = false)
	{
		if (quick)
		{
			if (active)
			{
				inputField.enabled = true;
				backgroundParticles.gameObject.SetActive(value: true);
				backgroundParticles.Play();
				PlaceholderLines.color = Color.white;
				InputText.color = Color.white;
				OverflowText.color = Color.white;
				return;
			}
			inputField.enabled = false;
			backgroundParticles.gameObject.SetActive(value: false);
			backgroundParticles.Stop();
			if (textLerpRoutine != null)
			{
				StopCoroutine(textLerpRoutine);
			}
			PlaceholderLines.color = Color.clear;
			InputText.color = Color.clear;
			OverflowText.color = Color.clear;
		}
		else
		{
			if (textLerpRoutine != null)
			{
				Debug.LogError("The previous text routine hasn't finished yet! How short is this video?!");
			}
			if (active)
			{
				inputField.enabled = true;
				backgroundParticles.gameObject.SetActive(value: true);
				backgroundParticles.Play();
				textLerpRoutine = StartCoroutine(LerpTextColorRoutine(lerpIn: true));
			}
			else
			{
				inputField.enabled = false;
				backgroundParticles.Stop();
				textLerpRoutine = StartCoroutine(LerpTextColorRoutine(lerpIn: false));
			}
		}
	}

	private IEnumerator LerpTextColorRoutine(bool lerpIn)
	{
		percentage = 0f;
		if (lerpIn)
		{
			while (PlaceholderLines.color != Color.white)
			{
				percentage += Time.deltaTime;
				PlaceholderLines.color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, percentage / textFadeTime);
				InputText.color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, percentage / textFadeTime);
				OverflowText.color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, percentage / textFadeTime);
				yield return new WaitForEndOfFrame();
			}
		}
		else
		{
			while (PlaceholderLines.color != new Color(1f, 1f, 1f, 0f))
			{
				percentage += Time.deltaTime;
				PlaceholderLines.color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), percentage / textFadeTime);
				InputText.color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), percentage / textFadeTime);
				OverflowText.color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), percentage / textFadeTime);
				yield return new WaitForEndOfFrame();
			}
		}
		textLerpRoutine = null;
	}

	public void InitalizeCorrectWord(string word, bool allowAnyInput, bool overflow)
	{
		AllowAnyInput = allowAnyInput;
		UseOverflow = overflow;
		inputField.text = "";
		CorrectWord = word;
		if (LimitInputSize && !overflow)
		{
			inputField.characterLimit = CorrectWord.Length;
		}
		else
		{
			inputField.characterLimit = 0;
		}
		PlaceholderLines.text = "";
		PlaceholderLines.maxVisibleCharacters = CorrectWord.Length;
		for (int i = 0; i < CorrectWord.Length - inputField.text.Length; i++)
		{
			PlaceholderLines.text += "_";
		}
		ToggleTypingInputActive(active: true);
	}

	private void Update()
	{
		inputField.Select();
		inputField.ActivateInputField();
		inputField.caretPosition = inputField.text.Length;
	}

	public void Event_InputChanged()
	{
		PlaceholderLines.maxVisibleCharacters = CorrectWord.Length - inputField.text.Length;
		if (overflowing)
		{
			return;
		}
		if (ForceCorrectInput && inputField.text != CorrectWord)
		{
			string text = "";
			for (int i = 0; i < inputField.text.Length; i++)
			{
				text += CorrectWord[i];
			}
			inputField.text = text;
		}
		else if (!AllowAnyInput && !UseOverflow)
		{
			if (inputField.text.ToLower() == CorrectWord.ToLower())
			{
				Debug.Log("Scene completed");
				TapeManager.Instance.CompleteCurrentScene();
				ToggleTypingInputActive(active: false);
			}
			else if (inputField.text.Length == CorrectWord.Length)
			{
				TapeManager.Instance.FailCurrentScene();
				ToggleTypingInputActive(active: false);
			}
		}
		else if (AllowAnyInput)
		{
			if (inputField.text.ToLower() == "yes" || inputField.text.ToLower() == "yee" || inputField.text.ToLower() == "aye" || inputField.text.ToLower() == "yea" || inputField.text.ToLower() == "yep" || inputField.text.ToLower() == "ok" || inputField.text.ToLower() == "oui" || inputField.text.ToLower() == "yep" || inputField.text.ToLower() == "no" || inputField.text.ToLower() == "nay" || inputField.text.ToLower() == "nix" || inputField.text.ToLower() == "eh" || inputField.text.ToLower() == "nah" || inputField.text.Length == CorrectWord.Length)
			{
				TapeManager.Instance.CompleteCurrentScene();
				ToggleTypingInputActive(active: false);
			}
		}
		else if (inputField.text.Length == 3)
		{
			StartCoroutine(OverflowRoutine());
			overflowing = true;
		}
	}

	private IEnumerator OverflowRoutine()
	{
		AudioManager.Instance.PlayMonsterSound();
		string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		for (int i = 0; i < 32; i++)
		{
			inputField.text += letters[Random.Range(0, letters.Length)];
			inputField.textComponent.enableWordWrapping = true;
			yield return new WaitForSeconds(OverflowTypeTime);
		}
		for (int i = 0; i < 29; i++)
		{
			OverflowText.text += letters[Random.Range(0, letters.Length)];
			yield return new WaitForSeconds(OverflowTypeTime);
		}
		ToggleTypingInputActive(active: false, quick: true);
		PlaceholderLines.gameObject.SetActive(value: false);
		InputText.gameObject.SetActive(value: false);
		OverflowText.gameObject.SetActive(value: false);
		yield return new WaitForEndOfFrame();
		if (this.OnOverflowEnd != null)
		{
			this.OnOverflowEnd();
		}
	}

	public void OverflowEnd()
	{
		Material[] sharedMaterials = TVRenderer.sharedMaterials;
		sharedMaterials[1] = TVReflectionMat;
		TVRenderer.sharedMaterials = sharedMaterials;
		MenuManager.Instance.StartEnding();
	}
}
