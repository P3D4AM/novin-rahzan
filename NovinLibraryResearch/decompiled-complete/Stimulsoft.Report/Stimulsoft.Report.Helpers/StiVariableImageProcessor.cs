using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Helpers;

public static class StiVariableImageProcessor
{
	public static byte[] GetImage(StiReport report, string expression)
	{
		if (string.IsNullOrWhiteSpace(expression))
		{
			return null;
		}
		string variableNameFromExpression = GetVariableNameFromExpression(expression);
		if (variableNameFromExpression == null)
		{
			return null;
		}
		StiVariable stiVariable = report.Dictionary.Variables[variableNameFromExpression];
		if (stiVariable != null)
		{
			if (stiVariable.ValueObject is Image)
			{
				object valueObject = stiVariable.ValueObject;
				return StiImageConverter.ImageToBytes((Image)((valueObject is Image) ? valueObject : null), false);
			}
			if (stiVariable.ValueObject is byte[] && StiImageHelper.IsImage(stiVariable.ValueObject as byte[]))
			{
				return stiVariable.ValueObject as byte[];
			}
		}
		return null;
	}

	public static string GetVariableNameFromExpression(string expression)
	{
		if (expression == null)
		{
			return null;
		}
		string text = expression.Trim();
		if (!text.StartsWith("{") || !text.EndsWith("}"))
		{
			return null;
		}
		return text.Substring(1, text.Length - 2);
	}
}
