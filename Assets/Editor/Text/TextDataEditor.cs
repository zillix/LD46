using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TextData))]
public class TransformInspector : Editor
{
	[MenuItem("Assets/Create/TextData")]
	public static void CreateAsset()
	{
		ScriptableObjectUtility.CreateAsset<TextData>();
	}

	Vector2 textScrollPos;
	public override void OnInspectorGUI()
	{

		TextData t = (TextData)target;

		// Replicate the standard transform inspector gui
		EditorGUI.indentLevel = 0;
		textScrollPos = EditorGUILayout.BeginScrollView(textScrollPos, GUILayout.Height(100));
		EditorStyles.textArea.wordWrap = true;
		EditorStyles.textField.wordWrap = true;
		t.text = EditorGUILayout.TextArea(t.text, GUILayout.ExpandHeight(true));
		EditorGUILayout.EndScrollView();
		t.skippable = EditorGUILayout.Toggle("Skippable", t.skippable);
		EditorGUILayout.Space();


		EditorGUIUtility.labelWidth += 30;
		t.doOverridePauseDuration = EditorGUILayout.Toggle("Override Pause Duration", t.doOverridePauseDuration);
		EditorGUIUtility.labelWidth -= 30;
		if (t.doOverridePauseDuration)
		{
			EditorGUI.indentLevel++;
			t.pauseDuration = EditorGUILayout.FloatField("Seconds", t.pauseDuration);
			EditorGUI.indentLevel--;
		}

		EditorGUIUtility.labelWidth += 30;
		t.doOverrideTypeSpeed = EditorGUILayout.Toggle("Override Type Speed", t.doOverrideTypeSpeed);
		EditorGUIUtility.labelWidth -= 30;
		if (t.doOverrideTypeSpeed)
		{
			EditorGUI.indentLevel++;
			t.typeSpeed = EditorGUILayout.FloatField("Seconds", t.typeSpeed);
			EditorGUI.indentLevel--;
		}

		EditorGUIUtility.labelWidth += 30;
		t.doJitter = EditorGUILayout.Toggle("Jitter", t.doJitter);
		EditorGUIUtility.labelWidth -= 30;
		if (t.doJitter)
		{
			EditorGUI.indentLevel++;
			t.jitterMagnitude = EditorGUILayout.Vector2Field("Magnitude", t.jitterMagnitude);
			t.jitterFrequencySeconds = EditorGUILayout.FloatField("Frequency", t.jitterFrequencySeconds);
			EditorGUI.indentLevel--;
		}

		if (GUI.changed)
		{
			EditorUtility.SetDirty(t);
		}
	}
}
