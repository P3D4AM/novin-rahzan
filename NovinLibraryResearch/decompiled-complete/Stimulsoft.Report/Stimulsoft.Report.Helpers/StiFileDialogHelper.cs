using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Helpers;

public static class StiFileDialogHelper
{
	public static string[] DataExts = new string[8] { "xlsx", "xls", "csv", "dbf", "json", "xml", "xsd", "geojson" };

	public static string[] ImageExts = new string[10] { "gif", "png", "jpeg", "jpg", "bmp", "tiff", "ico", "emf", "wmf", "svg" };

	public static string[] ReportExts = new string[4] { "mrt", "mrz", "mdc", "mdz" };

	public static string[] TextExts = new string[2] { "rtf", "txt" };

	public static string[] DocumentExts = new string[3] { "pdf", "doc", "docx" };

	public static string[] FontExts = new string[3] { "ttf", "otf", "ttc" };

	public static string[] CustomMapExts = new string[1] { "map" };

	public static string[] BlocklyExts = new string[1] { "blockly" };

	private static string GetFilterString(string[] extensions)
	{
		return string.Join(";", extensions.Select((string s) => "*." + s));
	}

	public static string[] GetAllResourceExtensions()
	{
		List<string> list = DataExts.ToList();
		list.AddRange(ImageExts);
		list.AddRange(ReportExts);
		list.AddRange(TextExts);
		list.AddRange(DocumentExts);
		list.AddRange(FontExts);
		list.AddRange(CustomMapExts);
		return list.ToArray();
	}

	public static string GetImageFilter()
	{
		return StiLocalization.Get("FileFilters", "AllImageFiles") + "|" + GetFilterString(ImageExts);
	}

	public static string GetAllResourceFilter()
	{
		return StiLocalization.Get("FileFilters", "AllFiles") + "|" + GetFilterString(DataExts) + ";" + GetFilterString(ImageExts) + ";" + GetFilterString(ReportExts) + ";" + GetFilterString(TextExts) + ";" + GetFilterString(DocumentExts) + ";" + GetFilterString(FontExts) + ";" + GetFilterString(CustomMapExts);
	}

	public static string GetBlocklyFilter()
	{
		return "Blockly|" + GetFilterString(BlocklyExts);
	}
}
