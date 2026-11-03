using System.Globalization;

namespace Stimulsoft.Report;

public interface IStiGlobalizationManager
{
	CultureInfo Culture { get; set; }

	string GetString(string name);

	object GetObject(string name);
}
