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
				return "the arcane";
			case CharacterName.feral:
				return "the feral";
			case CharacterName.errant:
				return "the errant";
			case CharacterName.pious:
				return "the pious";
		}

		return "the mistake";
	}
}