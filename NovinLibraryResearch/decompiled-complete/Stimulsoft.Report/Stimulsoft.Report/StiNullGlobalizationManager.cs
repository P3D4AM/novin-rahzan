using System.Globalization;

namespace Stimulsoft.Report;

public sealed class StiNullGlobalizationManager : IStiGlobalizationManager
{
	public CultureInfo Culture { get; set; }

	public string GetString(string name)
	{
		return name;
	}

	public object GetObject(string name)
	{
		return null;
	}

	public StiNullGlobalizationManager()
	{
		Culture = CultureInfo.CurrentCulture;
	}
}
