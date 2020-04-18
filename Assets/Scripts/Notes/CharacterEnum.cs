public enum CharacterName
{
	pious,
	feral,
	aspirant,
	errant,
	lettered,
	vigilant
}

public static class CharacterData
{
	public static string GetCharacterName(CharacterName charName)
	{
		switch (charName)
		{
			case CharacterName.aspirant:
				return "the aspirant";
			case CharacterName.feral:
				return "the feral";
			case CharacterName.lettered:
				return "the lettered";
			case CharacterName.errant:
				return "the errant";
			case CharacterName.pious:
				return "the pious";
			case CharacterName.vigilant:
				return "the vigilant";
		}

		return "the mistake";
	}
}