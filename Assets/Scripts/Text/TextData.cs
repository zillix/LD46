using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextData : ScriptableObject
{
	public string text;
	public bool skippable;

	public bool doOverridePauseDuration;
	public float pauseDuration;

	public bool doOverrideTypeSpeed;
	public float typeSpeed;

	public bool doJitter;
	public Vector2 jitterMagnitude;
	public float jitterFrequencySeconds;

}
