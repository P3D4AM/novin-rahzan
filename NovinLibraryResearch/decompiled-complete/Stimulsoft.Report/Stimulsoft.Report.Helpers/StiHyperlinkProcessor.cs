using System;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Helpers;

public static class StiHyperlinkProcessor
{
	public const string ServerIdent = "stimulsoft-server://";

	public const string ResourceIdent = "resource://";

	public const string VariableIdent = "variable://";

	public const string DataColumnIdent = "datacolumn://";

	public const string FileIdent = "file://";

	public const string HttpIdent = "http://";

	public const string HttpsIdent = "https://";

	public static byte[] TryGetBytes(StiReport report, string hyperlink, bool firstPositionInDataSource = false, bool allowDataLoading = false)
	{
		try
		{
			return GetBytes(report, hyperlink, firstPositionInDataSource, allowDataLoading);
		}
		catch
		{
			return null;
		}
	}

	public static byte[] GetBytes(StiReport report, string hyperlink, bool firstPositionInDataSource = false, bool allowDataLoading = false)
	{
		string resourceNameFromHyperlink = GetResourceNameFromHyperlink(hyperlink);
		if (resourceNameFromHyperlink != null)
		{
			return GetResource(report, resourceNameFromHyperlink)?.Content;
		}
		string variableNameFromHyperlink = GetVariableNameFromHyperlink(hyperlink);
		if (variableNameFromHyperlink != null)
		{
			StiVariable variable = GetVariable(report, variableNameFromHyperlink);
			if (variable != null)
			{
				if (variable.ValueObject is Image)
				{
					object valueObject = variable.ValueObject;
					return StiImageConverter.ImageToBytes((Image)((valueObject is Image) ? valueObject : null), true);
				}
				if (variable.ValueObject is byte[] && StiImageHelper.IsImage(variable.ValueObject as byte[]))
				{
					return variable.ValueObject as byte[];
				}
				if (variable.ValueObject is string && IsHttpOrHttpsHyperlink(variable.ValueObject as string))
				{
					return StiBytesFromURL.Load(variable.ValueObject as string, report?.CookieContainer, report?.HttpHeadersContainer);
				}
				if (variable.ValueObject is string && IsFilePath(variable.ValueObject as string))
				{
					return File.ReadAllBytes(variable.ValueObject as string);
				}
			}
			return null;
		}
		string dataColumnNameFromHyperlink = GetDataColumnNameFromHyperlink(hyperlink);
		if (dataColumnNameFromHyperlink != null)
		{
			if (allowDataLoading)
			{
				byte[] bytesFromColumnWithLoading = GetBytesFromColumnWithLoading(report, dataColumnNameFromHyperlink);
				if (bytesFromColumnWithLoading != null)
				{
					return bytesFromColumnWithLoading;
				}
			}
			return StiDataColumn.GetDatasFromDataColumn(report.Dictionary, dataColumnNameFromHyperlink, null, firstPositionInDataSource).FirstOrDefault() as byte[];
		}
		string fileNameFromHyperlink = GetFileNameFromHyperlink(hyperlink);
		if (fileNameFromHyperlink != null)
		{
			if (!File.Exists(fileNameFromHyperlink))
			{
				return null;
			}
			return File.ReadAllBytes(fileNameFromHyperlink);
		}
		return StiDownloadCache.Get(hyperlink, (CookieContainer)null, (NameValueCollection)null);
	}

	private static byte[] GetBytesFromColumnWithLoading(StiReport report, string dataColumnName)
	{
		StiDataSource dataSourceFromDataColumn = StiDataColumn.GetDataSourceFromDataColumn(report.Dictionary, dataColumnName);
		if (dataSourceFromDataColumn == null)
		{
			return null;
		}
		DataTable dataTable = StiDataPicker.Fetch((IStiApp)(object)report, (IStiAppDataSource)(object)dataSourceFromDataColumn, (StiDataRequestOption)3);
		if (dataTable == null)
		{
			return null;
		}
		if (!dataTable.Columns.Contains(dataColumnName))
		{
			return null;
		}
		object obj = dataTable.Rows[0][dataColumnName];
		if (obj is string)
		{
			try
			{
				obj = Convert.FromBase64String(obj as string);
			}
			catch
			{
			}
		}
		return obj as byte[];
	}

