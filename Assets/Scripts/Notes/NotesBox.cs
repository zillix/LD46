using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class NotesBox : MonoBehaviour
{
	public CharacterNotes pious;
	public CharacterNotes feral;
	public CharacterNotes errant;
	public CharacterNotes arcane;

	public NotesController notes;


	public float BobMagnitude = 10f;
	public float BobAngleSpeed = 360f;

	private float bobAngle = 0f;

	private Vector3 targetPos;
	public Vector3 bobOffset { get { return new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * bobAngle) * BobMagnitude, 0); } }


	private bool isOpen = true;

	public void Start()
	{
		Close();
		targetPos = transform.position;
	}

	public void Update()
	{
		bobAngle += Time.deltaTime * BobAngleSpeed;
		transform.position = targetPos + bobOffset;
	}

	public void Open()
	{
		if (isOpen)
		{
			return;
		}
		Sounds.PlayOneShot(Sounds.instance.openNotes, .7f);

		isOpen = true;
		gameObject.SetActive(true);
		foreach (CharacterName character in Enum.GetValues(typeof(CharacterName))) {

			setupCharacter(GetCharacter(character), character, notes.GetNotes(character), notes.GetDiscoveredNames.Contains(character), notes.GetColor(character));
		}
	}
	public void Close()
	{
		if (!isOpen)
		{
			return;
		}
		isOpen = false;
		gameObject.SetActive(false);
	}

	private CharacterNotes GetCharacter(CharacterName charName)
	{
		switch (charName)
		{
			case CharacterName.pious: return pious;
			case CharacterName.feral: return feral;
			case CharacterName.errant: return errant;
			case CharacterName.arcane: return arcane;
			default: return pious;
		}
	}

	private void setupCharacter(CharacterNotes notesUi, CharacterName name, HashSet<string> notes, bool discoveredNames, Color color)
	{
		color.a = 1;
		if (!discoveredNames)
		{
			notesUi.title.text = "unknown";
		}
		else
		{
			notesUi.title.text = CharacterData.GetCharacterName(name);
		}
		notesUi.title.color = color;

		string[] notesArray = notes == null ? new string[0] : notes.ToArray();
		for (int i = 0; i < notesUi.clues.Count; ++i)
		{
			if (notesArray.Length > i)
			{
				notesUi.clues[i].text = notesArray[i];
			}
			else
			{
				notesUi.clues[i].text = "???";
			}
			notesUi.clues[i].color = color;
			notesUi.clues[i].ForceMeshUpdate();
		}

		if (notesArray.Length == notesUi.clues.Count)
		{
			notesUi.checkbox.enabled = true;
		}
		else
		{
			notesUi.checkbox.enabled = false;
		}
		notesUi.checkbox.color = color;

	}
}
