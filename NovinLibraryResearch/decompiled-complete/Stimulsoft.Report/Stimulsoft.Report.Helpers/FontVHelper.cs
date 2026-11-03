using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Helpers;

public static class FontVHelper
{
	public static bool LoadFontsFromResources(StiReport report)
	{
		bool haveFonts = false;
		(from r in report.Dictionary.Resources.ToList()
			where IsFont(r.Type) && r.Content != null
			select r).ToList().ForEach(delegate(StiResource r)
		{
			StiFontCollection.AddResourceFont(r.Name, r.Content, r.Type.ToString().Substring(4), r.Alias);
			haveFonts = true;
		});
		return haveFonts;
	}

	public static void RemoveFontsFromResources(StiReport report)
	{
		(from r in report.Dictionary.Resources.ToList()
			where IsFont(r.Type) && r.Content != null
			select r).ToList().ForEach(delegate(StiResource r)
		{
			StiFontCollection.RemoveResourceFont(r.Name, r.Content);
		});
	}

	public static bool IsFont(StiResourceType type)
	{
		if (type != StiResourceType.FontOtf && type != StiResourceType.FontTtc)
		{
			return type == StiResourceType.FontTtf;
		}
		return true;
	}
}
