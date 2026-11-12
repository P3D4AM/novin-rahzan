using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Progress;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Visuals.Progress;

public abstract class StiProgressVisual : StiVisual
{
	protected StiProgressElementStyle Style { get; set; }

	protected virtual byte MinElementSide { get; set; } = 2;


	protected byte MinFontSize { get; set; } = 3;


	public List<StiProgressIteration> Iterations { get; set; }

	public StiProgressElement Element { get; }

	private decimal GetMaxValue()
	{
		return Iterations.Select((StiProgressIteration iteration) => Math.Abs(iteration.Value)).Concat(new decimal[1]).Max();
	}

	public List<decimal> GetTargetValues()
	{
		decimal maxValue = GetMaxValue();
		List<decimal> list = new List<decimal>();
		foreach (StiProgressIteration iteration in Iterations)
		{
			list.Add(iteration.Target ?? maxValue);
		}
		return list;
	}

	public List<string> GetTextValues()
	{
		List<string> list = new List<string>();
		List<decimal> targetValues = GetTargetValues();
		for (int i = 0; i < Iterations.Count; i++)
		{
			decimal num = targetValues[i];
			decimal num2 = ((num != 0m) ? (Math.Abs(Iterations[i].Value) / num) : 0m);
			list.Add(StiTextFormatHelper.FormatAsPercentage(Element?.Report, Element.TextFormat, num2));
		}
		return list;
	}

	public Color GetColor(int index)
	{
		if (!Element.ColorEach)
		{
			return Style.BandColor;
		}
		return GetColors(Iterations.Count)[index];
	}

