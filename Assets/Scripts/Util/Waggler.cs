using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waggler : MonoBehaviour
{
	public float waggleMagnitude = 4.5f;
	public float waggleSpeedMax = 40;
	public float waggleSpeedMin = 40;

	private float waggleSpeed = 0;

	private float startAngle;

	private float cosAngle = 0;

	public static float BASE_WAGGLE_MAG_MULT = 1f;
	public static float BASE_WAGGLE_SPEED_MULT = 1f;

	public void Start()
	{
		startAngle = transform.rotation.eulerAngles.z;

		waggleSpeed = Random.Range(waggleSpeedMin, waggleSpeedMax);
		if (Random.value < .5f) waggleSpeed *= 0.5f;
	}

	public void Update()
	{
		cosAngle += Time.deltaTime * waggleSpeed * BASE_WAGGLE_SPEED_MULT;

		float currentAngle = startAngle + (waggleMagnitude * BASE_WAGGLE_MAG_MULT) * Mathf.Cos(cosAngle * Mathf.Deg2Rad);
		Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);
		transform.localRotation = rotation;
	}
}
