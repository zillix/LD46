using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StoryManager : MonoBehaviour
{
	public TextEngine playerText;
	public TextEngine narratorText;
	public ChoiceBox choiceBox;
	public FireController fireController;
	public IslandCameraController cameraController;

	public StoryData startStory;

	private HashSet<string> visitedStories = new HashSet<string>();


	private StoryData activeStory;

	public void Start()
	{
		if (startStory != null)
		{
			PlayStory(startStory);
		}
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			playerText.Skip();
			narratorText.Skip();
			choiceBox.Choose();
		}

		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			choiceBox.Cycle(-1);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			choiceBox.Cycle(1);
		}
	}


	public void PlayStory(StoryData story)
	{
		visitedStories.Add(story.name);

		for (int i = 0; i < story.textDatas.Count; ++i)
		{
			Action callback = null; 
			if (i == story.textDatas.Count - 1)
			{
				callback = () => {
					displayChoices(story.relatedStories);
					fireController.StopVision();
				};
			}
			narratorText.enqueue(story.textDatas[i], callback);

			if (story.cameraTarget != null)
			{
				// We are showing the island
				fireController.StartVision();
				cameraController.ForceTransform(cameraController.startTransform);
				cameraController.TweenToPosition(story.cameraTarget);
			}

		}
	}

	private void displayChoices(List<StoryData.StoryChoice> choices)
	{
		choiceBox.Clear();
		foreach (StoryData.StoryChoice choice in choices)
		{
			bool greyOut = visitedStories.Contains(choice.linkedStory.storyName);
			choiceBox.AddChoice(choice.choiceText, () => { askChoice(choice); }, greyOut);
		}
	}

	private void askChoice(StoryData.StoryChoice choice)
	{
		for (int i = 0; i < choice.choiceTexts.Count; ++i)
		{
			Action callback = null;
			if (i == choice.choiceTexts.Count - 1)
			{
				callback = () => { PlayStory(choice.linkedStory); };
			}
			playerText.enqueue(choice.choiceTexts[i], callback);
		}
	}

}
