using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
	public float VisionOnOpacity = .9f;
	public float VisionOffOpacity = .8f;
	public float VisionOnColorAmt = .34f;
	public float VisionOffColorAmt = 1f;
	public float ToggleVisionTime = 1f;

	private bool visionActive = false;

	public ParticleSystemRenderer particleSystemRenderer;

	private Coroutine tween;

    // Start is called before the first frame update
    void Start()
    {
		setParticleSystemValues(VisionOffOpacity, VisionOffColorAmt);
		visionActive = false;
	}

    public void StartVision()
	{
		if (tween != null)
		{
			StopCoroutine(tween);
		}

		tween = StartCoroutine(tweenVision(ToggleVisionTime, VisionOffOpacity, VisionOnOpacity, VisionOffColorAmt, VisionOnColorAmt));
		visionActive = true;

	}

	public void StopVision()
	{
		if (!visionActive)
		{
			return;
		}

		if (tween != null)
		{
			StopCoroutine(tween);
		}
		tween = StartCoroutine(tweenVision(ToggleVisionTime, VisionOnOpacity, VisionOffOpacity, VisionOnColorAmt, VisionOffColorAmt));
		visionActive = false;
	}

	private IEnumerator tweenVision(float visionTime, float startOpacity, float endOpacity, float startColorAmt, float endColorAmt)
	{
 		float totalTime = Mathf.Max(visionTime, .01f);
		while (visionTime > 0)
		{
			visionTime = Mathf.Max(0, visionTime - Time.deltaTime);

			float opacity = Mathf.Lerp(startOpacity, endOpacity, 1 - visionTime / totalTime);
			float colorAmt = Mathf.Lerp(startColorAmt, endColorAmt, 1 - visionTime / totalTime);
			setParticleSystemValues(opacity, colorAmt);

			yield return null;
		}
	}

	private void setParticleSystemValues(float opacity, float colorAmt)
	{
		particleSystemRenderer.material.SetFloat("_Alpha", opacity);
		particleSystemRenderer.material.SetFloat("_ColorAmt", colorAmt);

	}
}
