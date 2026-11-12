using System.IO;

namespace Stimulsoft.Report.Design;

public static class StiNameHelper
{
	public static string GetNewResourceName(string file, StiReport report)
	{
		string text = Path.GetFileNameWithoutExtension(file).Normalize();
		if (StiNameCreation.IsResourceNameExists(report, text))
		{
			text = StiNameCreation.CreateResourceName(report, text);
			if (StiNameCreation.IsResourceNameExists(report, text))
			{
				text = StiNameCreation.CreateResourceName(report, text);
			}
		}
		return text;
	}

	public static string GetNewDatabaseName(string file, StiReport report)
	{
		string text = Path.GetFileNameWithoutExtension(file).Normalize();
		if (StiNameCreation.IsConnectionNameExists(report, text))
		{
			text = StiNameCreation.CreateConnectionName(report, text);
			if (StiNameCreation.IsConnectionNameExists(report, text))
			{
				text = StiNameCreation.CreateConnectionName(report, text);
			}
		}
		return text;
	}
}
