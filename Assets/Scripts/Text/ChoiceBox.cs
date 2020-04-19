using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChoiceBox : FollowingTextBox
{
	private List<ChoiceElement> choices = new List<ChoiceElement>();

	public ChoiceElement choicePrefab;

	public Transform choiceVerticalList;
	public float inputDelayTime = .5f;

	public bool AllowCycling = true;

	private int selectedIndex = 0;
	private bool lockedIn = false;
	private float inputDelay = 0f;

	public TextBox blockingBox; // if this is open, ignore input

	private static string GREYOUT_START = "<color=#888888>";
	private static string GREYOUT_STOP = "<color=#888888>";

	public void Clear()
	{
		lockedIn = false;
		choices.Clear();
		selectedIndex = 0;
		foreach (Transform child in choiceVerticalList.transform)
		{
			GameObject.Destroy(child.gameObject);
		}
	}

	public void Abort()
	{
		foreach (ChoiceElement choice in choices)
		{
			choice.Abort();
		}
		Clear();
		hide();
	}

	protected override void Update()
	{
		base.Update();
		inputDelay -= Time.deltaTime;
	}

	public void AddChoice(string text, Action callback, bool greyOut = false)
	{
		show();
		inputDelay = inputDelayTime;

		string textToEnter = text;
		if (greyOut)
		{
			textToEnter = GREYOUT_START + text + GREYOUT_STOP;
		}

		ChoiceElement choice = GameObject.Instantiate<ChoiceElement>(choicePrefab);
		choice.Initialize(textToEnter, () => { callback(); hide(); Clear(); });

		if (choices.Count == 0)
		{
			choice.SetSelected(true);
		}
		else
		{
			choice.SetSelected(false);
		}

		choice.transform.SetParent(choiceVerticalList, false);

		choices.Add(choice);

		
	}

	public void Cycle(int direction)
	{
		if (blockingBox && blockingBox.IsVisible) { return; }
		if (lockedIn || choices.Count == 0) { return; }
		bool isUp = direction < 0;
		int newIndex = selectedIndex;
		if (isUp)
		{
			if (AllowCycling)
			{
				newIndex -= 1;
				if (newIndex < 0)
				{
					newIndex = choices.Count - 1;
				}
			}
			else
			{
				newIndex = Mathf.Max(0, selectedIndex - 1);
			}
		}
		else
		{
			if (AllowCycling)
			{
				newIndex = (selectedIndex + 1) % choices.Count;
			}
			else
			{
				newIndex = Math.Min(choices.Count - 1, selectedIndex + 1);
			}
		}

		if (newIndex != selectedIndex)
		{
			choices[selectedIndex].SetSelected(false);
			choices[newIndex].SetSelected(true);
			selectedIndex = newIndex;
		}
	}

	public void Choose()
	{
		if (blockingBox && blockingBox.IsVisible) { return; }
		if (inputDelay > 0)
		{
			return;
		}

		if (choices.Count == 0)
		{
			return;
		}
		choices[selectedIndex].OnChosen();
		lockedIn = true;
	}


}
