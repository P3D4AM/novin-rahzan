using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Design.Helpers;

public static class StiFontResourceHelper
{
	public static bool IsResourceFont(string fontName, StiReport report)
	{
		return (from r in report.Dictionary.Resources.ToList()
			where FontVHelper.IsFont(r.Type) && r.Content != null
			select r).Any((StiResource f) => f.Name == fontName);
	}

	public static List<StiResource> GetAllFonts(StiReport report)
	{
		return (from r in report.Dictionary.Resources.ToList()
			where FontVHelper.IsFont(r.Type) && r.Content != null
			select r).ToList();
	}
}
