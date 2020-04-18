using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingTextBox : TextBox
{
	public Transform follow;
	public Transform scaleTransform;
	public UiLineConnector connector;
	public bool followX;
	public bool followY;
	public Vector2 followDist = new Vector2(2, 2);
	public Vector2 dampMult = new Vector2(1, 1);
	public Vector2 followSpeed = new Vector2(0, 0);

	public Vector3 targetOffset = new Vector3(0, -5);

	public bool moveHide = false; // otherwise just disappear
	public Vector3 hideOffset = new Vector3(0, 0);
	public Vector2 hideDampMult = new Vector2(100, 100);
	public Vector2 hideSpeed = new Vector2(0, 0);

	public float BobMagnitude = 10f;
	public float BobAngleSpeed = 360f;

	private float bobAngle = 0f;



	public float hideScaleSpeed = 1f;

	private float baseScale = 1f;
	private bool isVisible = false;
	private Vector3 activeOffset { get { return isVisible ? targetOffset : hideOffset; } }
	private Vector2 activeFollowDist {  get { return isVisible ? followDist : Vector2.zero; } }
	private Vector2 activeDampMult {  get { return isVisible ? dampMult : hideDampMult; } }
	private Vector2 activeSpeed { get { return isVisible ? followSpeed : hideSpeed; } }

	public Vector3 bobOffset {  get { return new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * bobAngle) * BobMagnitude, 0); } }


	// Start is called before the first frame update
	protected override void Start()
    {
		base.Start();
		hide();
		baseScale = scaleTransform.localScale.x;
    }

	public override void hide()
	{
		if (moveHide)
		{
			isVisible = false;
		}
		else
		{
			base.hide();
			if (connector)
			{
				connector.SetVisible(false);
			}
		}
	}

	public override void show()
	{
		if (moveHide)
		{
			isVisible = true;
		}
		else
		{
			base.show();
			if (connector)
			{
				connector.SetVisible(true);
			}
		}
	}

	// Update is called once per frame
	protected override void Update()
    {
		base.Update();

		bobAngle += Time.deltaTime * BobAngleSpeed;

		Vector3 followPos = Camera.main.WorldToScreenPoint(follow.position);
		Vector3 myPos = transform.position;

		Vector3 targetPos = followPos + activeOffset + bobOffset;

		if (followX)
		{
			float deltaX = myPos.x - targetPos.x;
			if (Mathf.Abs(deltaX) > activeFollowDist.x)
			{
				float targetXPos = targetPos.x + (deltaX > 0 ? 1 : -1) * activeFollowDist.x;
				float xSpeed = activeDampMult.x * deltaX * -1 + (deltaX > 0 ? -1 : 1) * activeSpeed.x;
				float deltaPos = Time.deltaTime * xSpeed;
				myPos.x = Mathf.Abs(deltaPos) > Mathf.Abs(deltaX) ? targetXPos : myPos.x + deltaPos;
			}
			transform.position = myPos;
		}
		if (followY)
		{
			float deltaY = myPos.y - targetPos.y;
			if (Mathf.Abs(deltaY) > activeFollowDist.y)
			{
				float targetYPos = targetPos.y + (deltaY > 0 ? 1 : -1) * activeFollowDist.y;
				float ySpeed = activeDampMult.y * deltaY * -1 + (deltaY > 0 ? -1 : 1) * activeSpeed.y;
				float deltaPos = Time.deltaTime * ySpeed;
				myPos.y = Mathf.Abs(deltaPos) > Mathf.Abs(deltaY) ? targetYPos : myPos.y + deltaPos;
			}
			transform.position = myPos;
		}

		if (moveHide)
		{
			Vector2 scale = scaleTransform.localScale;
			float lastScale = scale.x;
			if (isVisible)
			{
				scale.x = scale.y = Mathf.Min(baseScale, scale.x + Time.deltaTime * hideScaleSpeed);
			}
			else
			{
				scale.x = scale.y = Mathf.Max(0, scale.x - Time.deltaTime * hideScaleSpeed);
			}
			scaleTransform.localScale = scale;
			if (lastScale != scale.x)
			{
				float relativeScale = scale.x / baseScale;
				base.setAlpha(relativeScale);
				if (connector)
				{
					connector.SetAlpha(relativeScale);
					if (Mathf.Abs(relativeScale) < .01f)
					{
						connector.SetVisible(false);
					}
					else
					{
						connector.SetVisible(true);
					}
				}
			}
		}
		
    }
}
