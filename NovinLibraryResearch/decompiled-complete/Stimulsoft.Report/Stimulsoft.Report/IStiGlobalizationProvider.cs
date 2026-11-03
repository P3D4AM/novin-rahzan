namespace Stimulsoft.Report;

public interface IStiGlobalizationProvider
{
	void SetString(string propertyName, string value);

	string GetString(string propertyName);

	string[] GetAllStrings();
}