	public static Image GetImage(StiReport report, string hyperlink, int width, int height)
	{
		string resourceNameFromHyperlink = GetResourceNameFromHyperlink(hyperlink);
		if (resourceNameFromHyperlink != null)
		{
			return GetResource(report, resourceNameFromHyperlink)?.GetResourceAsImage();
		}
		string variableNameFromHyperlink = GetVariableNameFromHyperlink(hyperlink);
		if (variableNameFromHyperlink != null)
		{
			object obj = GetVariable(report, variableNameFromHyperlink)?.ValueObject;
			if (!(obj is byte[]))
			{
				return (Image)((obj is Image) ? obj : null);
			}
			return StiImageConverter.BytesToImage(obj as byte[], 200, 200, true, false);
		}
		string dataColumnNameFromHyperlink = GetDataColumnNameFromHyperlink(hyperlink);
		if (dataColumnNameFromHyperlink != null)
		{
			object obj2 = StiDataColumn.GetDatasFromDataColumn(report.Dictionary, dataColumnNameFromHyperlink).FirstOrDefault();
			if (!(obj2 is byte[]))
			{
				return (Image)((obj2 is Image) ? obj2 : null);
			}
			return StiImageConverter.BytesToImage(obj2 as byte[], 200, 200, true, false);
		}
		string fileNameFromHyperlink = GetFileNameFromHyperlink(hyperlink);
		if (fileNameFromHyperlink != null)
		{
			return StiImageHelper.FromFile(fileNameFromHyperlink, width, height);
		}
		return StiImageFromURL.LoadImage(hyperlink, report?.CookieContainer, report?.HttpHeadersContainer);
	}

	public static string GetString(StiReport report, string hyperlink)
	{
		string resourceNameFromHyperlink = GetResourceNameFromHyperlink(hyperlink);
		if (resourceNameFromHyperlink != null)
		{
			StiResource resource = GetResource(report, resourceNameFromHyperlink);
			if (resource == null)
			{
				return null;
			}
			return StiBytesToStringConverter.ConvertBytesToString(resource.Content);
		}
		string variableNameFromHyperlink = GetVariableNameFromHyperlink(hyperlink);
		if (variableNameFromHyperlink != null)
		{
			return GetVariable(report, variableNameFromHyperlink)?.ValueObject as string;
		}
		string dataColumnNameFromHyperlink = GetDataColumnNameFromHyperlink(hyperlink);
		if (dataColumnNameFromHyperlink != null)
		{
			return StiDataColumn.GetDatasFromDataColumn(report.Dictionary, dataColumnNameFromHyperlink).FirstOrDefault() as string;
		}
		string fileNameFromHyperlink = GetFileNameFromHyperlink(hyperlink);
		if (fileNameFromHyperlink != null)
		{
			if (!File.Exists(fileNameFromHyperlink))
			{
				return null;
			}
			return File.ReadAllText(fileNameFromHyperlink);
		}
		return StiBytesToStringConverter.ConvertBytesToString(StiDownloadCache.Get(hyperlink, (CookieContainer)null, (NameValueCollection)null));
	}

	private static StiResource GetResource(StiReport report, string resourceName)
	{
		if (report == null || string.IsNullOrWhiteSpace(resourceName))
		{
			return null;
		}
		resourceName = resourceName.ToLowerInvariant().Trim();
		return report.Dictionary.Resources.ToList().FirstOrDefault((StiResource r) => r.Name != null && r.Name.ToLowerInvariant().Trim() == resourceName);
	}

	private static StiVariable GetVariable(StiReport report, string variableName)
	{
		if (report == null || string.IsNullOrWhiteSpace(variableName))
		{
			return null;
		}
		variableName = variableName.ToLowerInvariant().Trim();
		return report.Dictionary.Variables.ToList().FirstOrDefault((StiVariable v) => v.Name != null && v.Name.ToLowerInvariant().Trim() == variableName);
	}

	private static StiDataColumn GetDataColumn(StiReport report, string dataColumnName)
	{
		if (report == null || string.IsNullOrWhiteSpace(dataColumnName))
		{
			return null;
		}
		dataColumnName = dataColumnName.ToLowerInvariant().Trim();
		return StiDataBuilder.GetColumnFromPath(dataColumnName, report.Dictionary);
	}

	public static string GetServerNameFromHyperlink(string hyperlink)
	{
		if (!IsServerHyperlink(hyperlink))
		{
			return null;
		}
		return hyperlink.Remove(0, "stimulsoft-server://".Length);
	}

	public static string GetResourceNameFromHyperlink(string hyperlink)
	{
		if (!IsResourceHyperlink(hyperlink))
		{
			return null;
		}
		return hyperlink.Remove(0, "resource://".Length);
	}

	public static string GetVariableNameFromHyperlink(string hyperlink)
	{
		if (!IsVariableHyperlink(hyperlink))
		{
			return null;
		}
		return hyperlink.Remove(0, "variable://".Length);
	}

