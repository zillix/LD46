using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaBobber : MonoBehaviour
{
	private static float GLOBAL_BOB_MULT = 1f;

	public float BobMagnitude = .2f;
	public float BobAngleSpeed = 90;

	private float angle;
	private float startAlpha;

	public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		if (spriteRenderer)
		{
			startAlpha = spriteRenderer.color.a;
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (spriteRenderer != null)
		{
			angle += Time.deltaTime * BobAngleSpeed;
			float alpha = Mathf.Sin(Mathf.Deg2Rad * angle) * BobMagnitude + startAlpha;
			Color newColor = spriteRenderer.color;
			newColor.a = alpha;
			spriteRenderer.color = newColor;
		}
    }
}
