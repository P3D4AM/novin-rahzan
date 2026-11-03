using System.Text;
using Stimulsoft.Data.Functions;
using Stimulsoft.Report.CodeDom;

namespace Stimulsoft.Report;

public class StiNameValidator
{
	public static string CorrectName(string str, bool isDataExpression = false)
	{
		return CorrectName(str, checkKeywords: true, null, isDataExpression);
	}

	public static string CorrectName(string str, bool checkKeywords, bool isDataExpression = false)
	{
		return CorrectName(str, checkKeywords, null, isDataExpression);
	}

	public static string CorrectName(string str, StiReport report, bool isDataExpression = false)
	{
		return CorrectName(str, checkKeywords: true, report, isDataExpression);
	}

	public static string CorrectName(string str, bool checkKeywords, StiReport report, bool isDataExpression = false)
	{
		if (isDataExpression)
		{
			return Funcs.ToExpression(str);
		}
		StringBuilder stringBuilder = new StringBuilder(str);
		for (int i = 0; i < stringBuilder.Length; i++)
		{
			if (!char.IsLetterOrDigit(stringBuilder[i]) && stringBuilder[i] != '_')
			{
				stringBuilder[i] = '_';
			}
		}
		str = stringBuilder.ToString();
		if (str.Length > 0 && char.IsDigit(str[0]))
		{
			str = "n" + str;
		}
		if (StiOptions.Engine.FullTrust && checkKeywords)
		{
			str = CheckKeyword(str, report);
		}
		return str;
	}

	private static string CheckKeyword(string str, StiReport report)
	{
		StiReportLanguageType stiReportLanguageType = StiReportLanguageType.CSharp;
		if (report != null)
		{
			stiReportLanguageType = report.ScriptLanguage;
		}
		if (((stiReportLanguageType == StiReportLanguageType.CSharp || stiReportLanguageType == StiReportLanguageType.JS) && StiCSharpCodeGenerator.IsKeywordExist(str)) || (stiReportLanguageType == StiReportLanguageType.VB && StiVBCodeGenerator.IsKeywordExist(str.ToLowerInvariant())))
		{
			if (str == "date")
			{
				return str;
			}
			return str + "_";
		}
		return str;
	}

	public static string CorrectBusinessObjectName(string str, bool isDataExpression = false)
	{
		return CorrectBusinessObjectName(str, null, isDataExpression);
	}

	public static string CorrectBusinessObjectName(string str, StiReport report, bool isDataExpression = false)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}
		if (isDataExpression)
		{
			return Funcs.ToExpression(str);
		}
		string[] array = str.Split(new char[1] { '.' });
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(CorrectName(array[i], report));
			if (i < array.Length - 1)
			{
				stringBuilder.Append(".");
			}
		}
		return stringBuilder.ToString();
	}
}
