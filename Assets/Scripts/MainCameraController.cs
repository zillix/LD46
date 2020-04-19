using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class MainCameraController : MonoBehaviour
{
	public Image CameraFlash;

	public void Flash(Color flashColor, Action callback = null, float fadeIn = .3f, float wait = .01f, float maxAlpha = .8f)
	{
		CameraFlash.enabled = true;
		CameraFlash.color = new Color(0, 0, 0, 0);
		StartCoroutine(performFlash(flashColor, fadeIn, wait, maxAlpha, callback));
	}

	public IEnumerator Fade(float startAlpha,
		float endAlpha,
		float waitDuration,
		Color color)
	{
		Image image = CameraFlash;
		if (image.color.a == startAlpha)
		{
			color.a = startAlpha;
			image.color = color;

			for (float i = 0; i < 1.0; i += Time.deltaTime * (1 / waitDuration))
			{ //for the length of time

				color.a = Mathf.Lerp(startAlpha, endAlpha, i);
				image.color = color;
				yield return null;
				color.a = endAlpha;
				image.color = color;
			} //end for
		}
	}

	private IEnumerator performFlash(Color color, float fadeIn, float wait, float maxAlpha, Action callback = null)
	{
		StartCoroutine(Fade(0, maxAlpha, fadeIn, color));
		yield return new WaitForSeconds(fadeIn + wait);

		if (callback != null)

		{
			callback();
		}
		StartCoroutine(Fade(maxAlpha, 0, fadeIn, color));
	}
}