	public static string GetDataColumnNameFromHyperlink(string hyperlink)
	{
		if (!IsDataColumnHyperlink(hyperlink))
		{
			return null;
		}
		return hyperlink.Remove(0, "datacolumn://".Length);
	}

	public static string GetRealDataColumnFromHyperlink(StiReport report, string hyperlink)
	{
		string dataColumnNameFromHyperlink = GetDataColumnNameFromHyperlink(hyperlink);
		if (string.IsNullOrWhiteSpace(dataColumnNameFromHyperlink))
		{
			return null;
		}
		StiDataSource dataSourceFromDataColumn = StiDataColumn.GetDataSourceFromDataColumn(report.Dictionary, dataColumnNameFromHyperlink);
		if (dataSourceFromDataColumn == null)
		{
			return null;
		}
		string columnNameFromDataColumn = StiDataColumn.GetColumnNameFromDataColumn(report.Dictionary, dataColumnNameFromHyperlink);
		if (string.IsNullOrWhiteSpace(columnNameFromDataColumn))
		{
			return null;
		}
		return dataColumnNameFromHyperlink;
	}

	public static string GetFileNameFromHyperlink(string hyperlink)
	{
		if (!IsFileHyperlink(hyperlink))
		{
			return null;
		}
		return hyperlink.Remove(0, "file://".Length);
	}

	public static bool IsServerHyperlink(string hyperlink)
	{
		if (!string.IsNullOrWhiteSpace(hyperlink))
		{
			return StringExt.StartsWithInvariantIgnoreCase(hyperlink, "stimulsoft-server://");
		}
		return false;
	}

	public static bool IsResourceHyperlink(string hyperlink)
	{
		if (!string.IsNullOrWhiteSpace(hyperlink))
		{
			return StringExt.StartsWithInvariantIgnoreCase(hyperlink, "resource://");
		}
		return false;
	}

	public static bool IsVariableHyperlink(string hyperlink)
	{
		if (!string.IsNullOrWhiteSpace(hyperlink))
		{
			return StringExt.StartsWithInvariantIgnoreCase(hyperlink, "variable://");
		}
		return false;
	}

	public static bool IsDataColumnHyperlink(string hyperlink)
	{
		if (!string.IsNullOrWhiteSpace(hyperlink))
		{
			return StringExt.StartsWithInvariantIgnoreCase(hyperlink, "datacolumn://");
		}
		return false;
	}

	public static bool IsFileHyperlink(string hyperlink)
	{
		if (!string.IsNullOrWhiteSpace(hyperlink))
		{
			return StringExt.StartsWithInvariantIgnoreCase(hyperlink, "file://");
		}
		return false;
	}

	public static bool IsFilePath(string path)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(path) || path.Length > 260)
			{
				return false;
			}
			Path.GetFullPath(path);
		}
		catch (PathTooLongException)
		{
			return false;
		}
		return true;
	}

	public static bool IsHttpOrHttpsHyperlink(string hyperlink)
	{
		if (!string.IsNullOrWhiteSpace(hyperlink))
		{
			if (!StringExt.StartsWithInvariantIgnoreCase(hyperlink, "http://"))
			{
				return StringExt.StartsWithInvariantIgnoreCase(hyperlink, "https://");
			}
			return true;
		}
		return false;
	}

	public static string CreateResourceName(string name)
	{
		return "resource://" + name;
	}

	public static string CreateVariableName(string name)
	{
		return "variable://" + name;
	}

	public static string CreateDataColumnName(string name)
	{
		return "datacolumn://" + name;
	}

	public static string CreateFileName(string path)
	{
		return "file://" + path;
	}

	public static string HyperlinkToString(string hyperlink)
	{
		if (IsResourceHyperlink(hyperlink))
		{
			return Loc.Get("PropertyMain", "Resource") + ": " + GetResourceNameFromHyperlink(hyperlink);
		}
		if (IsVariableHyperlink(hyperlink))
		{
			return Loc.Get("PropertyMain", "Variable") + ": " + GetVariableNameFromHyperlink(hyperlink);
		}
		if (IsDataColumnHyperlink(hyperlink))
		{
			return Loc.Get("PropertyMain", "DataColumn") + ": " + GetDataColumnNameFromHyperlink(hyperlink);
		}
		if (IsServerHyperlink(hyperlink))
		{
			return "Server: " + GetVariableNameFromHyperlink(hyperlink);
		}
		if (IsFileHyperlink(hyperlink))
		{
			return Loc.Get("PropertyMain", "File") + ": " + GetFileNameFromHyperlink(hyperlink);
		}
		return Loc.Get("PropertyMain", "Hyperlink") + ": " + hyperlink;
	}
}
