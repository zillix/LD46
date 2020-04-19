using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
	public TextEngine playerText;
	public TextEngine narratorText;
	public ChoiceBox choiceBox;
	public ChoiceBox secretChoiceBox;
	public FireController fireController;
	public IslandCameraController cameraController;
	public StoryData finalSecret;
	public MainCameraController mainCamera;

	public StoryData startStory;
	public List<TextData> introTexts;
	public float StartTriggerDist = 4f;
	public Transform player;

	private bool waitingForStartTrigger = false;
	private bool startTriggered = false;

	public HashSet<string> visitedStories = new HashSet<string>();


	private StoryData activeStory;

	public void Start()
	{
		/*if (startStory != null)
		{
			PlayStory(startStory);
		}*/
	}

	public void StartGame()
	{
		for (int i = 0; i < introTexts.Count; ++i)
		{
			narratorText.enqueue(introTexts[i]);
		}
		waitingForStartTrigger = true;
	}

	public void Update()
	{
		if (waitingForStartTrigger)
		{
			float playerDist = Mathf.Abs(player.position.x - fireController.transform.position.x);
			if (playerDist < StartTriggerDist)
			{
				startTriggered = true;
				waitingForStartTrigger = false;
				PlayStory(startStory, ()=> { fireController.TurnOn(); });
			}
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			playerText.Skip();
			narratorText.Skip();
			choiceBox.Choose();
			secretChoiceBox.Choose();
		}

		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			choiceBox.Cycle(-1);
			secretChoiceBox.Cycle(-1);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			choiceBox.Cycle(1);
			secretChoiceBox.Cycle(1);
		}

		if (GameManager.DEBUG)
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				startEndGame();
			}
		}
	}

	public void Abort()
	{
		narratorText.Abort();
		playerText.Abort();
		choiceBox.Abort();
		StopAllCoroutines();
	}

	public void PlayStory(StoryData story, Action postChoiceCallback = null)
	{
		visitedStories.Add(story.storyName);

		for (int i = 0; i < story.textDatas.Count; ++i)
		{
			Action callback = null;
			if (i == story.textDatas.Count - 1)
			{
				callback = () => {
					if (story == finalSecret)
					{
						startEndGame();
					}
					else
					{

						displayChoices(story.relatedStories, postChoiceCallback);
						cameraController.TweenToPosition(cameraController.startTransform);
						//fireController.StopVision();
					}
				};
			}
			narratorText.enqueue(story.textDatas[i], callback);

			if (story.cameraTarget != null)
			{
				// We are showing the island
				fireController.StartVision();
				//cameraController.ForceTransform(cameraController.startTransform);
				cameraController.TweenToPosition(story.cameraTarget);
			}

		}
	}

	private void displayChoices(List<StoryData.StoryChoice> choices, Action postChoiceCallback = null)
	{
		choiceBox.Clear();
		foreach (StoryData.StoryChoice choice in choices)
		{
			bool greyOut = visitedStories.Contains(choice.linkedStory.storyName);
			choiceBox.AddChoice(choice.choiceText, () => { askChoice(choice, postChoiceCallback); }, greyOut);
		}
	}

	public void askChoice(StoryData.StoryChoice choice, Action postChoiceCallback = null)
	{
		for (int i = 0; i < choice.choiceTexts.Count; ++i)
		{
			Action callback = null;
			if (i == choice.choiceTexts.Count - 1)
			{
				callback = () => { PlayStory(choice.linkedStory); if (postChoiceCallback != null) { postChoiceCallback(); } };
			}
			playerText.enqueue(choice.choiceTexts[i], callback);
		}
	}

	private void startEndGame()
	{
		mainCamera.Flash(Color.black, restartGame, 1f, 2f, 1f);
	}

	private void restartGame()
	{
		SceneManager.LoadScene(0);
	}

}
