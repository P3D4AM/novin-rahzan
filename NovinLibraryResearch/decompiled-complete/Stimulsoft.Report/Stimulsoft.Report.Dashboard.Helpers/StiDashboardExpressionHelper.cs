using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Dashboard.Helpers;

public static class StiDashboardExpressionHelper
{
	public static Color GetBackColor(object component, Color defaultColor)
	{
		return GetColor(component, defaultColor, "BackColor");
	}

	public static Color GetForeColor(object component, Color defaultColor)
	{
		return GetColor(component, defaultColor, "ForeColor");
	}

	private static Color GetColor(object component, Color defaultColor, string propertyName)
	{
		StiComponent stiComponent = component as StiComponent;
		if (StiAppExpressionHelper.IsExpressionSpecified((object)stiComponent, propertyName))
		{
			return StiAppExpressionParser.ParseColorExpression(stiComponent, propertyName, allowDataLoading: true);
		}
		return defaultColor;
	}
}
