using System;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report;

public static class StiRenderingMessagesHelper
{
	public static bool WriteConnectionException(StiDictionary dictionary, string name, Exception e)
	{
		if (dictionary == null || dictionary.Report == null || dictionary.Report.IsDesigning)
		{
			return false;
		}
		dictionary.Report.WriteToReportRenderingMessages(Loc.Get("Database", "Connection") + " '" + name + "': " + e.Message);
		return true;
	}
}
