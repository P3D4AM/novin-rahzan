using System.Globalization;
using System.IO;
using System.Threading;
using System.Xml;

namespace Stimulsoft.Report.Design;

public sealed class StiPageTypeHelper
{
	public static string GetPageTypeFromFile(string fileName)
	{
		if (!File.Exists(fileName))
		{
			return null;
		}
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		FileStream fileStream = null;
		try
		{
			fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			if (StiReport.IsJsonFile(fileStream))
			{
				return "StiPage";
			}
			XmlTextReader xmlTextReader = new XmlTextReader(fileStream);
			xmlTextReader.DtdProcessing = DtdProcessing.Ignore;
			xmlTextReader.Read();
			xmlTextReader.Read();
			if (xmlTextReader.IsStartElement())
			{
				return (xmlTextReader.Name == "StiSerializer") ? xmlTextReader.GetAttribute("application") : "StiPage";
			}
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
			fileStream.Close();
		}
		return "StiPage";
	}
}
