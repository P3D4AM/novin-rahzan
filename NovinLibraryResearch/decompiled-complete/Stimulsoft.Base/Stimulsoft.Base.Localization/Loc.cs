namespace Stimulsoft.Base.Localization;

public class Loc
{
	public static bool IsEn => StiLocalization.IsEn;

	public static bool IsRussian => StiLocalization.IsRussian;

	public static bool IsCyrillic => StiLocalization.IsCyrillic;

	public static string Get(string category, string key)
	{
		return StiLocalization.Get(category, key);
	}

	public static string GetCleaned(string category, string key)
	{
		return Get(category, key).Replace(":", "").Replace("&", "").Replace("...", "");
	}

	public static string GetMain(string key)
	{
		return StiLocalization.Get("PropertyMain", key);
	}

	public static string GetReport(string key)
	{
		return StiLocalization.Get("Report", key);
	}

	public static string GetEnum(string key)
	{
		return StiLocalization.Get("PropertyEnum", key);
	}

	public static string GetNotice(string key)
	{
		return StiLocalization.Get("Notices", key);
	}
}
