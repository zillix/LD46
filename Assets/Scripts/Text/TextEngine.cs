using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class TextEngine : MonoBehaviour
{

	private List<QueuedText> textQueue;

	private QueuedText currentText;
	private float currentDuration;
	private bool textFinishedPlaying = false;

	public float DefaultPauseDuration = 3f;

	private ITextBox textBox;
	public GameObject textBoxObject;
	public TextPlayer textPlayer;

	private class QueuedText
	{
		public TextData text;
		public Action callback;

		public QueuedText(TextData t, Action a)
		{
			text = t;
			callback = a;
		}
	}

	void Awake()
	{
		textQueue = new List<QueuedText>();

	}

	// Use this for initialization
	void Start()
	{
		textBox = (ITextBox)textBoxObject.GetComponent(typeof(ITextBox));
		textBoxObject.SetActive(true);
		textBox.hide();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.L)
			&& currentText != null
			&& currentText.text.skippable)
		{
			advanceText();
			return;
		}


		if (currentText != null)
		{
			float pauseDuration = currentText.text.doOverridePauseDuration ? currentText.text.pauseDuration : DefaultPauseDuration;
			if (textFinishedPlaying && currentDuration < pauseDuration)
			{
				currentDuration += Time.deltaTime;
				if (currentDuration >= pauseDuration)
				{
					advanceText();
				}

			}
		}
		else if (textQueue.Count > 0)
		{
			advanceText();
		}
	}

	public void SetTextBoxColor(Color color)
	{
		Image image = textBoxObject.GetComponentInChildren<Image>();
		if (image != null)
		{
			image.color = color;
		}
	}

	public void Abort()
	{
		textQueue.Clear();
		textPlayer.Abort();
		textBox.hide();
		currentText = null;
	}

	public void Skip()
	{
		if (currentText != null && !currentText.text.skippable)
		{
			return;
		}

		if (textFinishedPlaying)
		{
			advanceText();
		}
		else
		{
			textPlayer.Skip();
		}
	}

	public void Enqueue(List<TextData> text, bool force = false)
	{
		if (text.Count == 0)
		{
			return;
		}

		if (force)
		{
			textQueue.Clear();
			currentText = null;
			currentDuration = 0;
		}
		text.ForEach((q) => { textQueue.Add(new QueuedText(q, null)); });
	}

	public void enqueue(TextData textData, Action callback = null)
	{
		textQueue.Add(new QueuedText(textData, callback));
	}

	private void advanceText()
	{
		currentDuration = 0;

		if (currentText != null)
		{
			if (currentText.callback != null)
			{
				currentText.callback();
			}
		}

		if (textQueue.Count > 0)
		{
			if (textQueue[0].text.text.Length > 0)
			{
				textBox.show();
			}
			else
			{
				textBox.hide();
			}
			currentText = textQueue[0];
			textQueue.RemoveAt(0);
			textFinishedPlaying = false;
			textPlayer.PlayText(currentText.text,onTextPlayed );
			Sounds.PlayOneShot(Sounds.instance.advanceText, .4f);
		}
		else
		{
			reset();
		}
	}

	private void onTextPlayed()
	{
		textFinishedPlaying = true;
	}

	private void reset()
	{
		currentText = null;
		currentDuration = 0;
		textBox.hide();
	}

	public bool isBusy
	{
		get
		{
			return currentText != null;
		}
	}


}
