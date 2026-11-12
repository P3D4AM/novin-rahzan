using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Table;

public static class StiTableElementConditionHelper
{
	private static bool GetConditionResult(object value, IStiTableElementCondition condition, StiTableElement element)
	{
		bool result = false;
		if (value != null)
		{
			try
			{
				if (condition.DataType == Stimulsoft.Report.Components.StiFilterDataType.Numeric)
				{
					double result2 = 0.0;
					if (double.TryParse(value.ToString(), out result2))
					{
						value = result2;
					}
				}
				else if (condition.DataType == Stimulsoft.Report.Components.StiFilterDataType.DateTime)
				{
					value = ((!DateTime.TryParse(value.ToString(), out var result3)) ? ((object)DateTime.FromOADate((double)value)) : ((object)result3));
				}
			}
			catch
			{
			}
			object obj2 = GetConditionValue(condition, element);
			IComparable comparable = value as IComparable;
			if (comparable != null && obj2 != null)
			{
				if (value.GetType() != obj2.GetType())
				{
					value = value.ToString();
					obj2 = obj2.ToString();
					comparable = value as IComparable;
				}
				try
				{
					switch (condition.Condition)
					{
					case Stimulsoft.Report.Components.StiFilterCondition.EqualTo:
						return comparable.CompareTo(obj2) == 0;
					case Stimulsoft.Report.Components.StiFilterCondition.NotEqualTo:
						return comparable.CompareTo(obj2) != 0;
					case Stimulsoft.Report.Components.StiFilterCondition.GreaterThan:
						return comparable.CompareTo(obj2) == 1;
					case Stimulsoft.Report.Components.StiFilterCondition.GreaterThanOrEqualTo:
						return comparable.CompareTo(obj2) >= 0;
					case Stimulsoft.Report.Components.StiFilterCondition.LessThan:
						return comparable.CompareTo(obj2) == -1;
					case Stimulsoft.Report.Components.StiFilterCondition.LessThanOrEqualTo:
						return comparable.CompareTo(obj2) <= 0;
					case Stimulsoft.Report.Components.StiFilterCondition.Containing:
					{
						string text4 = value.ToString();
						string value5 = obj2.ToString();
						return text4.IndexOf(value5, StringComparison.InvariantCulture) != -1;
					}
					case Stimulsoft.Report.Components.StiFilterCondition.NotContaining:
					{
						string text3 = value.ToString();
						string value4 = obj2.ToString();
						return text3.IndexOf(value4, StringComparison.InvariantCulture) == -1;
					}
					case Stimulsoft.Report.Components.StiFilterCondition.BeginningWith:
					{
						string text2 = value.ToString();
						string value3 = obj2.ToString();
						return text2.StartsWith(value3, StringComparison.InvariantCulture);
					}
					case Stimulsoft.Report.Components.StiFilterCondition.EndingWith:
					{
						string text = value.ToString();
						string value2 = obj2.ToString();
						return text.EndsWith(value2, StringComparison.InvariantCulture);
					}
					case Stimulsoft.Report.Components.StiFilterCondition.Between:
					case Stimulsoft.Report.Components.StiFilterCondition.NotBetween:
						break;
					}
				}
				catch
				{
				}
			}
		}
		return result;
	}

	private static object GetConditionValue(IStiTableElementCondition condition, StiTableElement element)
	{
		try
		{
			string text = (condition.IsExpression ? StiReportParser.Parse("{" + condition.Value + "}", element) : condition.Value);
			switch (condition.DataType)
			{
			case Stimulsoft.Report.Components.StiFilterDataType.String:
				return text;
			case Stimulsoft.Report.Components.StiFilterDataType.Numeric:
			{
				double result2 = 0.0;
				double.TryParse(text?.Replace(".", ",").Replace(",", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator), out result2);
				return result2;
			}
			case Stimulsoft.Report.Components.StiFilterDataType.DateTime:
			{
				CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us", useUserOverride: false);
				DateTime result;
				bool flag = DateTime.TryParse(text, out result);
				Thread.CurrentThread.CurrentCulture = currentCulture;
				if (flag)
				{
					return result;
				}
				break;
			}
			case Stimulsoft.Report.Components.StiFilterDataType.Boolean:
				return text.ToLower(CultureInfo.InvariantCulture) == "true";
			}
		}
		catch (Exception innerException)
		{
			string message = $"Problem when parsing Table Element {condition} value\n";
			throw new Exception(message, innerException);
		}
		return null;
	}

	public static Color? ProcessBackColor(Color? defaultColor, string keyColumn, StiTableElement tableElement, object[] rowValues, string[] columnKeys)
	{
		foreach (IStiTableElementCondition item2 in tableElement.TableConditions.Where((IStiTableElementCondition c) => c.KeyDestinationMeters != null && c.KeyDestinationMeters.Contains(keyColumn)))
		{
			for (int i = 0; i < columnKeys.Length; i++)
			{
				string item = columnKeys[i];
				object value = rowValues[i];
				if (item2.KeyDataFieldMeters.ToList().Contains(item) && (item2.Permissions & StiTableConditionPermissions.BackColor) > StiTableConditionPermissions.None && GetConditionResult(value, item2, tableElement))
				{
					return item2.BackColor;
				}
			}
		}
		return defaultColor;
	}

