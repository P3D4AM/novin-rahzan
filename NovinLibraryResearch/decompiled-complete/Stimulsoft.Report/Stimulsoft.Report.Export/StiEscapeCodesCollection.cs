using System;
using System.Collections;
using System.Text;

namespace Stimulsoft.Report.Export;

public class StiEscapeCodesCollection : Hashtable
{
	public string Name { get; set; } = string.Empty;


	public static StiEscapeCodesCollection GetEscapeCodesCollection(string escapeCodesCollectionName)
	{
		StiEscapeCodesCollection result = new StiEscapeCodesCollection();
		foreach (StiEscapeCodesCollection escapeCodesCollection in StiOptions.Export.Text.EscapeCodesCollectionList)
		{
			if (escapeCodesCollection.Name == escapeCodesCollectionName)
			{
				result = escapeCodesCollection;
			}
		}
		return result;
	}

	public static string ConvertEscapeCodes(string text, string escapeCodesCollectionName)
	{
		StiEscapeCodesCollection escapeCodesCollection = GetEscapeCodesCollection(escapeCodesCollectionName);
		return ConvertEscapeCodes(text, escapeCodesCollection);
	}

	public static string ConvertEscapeCodes(string text, StiEscapeCodesCollection escapeCodesCollection)
	{
		StringBuilder stringBuilder = new StringBuilder(text.Length);
		int num = 0;
		int startIndex = num;
		while ((startIndex = text.IndexOf("<#", startIndex, StringComparison.InvariantCulture)) != -1)
		{
			int num2 = text.IndexOf(">", startIndex + 3, StringComparison.InvariantCulture);
			if (num2 == -1)
			{
				break;
			}
			if (num2 - startIndex > 64)
			{
				startIndex += 2;
				continue;
			}
			stringBuilder.Append(text.Substring(num, startIndex - num));
			string text2 = text.Substring(startIndex + 2, num2 - startIndex - 2).Trim();
			string key = text2.ToLowerInvariant();
			if (escapeCodesCollection.ContainsKey(text2))
			{
				string value = (string)escapeCodesCollection[text2];
				stringBuilder.Append(value);
			}
			else if (escapeCodesCollection.ContainsKey(key))
			{
				string value2 = (string)escapeCodesCollection[key];
				stringBuilder.Append(value2);
			}
			num = num2 + 1;
			startIndex = num;
		}
		if (num < text.Length)
		{
			stringBuilder.Append(text.Substring(num));
		}
		return stringBuilder.ToString();
	}
}
