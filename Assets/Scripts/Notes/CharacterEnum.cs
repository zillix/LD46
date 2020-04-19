public enum CharacterName
{
	pious,
	feral,
	errant,
	arcane
}

public static class CharacterData
{
	public static string GetCharacterName(CharacterName charName)
	{
		switch (charName)
		{
			case CharacterName.arcane:
				return "the Arcane";
			case CharacterName.feral:
				return "the Feral";
			case CharacterName.errant:
				return "the Errant";
			case CharacterName.pious:
				return "the Pious";
		}

		return "the mistake";
	}
}