using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class ChoiceElement : MonoBehaviour
{

	public Image selectedImage;
	public TMP_Text textField;

	public float BlinkDuration = .5f;
	public float BlinkWait = .1f;

	private Action callback;

	private Coroutine blinkCoroutine = null;

	public void Initialize(string text, Action callback)
	{
		textField.text = text;
		this.callback = callback;

	}

	public void Abort()
	{
		if (blinkCoroutine != null)
		{
			StopCoroutine(blinkCoroutine);
		}
	}

	public void SetSelected(bool isSelected)
	{
		selectedImage.enabled = isSelected;
	}

	public void OnChosen()
	{
		if (blinkCoroutine != null)
		{
			return;
		}


		Sounds.PlayOneShot(Sounds.instance.questionChosen, .6f);

		blinkCoroutine = StartCoroutine(blinkThenCallback(BlinkDuration, callback));
	}



	private IEnumerator blinkThenCallback(float waitTime, Action callback)
	{
		while (waitTime > 0)
		{
			waitTime -= Time.deltaTime + BlinkWait;
			yield return new WaitForSeconds(BlinkWait);
			textField.enabled = !textField.enabled;

		}

		textField.enabled = true;

		if (callback != null)
		{
			callback();
		}
	}
}
