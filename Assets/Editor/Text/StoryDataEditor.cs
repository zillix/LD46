using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(StoryData))]
public class StoryDataEditor : Editor
{
	[MenuItem("Assets/Create/StoryData")]
	public static void CreateAsset()
	{
		ScriptableObjectUtility.CreateAsset<StoryData>();
	}

	private int relatedStoryCount = -1;
	private Dictionary<StoryData.StoryChoice, int> choiceTextCounts = new Dictionary<StoryData.StoryChoice, int>();


	Vector2 textScrollPos;
	public override void OnInspectorGUI()
	{

		StoryData data = (StoryData)target;

		data.storyName = EditorGUILayout.TextField("Name", data.storyName);

		EditorGUILayout.Space();

		var serializedObject = new SerializedObject(target);
		var storyText = serializedObject.FindProperty("textDatas");
		serializedObject.Update();
		EditorGUILayout.PropertyField(storyText, true);
		serializedObject.ApplyModifiedProperties();

		EditorGUILayout.Space();

		data.cameraTarget = (Transform)EditorGUILayout.ObjectField("Camera Target", data.cameraTarget, typeof(Transform), true);

		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		if (relatedStoryCount <0)
		{
			relatedStoryCount = data.relatedStories.Count;
		}
		relatedStoryCount = EditorGUILayout.IntField("Related Stories (" + data.relatedStories.Count + ")", relatedStoryCount);
		if (GUILayout.Button("Apply"))
		{
			if (relatedStoryCount != data.relatedStories.Count)
			{
				data.relatedStories.Resize<StoryData.StoryChoice>(relatedStoryCount);
			}
		}
		EditorGUILayout.EndHorizontal();

		EditorGUI.indentLevel++;
		foreach (StoryData.StoryChoice choice in data.relatedStories)
		{
			if (!choiceTextCounts.ContainsKey(choice))
			{
				choiceTextCounts.Add(choice, choice.choiceTexts.Count);
			}

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			choice.linkedStory = (StoryData)EditorGUILayout.ObjectField("Linked Story", choice.linkedStory, typeof(StoryData), false);
			choice.choiceText = EditorGUILayout.TextField("Choice Text", choice.choiceText);
			EditorGUILayout.BeginHorizontal();
			choiceTextCounts[choice] = EditorGUILayout.IntField("Choice Texts Count", choiceTextCounts[choice]);
			if (GUILayout.Button("Apply"))
			{
				choice.choiceTexts.Resize(choiceTextCounts[choice], null);
			}
			EditorGUILayout.EndHorizontal();
			EditorGUI.indentLevel++;
			EditorGUILayout.BeginVertical(EditorStyles.label);

			for (int i = 0; i < choice.choiceTexts.Count; ++i)
			{
				choice.choiceTexts[i] = (TextData)EditorGUILayout.ObjectField(choice.choiceTexts[i], typeof(TextData), false);
			}
			EditorGUILayout.EndVertical();
			EditorGUI.indentLevel--;
			EditorGUILayout.EndVertical();
		}

		EditorGUI.indentLevel--;

		if (GUI.changed)
		{
			EditorUtility.SetDirty(data);
		}
	}
}
