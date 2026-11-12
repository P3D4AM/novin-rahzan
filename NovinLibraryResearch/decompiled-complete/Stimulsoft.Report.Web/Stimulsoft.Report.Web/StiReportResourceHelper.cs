using System;
using System.Collections;
using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Web;

internal class StiReportResourceHelper
{
	public static ArrayList GetResourcesItems(StiReport report)
	{
		ArrayList arrayList = new ArrayList();
		foreach (StiResource resource in report.Dictionary.Resources)
		{
			if (resource.AvailableInTheViewer)
			{
				Hashtable hashtable = new Hashtable();
				hashtable["name"] = resource.Name;
				hashtable["alias"] = resource.Alias;
				hashtable["type"] = resource.Type;
				hashtable["size"] = ((resource.Content != null) ? resource.Content.Length : 0);
				arrayList.Add(hashtable);
			}
		}
		if (arrayList.Count <= 0)
		{
			return null;
		}
		return arrayList;
	}

	public static string GetStimulsoftFontBase64Data()
	{
		using Stream stream = typeof(StiFontIconsHelper).Assembly.GetManifestResourceStream("Stimulsoft.Base.FontIcons.Stimulsoft.ttf");
		byte[] array = new byte[stream.Length];
		stream.Read(array, 0, array.Length);
		return $"data:application/x-font-ttf;base64,{Convert.ToBase64String(array)}";
	}

	public static string GetBase64DataFromFontResourceContent(StiResourceType resourceType, byte[] content)
	{
		if (content != null)
		{
			string arg = "application/octet-stream";
			switch (resourceType)
			{
			case StiResourceType.FontEot:
				arg = "application/vnd.ms-fontobject";
				break;
			case StiResourceType.FontTtf:
				arg = "application/x-font-ttf";
				break;
			case StiResourceType.FontWoff:
				arg = "application/font-woff";
				break;
			case StiResourceType.FontOtf:
				arg = "application/x-font-opentype";
				break;
			}
			return $"data:{arg};base64,{Convert.ToBase64String(content)}";
		}
		return string.Empty;
	}

	public static bool IsFontResourceType(StiResourceType resourceType)
	{
		if (resourceType != StiResourceType.FontOtf && resourceType != StiResourceType.FontTtc)
		{
			return resourceType == StiResourceType.FontTtf;
		}
		return true;
	}

	public static ArrayList GetFontResourcesArray(StiReport report)
	{
		ArrayList arrayList = new ArrayList();
		if (report != null)
		{
			foreach (StiResource resource in report.Dictionary.Resources)
			{
				if (IsFontResourceType(resource.Type))
				{
					Hashtable hashtable = new Hashtable();
					hashtable["contentForCss"] = GetBase64DataFromFontResourceContent(resource.Type, resource.Content);
					hashtable["originalFontFamily"] = StiFontCollection.GetFontFamily(report.GetResourceFontName(resource.Name)).Name;
					arrayList.Add(hashtable);
				}
			}
		}
		return arrayList;
	}

	public static void LoadResourcesToReport(StiReport report, StiResourcesCollection resources)
	{
		report.Dictionary.Resources.Clear();
		foreach (StiResource resource in resources)
		{
			report.Dictionary.Resources.Add(resource);
		}
	}
}
