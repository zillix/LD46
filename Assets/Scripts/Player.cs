using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float speed = 1;
	private float baseScale;
	public Animator animator;
	public Transform sprite;

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

		Vector3 scale = sprite.localScale;
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			Vector3 position = transform.position;
			position.x -= speed * Time.deltaTime;
			transform.position = position;
			isMoving = true;
			scale.x = -1 * baseScale;
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			Vector3 position = transform.position;
			position.x += speed * Time.deltaTime;
			transform.position = position;
			isMoving = true;
			scale.x = 1 * baseScale;

		}
		sprite.localScale = scale;

		animator.SetBool("IsMoving", isMoving);

	}
}
