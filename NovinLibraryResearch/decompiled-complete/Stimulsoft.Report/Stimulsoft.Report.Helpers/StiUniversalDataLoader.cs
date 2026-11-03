using System.Collections.Generic;
using System.Collections.Specialized;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Helpers;

public static class StiUniversalDataLoader
{
	public static List<Data> LoadMutiple(StiReport report, string path, string filter)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrEmpty(path))
		{
			return null;
		}
		if (StiHyperlinkProcessor.IsResourceHyperlink(path))
		{
			return new Data(StiHyperlinkProcessor.GetResourceNameFromHyperlink(path), StiHyperlinkProcessor.GetBytes(report, path)).ToList();
		}
		return StiDataLoaderHelper.LoadMultiple(path, filter);
	}

	public static Data LoadSingle(StiReport report, string path, NameValueCollection headers = null)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		if (string.IsNullOrEmpty(path))
		{
			return null;
		}
		if (StiHyperlinkProcessor.IsResourceHyperlink(path))
		{
			return new Data(StiHyperlinkProcessor.GetResourceNameFromHyperlink(path), StiHyperlinkProcessor.GetBytes(report, path));
		}
		return StiDataLoaderHelper.LoadSingle(path, report?.CookieContainer, headers);
	}
}
