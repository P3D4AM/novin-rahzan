using Stimulsoft.Report.Components;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Helpers;

public class StiExpressionHelper
{
	public static string ParseText(StiPage page, string text, bool allowReturnNull = false)
	{
		if (string.IsNullOrEmpty(text) || page == null)
		{
			return text;
		}
		if (text.Contains("{") && text.Contains("}") && !text.Contains("{{") && !text.Contains("}}"))
		{
			try
			{
				StiText component = new StiText
				{
					Page = page
				};
				object obj = StiParser.ParseTextValue(text, component);
				if (obj != null)
				{
					return obj.ToString();
				}
				if (allowReturnNull)
				{
					return null;
				}
			}
			catch
			{
			}
		}
		return text;
	}

	public static bool ParseBool(StiPage page, string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		try
		{
			StiText component = new StiText
			{
				Page = page
			};
			text = text.Trim();
			if (!text.StartsWith("{"))
			{
				text = "{" + text;
			}
			if (!text.EndsWith("}"))
			{
				text += "}";
			}
			object obj = StiParser.ParseTextValue(text, component);
			if (obj is bool result)
			{
				return result;
			}
			if (obj is string && bool.TryParse((string)obj, out var result2))
			{
				return result2;
			}
		}
		catch
		{
		}
		return false;
	}
}