	public static Color ProcessForeColor(Color defaultColor, string keyColumn, StiTableElement tableElement, object[] rowValues, string[] columnKeys)
	{
		foreach (IStiTableElementCondition item2 in tableElement.TableConditions.Where((IStiTableElementCondition c) => c.KeyDestinationMeters != null && c.KeyDestinationMeters.Contains(keyColumn)))
		{
			for (int i = 0; i < columnKeys.Length; i++)
			{
				string item = columnKeys[i];
				object value = rowValues[i];
				if (item2.KeyDataFieldMeters.ToList().Contains(item) && (item2.Permissions & StiTableConditionPermissions.ForeColor) > StiTableConditionPermissions.None && GetConditionResult(value, item2, tableElement))
				{
					return item2.ForeColor;
				}
			}
		}
		return defaultColor;
	}

	public static FontStyle ProcessFontStyle(Font fontText, string keyColumn, StiTableElement tableElement, object[] rowValues, string[] columnKeys)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		bool flag = ProcessFontBold(fontText.Bold, keyColumn, tableElement, rowValues, columnKeys);
		bool flag2 = ProcessFontItalic(fontText.Italic, keyColumn, tableElement, rowValues, columnKeys);
		bool flag3 = ProcessFontUnderline(fontText.Underline, keyColumn, tableElement, rowValues, columnKeys);
		bool flag4 = ProcessFontStrikeout(fontText.Strikeout, keyColumn, tableElement, rowValues, columnKeys);
		FontStyle val = (FontStyle)0;
		if (flag)
		{
			val = (FontStyle)(val ^ 1);
		}
		if (flag2)
		{
			val = (FontStyle)(val ^ 2);
		}
		if (flag3)
		{
			val = (FontStyle)(val ^ 4);
		}
		if (flag4)
		{
			val = (FontStyle)(val ^ 8);
		}
		return val;
	}

	private static bool ProcessFontStrikeout(bool strikeout, string keyColumn, StiTableElement tableElement, object[] rowValues, string[] columnKeys)
	{
		foreach (IStiTableElementCondition item2 in tableElement.TableConditions.Where((IStiTableElementCondition c) => c.KeyDestinationMeters != null && c.KeyDestinationMeters.Contains(keyColumn)))
		{
			for (int i = 0; i < columnKeys.Length; i++)
			{
				string item = columnKeys[i];
				object value = rowValues[i];
				if (item2.KeyDataFieldMeters.ToList().Contains(item) && (item2.Permissions & StiTableConditionPermissions.FontStyleStrikeout) > StiTableConditionPermissions.None && GetConditionResult(value, item2, tableElement))
				{
					return item2.Font.Strikeout;
				}
			}
		}
		return strikeout;
	}

	private static bool ProcessFontUnderline(bool underline, string keyColumn, StiTableElement tableElement, object[] rowValues, string[] columnKeys)
	{
		foreach (IStiTableElementCondition item2 in tableElement.TableConditions.Where((IStiTableElementCondition c) => c.KeyDestinationMeters != null && c.KeyDestinationMeters.Contains(keyColumn)))
		{
			for (int i = 0; i < columnKeys.Length; i++)
			{
				string item = columnKeys[i];
				object value = rowValues[i];
				if (item2.KeyDataFieldMeters.ToList().Contains(item) && (item2.Permissions & StiTableConditionPermissions.FontStyleUnderline) > StiTableConditionPermissions.None && GetConditionResult(value, item2, tableElement))
				{
					return item2.Font.Underline;
				}
			}
		}
		return underline;
	}

	private static bool ProcessFontItalic(bool italic, string keyColumn, StiTableElement tableElement, object[] rowValues, string[] columnKeys)
	{
		foreach (IStiTableElementCondition item2 in tableElement.TableConditions.Where((IStiTableElementCondition c) => c.KeyDestinationMeters != null && c.KeyDestinationMeters.Contains(keyColumn)))
		{
			for (int i = 0; i < columnKeys.Length; i++)
			{
				string item = columnKeys[i];
				object value = rowValues[i];
				if (item2.KeyDataFieldMeters.ToList().Contains(item) && (item2.Permissions & StiTableConditionPermissions.FontStyleItalic) > StiTableConditionPermissions.None && GetConditionResult(value, item2, tableElement))
				{
					return item2.Font.Italic;
				}
			}
		}
		return italic;
	}

	private static bool ProcessFontBold(bool bold, string keyColumn, StiTableElement tableElement, object[] rowValues, string[] columnKeys)
	{
		foreach (IStiTableElementCondition item2 in tableElement.TableConditions.Where((IStiTableElementCondition c) => c.KeyDestinationMeters != null && c.KeyDestinationMeters.Contains(keyColumn)))
		{
			for (int i = 0; i < columnKeys.Length; i++)
			{
				string item = columnKeys[i];
				object value = rowValues[i];
				if (item2.KeyDataFieldMeters.ToList().Contains(item) && (item2.Permissions & StiTableConditionPermissions.FontStyleBold) > StiTableConditionPermissions.None && GetConditionResult(value, item2, tableElement))
				{
					return item2.Font.Bold;
				}
			}
		}
		return bold;
	}
}
