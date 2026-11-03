using System;

namespace Stimulsoft.Report.CodeDom;

public abstract class StiLanguage
{
	public abstract StiCodeDomProvider GetProvider();

	public static void GetGeneratedCodePos(string text, out int index, out int length, StiLanguage language)
	{
		if (!(language.GetProvider().CreateGenerator() is StiCodeGenerator stiCodeGenerator))
		{
			throw new Exception("Provider must be inherited from StiCodeGenerator");
		}
		string regionStart = stiCodeGenerator.GetRegionStart("StiReport Designer generated code - do not modify");
		string regionEnd = stiCodeGenerator.GetRegionEnd("StiReport Designer generated code - do not modify");
		int num = text.IndexOf(regionStart, StringComparison.InvariantCulture);
		index = num + regionStart.Length;
		int num2 = text.IndexOf(regionEnd, StringComparison.InvariantCulture);
		if (num == -1 || num2 == -1)
		{
			index = 0;
			num2 = 0;
		}
		length = num2 - index;
	}

	public static string RemoveGeneratedCode(string text, StiLanguage language)
	{
		GetGeneratedCodePos(text, out var index, out var length, language);
		if (length <= 0)
		{
			return text;
		}
		return text.Remove(index, length);
	}

	public static string GetGeneratedCode(string text, StiLanguage language)
	{
		GetGeneratedCodePos(text, out var index, out var length, language);
		if (length <= 0)
		{
			return string.Empty;
		}
		return text.Substring(index, length);
	}

	public static string InsertGeneratedCode(string text, string script, StiLanguage language)
	{
		text = RemoveGeneratedCode(text, language);
		GetGeneratedCodePos(text, out var index, out var _, language);
		if (index >= 0 && index <= text.Length)
		{
			return text.Insert(index, script);
		}
		return text;
	}

	public static string ReplaceGeneratedCode(string text, string script, StiLanguage language)
	{
		string generatedCode = GetGeneratedCode(script, language);
		if (string.IsNullOrEmpty(text) || (text.Length < 10 && text.Trim().Length == 0))
		{
			return script;
		}
		text = RemoveGeneratedCode(text, language);
		return InsertGeneratedCode(text, generatedCode, language);
	}
}
