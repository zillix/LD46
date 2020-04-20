using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandCameraController : MonoBehaviour
{
	public float CameraTweenTime = 1f;

	public Vector2 CameraBobMagnitude = new Vector2(0, 0);
	public float CameraBobAngleSpeed = 360f;
	public Camera camera;

	public Transform startTransform;

	private float cameraBobAngle;
	private Transform targetTransform;
	private Coroutine tween;

	public Transform mawPoint;
	public Transform peakPoint;
	public Transform overlookPoint;
	public Transform cavernPoint;
	public Transform archPoint;
	public Transform centerPoint;
	public Transform templePoint;
	public Transform spirePoint;
	public Transform woodPoint;
	public Transform keepPoint;

	public void Awake()
	{

		ForceTransform(startTransform);
	}

	public Transform GetViewpoint(StoryData.CameraViewpoint viewpoint)
	{
		switch (viewpoint)
		{
			case StoryData.CameraViewpoint.arch:
				return archPoint;
			case StoryData.CameraViewpoint.cavern:
				return cavernPoint;
			case StoryData.CameraViewpoint.center:
				return centerPoint;
			case StoryData.CameraViewpoint.keep:
				return keepPoint;
			case StoryData.CameraViewpoint.maw:
				return mawPoint;
			case StoryData.CameraViewpoint.none:
				return null;
			case StoryData.CameraViewpoint.overlook:
				return overlookPoint;
			case StoryData.CameraViewpoint.peak:
				return peakPoint;
			case StoryData.CameraViewpoint.spire:
				return spirePoint;
			case StoryData.CameraViewpoint.temple:
				return templePoint;
			case StoryData.CameraViewpoint.wood:
				return woodPoint;
			default:return null;
		}
	}

	public void Start()
	{
	}

	public void Update()
	{
		/*(if (targetTransform != null)
		{
			cameraBobAngle += CameraBobAngleSpeed * Time.deltaTime;
			Vector3 targetPos = targetTransform.position;
			transform.position = targetPos + new Vector3(CameraBobMagnitude.x * Mathf.Cos(Mathf.Deg2Rad * cameraBobAngle), CameraBobMagnitude.y * Mathf.Sin(Mathf.Deg2Rad * cameraBobAngle), 0);
		}*/
	}

	public void ForceTransform(Transform target)
	{
		targetTransform = target;
		transform.position = targetTransform.position;
	}

	public void TweenToPosition(Transform target)
	{
		targetTransform = target;
		if (tween != null)
		{
			StopCoroutine(tween);
		}
		tween = StartCoroutine(startTween(CameraTweenTime, targetTransform, transform.position));
	}

	private IEnumerator startTween(float tweenTime, Transform target, Vector3 startPosition)
	{
		float totalTime = Mathf.Max(tweenTime, .01f);
		while (tweenTime > 0)
		{
			tweenTime = Mathf.Max(0, tweenTime - Time.deltaTime);

			Vector3 targetPosition = Vector3.Lerp(startPosition, target.position, 1 - (tweenTime / totalTime));
			transform.position = targetPosition;

			yield return null;
		}
		tween = null;
	}

}
