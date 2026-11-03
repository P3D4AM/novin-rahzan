using System.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Helpers;

public static class StiResourceTypeHelper
{
	public static StiResourceType? GetTypeFromExtension(string extension)
	{
		extension = extension.ToLowerInvariant();
		if (IsImageType(extension))
		{
			return StiResourceType.Image;
		}
		switch (extension)
		{
		case ".csv":
			return StiResourceType.Csv;
		case ".dbf":
			return StiResourceType.Dbf;
		case ".xls":
		case ".xlsx":
			return StiResourceType.Excel;
		case ".json":
			return StiResourceType.Json;
		case ".xml":
			return StiResourceType.Xml;
		case ".xsd":
			return StiResourceType.Xsd;
		case ".ttf":
			return StiResourceType.FontTtf;
		case ".otf":
			return StiResourceType.FontOtf;
		case ".woff":
			return StiResourceType.FontWoff;
		case ".ttc":
			return StiResourceType.FontTtc;
		case ".eot":
			return StiResourceType.FontEot;
		case ".rtf":
			return StiResourceType.Rtf;
		case ".txt":
			return StiResourceType.Txt;
		case ".mrt":
		case ".mrz":
			return StiResourceType.Report;
		case ".mdc":
		case ".mdz":
			return StiResourceType.ReportSnapshot;
		case ".pdf":
			return StiResourceType.Pdf;
		case ".doc":
		case ".docx":
			return StiResourceType.Word;
		case ".map":
			return StiResourceType.Map;
		case ".wkt":
		case ".geojson":
			return StiResourceType.Gis;
		default:
			return null;
		}
	}

	public static string GetExtension(StiResourceType type, byte[] content)
	{
		return type switch
		{
			StiResourceType.Excel => "xls", 
			StiResourceType.Image => "png", 
			StiResourceType.Report => "mrt", 
			StiResourceType.ReportSnapshot => "mdc", 
			StiResourceType.Word => "doc", 
			_ => type.ToString().ToLower().Replace("font", ""), 
		};
	}

	public static string GetDataSourceIconName(StiResourceType type)
	{
		switch (type)
		{
		case StiResourceType.Csv:
			return "Csv";
		case StiResourceType.Dbf:
			return "Dbf";
		case StiResourceType.Excel:
			return "Excel";
		case StiResourceType.Json:
			return "Json";
		case StiResourceType.Xml:
		case StiResourceType.Xsd:
			return "Xml";
		default:
			return null;
		}
	}

	public static bool PossibleConvertToDataSource(StiResourceType type)
	{
		if (type == StiResourceType.Csv || type == StiResourceType.Dbf || type == StiResourceType.Excel || type == StiResourceType.Json || type == StiResourceType.Gis || type == StiResourceType.Xml || type == StiResourceType.Xsd)
		{
			return true;
		}
		return false;
	}

	public static string GetFilter(StiResourceType type)
	{
		return type switch
		{
			StiResourceType.Csv => StiLocalization.Get("FileFilters", "CsvFiles"), 
			StiResourceType.Dbf => StiLocalization.Get("FileFilters", "DbfFiles"), 
			StiResourceType.Excel => StiLocalization.Get("FileFilters", "ExcelAllFiles"), 
			StiResourceType.Json => StiLocalization.Get("FileFilters", "JsonFiles"), 
			StiResourceType.Xml => StiLocalization.Get("FileFilters", "XmlFiles"), 
			StiResourceType.Xsd => StiLocalization.Get("FileFilters", "DataSetXmlSchema"), 
			StiResourceType.FontTtf => "(.ttf)|*.ttf", 
			StiResourceType.FontOtf => "(.otf)|*.otf", 
			StiResourceType.FontWoff => "(.woff)|*.woff", 
			StiResourceType.FontTtc => "(.ttc)|*.ttc", 
			StiResourceType.FontEot => "(.eot)|*.eot", 
			StiResourceType.Map => "(.map)|*.map", 
			StiResourceType.Rtf => StiLocalization.Get("FileFilters", "RtfFiles"), 
			StiResourceType.Txt => StiLocalization.Get("FileFilters", "TxtFiles"), 
			StiResourceType.Pdf => StiLocalization.Get("FileFilters", "PdfFiles"), 
			StiResourceType.Word => string.Format("{0}|{1}", StiLocalization.Get("FileFilters", "Word2007Files"), StiLocalization.Get("FileFilters", "WordFiles")), 
			StiResourceType.Report => string.Format("{0}|{1}", StiLocalization.Get("FileFilters", "ReportFiles"), StiLocalization.Get("FileFilters", "PackedReportFiles")), 
			StiResourceType.ReportSnapshot => string.Format("{0}|{1}", StiLocalization.Get("FileFilters", "DocumentFiles"), StiLocalization.Get("FileFilters", "PackedDocumentFiles")), 
			StiResourceType.Image => string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}", StiLocalization.Get("FileFilters", "GifFiles"), StiLocalization.Get("FileFilters", "PngFiles"), StiLocalization.Get("FileFilters", "JpegFiles"), StiLocalization.Get("FileFilters", "BmpFiles"), StiLocalization.Get("FileFilters", "TiffFiles"), "(.ico)|*.ico", StiLocalization.Get("FileFilters", "EmfFiles"), "(.wmf)|*.wmf", StiLocalization.Get("FileFilters", "SvgFiles")), 
			_ => null, 
		};
	}

	public static bool IsImageType(string ext)
	{
		return IsExtensionType(StiFileDialogHelper.ImageExts, ext);
	}

	public static bool IsTextType(string ext)
	{
		return IsExtensionType(StiFileDialogHelper.TextExts, ext);
	}

	private static bool IsExtensionType(string[] exts, string ext)
	{
		ext = (ext.StartsWith(".") ? ext.Substring(1) : ext);
		ext = ext.ToLowerInvariant();
		return exts.Any((string e) => e == ext);
	}
}
