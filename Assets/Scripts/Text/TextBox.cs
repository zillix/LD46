using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class TextBox : MonoBehaviour, ITextBox
{
	public float VisibleAlpha = .85f;
	public TMP_Text textField;
	public TextBoxResizer resizer;

	private string lastText;

	private bool thisIsVisible = true;

	Image image;
		// Use this for initialization
	void Awake()
	{
		image = gameObject.GetComponentInChildren<Image>();
	}

	protected virtual void Start()
	{
		if (resizer)
		{
			resizer.text = textField;
			resizer.image = image;
		}
	}

	// Update is called once per frame
	protected virtual void Update()
	{
		/*if (textField.text != lastText)
		{
			lastText = textField.text;

			Vector2 preferredWidth = textField.GetPreferredValues(textField.text);
			Resize(textField.renderedWidth, textField.renderedHeight);
		}*/
	}

	public void onTextStarted()
	{
		textField.ForceMeshUpdate();
		//Vector2 preferredWidth = textField.GetPreferredValues(textField.text,);
		Resize(textField.renderedWidth, textField.renderedHeight);
	}

	public virtual void hide()
	{
		setAlpha(0);
		thisIsVisible = false;
	}
	public virtual void show()
	{
		setAlpha(VisibleAlpha);
		thisIsVisible = true;
	}

	public bool IsVisible {  get { return thisIsVisible; } }

	protected void setAlpha(float value)
	{
		if (textField)
		{
			Color textColor = textField.color;
			textColor.a = value > 0 ? 1 : 0;
			textField.color = textColor;
		}

		Color color = image.color;
		color.a = value;
		image.color = color;
	}

	public void Resize(float textWidth, float textHeight)
	{
		resizer.Resize(textWidth, textHeight);
	}

	public string text
	{
		get
		{
			return textField.text;
		}

		set
		{
			textField.text = value;
		}
	}
}
