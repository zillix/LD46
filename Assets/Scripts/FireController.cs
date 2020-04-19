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

	public float PlayerTriggerDist = 2f;
	public Player player;

	private bool visionActive = false;
	private bool playerNear = false;

	public ParticleSystemRenderer particleSystemRenderer;
	public ParticleSystem emitter;

	public float OffSize = .2f;
	public float OffSpeed = .1f;
	public float OffLifetime = 2f;
	public float OffRadialSpeed = 0;
	public float OffOrbitalX = 0f;
	public float OffLinearY = 1f;

	private float onSize;
	private float onSpeed;
	private float onLifetime;
	private float radialSpeed;
	private float orbitalX;
	private float linearY;

	private Coroutine tween;
	private bool isOn = false;
	private ParticleSystem.MainModule particleOnModule;

    // Start is called before the first frame update
    void Start()
    {
		setParticleSystemValues(VisionOffOpacity, VisionOffColorAmt);
		visionActive = false;
	
		ParticleSystem.MainModule offModule = emitter.main;
		onSize = offModule.startSizeMultiplier;
		onSpeed = offModule.startSpeedMultiplier;
		onLifetime = offModule.startLifetimeMultiplier;
		ParticleSystem.VelocityOverLifetimeModule offVel = emitter.velocityOverLifetime;
		orbitalX = offVel.orbitalXMultiplier;
		linearY = offVel.yMultiplier;
		radialSpeed = offVel.radialMultiplier;

		offModule.startSize = OffSize;
		offModule.startSpeed = OffSpeed;
		offModule.startLifetime = OffLifetime;
		offVel.yMultiplier = OffLinearY;
		offVel.radialMultiplier = OffRadialSpeed;
		offVel.orbitalX = OffOrbitalX;
	}

	public void TurnOn()
	{
		ParticleSystem.MainModule main = emitter.main;
		main.startSize = onSize;
		main.startSpeed = onSpeed;
		main.startLifetime = onLifetime;
		ParticleSystem.VelocityOverLifetimeModule vel = emitter.velocityOverLifetime;
		vel.yMultiplier = linearY;
		vel.radialMultiplier = radialSpeed;
		vel.orbitalX = orbitalX;

	}

	public void Update()
	{
		bool lastPlayerNear = playerNear;
		playerNear = Mathf.Abs(player.transform.position.x - transform.position.x) < PlayerTriggerDist;
		if (lastPlayerNear != playerNear)
		{
			if (visionActive && !playerNear)
			{
				if (tween != null)
					StopCoroutine(tween);
				tween = StartCoroutine(tweenVision(ToggleVisionTime, CurrentAlpha, VisionOffOpacity, CurrentColorAmt, VisionOffColorAmt));

			}
			if (visionActive && playerNear)
			{
				if (tween != null)
					StopCoroutine(tween);
				tween = StartCoroutine(tweenVision(ToggleVisionTime, CurrentAlpha, VisionOnOpacity, CurrentColorAmt, VisionOnColorAmt));

			}
		}
	}

	public void StartVision()
	{

		visionActive = true;
		if (!playerNear) { return; }

		if (tween != null)
		{
			StopCoroutine(tween);
		}

		tween = StartCoroutine(tweenVision(ToggleVisionTime, CurrentAlpha, VisionOnOpacity, CurrentColorAmt, VisionOnColorAmt));

	}

	public void StopVision()
	{
		if (!visionActive)
		{
			return;
		}
		visionActive = false;

		if (!playerNear) { return; }

		if (tween != null)
		{
			StopCoroutine(tween);
		}
		tween = StartCoroutine(tweenVision(ToggleVisionTime, CurrentAlpha, VisionOffOpacity, CurrentColorAmt, VisionOffColorAmt));
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

	private float CurrentAlpha
	{
		get
		{
			return particleSystemRenderer.material.GetFloat("_Alpha");
		}
	}
	private float CurrentColorAmt
	{
		get
		{
			return particleSystemRenderer.material.GetFloat("_ColorAmt");
		}
	}

	private void setParticleSystemValues(float opacity, float colorAmt)
	{
		particleSystemRenderer.material.SetFloat("_Alpha", opacity);
		particleSystemRenderer.material.SetFloat("_ColorAmt", colorAmt);

	}
}
