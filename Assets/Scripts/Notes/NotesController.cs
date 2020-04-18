using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NotesController : MonoBehaviour
{
	public Color pious;
	public Color feral;
	public Color aspirant;
	public Color errant;
	public Color lettered;
	public Color vigilant;

	public Transform notesPosition;
	public float notesTriggerDist = 5;
	public Player player;
	public NotesBox notesUi;

	private Dictionary<CharacterName, HashSet<string>> notesFound = new Dictionary<CharacterName, HashSet<string>>();

	public HashSet<string> GetNotes(CharacterName character) { return notesFound.ContainsKey(character) ? notesFound[character] : null; }
	public Color GetColor(CharacterName character)
	{
		switch (character)
		{
			case CharacterName.pious: return pious;
			case CharacterName.feral: return feral;
			case CharacterName.errant: return errant;
			case CharacterName.lettered: return lettered;
			case CharacterName.vigilant: return vigilant;
			case CharacterName.aspirant: return aspirant;
			default: return Color.black;
		}
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
		if (Mathf.Abs(player.transform.position.x - notesPosition.position.x) < notesTriggerDist)
		{
			notesUi.Open();
		}
		else
		{
			notesUi.Close();
		}
	}


}
