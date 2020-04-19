using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float speed = 1;
	private float baseScale;
	public Animator animator;
	public Transform sprite;

	public Transform leftBound;
	public Transform rightBound;

	public float FootstepDelay = .6f;
	private float footstepTime = 0f;

	private bool _isTalking = false;
	public bool isTalking
	{
		get { return _isTalking; }
		set
		{
			_isTalking = value;
			animator.SetBool("IsTalking", _isTalking);
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		baseScale = sprite.localScale.x;
	}

	// Update is called once per frame
	void Update()
	{
		bool isMoving = false;
		bool isFacingRight = false;

		Vector3 scale = sprite.localScale;
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			Vector3 position = transform.position;
			float lastX = position.x;
			position.x = Mathf.Max(leftBound.position.x, position.x - speed * Time.deltaTime);
			transform.position = position;
			isMoving = lastX != position.x;
			scale.x = -1 * baseScale;
			isFacingRight = false;
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			Vector3 position = transform.position;
			float lastX = position.x;
			position.x = Mathf.Min(rightBound.position.x, position.x + speed * Time.deltaTime);
			transform.position = position;
			isMoving = lastX != position.x;
			scale.x = 1 * baseScale;
			isFacingRight = true;

		}

		if (isMoving)
		{
			footstepTime -= Time.deltaTime;
			if (footstepTime < 0)
			{
				footstepTime = FootstepDelay;
				Sounds.PlayOneShot(Sounds.instance.footstep, 1f);
			}
		}


		isFacingRight = scale.x > 0;
		if (!StoryManager.hasFiredStarted)
		{
			isFacingRight = false;
		}

		sprite.localScale = scale;

		animator.SetBool("IsMoving", isMoving);
		animator.SetBool("IsFacingRight", isFacingRight);

	}
}
