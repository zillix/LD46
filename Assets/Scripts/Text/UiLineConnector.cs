using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiLineConnector : MonoBehaviour
{
	public Transform worldTransform;
	public RectTransform uiTransform;
	public LineRenderer lineRenderer;
	public float speed;
	public int segments;
	public float wiggleMagnitudeStart = 1;
	public float wiggleMagnitudeEnd = 1;
	public float wiggleFrequency;
	public float wiggleRotationSpeed;
	public float startWidth;
	public float endWidth;

	private float wiggleAngle = 0;
	private bool isVisible = true;

	// Start is called before the first frame update
	void Start()
    {
		Vector3 pos = transform.position;
		pos.z = -.2f;
		transform.position = pos;

		lineRenderer.positionCount = 2 + segments;
		lineRenderer.startWidth = startWidth;
		lineRenderer.endWidth = endWidth;
		//StartCoroutine(AnimateLineBetween(worldTransform, uiTransform));
	}

	public void SetAlpha(float alpha)
	{
		Color start = lineRenderer.startColor;
		Color end = lineRenderer.endColor;
		start.a = alpha;
		end.a = alpha;
		lineRenderer.startColor = start;
		lineRenderer.endColor = end;
	}

	public void SetVisible(bool visible)
	{
		if (visible != isVisible)
		{
			isVisible = visible;
			lineRenderer.positionCount = (visible ? 2 + segments : 0);
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (!isVisible)
		{
			return;
		}

		wiggleAngle += wiggleRotationSpeed * Time.deltaTime;

		Vector3 a = worldTransform.position;
		Vector3 bScreen = uiTransform.position;
		bScreen.z = a.z - Camera.main.transform.position.z;
		Vector3 b = Camera.main.ScreenToWorldPoint(bScreen);
		b.z = a.z;

		float dist = (a - b).magnitude;
		Vector3 delta = (b - a) / Mathf.Max(segments + 1, 1);

		// set first point
		lineRenderer.SetPosition(0, a);

		for (int i = 0; i < segments; ++i)
		{
			Vector3 pos = a + delta * (i + 1);
			Vector3 right = new Vector3(-delta.y, delta.x, 0).normalized;

			float wiggleMagnitude = i / (float)segments * (wiggleMagnitudeEnd - wiggleMagnitudeStart) + wiggleMagnitudeStart; 
			float offset = Mathf.Sin(Mathf.Deg2Rad * (wiggleAngle + wiggleFrequency * (i + 1) / segments)) * wiggleMagnitude;

			lineRenderer.SetPosition(i + 1, pos + offset * right);
		}

		// initialize last point
		lineRenderer.SetPosition(segments + 1, b);
	}

	IEnumerator AnimateLineBetween(Transform worldTransform, Transform uiTransform)
	{
		Vector3 a = worldTransform.position;
		Vector3 b = Camera.main.ScreenToWorldPoint(uiTransform.position);

		// set first point
		lineRenderer.SetPosition(0, a);
		// initialize second point
		lineRenderer.SetPosition(1, a);

		// the distance (and direction) between the two points
		Vector3 distance = b - a;
		for (float i = 0; i < 1; i += speed / 200)
		{
			// each frame, advance a fraction of the way
			lineRenderer.SetPosition(1, distance * i);
			yield return null;
		}
	}
}
