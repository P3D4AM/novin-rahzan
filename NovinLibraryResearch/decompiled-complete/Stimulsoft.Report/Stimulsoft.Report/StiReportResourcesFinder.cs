using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report;

public static class StiReportResourcesFinder
{
	public const string IdentResourceWatermark = "Watermark";

	public const string IdentResourceImage = "Image";

	public const string IdentResourceVariable = "ValueObject";

	public const string IdentResourceReport = "Resource";

	public static List<StiPage> GetResourcePages(StiReport report)
	{
		return (from p in report.Pages.ToList()
			where p.Watermark.ExistImage()
			select p).ToList();
	}

	public static List<StiImage> GetResourceImageComponents(StiReport report)
	{
		return (from StiImage c in from c in report.GetComponents().ToList()
				where c is StiImage
				select c
			where c.ExistImage()
			select c).ToList();
	}

	public static List<StiVariable> GetResourceVariables(StiReport report)
	{
		return (from v in report.Dictionary.Variables.ToList()
			where !string.IsNullOrEmpty(v.Value) && v.Value.Length > 1000 && (v.ValueObject is Image || v.ValueObject is byte[])
			select v).ToList();
	}

	public static List<StiResource> GetResourceReport(StiReport report)
	{
		return (from r in report.Dictionary.Resources.ToList()
			where r.Content != null
			select r).ToList();
	}

	public static bool ResourcesExits(StiReport report)
	{
		if (GetResourcePages(report).Any())
		{
			return true;
		}
		if (GetResourceImageComponents(report).Any())
		{
			return true;
		}
		if (GetResourceVariables(report).Any())
		{
			return true;
		}
		if (GetResourceReport(report).Any())
		{
			return true;
		}
		return false;
	}
}
