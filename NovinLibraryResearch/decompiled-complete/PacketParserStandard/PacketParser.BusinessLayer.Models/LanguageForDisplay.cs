using PacketParser.Assistance.MultiLingual;

namespace PacketParser.BusinessLayer.Models;

public class LanguageForDisplay
{
	public LanguageTypes LanguageTypes { get; set; }

	public string ForDisplay => LanguageTypes.GetDisplay().GetTranslate();

	public LanguageForDisplay(LanguageTypes languageTypes)
	{
		LanguageTypes = languageTypes;
	}
}
