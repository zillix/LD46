using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Text.RegularExpressions;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextTyper : MonoBehaviour
{
	public TextBox textBox;
	public string RawTextTypedSoFar { get; private set; }
	public string RawTextRemaining { get; private set; }
	public string VolatileTextTypedSoFar { get; set; } // This can be externally formatted
	private Coroutine current;
	private bool useSkipDelay = false;

	private static float SKIP_DELAY = .0005f;

	private static string HIDDEN_START = "<color=#00000000>";
	private static string HIDDEN_END = "</color>";

	private void Start()
	{
	}

	public void Skip()
	{
		useSkipDelay = true;
	}

	public void Abort()
	{
		if (current != null)
		{
			StopCoroutine(current);
		}
	}

	public void TypeText(string textToWrite, float letterDelay, Action onFinishedPlaying)
	{
		RawTextTypedSoFar = "";
		VolatileTextTypedSoFar = "";
		useSkipDelay = false;
		if (current != null)
		{
			StopCoroutine(current);
		}

		List<TextTypeNode> nodes = new List<TextTypeNode>();
		TextTypeNode node = new TextTypeNode().InitText(textToWrite);

		current = StartCoroutine(typeText(nodes, letterDelay, onFinishedPlaying));
	}

	public void TypeText(List<TextTypeNode> nodes, float letterDelay, Action onFinishedPlaying)
	{
		RawTextTypedSoFar = "";
		VolatileTextTypedSoFar = "";
		useSkipDelay = false;
		if (current != null)
		{
			StopCoroutine(current);
		}
		current = StartCoroutine(typeText(nodes, letterDelay, onFinishedPlaying));
	}

	private IEnumerator typeText(List<TextTypeNode> nodes, float letterDelay, Action onFinishedPlaying)
	{
		string fullString = "";
		List<string> charactersToType = new List<string>();
		List<int> typeIndices = new List<int>();
		Dictionary<int, float> pauseIndices = new Dictionary<int, float>();
		int cumulativeIndex = 0;
		foreach (TextTypeNode node in nodes)
		{
			switch (node.type)
			{
				case TextTypeNode.TextNodeType.Tag:
					fullString += node.tag;
					if (charactersToType.Count == 0)
					{
						charactersToType.Add(node.tag);
					}
					else
					{
						charactersToType[charactersToType.Count - 1] += node.tag;
					}
					cumulativeIndex += node.tag.Length;
					typeIndices.Add(node.tag.Length);
					break;
				case TextTypeNode.TextNodeType.Text:
					fullString += node.text;
					foreach (char c in node.text)
					{
						charactersToType.Add("" + c);
						typeIndices.Add(1);
					}
					cumulativeIndex += node.text.Length;
					break;
				case TextTypeNode.TextNodeType.Pause:
					pauseIndices[cumulativeIndex] = node.pauseTime;
					break;
			}
		}

		cumulativeIndex = 0;
		for (int i = 0; i < typeIndices.Count; ++i)
		{
			int typeIndex = typeIndices[i];

			if (pauseIndices.ContainsKey(cumulativeIndex))
			{
				yield return new WaitForSeconds(useSkipDelay ? SKIP_DELAY : pauseIndices[cumulativeIndex]);
			}

			int previousIndex = cumulativeIndex;
			cumulativeIndex += typeIndex;

			string newlyTypedString = fullString.Substring(previousIndex, cumulativeIndex - previousIndex);
			string remainingHiddenString = fullString.Substring(cumulativeIndex);
			remainingHiddenString = Regex.Replace(remainingHiddenString, "<color=#[a-zA-Z0-9]{6}>", ""); // strip any color tags, we don't need em and they'll mess stuff up
			remainingHiddenString = Regex.Replace(remainingHiddenString, "</color>", ""); // strip any color tags, we don't need em and they'll mess stuff up

			// effectively: track the 'real' text, but use a second copy so other things
			// can externally mess and format with the stuff we've already typed
			RawTextTypedSoFar += newlyTypedString;
			RawTextRemaining = HIDDEN_START + remainingHiddenString + HIDDEN_END;
			VolatileTextTypedSoFar += newlyTypedString;

			string typedText = VolatileTextTypedSoFar + RawTextRemaining;
			textBox.text = typedText;

			if (i == 0)
			{
				textBox.onTextStarted();
			}

			/*string typedText = fullString.Substring(0, cumulativeIndex)
				+ HIDDEN_START
				+ fullString.Substring(cumulativeIndex)
				+ HIDDEN_END;*/
			//text.text = existingText + typedText;
			//GameManager.instance.sounds.PlayOneShot(GameManager.instance.sounds.letterType, .2f);
			if (!useSkipDelay || i % 4 == 0)
			{
				// when skipping type several letters at a time
				yield return new WaitForSeconds(useSkipDelay ? SKIP_DELAY : letterDelay);
			}
		}

		onFinishedPlaying();
	}
}
