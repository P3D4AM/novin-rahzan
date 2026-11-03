using System.Collections.Generic;
using System.Windows.Forms;

namespace Stimulsoft.Report.Helpers;

public class StiClipboardHelper
{
	public static bool Enabled { get; set; } = true;


	public static IDictionary<string, object> GetClipboardData()
	{
		if (!Enabled)
		{
			return null;
		}
		try
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			IDataObject dataObject = Clipboard.GetDataObject();
			string[] array = ((dataObject != null) ? dataObject.GetFormats() : null);
			foreach (string text in array)
			{
				dictionary.Add(text, dataObject.GetData(text));
			}
			return dictionary;
		}
		catch
		{
		}
		return null;
	}

	public static void SetClipboardData(IDictionary<string, object> dict)
	{
		try
		{
			Clipboard.Clear();
			if (!Enabled || dict == null)
			{
				return;
			}
			foreach (KeyValuePair<string, object> item in dict)
			{
				Clipboard.SetData(item.Key, item.Value);
			}
		}
		catch
		{
		}
	}
}
