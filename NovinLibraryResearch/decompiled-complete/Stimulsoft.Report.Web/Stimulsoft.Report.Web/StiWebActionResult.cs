using System;
using System.Collections;
using System.IO;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Web;

public class StiWebActionResult
{
	public byte[] Data { get; set; }

	public string ContentType { get; set; }

	public string FileName { get; set; }

	public bool ShowSaveFileDialog { get; set; }

	private static StiWebActionResult CompressedResult(string data)
	{
		string data2 = StiGZipHelper.Pack(data);
		return new StiWebActionResult(data2);
	}

	public static StiWebActionResult StringResult(StiRequestParams requestParams, string data)
	{
		if (requestParams.Server.UseCompression)
		{
			return CompressedResult(data);
		}
		return new StiWebActionResult(data);
	}

	public static StiWebActionResult JsonResult(StiRequestParams requestParams, Hashtable data)
	{
		string data2 = JsonConvert.SerializeObject(data, Formatting.None, new StringEnumConverter());
		if (requestParams.Server.UseCompression)
		{
			return CompressedResult(data2);
		}
		return new StiWebActionResult(data2, "application/json");
	}

	public static StiWebActionResult DashboardNotSupportedResult(StiRequestParams requestParams)
	{
		string text = string.Format("{0}<br><br>{1}", StiLocalization.Get("Dashboard", "CannotLoadDashboard"), "You should use the Stimulsoft Dashboards.WEB product.");
		if (requestParams.ComponentType == StiComponentType.Designer)
		{
			Hashtable hashtable = new Hashtable();
			hashtable["infoMessage"] = text;
			return JsonResult(requestParams, hashtable);
		}
		return ErrorResult(requestParams, text);
	}

	public static StiWebActionResult ErrorResult(StiRequestParams requestParams, string data)
	{
		if (requestParams.ComponentType == StiComponentType.Designer)
		{
			Hashtable hashtable = new Hashtable();
			hashtable["error"] = data;
			if (requestParams.Designer.Command == StiDesignerCommand.SaveReport && requestParams.Designer.IsNewReport)
			{
				hashtable["isNewReport"] = true;
			}
			return JsonResult(requestParams, hashtable);
		}
		return StringResult(requestParams, "ServerError:" + data);
	}

	public static StiWebActionResult EmptyResult()
	{
		return new StiWebActionResult();
	}

	public static StiWebActionResult EmptyJsonResult()
	{
		return new StiWebActionResult("{}", "application/json");
	}

	public static StiWebActionResult EmptyReportResult()
	{
		return new StiWebActionResult("ServerError:The report is not specified.");
	}

	public static string GetErrorMessageText(Exception e)
	{
		if (e == null)
		{
			return string.Empty;
		}
		return ((!string.IsNullOrEmpty(e.Message)) ? e.Message : string.Empty) + ((e.InnerException != null && !string.IsNullOrEmpty(e.InnerException.Message)) ? (" " + e.InnerException.Message) : string.Empty);
	}

	public StiWebActionResult()
		: this(string.Empty)
	{
	}

	public StiWebActionResult(string data)
		: this(data, "text/plain")
	{
	}

	public StiWebActionResult(string data, string contentType, string fileName = null, bool showSaveFileDialog = true)
		: this(Encoding.UTF8.GetBytes(data), contentType, fileName, showSaveFileDialog)
	{
	}

	public StiWebActionResult(Stream data, string contentType, string fileName = null, bool showSaveFileDialog = true)
		: this(((MemoryStream)data).ToArray(), contentType, fileName, showSaveFileDialog)
	{
	}

	public StiWebActionResult(byte[] data, string contentType, string fileName = null, bool showSaveFileDialog = true)
	{
		Data = data;
		ContentType = contentType;
		FileName = fileName;
		ShowSaveFileDialog = !string.IsNullOrEmpty(fileName) && showSaveFileDialog;
	}
}
