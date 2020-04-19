using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteEffect : MonoBehaviour
{
	public Renderer aura;
	public Bobber bobber;

	private bool isUnread = true;

	public void Start()
	{
		SetUnread(false);
	}

	public void SetUnread(bool unread)
	{
		if (unread == isUnread)
		{
			return;
		}

		isUnread = unread;
		bobber.enabled = unread;
		aura.enabled = unread;
	}
}
