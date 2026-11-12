using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Check.Helper;

public static class StiUsedResourceHelper
{
	public static List<StiDatabase> GetDatabasesUsedResource(StiReport report, StiResource resource)
	{
		string text = "resource://" + resource.Name;
		List<StiDatabase> list = new List<StiDatabase>();
		foreach (object database in report.Dictionary.Databases)
		{
			StiFileDatabase stiFileDatabase = null;
			switch (resource.Type)
			{
			case StiResourceType.Csv:
				stiFileDatabase = database as StiCsvDatabase;
				break;
			case StiResourceType.Dbf:
				stiFileDatabase = database as StiDBaseDatabase;
				break;
			case StiResourceType.Excel:
				stiFileDatabase = database as StiExcelDatabase;
				break;
			case StiResourceType.Json:
				stiFileDatabase = database as StiJsonDatabase;
				break;
			case StiResourceType.Gis:
				stiFileDatabase = database as StiGisDatabase;
				break;
			case StiResourceType.Xml:
			case StiResourceType.Xsd:
				stiFileDatabase = database as StiXmlDatabase;
				break;
			}
			if (database is StiXmlDatabase && resource.Type == StiResourceType.Xsd)
			{
				if (((StiXmlDatabase)database).PathSchema == text)
				{
					list.Add(stiFileDatabase);
				}
			}
			else if (stiFileDatabase != null && stiFileDatabase.PathData == text)
			{
				list.Add(stiFileDatabase);
			}
		}
		return list;
	}

	public static List<StiComponent> GetComponentsUsedResource(StiReport report, StiResource resource)
	{
		List<StiComponent> list = new List<StiComponent>();
		switch (resource.Type)
		{
		case StiResourceType.Image:
		{
			List<StiComponent> imageComponentsUsedResource = GetImageComponentsUsedResource(report, resource);
			list.AddRange(imageComponentsUsedResource);
			break;
		}
		case StiResourceType.Rtf:
		case StiResourceType.Txt:
		{
			List<StiComponent> richTextComponentsUsedResource = GetRichTextComponentsUsedResource(report, resource);
			list.AddRange(richTextComponentsUsedResource);
			break;
		}
		case StiResourceType.Report:
		case StiResourceType.ReportSnapshot:
		{
			List<StiComponent> reportsUsedResource = GetReportsUsedResource(report, resource);
			list.AddRange(reportsUsedResource);
			break;
		}
		case StiResourceType.FontTtc:
		case StiResourceType.FontTtf:
		case StiResourceType.FontOtf:
		case StiResourceType.FontEot:
		case StiResourceType.FontWoff:
		{
			List<StiComponent> textsUsedResource = GetTextsUsedResource(report, resource);
			list.AddRange(textsUsedResource);
			break;
		}
		}
		return list;
	}

	private static List<StiComponent> GetImageComponentsUsedResource(StiReport report, StiResource resource)
	{
		string resName = "resource://" + resource.Name;
		IEnumerable<StiComponent> source = from d in report.GetComponents().ToList()
			where d.GetType() == typeof(StiImage) || d is IStiImageElement
			select d;
		return source.Where(delegate(StiComponent d)
		{
			if (d is StiImage stiImage)
			{
				return stiImage.ImageURL.Value.Equals(resName);
			}
			return d is IStiImageElement stiImageElement && !string.IsNullOrEmpty(stiImageElement.ImageHyperlink) && stiImageElement.ImageHyperlink.Equals(resName);
		}).ToList();
	}

	private static List<StiComponent> GetRichTextComponentsUsedResource(StiReport report, StiResource resource)
	{
		string resName = "resource://" + resource.Name;
		IEnumerable<StiComponent> source = from d in report.GetComponents().ToList()
			where d.GetType() == typeof(StiRichText)
			select d;
		return source.Where((StiComponent d) => ((StiRichText)d).DataUrl.Value.Equals(resName)).ToList();
	}

	private static List<StiComponent> GetReportsUsedResource(StiReport report, StiResource resource)
	{
		string resName = "resource://" + resource.Name;
		IEnumerable<StiComponent> source = from d in report.GetComponents().ToList()
			where d.GetType() == typeof(StiSubReport)
			select d;
		return source.Where((StiComponent d) => ((StiSubReport)d).SubReportUrl.Equals(resName)).ToList();
	}

	private static List<StiComponent> GetTextsUsedResource(StiReport report, StiResource resource)
	{
		string fontName = report.GetResourceFontName(resource.Name);
		IEnumerable<StiComponent> source = from d in report.GetComponents().ToList()
			where d.GetType() == typeof(StiText)
			select d;
		return source.Where((StiComponent d) => ((StiText)d).Font.Name.Equals(fontName)).ToList();
	}
}
