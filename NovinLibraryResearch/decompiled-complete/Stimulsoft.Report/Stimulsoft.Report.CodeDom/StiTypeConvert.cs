using System;

namespace Stimulsoft.Report.CodeDom;

public sealed class StiTypeConvert
{
	public static string GetBaseType(Type type, StiLanguage language)
	{
		StiCodeGenerator stiCodeGenerator = null;
		if (language != null)
		{
			stiCodeGenerator = language.GetProvider().CreateGenerator() as StiCodeGenerator;
		}
		string fullName = type.FullName;
		if (fullName[fullName.Length - 1] == ']')
		{
			string text = ((language != null) ? stiCodeGenerator.GetBaseTypeOutput(fullName.Substring(0, fullName.Length - 2)) : fullName);
			if (!(text == fullName))
			{
				return text + "[]";
			}
			return type.Name;
		}
		string text2 = ((language != null) ? stiCodeGenerator.GetBaseTypeOutput(type.FullName) : type.FullName);
		if (!(text2 == fullName))
		{
			return text2;
		}
		return type.Name;
	}
}
