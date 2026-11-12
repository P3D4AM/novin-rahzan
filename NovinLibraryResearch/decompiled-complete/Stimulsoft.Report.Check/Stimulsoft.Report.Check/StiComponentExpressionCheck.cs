using System.Xml;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiComponentExpressionCheck : StiComponentCheck
{
	public string ComponentName;

	public string PropertyName;

	public string Message;

	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiComponentExpressionCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiComponentExpressionCheckLong") + " " + Message, PropertyName, ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	private string CheckExpression(StiReport report, StiComponent comp, string expression)
	{
		return StiParser.CheckExpression(expression, comp)?.Message;
	}

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		try
		{
			if (report != null && report.CalculationMode != StiCalculationMode.Interpretation)
			{
				return null;
			}
			if (obj is StiText stiText)
			{
				string text = CheckExpression(report, stiText, stiText.Text);
				if (text != null)
				{
					StiComponentExpressionCheck stiComponentExpressionCheck = new StiComponentExpressionCheck();
					stiComponentExpressionCheck.Element = obj;
					stiComponentExpressionCheck.PropertyName = "Text";
					stiComponentExpressionCheck.ComponentName = stiText.Name;
					stiComponentExpressionCheck.Message = text;
					stiComponentExpressionCheck.Actions.Add(new StiEditPropertyAction());
					return stiComponentExpressionCheck;
				}
			}
			if (obj is StiRichText stiRichText)
			{
				string expression = XmlConvert.DecodeName(stiRichText.Text.Value).Replace('\0', ' ');
				string text = CheckExpression(report, stiRichText, expression);
				if (text != null)
				{
					StiComponentExpressionCheck stiComponentExpressionCheck2 = new StiComponentExpressionCheck();
					stiComponentExpressionCheck2.Element = obj;
					stiComponentExpressionCheck2.PropertyName = "Text";
					stiComponentExpressionCheck2.ComponentName = stiRichText.Name;
					stiComponentExpressionCheck2.Message = text;
					stiComponentExpressionCheck2.Actions.Add(new StiEditPropertyAction());
					return stiComponentExpressionCheck2;
				}
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
