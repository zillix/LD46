using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StoryData : ScriptableObject
{
	[Serializable]
	public class StoryChoice
	{
		public string choiceText;
		public List<TextData> choiceTexts = new List<TextData>();
		public StoryData linkedStory;
	}

	public string storyName;

	public List<TextData> textDatas;

	public List<StoryChoice> relatedStories = new List<StoryChoice>();

	public Transform cameraTarget;
}
