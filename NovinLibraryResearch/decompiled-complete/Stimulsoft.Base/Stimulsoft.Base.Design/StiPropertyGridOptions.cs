namespace Stimulsoft.Base.Design;

public class StiPropertyGridOptions
{
	public static StiLevel? PropertyLevel { get; set; }

	public static bool ShowDescription { get; set; }

	public static bool Localizable { get; set; }

	static StiPropertyGridOptions()
	{
		Localizable = true;
		ShowDescription = true;
		PropertyLevel = null;
	}
}
