using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.BlocklyEditor.Wpf.Helper;

public static class StiFontHelper
{
	public static List<string> LoadFonts(StiReport report)
	{
		List<string> list = new List<string>();
		if (report == null)
		{
			List<StiResource> list2 = (from r in report.Dictionary.Resources.ToList()
				where FontVHelper.IsFont(r.Type) && r.Content != null
				select r).ToList();
			foreach (StiResource item in list2)
			{
				string resourceFontName = report.GetResourceFontName(item.Name);
				list.Add(resourceFontName);
			}
			FontFamily[] families = ((FontCollection)StiFontCollection.Instance).Families;
			foreach (FontFamily val in families)
			{
				list.Add(val.Name);
			}
		}
		FontFamily[] families2 = FontFamily.Families;
		foreach (FontFamily val2 in families2)
		{
			if (!string.IsNullOrWhiteSpace((val2 != null) ? val2.Name : null))
			{
				list.Add(val2.Name);
			}
		}
		return list;
	}
}
