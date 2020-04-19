using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NotesController : MonoBehaviour
{
	public Color pious;
	public Color feral;
	public Color arcane;
	public Color errant;

	public TextData piousQuestion;
	public TextData feralQuestion;
	public TextData arcaneQuestion;
	public TextData errantQuestion;
	public TextData finalQuestion;
	public TextData finalQuestion2;

	public StoryData piousSecretStory;
	public StoryData feralSecretStory;
	public StoryData arcaneSecretStory;
	public StoryData errantSecretStory;
	public StoryData finalSecretStory;

	public Transform notesPosition;
	public float notesTriggerDist = 5;
	public Player player;
	public NotesBox notesUi;
	public ChoiceBox secretChoiceBox;
	public StoryManager storyManager;
	public float UnsuspendSecretChoiceDist = 7f;

	private bool suspendSecretChoice = false;

	private static int maxNotes = 3;

	private Dictionary<CharacterName, HashSet<string>> notesFound = new Dictionary<CharacterName, HashSet<string>>();
	private HashSet<CharacterName> discoveredNames = new HashSet<CharacterName>();

	public HashSet<string> GetNotes(CharacterName character) { return notesFound.ContainsKey(character) ? notesFound[character] : null; }
	public HashSet<CharacterName> GetDiscoveredNames {  get { return discoveredNames; } }
	public Color GetColor(CharacterName character)
	{
		switch (character)
		{
			case CharacterName.pious: return pious;
			case CharacterName.feral: return feral;
			case CharacterName.errant: return errant;
			case CharacterName.arcane: return arcane;
			default: return Color.black;
		}
	}

	private TextData getQuestion(CharacterName character)
	{
		switch (character)
		{
			case CharacterName.pious: return piousQuestion;
			case CharacterName.feral: return feralQuestion;
			case CharacterName.errant: return errantQuestion;
			case CharacterName.arcane: return arcaneQuestion;
			default: return piousQuestion;
		}
	}

	private StoryData getSecretStory(CharacterName character)
	{
		switch (character)
		{
			case CharacterName.pious: return piousSecretStory;
			case CharacterName.feral: return feralSecretStory;
			case CharacterName.errant: return errantSecretStory;
			case CharacterName.arcane: return arcaneSecretStory;
			default: return piousSecretStory;
		}
	}

	public void NameDiscovered(CharacterName character)
	{
		discoveredNames.Add(character);
	}

	public void RecordNote(CharacterName character, string note)
	{
		if (!notesFound.ContainsKey(character))
		{
			notesFound.Add(character, new HashSet<string>());
		}

		notesFound[character].Add(note);
	}

	public void Update()
	{
		if (GameManager.DEBUG)
		{
			CharacterName characterToFake = (CharacterName)(-1);
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				characterToFake = CharacterName.arcane;
			}
			else if (Input.GetKey(KeyCode.Alpha2))
			{
				characterToFake = CharacterName.errant;
			}
			else if (Input.GetKey(KeyCode.Alpha3))
			{
				characterToFake = CharacterName.feral;
			}
			else if (Input.GetKey(KeyCode.Alpha4))
			{
				characterToFake = CharacterName.pious;
			}

			if ((int)characterToFake != -1)
			{
				notesFound[characterToFake] = new HashSet<string>() { "a", "b", "c" };
			}
		}

		float dist = Mathf.Abs(player.transform.position.x - notesPosition.position.x);
		if (dist < notesTriggerDist && !suspendSecretChoice)
		{
			notesUi.Open();

			if (!secretChoiceBox.IsVisible)
			{
				List<StoryData.StoryChoice> choices = getSecretChoices();
				if (choices.Count > 0)
				{
					secretChoiceBox.show();
					secretChoiceBox.Clear();
					foreach (StoryData.StoryChoice choice in choices)
					{
						string storyText = choice.choiceText;
						bool greyOut = storyManager.visitedStories.Contains(choice.linkedStory.storyName);

						secretChoiceBox.AddChoice(storyText, () => { suspendSecretChoice = true; // close the notes until we leave & return
							storyManager.Abort();  storyManager.askChoice(choice); }, greyOut);
					}
				}
			}
			
		}
		else
		{
			if (dist > UnsuspendSecretChoiceDist)
			{
				suspendSecretChoice = false;
			}

			notesUi.Close();
			secretChoiceBox.hide();
			secretChoiceBox.Clear();
		}
	}

	private List<StoryData.StoryChoice> getSecretChoices()
	{
		List<StoryData.StoryChoice> choices = new List<StoryData.StoryChoice>();
		foreach (CharacterName character in Enum.GetValues(typeof(CharacterName)))
		{
			if (notesFound.ContainsKey(character) && notesFound[character].Count >= maxNotes)
			{
				StoryData.StoryChoice choice = new StoryData.StoryChoice();
				choice.choiceText = CharacterData.GetCharacterName(character);
				choice.choiceTexts = new List<TextData>{ getQuestion(character) };
				choice.linkedStory = getSecretStory(character);

				string storyText = choice.choiceText;

				bool greyOut = storyManager.visitedStories.Contains(choice.linkedStory.storyName);
				if (!greyOut)
				{
					string colorHex = ColorUtility.ToHtmlStringRGB(GetColor(character));

					storyText = "<color=#" + colorHex + ">" + storyText + "</color>";
				}
				choice.choiceText = storyText;

				choices.Add(choice);
			}
		}

		if (choices.Count >= 4)
		{
			bool allRead = true;
			foreach (StoryData.StoryChoice newChoice in choices )
			{
				bool greyOut = storyManager.visitedStories.Contains(newChoice.linkedStory.storyName);

				if (!greyOut)
				{
					allRead = false;
				}
			}
			if (allRead)
			{
				StoryData.StoryChoice choice = new StoryData.StoryChoice();
				choice.choiceText = "final question";
				choice.choiceTexts = new List<TextData> { finalQuestion, finalQuestion2 };
				choice.linkedStory = finalSecretStory;
				choices.Add(choice);
			}

		}

		return choices;
	}


}
