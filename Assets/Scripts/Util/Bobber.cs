using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour
{
	private static float GLOBAL_BOB_MULT = 1f;

	public float BobMagnitude = .2f;
	public float BobAngleSpeed = 90;
	public float BobMagnitudeX = 0f;

	private float angle;
	private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
		startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
		angle += Time.deltaTime * BobAngleSpeed;
		float bobY = Mathf.Sin(Mathf.Deg2Rad * angle) * BobMagnitude;
		float bobX = Mathf.Cos(Mathf.Deg2Rad * angle) * BobMagnitudeX;
		Vector3 newPosition = startPosition + new Vector3(bobX, bobY, 0);
		transform.position = newPosition;
    }
}
