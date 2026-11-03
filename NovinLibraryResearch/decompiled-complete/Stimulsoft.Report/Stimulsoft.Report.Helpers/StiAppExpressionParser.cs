using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Helpers;

public static class StiAppExpressionParser
{
	public static void ProcessExpressions(StiComponent component, bool allowDataLoading = false)
	{
		if (component == null)
		{
			return;
		}
		if (((component != null) ? ((IStiAppExpressionCollection)component).Expressions : null) == null || ((CollectionBase)(object)((IStiAppExpressionCollection)component).Expressions).Count == 0)
		{
			return;
		}
		Type type = ((object)component).GetType();
		foreach (StiAppExpression item in from e in ((IStiAppExpressionCollection)component).Expressions.ToList()
			where !e.IsEmpty
			select e)
		{
			try
			{
				PropertyInfo property = type.GetProperty(item.Name);
				if (property == null)
				{
					continue;
				}
				if (property.PropertyType == typeof(bool))
				{
					property.SetValue(component, ParseBoolExpression(component, item.Name, allowDataLoading));
				}
				if (property.PropertyType == typeof(string))
				{
					property.SetValue(component, ParseStringExpression(component, item.Name, allowDataLoading));
				}
				if (property.PropertyType == typeof(Color))
				{
					property.SetValue(component, ParseColorExpression(component, item.Name, allowDataLoading));
				}
				if (property.PropertyType == typeof(StiBrush))
				{
					property.SetValue(component, ParseBrushExpression(component, item.Name, allowDataLoading));
				}
				if (property.PropertyType.IsEnum)
				{
					object obj = ParseEnumExpression(component, item.Name, property.PropertyType, allowDataLoading);
					if (obj != null)
					{
						property.SetValue(component, obj);
					}
				}
			}
			catch
			{
			}
		}
	}

	public static bool ParseBoolExpression(StiComponent component, string propName, bool allowDataLoading = false)
	{
		object obj = ParseExpression(component, StiAppExpressionHelper.GetExpression((object)component, propName), allowDataLoading);
		return StiValueHelper.TryToBool(obj);
	}

	public static object ParseEnumExpression(StiComponent component, string propName, Type enumType, bool allowDataLoading = false)
	{
		object obj = ParseExpression(component, StiAppExpressionHelper.GetExpression((object)component, propName), allowDataLoading);
		try
		{
			string text = obj?.ToString();
			return Enum.Parse(enumType, text.Trim(), ignoreCase: true);
		}
		catch
		{
			return null;
		}
	}

	public static string ParseStringExpression(StiComponent component, string propName, bool allowDataLoading = false)
	{
		string text = ParseExpression(component, StiAppExpressionHelper.GetExpression((object)component, propName), allowDataLoading)?.ToString();
		return text ?? "";
	}

	public static Color ParseColorExpression(StiComponent component, string propName, bool allowDataLoading = false)
	{
		object obj = ParseExpression(component, StiAppExpressionHelper.GetExpression((object)component, propName), allowDataLoading);
		if (obj is Color)
		{
			return (Color)obj;
		}
		string text = obj?.ToString();
		if (!string.IsNullOrWhiteSpace(text))
		{
			return StiColor.Get(text);
		}
		return Color.Transparent;
	}

	public static StiBrush ParseBrushExpression(StiComponent component, string propName, bool allowDataLoading = false)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		object obj = ParseExpression(component, StiAppExpressionHelper.GetExpression((object)component, propName), allowDataLoading);
		if (obj is Color color)
		{
			return (StiBrush)new StiSolidBrush(color);
		}
		StiBrush val = (StiBrush)((obj is StiBrush) ? obj : null);
		if (val != null)
		{
			return val;
		}
		string text = obj?.ToString();
		return (StiBrush)new StiSolidBrush(string.IsNullOrWhiteSpace(text) ? Color.Transparent : StiColor.Get(text));
	}

	private static object ParseExpression(StiComponent component, StiAppExpression expression, bool allowDataLoading = false)
	{
		if (string.IsNullOrWhiteSpace((expression != null) ? expression.Expression : null))
		{
			return null;
		}
		return StiReportParser.ParseObject(expression.Expression, (IStiReportComponent)(object)component, allowCache: false, null, allowDataLoading, onlyExpression: true);
	}
}