	public Color[] GetColors(int seriesCount)
	{
		Color[] array = new Color[seriesCount];
		Color[] array2 = ((Element.SeriesColors != null && Element.SeriesColors.Length != 0) ? Element.SeriesColors : Style.SeriesColors);
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < seriesCount; i++)
		{
			Color color = ((array2 == null || array2.Length == 0) ? Color.FromArgb(255, 52, 152, 219) : array2[num]);
			if (num2 != 0)
			{
				int alpha = Math.Min(color.A + num2, 255);
				int red = Math.Min(color.R + num2, 255);
				int green = Math.Min(color.G + num2, 255);
				int blue = Math.Min(color.B + num2, 255);
				array[i] = Color.FromArgb(alpha, red, green, blue);
			}
			else
			{
				array[i] = color;
			}
			num++;
			if (num == array2.Length)
			{
				num = 0;
				num2 = 50;
			}
		}
		return array;
	}

	internal RectangleF GetContentRectangle(RectangleF rect)
	{
		if (Iterations == null || Iterations.Count() <= 1)
		{
			return rect;
		}
		List<RectangleD> locationRectangles = StiElementLocationHelper.GetLocationRectangles(RectangleD.CreateFromRectangle(rect), Iterations.Count(), Element);
		rect = StiElementLocationHelper.GetSumRectangle(locationRectangles).ToRectangleF();
		return rect;
	}

	private bool GetConditionResult(StiProgressIteration iteration, StiProgressElementCondition condition)
	{
		object obj = null;
		switch (condition.Field)
		{
		case StiProgressFieldCondition.Value:
			obj = iteration.Value;
			break;
		case StiProgressFieldCondition.Series:
			obj = iteration.Series;
			break;
		case StiProgressFieldCondition.Target:
			obj = iteration.Target;
			break;
		case StiProgressFieldCondition.Percentage:
		{
			decimal variation = GetVariation(iteration);
			StiPercentageFormatService format = new StiPercentageFormatService
			{
				DecimalDigits = 3
			};
			string text = StiTextFormatHelper.FormatAsPercentage(Element?.Report, format, variation);
			double result = 0.0;
			double.TryParse(text.Replace("%", ""), out result);
			obj = result;
			break;
		}
		}
		bool result2 = false;
		object obj2 = GetConditionValue(condition);
		if (obj != null)
		{
			if (condition.Field == StiProgressFieldCondition.Value || condition.Field == StiProgressFieldCondition.Target || condition.Field == StiProgressFieldCondition.Percentage)
			{
				obj = Convert.ToDouble(obj);
			}
			IComparable comparable = obj as IComparable;
			if (comparable != null && obj2 != null)
			{
				if (obj.GetType() != obj2.GetType())
				{
					obj = obj.ToString();
					obj2 = obj2.ToString();
					comparable = obj as IComparable;
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
						string text5 = obj.ToString();
						string value4 = obj2.ToString();
						return text5.IndexOf(value4, StringComparison.InvariantCulture) != -1;
					}
					case Stimulsoft.Report.Components.StiFilterCondition.NotContaining:
					{
						string text4 = obj.ToString();
						string value3 = obj2.ToString();
						return text4.IndexOf(value3, StringComparison.InvariantCulture) == -1;
					}
					case Stimulsoft.Report.Components.StiFilterCondition.BeginningWith:
					{
						string text3 = obj.ToString();
						string value2 = obj2.ToString();
						return text3.StartsWith(value2, StringComparison.InvariantCulture);
					}
					case Stimulsoft.Report.Components.StiFilterCondition.EndingWith:
					{
						string text2 = obj.ToString();
						string value = obj2.ToString();
						return text2.EndsWith(value, StringComparison.InvariantCulture);
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
		return result2;
	}

	protected Color ProcessColor(Color color, StiProgressIteration iteration)
	{
		foreach (StiProgressElementCondition progressCondition in Element.ProgressConditions)
		{
			if ((progressCondition.Permissions & StiProgressConditionPermissions.Color) > StiProgressConditionPermissions.None && GetConditionResult(iteration, progressCondition))
			{
				return progressCondition.Color;
			}
		}
		return color;
	}

	private decimal GetVariation(StiProgressIteration iteration)
	{
		decimal value = iteration.Value;
		decimal valueOrDefault = iteration.Target.GetValueOrDefault();
		decimal result = default(decimal);
		if (valueOrDefault != 0m)
		{
			return value / valueOrDefault;
		}
		return result;
	}

	private object GetConditionValue(StiProgressElementCondition condition)
	{
		if (string.IsNullOrEmpty(condition.Value))
		{
			return null;
		}
		StiProgressFieldCondition field = condition.Field;
		if (field == StiProgressFieldCondition.Value || (uint)(field - 2) <= 1u)
		{
			return double.Parse(condition.Value.Replace(".", ",").Replace(",", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator));
		}
		return condition.Value.ToString();
	}

	protected Color ProcessForeColor(StiProgressIteration iteration, Color? defaultColor = null)
	{
		foreach (StiProgressElementCondition progressCondition in Element.ProgressConditions)
		{
			if ((progressCondition.Permissions & StiProgressConditionPermissions.TextColor) > StiProgressConditionPermissions.None && GetConditionResult(iteration, progressCondition))
			{
				return progressCondition.TextColor;
			}
		}
		return StiDashboardStyleHelper.GetForeColor(Element, defaultColor);
	}

	protected Color ProcessTrackColor(Color color, StiProgressIteration iteration)
	{
		foreach (StiProgressElementCondition progressCondition in Element.ProgressConditions)
		{
			if ((progressCondition.Permissions & StiProgressConditionPermissions.TrackColor) > StiProgressConditionPermissions.None && GetConditionResult(iteration, progressCondition))
			{
				return progressCondition.TrackColor;
			}
		}
		return color;
	}

	protected FontStyle ProcessFontStyle(Font fontText, StiProgressIteration iteration)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		bool flag = ProcessFontBold(fontText.Bold, iteration);
		bool flag2 = ProcessFontItalic(fontText.Italic, iteration);
		bool flag3 = ProcessFontUnderline(fontText.Underline, iteration);
		bool flag4 = ProcessFontStrikeout(fontText.Strikeout, iteration);
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

	private bool ProcessFontStrikeout(bool strikeout, StiProgressIteration iteration)
	{
		foreach (StiProgressElementCondition progressCondition in Element.ProgressConditions)
		{
			if ((progressCondition.Permissions & StiProgressConditionPermissions.FontStyleStrikeout) > StiProgressConditionPermissions.None && GetConditionResult(iteration, progressCondition))
			{
				return progressCondition.Font.Strikeout;
			}
		}
		return strikeout;
	}

	private bool ProcessFontUnderline(bool underline, StiProgressIteration iteration)
	{
		foreach (StiProgressElementCondition progressCondition in Element.ProgressConditions)
		{
			if ((progressCondition.Permissions & StiProgressConditionPermissions.FontStyleUnderline) > StiProgressConditionPermissions.None && GetConditionResult(iteration, progressCondition))
			{
				return progressCondition.Font.Underline;
			}
		}
		return underline;
	}

	private bool ProcessFontItalic(bool italic, StiProgressIteration iteration)
	{
		foreach (StiProgressElementCondition progressCondition in Element.ProgressConditions)
		{
			if ((progressCondition.Permissions & StiProgressConditionPermissions.FontStyleItalic) > StiProgressConditionPermissions.None && GetConditionResult(iteration, progressCondition))
			{
				return progressCondition.Font.Italic;
			}
		}
		return italic;
	}

	private bool ProcessFontBold(bool bold, StiProgressIteration iteration)
	{
		foreach (StiProgressElementCondition progressCondition in Element.ProgressConditions)
		{
			if ((progressCondition.Permissions & StiProgressConditionPermissions.FontStyleBold) > StiProgressConditionPermissions.None && GetConditionResult(iteration, progressCondition))
			{
				return progressCondition.Font.Bold;
			}
		}
		return bold;
	}

	protected string ProcessFontName(string fontName, StiProgressIteration iteration)
	{
		foreach (StiProgressElementCondition progressCondition in Element.ProgressConditions)
		{
			if ((progressCondition.Permissions & StiProgressConditionPermissions.Font) > StiProgressConditionPermissions.None && GetConditionResult(iteration, progressCondition))
			{
				return progressCondition.Font.Name;
			}
		}
		return fontName;
	}

	public StiProgressVisual(StiProgressElement element, List<StiProgressIteration> iterations)
	{
		Element = element;
		Iterations = iterations;
		Style = StiDashboardStyleHelper.GetProgressStyle(Element);
	}
}
