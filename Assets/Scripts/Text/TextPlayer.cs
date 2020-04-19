using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TextPlayer : MonoBehaviour
{
	public TextTyper typer;
	public TextBox textBox;

	public TextData currentText;
	public NotesController notes;

	private float lastJitterTime;

	private const float DEFAULT_TYPE_SPEED = 0.03f;

	public void PlayText(TextData data, Action onFinishedPlaying)
	{
		currentText = data;


		List<TextTypeNode> blobs = parseText(data);
		
		if (typer)
		{
			textBox.text = "";

			typer.TypeText(blobs, currentText.doOverrideTypeSpeed ? currentText.typeSpeed : DEFAULT_TYPE_SPEED, onFinishedPlaying);

		}
		else
		{
			textBox.text = currentText.text;

			textBox.onTextStarted();
			onFinishedPlaying();
		}
	}

	public void Skip()
	{
		if (currentText != null && currentText.skippable)
		{
			typer.Skip();
		}
	}

	public void Abort()
	{
		if (typer)
		{
			typer.Abort();
		}
	}

	string ExtractString(string s, string tag)
	{
		// You should check for errors in real-world code, omitted for brevity
		var startTag = "<" + tag + ">";
		int startIndex = s.IndexOf(startTag) + startTag.Length;
		int endIndex = s.IndexOf("</" + tag + ">", startIndex);
		return s.Substring(startIndex, endIndex - startIndex);
	}

	private string getCharName(CharacterName chName)
	{
		switch (chName)
		{
			case CharacterName.pious:
				return "pious";
			case CharacterName.arcane:
				return "arcane";
			case CharacterName.errant:
				return "errant";
			case CharacterName.feral:
				return "feral";
			default:
				return "doesn'texist";
		}
	}

	private string stripNotes(string text)
	{
		foreach (CharacterName charName in Enum.GetValues(typeof(CharacterName)))
		{
			string tag = getCharName(charName);
			if (text.Contains(tag))
			{
				string note = ExtractString(text, tag);
				string colorHex = ColorUtility.ToHtmlStringRGB(notes.GetColor(charName));
				text = text.Replace("<" + tag + ">", "<color=#" + colorHex + ">");
				text = text.Replace("</" + tag + ">", "</color>");

				if (note.IndexOf(tag, StringComparison.OrdinalIgnoreCase) < 0)
				{
					notes.RecordNote(charName, note);
				}
				else
				{
					notes.NameDiscovered(charName);
				}
			}
		}

		return text;
	}

	private List<TextTypeNode> parseText(TextData data)
	{
		string text = data.text;
		text = stripNotes(text);

		List<TextTypeNode> blobs = new List<TextTypeNode>(); //yolo

		for (int i = 0; i < text.Length; ++i)
		{
			if (text[i] == '<')
			{
				int closeIndex = text.IndexOf('>', i);
				string tagString = text.Substring(i, closeIndex - i + 1);
				i = closeIndex;
				TextTypeNode node = new TextTypeNode().InitTag(tagString);
				blobs.Add(node);
			}
			else if (text[i] == '{')
			{
				int closeIndex = text.IndexOf('}', i);
				string pauseString = text.Substring(i + 1, closeIndex - i - 1);
				i = closeIndex;
				TextTypeNode node = new TextTypeNode().InitPause(float.Parse(pauseString));
				blobs.Add(node);
			}
			else
			{
				char[] args = { '<', '{' };
				int endIndex = text.IndexOfAny(args, i);
				string mainString;
				if (endIndex >= 0)
				{
					mainString = text.Substring(i, endIndex - i);
					i = endIndex - 1;
				}
				else
				{
					mainString = text.Substring(i);
					i = text.Length;
				}

				TextTypeNode node = new TextTypeNode().InitText(mainString);
				blobs.Add(node);

			}
		}

		return blobs;
	}

	public void Update()
	{
		if (currentText && currentText.doJitter)
		{
			if (lastJitterTime <= 0)
			{
				doJitter(currentText.jitterMagnitude);
				lastJitterTime = currentText.jitterFrequencySeconds;
			}
			else
			{
				lastJitterTime -= Time.deltaTime;
			}
		}
	}

	private void doJitter(Vector2 magnitude)
	{
		string finishedText = typer.RawTextTypedSoFar;
		
		string output = "";

		bool inTag = false;
		foreach (char c in finishedText)
		{
			if (inTag)
			{
				if (c == '>')
				{
					inTag = false;
				}
				output += c;
				continue;
			}

			if (c == '<')
			{
				inTag = true;
				output += c;
				continue;
			}

			string offsetStr = "" + c;
			if (magnitude.x != 0)
			{
				int magnituideX = (int)((UnityEngine.Random.value * 2 - 1) * magnitude.x);
				if (magnituideX != 0)
				{
					offsetStr = "<space=" + magnituideX + "px>" + offsetStr;
				}
			}

			if (magnitude.y != 0)
			{
				int magnituideY = (int)((UnityEngine.Random.value * 2 - 1) * magnitude.y);
				if (magnituideY != 0)
				{
					offsetStr = "<voffset=" + magnituideY + ">" + offsetStr + "</voffset>";
				}
			}
			output += offsetStr;


		}

		typer.VolatileTextTypedSoFar = output;
		textBox.text = output + typer.RawTextRemaining;
	}
}
