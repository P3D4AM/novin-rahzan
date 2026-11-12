using System;
using System.IO;
using System.Text;

namespace Stimulsoft.Base.Helpers;

public static class StiReportContentHelper
{
	public sealed class StiReportContent
	{
		public byte[] ReportIcon { get; set; }

		public string Path { get; set; }

		public string ReportName { get; set; } = string.Empty;


		public string ReportDescription { get; set; }

		public bool IsDbs { get; set; }

		public bool ContainsDbs { get; set; }

		public bool IsMultiPages { get; set; }

		public bool IsLandscape { get; set; }

		public DateTime Modified { get; set; } = DateTime.Now;

	}

	public static StiReportContent Get(string path)
	{
		try
		{
			string fileText = GetFileText(path);
			if (string.IsNullOrEmpty(fileText))
			{
				return new StiReportContent();
			}
			StiReportContent stiReportContent = null;
			if (fileText.StartsWith("<"))
			{
				stiReportContent = GetReportInfoFromXml(fileText);
			}
			else if (fileText.StartsWith("{"))
			{
				stiReportContent = GetReportInfoFromJson(fileText);
			}
			if (stiReportContent != null)
			{
				stiReportContent.Modified = File.GetLastWriteTimeUtc(path);
				return stiReportContent;
			}
		}
		catch
		{
		}
		return new StiReportContent();
	}

	public static bool ContainsDbs(string path)
	{
		try
		{
			string fileText = GetFileText(path);
			if (!string.IsNullOrEmpty(fileText))
			{
				if (fileText.StartsWith("<"))
				{
					return IsReportXmlContainsDbs(fileText);
				}
				if (fileText.StartsWith("{"))
				{
					return IsReportJsonContainsDbs(fileText);
				}
			}
		}
		catch
		{
		}
		return false;
	}

	private static string GetFileText(string path)
	{
		string text = Path.GetExtension(path).ToLowerInvariant();
		if (text == ".mrt")
		{
			return File.ReadAllText(path);
		}
		if (text == ".mrz")
		{
			byte[] bytes = File.ReadAllBytes(path);
			bytes = StiGZipHelper.Unpack(bytes);
			return Encoding.Default.GetString(bytes);
		}
		return null;
	}

	private static bool IsReportXmlContainsDbs(string text)
	{
		int num = text.IndexOfInvariant("type=\"Stimulsoft.Dashboard.Components.StiDashboard\"");
		if (num == -1)
		{
			num = text.IndexOfInvariant("type=\"Dashboard\"");
		}
		return num != -1;
	}

	private static StiReportContent GetReportInfoFromXml(string text)
	{
		StiReportContent stiReportContent = new StiReportContent();
		int num = text.IndexOfInvariant("<ReportIcon>");
		int num2 = text.IndexOfInvariant("</ReportIcon>");
		if (num != -1 && num2 != -1 && num2 > num)
		{
			try
			{
				string s = text.Substring(num + "<ReportIcon>".Length, num2 - num - "<ReportIcon>".Length);
				stiReportContent.ReportIcon = Convert.FromBase64String(s);
			}
			catch
			{
			}
		}
		int num3 = text.IndexOfInvariant("<ReportName>");
		int num4 = text.IndexOfInvariant("</ReportName>");
		if (num3 != -1 && num4 != -1 && num4 > num3)
		{
			stiReportContent.ReportName = text.Substring(num3 + "<ReportName>".Length, num4 - num3 - "<ReportName>".Length);
		}
		num3 = text.IndexOfInvariant("<ReportDescription>");
		num4 = text.IndexOfInvariant("</ReportDescription>");
		if (num3 != -1 && num4 != -1 && num4 > num3)
		{
			stiReportContent.ReportDescription = text.Substring(num3 + "<ReportDescription>".Length, num4 - num3 - "<ReportDescription>".Length);
		}
		int num5 = text.IndexOfInvariant("type=\"Stimulsoft.Dashboard.Components.StiPage\"");
		if (num5 == -1)
		{
			num5 = text.IndexOfInvariant("type=\"Page\"");
		}
		int num6 = text.IndexOfInvariant("type=\"Stimulsoft.Dashboard.Components.StiDashboard\"");
		if (num6 == -1)
		{
			num6 = text.IndexOfInvariant("type=\"Dashboard\"");
		}
		stiReportContent.ContainsDbs = num6 != -1;
		stiReportContent.IsDbs = num6 != -1 && (num5 == -1 || num6 < num5);
		int num7 = 0;
		num7 = ((num6 == -1) ? num5 : ((num5 != -1) ? Math.Min(num5, num6) : num6));
		if (num7 == -1)
		{
			return stiReportContent;
		}
		num5 = text.IndexOfInvariant("type=\"Stimulsoft.Dashboard.Components.StiPage\"", num7 + 10);
		if (num5 == -1)
		{
			num5 = text.IndexOfInvariant("type=\"Page\"", num7 + 10);
		}
		num6 = text.IndexOfInvariant("type=\"Stimulsoft.Dashboard.Components.StiDashboard\"", num7 + 10);
		if (num6 == -1)
		{
			num6 = text.IndexOfInvariant("type=\"Dashboard\"", num7 + 10);
		}
		int num8 = ((num6 == -1) ? num5 : ((num5 != -1) ? Math.Min(num5, num6) : num6));
		if (num8 > 0)
		{
			stiReportContent.IsMultiPages = true;
		}
		string xmlProperty = GetXmlProperty(text, "Orientation", num7, num8);
		if (xmlProperty == "Landscape")
		{
			stiReportContent.IsLandscape = true;
		}
		return stiReportContent;
	}

