
public class TextTypeNode
{
	public enum TextNodeType
	{
		Text,
		Tag,
		Pause
	}

	public string text { get; private set; }
	public string tag { get; private set; }
	public float pauseTime { get; private set; }
	public TextNodeType type { get; private set; }
	public bool IsText { get { return type == TextNodeType.Text; } }
	public bool IsTag { get { return type == TextNodeType.Tag; } }
	public bool IsPause { get { return type == TextNodeType.Pause; } }

	public TextTypeNode InitText(string t) { text = t; type = TextNodeType.Text; return this; }
	public TextTypeNode InitTag(string t) { tag = t; type = TextNodeType.Tag; return this; }
	public TextTypeNode InitPause(float p) { pauseTime = p; type = TextNodeType.Pause; return this; }
}