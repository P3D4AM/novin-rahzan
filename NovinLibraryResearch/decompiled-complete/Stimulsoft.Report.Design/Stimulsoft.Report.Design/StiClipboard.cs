using System;
using System.Windows.Forms;

namespace Stimulsoft.Report.Design;

public class StiClipboard
{
	private static string buffer;

	public static bool LocalMode { get; set; }

	public static void Set(string text)
	{
		try
		{
			if (!LocalMode)
			{
				Clipboard.SetDataObject((object)text);
			}
			else
			{
				buffer = text;
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(typeof(StiClipboard), "Set clipboard...ERROR");
			StiLogService.Write(typeof(StiClipboard), e);
		}
	}

	public static void SetData(string typeKey, object obj)
	{
		try
		{
			Clipboard.SetData(typeKey, obj);
		}
		catch (Exception e)
		{
			StiLogService.Write(typeof(StiClipboard), "Set clipboard...ERROR");
			StiLogService.Write(typeof(StiClipboard), e);
		}
	}

	public static string GetData(string typeKey)
	{
		try
		{
			if (Clipboard.ContainsData(typeKey))
			{
				return (string)Clipboard.GetData(typeKey);
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(typeof(StiClipboard), "Get clipboard...ERROR");
			StiLogService.Write(typeof(StiClipboard), e);
		}
		return null;
	}

	public static string Get()
	{
		try
		{
			if (LocalMode)
			{
				return buffer;
			}
			IDataObject dataObject = Clipboard.GetDataObject();
			object data = dataObject.GetData(typeof(string));
			if (data is string)
			{
				return data as string;
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(typeof(StiClipboard), "Get clipboard...ERROR");
			StiLogService.Write(typeof(StiClipboard), e);
		}
		return null;
	}
}