	private static StiReportContent GetReportInfoFromJson(string text)
	{
		StiReportContent stiReportContent = new StiReportContent();
		int num = text.IndexOfInvariant("\"ReportIcon\":");
		if (num != -1)
		{
			try
			{
				int num2 = text.IndexOfInvariant("\"", num + "\"ReportIcon\":".Length) + 1;
				int num3 = text.IndexOfInvariant("\",", num2);
				string s = text.Substring(num2, num3 - num2);
				stiReportContent.ReportIcon = Convert.FromBase64String(s);
			}
			catch
			{
			}
		}
		int num4 = text.IndexOfInvariant("\"ReportName\":");
		if (num4 != -1)
		{
			int num5 = text.IndexOfInvariant("\"", num4 + "\"ReportName\":".Length) + 1;
			int num6 = text.IndexOfInvariant("\",", num5);
			stiReportContent.ReportName = text.Substring(num5, num6 - num5);
		}
		num4 = text.IndexOfInvariant("\"ReportDescription\":");
		if (num4 != -1)
		{
			int num7 = text.IndexOfInvariant("\"", num4 + "\"ReportDescription\":".Length) + 1;
			int num8 = text.IndexOfInvariant("\",", num7);
			stiReportContent.ReportDescription = text.Substring(num7, num8 - num7);
		}
		int num9 = text.IndexOfInvariant("\"StiPage\",");
		int num10 = text.IndexOfInvariant("\"StiDashboard\",");
		stiReportContent.ContainsDbs = num10 != -1;
		stiReportContent.IsDbs = num10 != -1 && num10 < num9;
		int num11 = ((num10 == -1) ? num9 : ((num9 != -1) ? Math.Min(num9, num10) : num10));
		if (num11 == -1)
		{
			return stiReportContent;
		}
		num9 = text.IndexOfInvariant("\"StiPage\",", num11 + 10);
		num10 = text.IndexOfInvariant("\"StiDashboard\",", num11 + 10);
		int num12 = ((num10 == -1) ? num9 : ((num9 != -1) ? Math.Min(num9, num10) : num10));
		if (num12 > 0)
		{
			stiReportContent.IsMultiPages = true;
		}
		string jsonProperty = GetJsonProperty(text, "\"Orientation\"", num11, num12);
		if (jsonProperty == "Landscape")
		{
			stiReportContent.IsLandscape = true;
		}
		return stiReportContent;
	}

	private static bool IsReportJsonContainsDbs(string text)
	{
		int num = text.IndexOfInvariant("\"StiDashboard\",");
		return num != -1;
	}

	private static string GetJsonProperty(string text, string propName, int startIndex, int endIndex)
	{
		int num = text.IndexOfInvariant(propName, startIndex);
		if (num == -1)
		{
			return string.Empty;
		}
		if (endIndex != -1 && num >= endIndex)
		{
			return string.Empty;
		}
		int num2 = text.IndexOfInvariant("\"", num + propName.Length + 1);
		if (num2 == -1)
		{
			return string.Empty;
		}
		int num3 = text.IndexOfInvariant("\"", num2 + 1);
		if (num3 == -1)
		{
			return string.Empty;
		}
		return text.Substring(num2 + 1, num3 - num2 - 1);
	}

	private static string GetXmlProperty(string text, string propName, int startIndex, int endIndex)
	{
		string text2 = "<" + propName + ">";
		string value = "</" + propName + ">";
		int num = text.IndexOfInvariant(text2, startIndex);
		if (num == -1)
		{
			return string.Empty;
		}
		if (endIndex != -1 && num >= endIndex)
		{
			return string.Empty;
		}
		int num2 = text.IndexOfInvariant(value, num + text2.Length);
		if (num2 == -1)
		{
			return string.Empty;
		}
		return text.Substring(num + text2.Length, num2 - num - text2.Length);
	}
}
