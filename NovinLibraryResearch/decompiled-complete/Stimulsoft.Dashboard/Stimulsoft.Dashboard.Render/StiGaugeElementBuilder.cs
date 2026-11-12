using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Meters;
using Stimulsoft.Dashboard.Components.Gauge;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Dashboard.Visuals.Gauge;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Exceptions;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Report.Components.Gauge;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Gauge;

namespace Stimulsoft.Dashboard.Render;

internal class StiGaugeElementBuilder : StiElementBuilder
{
	private void FindMinMaxValues(Stimulsoft.Dashboard.Components.Gauge.StiGaugeElement element, SizeD size, IEnumerable<object> seriesKeys, StiDataTable dataTable, int valueIndex, int seriesIndex, int seriesKeysCount, int targetIndex, decimal minValue, decimal maxValue, out double resMinimum, out double resMaximum)
	{
		double? num = null;
		double? num2 = null;
		foreach (object seriesKey in seriesKeys)
		{
			decimal value = GetValue(dataTable, valueIndex, seriesIndex, seriesKeysCount, ToString(seriesKey));
			decimal? target = GetTarget(dataTable, targetIndex, seriesIndex, seriesKeysCount, ToString(seriesKey));
			double num3 = 0.0;
			double num4 = 0.0;
			if (value == minValue && value == maxValue)
			{
				if (value == 0m)
				{
					num3 = -50.0;
					num4 = 50.0;
				}
				else
				{
					num3 = (double)(value - value);
					num4 = (double)(value + value);
				}
			}
			int num5 = 5;
			switch (element.Type)
			{
			case StiGaugeType.FullCircular:
			case StiGaugeType.HalfCircular:
			{
				double num6 = Math.Min(size.Width, size.Height);
				num5 = ((!(num6 < 330.0)) ? ((!(num6 < 450.0)) ? ((!(num6 < 650.0)) ? 10 : 7) : 5) : 2);
				break;
			}
			case StiGaugeType.Linear:
			{
				double height = size.Height;
				num5 = ((!(height < 330.0)) ? ((!(height < 450.0)) ? ((!(height < 650.0)) ? 10 : 7) : 5) : 2);
				break;
			}
			case StiGaugeType.HorizontalLinear:
			{
				double width2 = size.Width;
				num5 = ((!(width2 < 330.0)) ? ((!(width2 < 500.0)) ? ((!(width2 < 650.0)) ? 10 : 8) : 5) : 2);
				break;
			}
			case StiGaugeType.Bullet:
			{
				double width = size.Width;
				num5 = ((width < 330.0) ? 2 : 5);
				break;
			}
			}
			double num7 = 0.0;
			double num8 = 0.0;
			if (element.CalculationMode == StiGaugeCalculationMode.Auto)
			{
				decimal summary = GetSummary(value, (decimal)num3, (decimal)num4);
				if (value <= (decimal)num3)
				{
					num3 = (double)Math.Round(value - summary * 0.3m);
				}
				if (value >= (decimal)num4)
				{
					num4 = (double)Math.Round(value + summary * 0.3m);
				}
				if (target.HasValue)
				{
					decimal valueOrDefault = target.GetValueOrDefault();
					if (valueOrDefault < (decimal)num3)
					{
						num3 = (double)Math.Round(valueOrDefault - summary * 0.3m);
					}
					if (valueOrDefault > (decimal)num4)
					{
						num4 = (double)Math.Round(valueOrDefault + summary * 0.3m);
					}
				}
				num7 = num3;
				num8 = num4;
				if (value > -1m && value < 1m)
				{
					if (!num.HasValue || num7 < num)
					{
						num = num7;
					}
					if (!num2.HasValue || num8 > num2)
					{
						num2 = num8;
					}
					continue;
				}
				double num9 = num7 % 5.0;
				if (num9 != 0.0)
				{
					num7 -= num9;
				}
				double num10 = num8 - num7;
				double num11 = num10 / (double)num5;
				if (num11 - (double)(long)num11 > 0.0)
				{
					num11 = (long)num11 + 1;
				}
				double num12 = num11 % 5.0;
				if (num12 != 0.0)
				{
					num11 += 5.0 - num12;
				}
				num10 = num11 * (double)num5;
				num8 = num7 + num10;
			}
			else
			{
				num7 = (double)element.Minimum;
				num8 = (double)element.Maximum;
			}
			if (!num.HasValue || num7 < num)
			{
				num = num7;
			}
			if (!num2.HasValue || num8 > num2)
			{
				num2 = num8;
			}
		}
		resMinimum = num.GetValueOrDefault();
		resMaximum = num2.GetValueOrDefault();
	}

	public IEnumerable<StiGaugeIteration> Render(Stimulsoft.Dashboard.Components.Gauge.StiGaugeElement element, SizeD size, StiDataTable dataTable)
	{
		CultureInfo culture = StiCultureHelper.Set(element?.Report);
		try
		{
			int valueIndex = GetValueMeterIndex(dataTable);
			int seriesIndex = GetSeriesMeterIndex(dataTable);
			int targetIndex = GetTargetMeterIndex(dataTable);
			int minMeterIndex = GetMinMeterIndex(dataTable);
			int maxMeterIndex = GetMaxMeterIndex(dataTable);
			IEnumerable<object> seriesKeyValues = GetSeriesKeyValues(dataTable, seriesIndex, GetSeriesKeysLimitCount(element));
			int seriesKeysCount = seriesKeyValues.Count();
			decimal minValue = GetMinValue(element, dataTable, minMeterIndex);
			decimal maxValue = GetMaxValue(element, dataTable, maxMeterIndex);
			FindMinMaxValues(element, size, seriesKeyValues, dataTable, valueIndex, seriesIndex, seriesKeysCount, targetIndex, minValue, maxValue, out var resMinimum, out var resMaximum);
			return seriesKeyValues.Select(delegate(object seriesKey)
			{
				decimal value = GetValue(dataTable, valueIndex, seriesIndex, seriesKeysCount, ToString(seriesKey));
				decimal? nullableTarget = GetNullableTarget(dataTable, targetIndex, seriesIndex, seriesKeysCount, ToString(seriesKey));
				FundActualValues(element, size, value, resMinimum, resMaximum, out var resMajorInterval, out var resMinorInterval, out var resSkipRotateLabels);
				StiScaleBase stiScaleBase = CreateScale(element, resSkipRotateLabels, size, resMinimum, resMaximum, nullableTarget);
				IEnumerable<StiIndicatorBase> enumerable = stiScaleBase.Items.OfType<StiIndicatorBase>();
				foreach (StiIndicatorBase item in enumerable)
				{
					item.ValueObj = (float)value;
				}
				stiScaleBase.Minimum = (float)resMinimum;
				stiScaleBase.Maximum = (float)resMaximum;
				stiScaleBase.MajorInterval = (float)resMajorInterval;
				stiScaleBase.MinorInterval = (float)resMinorInterval;
				if (element.Type == StiGaugeType.Linear || element.Type == StiGaugeType.HorizontalLinear || element.Type == StiGaugeType.Bullet)
				{
					StiLinearRangeList stiLinearRangeList = stiScaleBase.Items.OfType<StiLinearRangeList>().FirstOrDefault();
					if (stiLinearRangeList != null && stiLinearRangeList.Ranges.Count == 1)
					{
						((StiLinearRange)stiLinearRangeList.Ranges[0]).StartValue = stiScaleBase.Minimum;
						((StiLinearRange)stiLinearRangeList.Ranges[0]).EndValue = stiScaleBase.Maximum;
					}
				}
				StiGauge stiGauge = new StiGauge(element.ClientRectangle)
				{
					AllowApplyStyle = true,
					PreviousAnimations = element.PreviousAnimations
				};
				SetStyle(element, stiGauge);
				stiGauge.Scales.Add(stiScaleBase);
				stiGauge.ApplyStyle(stiGauge.Style);
				return new StiGaugeIteration
				{
					Gauge = stiGauge,
					Series = GetSeries(dataTable, seriesIndex, seriesKey, shortValue: true, element.Report)
				};
			});
		}
		finally
		{
			StiCultureHelper.Restore(culture);
		}
	}

	private static void FundActualValues(Stimulsoft.Dashboard.Components.Gauge.StiGaugeElement element, SizeD size, decimal value, double minimum, double maximum, out double resMajorInterval, out double resMinorInterval, out bool resSkipRotateLabels)
	{
		int num = 5;
		resSkipRotateLabels = false;
		switch (element.Type)
		{
		case StiGaugeType.FullCircular:
		case StiGaugeType.HalfCircular:
		{
			double num2 = Math.Min(size.Width, size.Height);
			if (!element.Labels.Visible)
			{
				break;
			}
			if (element.Labels.RotationMode == StiLabelRotationMode.SurroundOut)
			{
				if (num2 < (double)StiScale.XXI(170.0))
				{
					num = 0;
				}
				else if (num2 < (double)StiScale.XXI(220.0))
				{
					num = 2;
					resSkipRotateLabels = true;
				}
				else
				{
					num = ((!(num2 < (double)StiScale.XXI(250.0))) ? ((!(num2 < (double)StiScale.XXI(320.0))) ? 10 : 7) : 5);
				}
			}
			else if (num2 < (double)StiScale.XXI(170.0))
			{
				num = 0;
			}
			else if (num2 < (double)StiScale.XXI(220.0))
			{
				num = 2;
				resSkipRotateLabels = true;
			}
			else
			{
				num = 4;
			}
			break;
		}
		case StiGaugeType.Linear:
		{
			double height = size.Height;
			if (height < 330.0)
			{
				num = 2;
				resSkipRotateLabels = true;
			}
			else
			{
				num = ((!(height < 450.0)) ? ((!(height < 650.0)) ? 10 : 7) : 5);
			}
			break;
		}
		case StiGaugeType.HorizontalLinear:
		{
			double width2 = size.Width;
			if (width2 < 330.0)
			{
				num = 2;
				resSkipRotateLabels = true;
			}
			else
			{
				num = ((!(width2 < 500.0)) ? ((!(width2 < 650.0)) ? 10 : 8) : 5);
			}
			break;
		}
		case StiGaugeType.Bullet:
		{
			double width = size.Width;
			if (width < 330.0)
			{
				num = 2;
				resSkipRotateLabels = true;
			}
			else
			{
				num = 5;
			}
			break;
		}
		}
		if (element.CalculationMode == StiGaugeCalculationMode.Auto)
		{
			decimal summary = GetSummary(value, (decimal)minimum, (decimal)maximum);
			if (num > 0)
			{
				resMajorInterval = Math.Abs((double)Math.Round(summary / (decimal)num));
				resMinorInterval = resMajorInterval / 2.0;
			}
			else
			{
				resMajorInterval = (double)Math.Truncate(summary / 2m);
				resMinorInterval = (double)Math.Truncate(summary / 2m);
			}
		}
		else
		{
			if (num > 0)
			{
				resMajorInterval = Math.Abs((double)Math.Round((element.Maximum - element.Minimum) / (decimal)num));
				if (resMajorInterval == 0.0)
				{
					resMajorInterval = 1.0;
				}
				resMinorInterval = resMajorInterval / 2.0;
				return;
			}
			resMajorInterval = 1.0;
			resMinorInterval = 1.0;
		}
		if (!(value > -1m) || !(value < 1m))
		{
			double num3 = maximum - minimum;
			double num4 = num3 / (double)num;
			if (num4 - Math.Round(num4) > 0.0)
			{
				num4 = Math.Round(num4) + 1.0;
			}
			double num5 = num4 % 5.0;
			if (num5 != 0.0)
			{
				num4 += 5.0 - num5;
			}
			num3 = num4 * (double)num;
			resMajorInterval = Math.Abs(num3 / (double)num);
			resMinorInterval = resMajorInterval / 2.0;
		}
	}

	private static decimal GetSummary(decimal value, decimal min, decimal max)
	{
		decimal val = Math.Round(max - min);
		decimal val2 = Math.Max(value, val);
		val2 = Math.Max(val2, min);
		val2 = Math.Max(val2, max);
		if (val2 == 0m)
		{
			val2 = 100m;
		}
		return val2;
	}

	private static void SetStyle(Stimulsoft.Dashboard.Components.Gauge.StiGaugeElement element, StiGauge gauge)
	{
		if (element.Style != StiElementStyleIdent.Custom)
		{
			gauge.Style = StiDashboardStyleHelper.GetGaugeStyle(element);
			return;
		}
		StiCustomGaugeStyle customGaugeStyle = element.Report.Styles.GetCustomGaugeStyle(element.CustomStyleName);
		if (customGaugeStyle != null)
		{
			gauge.Style = customGaugeStyle;
			gauge.CustomStyleName = element.CustomStyleName;
			gauge.Brush = new StiSolidBrush(StiDashboardStyleHelper.GetBackColor(element));
			gauge.AllowApplyStyle = false;
		}
		else
		{
			gauge.Style = StiDashboardStyleHelper.GetGaugeStyle(StiElementStyleIdent.Blue);
		}
	}

	private static StiBrush GetTargetBrush(Stimulsoft.Dashboard.Components.Gauge.StiGaugeElement element)
	{
		if (element.Style != StiElementStyleIdent.Custom)
		{
			return new StiSolidBrush(StiDashboardStyleHelper.GetGaugeStyle(element).Core.TargetColor);
		}
		StiCustomGaugeStyle customGaugeStyle = element.Report.Styles.GetCustomGaugeStyle(element.CustomStyleName);
		if (customGaugeStyle != null && customGaugeStyle.Core != null)
		{
			return new StiSolidBrush(customGaugeStyle.Core.TargetColor);
		}
		return new StiSolidBrush(StiDashboardStyleHelper.GetGaugeStyle(StiElementStyleIdent.Blue).Core.TargetColor);
	}

	private static StiBrush GetTickMarkBrush(Stimulsoft.Dashboard.Components.Gauge.StiGaugeElement element)
	{
		if (element.Style != StiElementStyleIdent.Custom)
		{
			return StiDashboardStyleHelper.GetGaugeStyle(element).Core.TickMarkMajorBrush;
		}
		StiCustomGaugeStyle customGaugeStyle = element.Report.Styles.GetCustomGaugeStyle(element.CustomStyleName);
		if (customGaugeStyle != null && customGaugeStyle.Core != null)
		{
			return customGaugeStyle.Core.TickMarkMajorBrush;
		}
		return StiDashboardStyleHelper.GetGaugeStyle(StiElementStyleIdent.Blue).Core.TickMarkMajorBrush;
	}

	private static StiBrush GetTickLabelBrush(Stimulsoft.Dashboard.Components.Gauge.StiGaugeElement element)
	{
		if (element.Style != StiElementStyleIdent.Custom)
		{
			return StiDashboardStyleHelper.GetGaugeStyle(element).Core.TickLabelMajorTextBrush;
		}
		StiCustomGaugeStyle customGaugeStyle = element.Report.Styles.GetCustomGaugeStyle(element.CustomStyleName);
		if (customGaugeStyle != null && customGaugeStyle.Core != null)
		{
			return customGaugeStyle.Core.TickLabelMajorTextBrush;
		}
		return StiDashboardStyleHelper.GetGaugeStyle(StiElementStyleIdent.Blue).Core.TickLabelMajorTextBrush;
	}

	private StiScaleBase CreateScale(Stimulsoft.Dashboard.Components.Gauge.StiGaugeElement element, bool skipRotateLabels, SizeD size, double minValue, double maxValue, decimal? target)
	{
		return element.Type switch
		{
			StiGaugeType.FullCircular => CreateFullCircularScale(element, skipRotateLabels, size, minValue, maxValue, target), 
			StiGaugeType.HalfCircular => CreateHalfCircularScale(element, skipRotateLabels, size, minValue, maxValue, target), 
			StiGaugeType.Linear => CreateLinearScale(element, minValue, maxValue, isHorizontal: false, target), 
			StiGaugeType.HorizontalLinear => CreateLinearScale(element, minValue, maxValue, isHorizontal: true, target), 
			StiGaugeType.Bullet => CreateBullet(element, minValue, maxValue, target), 
			_ => throw new StiTypeNotRecognizedException(element.Type), 
		};
	}

	private float CalculateMinorInterval(double minValue, double maxValue)
	{
		if (minValue < 0.0 && maxValue < 0.0)
		{
			return (float)Math.Abs((minValue - maxValue) / 5.0);
		}
		if (minValue < 0.0)
		{
			return (float)(maxValue + Math.Abs(minValue)) / 5f;
		}
		return (float)(maxValue - minValue) / 5f;
	}

	private StiScaleBase CreateFullCircularScale(Stimulsoft.Dashboard.Components.Gauge.StiGaugeElement element, bool skipRotateLabels, SizeD size, double minValue, double maxValue, decimal? target)
	{
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Expected O, but got Unknown
		double num = Math.Min(size.Width, size.Height);
		bool flag = num >= (double)StiScale.XXI(170.0);
		bool flag2 = num < (double)StiScale.XXI(220.0);
		StiRadialScale stiRadialScale = new StiRadialScale
		{
			AllowApplyStyle = false,
			Radius = ((!element.Labels.Visible) ? 0.8f : ((element.Labels.Placement == StiLabelPlacement.Inside) ? 0.8f : 0.63f)),
			StartAngle = 120f,
			SweepAngle = 300f,
			StartWidth = 0.05f,
			EndWidth = 0.05f,
			Minimum = 0f,
			Maximum = 180f,
			MajorInterval = CalculateMinorInterval(minValue, maxValue),
			BorderBrush = new StiEmptyBrush(),
			Brush = new StiEmptyBrush()
		};
		bool flag3 = AddRadialRanges(element, minValue, maxValue, stiRadialScale);
		stiRadialScale.Items.Add(new StiRadialBar
		{
			StartWidth = 0.1f,
			EndWidth = 0.1f,
			Offset = 0.1f,
			Placement = StiPlacement.Outside,
			ValueObj = 90f
		});
		stiRadialScale.Items.Add(new StiNeedle
		{
			RelativeHeight = 0.3f,
			RelativeWidth = 0.4f,
			StartWidth = 0.1f,
			EndWidth = 1f,
			CapBrush = new StiEmptyBrush(),
			Brush = new StiEmptyBrush(),
			ValueObj = 90f
		});
		if (element.Labels.Visible && flag)
		{
			StiRadialTickMarkMajor stiRadialTickMarkMajor = new StiRadialTickMarkMajor
			{
				RelativeWidth = 0.03f,
				RelativeHeight = 0.006f,
				Skin = StiTickMarkSkin.Rectangle,
				Brush = GetTickMarkBrush(element)
			};
			if (element.Labels.Placement == StiLabelPlacement.Inside)
			{
				stiRadialTickMarkMajor.Placement = StiPlacement.Inside;
				stiRadialTickMarkMajor.Offset = (flag3 ? 0f : (-0.1f));
			}
			else
			{
				stiRadialTickMarkMajor.Placement = StiPlacement.Outside;
				stiRadialTickMarkMajor.Offset = 0.2f;
			}
			stiRadialScale.Items.Add(stiRadialTickMarkMajor);
		}
		if (element.Labels.Visible && flag)
		{
			StiRadialTickLabelMajor stiRadialTickLabelMajor = new StiRadialTickLabelMajor
			{
				AllowApplyStyle = false,
				TextBrush = GetTickLabelBrush(element),
				LabelRotationMode = ((!skipRotateLabels) ? element.Labels.RotationMode : StiLabelRotationMode.None),
				FormatService = element.ValueFormat
			};
			if (target.HasValue)
			{
				stiRadialTickLabelMajor.SkipValuesObj = new float[1] { (float)target.GetValueOrDefault() };
			}
			if (flag2)
			{
				stiRadialTickLabelMajor.Font = new Font("Arial", 8f);
			}
			if (element.Labels.Placement == StiLabelPlacement.Inside)
			{
				stiRadialTickLabelMajor.Placement = StiPlacement.Inside;
				stiRadialTickLabelMajor.Offset = (flag3 ? 0.07f : (-0.03f));
			}
			else
			{
				stiRadialTickLabelMajor.Placement = StiPlacement.Outside;
				stiRadialTickLabelMajor.Offset = 0.22f;
			}
			stiRadialScale.Items.Add(stiRadialTickLabelMajor);
		}
		if (target.HasValue)
		{
			StiRadialTickMarkCustom element2 = new StiRadialTickMarkCustom
			{
				Placement = StiPlacement.Overlay,
				Offset = -0.15f,
				RelativeWidth = 0.1f,
				RelativeHeight = 0.02f,
				BorderWidth = 0f,
				Brush = GetTargetBrush(element),
				ValueObj = (float)target.GetValueOrDefault()
			};
			stiRadialScale.Items.Add(element2);
			if (element.TargetSettings.ShowLabel)
			{
				StiRadialTickLabelCustom stiRadialTickLabelCustom = new StiRadialTickLabelCustom
				{
					AllowApplyStyle = false,
					FormatService = element.ValueFormat,
					TextBrush = GetTargetBrush(element),
					LabelRotationMode = ((!skipRotateLabels) ? element.Labels.RotationMode : StiLabelRotationMode.None),
					Placement = element.TargetSettings.Placement,
					ValueObj = (float)target.GetValueOrDefault(),
					TextObj = target.GetValueOrDefault().ToString()
				};
				switch (element.TargetSettings.Placement)
				{
				case StiPlacement.Overlay:
					stiRadialTickLabelCustom.Offset = -0.12f;
					break;
				case StiPlacement.Outside:
					stiRadialTickLabelCustom.Offset = 0.18f;
					break;
				}
				stiRadialScale.Items.Add(stiRadialTickLabelCustom);
			}
		}
		return stiRadialScale;
	}

	public StiScaleBase CreateHalfCircularScale(Stimulsoft.Dashboard.Components.Gauge.StiGaugeElement element, bool skipRotateLabels, SizeD size, double minValue, double maxValue, decimal? target)
	{
		double num = Math.Min(size.Width, size.Height);
		bool flag = num < 150.0;
		bool flag2 = num < 75.0;
		StiRadialScale stiRadialScale = new StiRadialScale
		{
			StartAngle = 180f,
			SweepAngle = 180f,
			StartWidth = 0.05f,
			EndWidth = 0.05f,
			Minimum = 0f,
			Maximum = 180f,
			Radius = 0.92f,
			MajorInterval = CalculateMinorInterval(minValue, maxValue),
			Center = new PointF(0.5f, 0.7f),
			RadiusMode = StiRadiusMode.Auto,
			Brush = new StiSolidBrush(Color.FromArgb(238, 238, 238))
		};
		stiRadialScale.Items.Add(new StiRadialBar
		{
			StartWidth = 0.05f,
			EndWidth = 0.05f,
			Offset = 0.1f,
			ValueObj = 135f,
			Placement = StiPlacement.Overlay,
			Brush = new StiSolidBrush(Color.Red)
		});
		AddRadialRanges(element, minValue, maxValue, stiRadialScale);
		if (element.Labels.Visible)
		{
			if (!flag2)
			{
				StiRadialTickMarkMajor stiRadialTickMarkMajor = new StiRadialTickMarkMajor
				{
					Offset = 0.05f,
					RelativeWidth = 0.03f,
					RelativeHeight = 0.006f,
					Skin = StiTickMarkSkin.Rectangle,
					Brush = GetTickMarkBrush(element)
				};
				if (element.Labels.Placement == StiLabelPlacement.Inside)
				{
					stiRadialTickMarkMajor.Placement = StiPlacement.Inside;
				}
				else
				{
					stiRadialTickMarkMajor.Placement = StiPlacement.Outside;
				}
				stiRadialScale.Items.Add(stiRadialTickMarkMajor);
			}
			else if (!flag)
			{
				StiRadialTickLabelMajor stiRadialTickLabelMajor = new StiRadialTickLabelMajor
				{
					AllowApplyStyle = false,
					TextBrush = GetTickLabelBrush(element),
					LabelRotationMode = ((!skipRotateLabels) ? element.Labels.RotationMode : StiLabelRotationMode.None),
					FormatService = element.ValueFormat
				};
				if (target.HasValue)
				{
					stiRadialTickLabelMajor.SkipValuesObj = new float[1] { (float)target.GetValueOrDefault() };
				}
				if (element.Labels.Placement == StiLabelPlacement.Inside)
				{
					stiRadialTickLabelMajor.Placement = StiPlacement.Inside;
					stiRadialTickLabelMajor.Offset = 0.12f;
				}
				else
				{
					stiRadialTickLabelMajor.Placement = StiPlacement.Outside;
					stiRadialTickLabelMajor.Offset = 0.1f;
				}
				stiRadialScale.Items.Add(stiRadialTickLabelMajor);
			}
		}
		if (target.HasValue)
		{
			StiRadialTickMarkCustom element2 = new StiRadialTickMarkCustom
			{
				Placement = StiPlacement.Overlay,
				RelativeWidth = 0.1f,
				RelativeHeight = 0.02f,
				BorderWidth = 0f,
				Brush = GetTargetBrush(element),
				ValueObj = (float)target.GetValueOrDefault()
			};
			stiRadialScale.Items.Add(element2);
			if (element.TargetSettings.ShowLabel)
			{
				StiRadialTickLabelCustom stiRadialTickLabelCustom = new StiRadialTickLabelCustom
				{
					AllowApplyStyle = false,
					FormatService = element.ValueFormat,
					TextBrush = GetTargetBrush(element),
					LabelRotationMode = ((!skipRotateLabels) ? element.Labels.RotationMode : StiLabelRotationMode.None),
					Placement = element.TargetSettings.Placement,
					ValueObj = (float)target.GetValueOrDefault(),
					TextObj = target.GetValueOrDefault().ToString()
				};
				switch (element.TargetSettings.Placement)
				{
				case StiPlacement.Inside:
					stiRadialTickLabelCustom.Offset = 0.12f;
					break;
				case StiPlacement.Outside:
					stiRadialTickLabelCustom.Offset = 0.05f;
					break;
				}
				stiRadialScale.Items.Add(stiRadialTickLabelCustom);
			}
		}
		stiRadialScale.Items.Add(new StiNeedle
		{
			RelativeHeight = 0.05f,
			RelativeWidth = 0.4f,
			StartWidth = 0.1f,
			EndWidth = 0.2f,
			CapBrush = new StiSolidBrush(Color.White),
			CapBorderBrush = new StiSolidBrush(Color.FromArgb(0, 150, 136)),
			CapBorderWidth = 2f,
			Brush = new StiSolidBrush(Color.FromArgb(0, 150, 136)),
			BorderWidth = 0f,
			TextBrush = new StiSolidBrush(Color.FromArgb(0, 150, 136)),
			ValueObj = 135f
		});
		return stiRadialScale;
	}

	public StiScaleBase CreateLinearScale(Stimulsoft.Dashboard.Components.Gauge.StiGaugeElement element, double minValue, double maxValue, bool isHorizontal, decimal? target)
	{
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		StiGaugeStyleXF gaugeStyle = StiDashboardStyleHelper.GetGaugeStyle(element);
		StiLinearScale stiLinearScale = new StiLinearScale
		{
			RelativeHeight = (isHorizontal ? 0.9f : 0.92f),
			Orientation = (Orientation)(!isHorizontal),
			StartWidth = 0.1f,
			EndWidth = 0.1f,
			Maximum = 100f,
			MajorInterval = CalculateMinorInterval(minValue, maxValue),
			BorderBrush = new StiEmptyBrush(),
			Brush = gaugeStyle.Core.LinearScaleBrush
		};
		if (element.Labels.Visible)
		{
			StiLinearTickLabelMajor stiLinearTickLabelMajor = new StiLinearTickLabelMajor
			{
				FormatService = element.ValueFormat,
				Font = new Font("Arial", 8f),
				AllowApplyStyle = false,
				TextBrush = GetTickLabelBrush(element)
			};
			if (target.HasValue)
			{
				stiLinearTickLabelMajor.SkipValuesObj = new float[1] { (float)target.GetValueOrDefault() };
			}
			Color foreColor = StiDashboardExpressionHelper.GetForeColor(element, element.ForeColor);
			if (foreColor != Color.Transparent)
			{
				stiLinearTickLabelMajor.AllowApplyStyle = false;
				stiLinearTickLabelMajor.TextBrush = new StiSolidBrush(foreColor);
			}
			StiLinearTickMarkMajor stiLinearTickMarkMajor = new StiLinearTickMarkMajor
			{
				RelativeHeight = (isHorizontal ? 0.03f : 0.003f),
				RelativeWidth = (isHorizontal ? 0.003f : 0.03f),
				BorderBrush = new StiEmptyBrush(),
				BorderWidth = 0f,
				Brush = GetTickMarkBrush(element)
			};
			if (element.Labels.Placement == StiLabelPlacement.Inside)
			{
				stiLinearTickMarkMajor.Placement = StiPlacement.Inside;
				stiLinearTickLabelMajor.Placement = StiPlacement.Inside;
				stiLinearTickLabelMajor.Offset = 0.04f;
			}
			else
			{
				stiLinearTickMarkMajor.Placement = StiPlacement.Outside;
				stiLinearTickLabelMajor.Placement = StiPlacement.Outside;
				stiLinearTickLabelMajor.Offset = 0.04f;
			}
			stiLinearScale.Items.Add(stiLinearTickLabelMajor);
			stiLinearScale.Items.Add(stiLinearTickMarkMajor);
		}
		AddLinearRanges(element, minValue, maxValue, stiLinearScale);
		if (target.HasValue)
		{
			StiLinearTickMarkCustom element2 = new StiLinearTickMarkCustom
			{
				Placement = StiPlacement.Overlay,
				RelativeWidth = (isHorizontal ? 0.012f : 0.1f),
				RelativeHeight = (isHorizontal ? 0.1f : 0.012f),
				BorderWidth = 0f,
				Brush = GetTargetBrush(element),
				ValueObj = (float)target.GetValueOrDefault()
			};
			stiLinearScale.Items.Add(element2);
			if (element.TargetSettings.ShowLabel)
			{
				StiLinearTickLabelCustom element3 = new StiLinearTickLabelCustom
				{
					AllowApplyStyle = false,
					FormatService = element.ValueFormat,
					TextBrush = GetTargetBrush(element),
					Placement = element.TargetSettings.Placement,
					Offset = 0.04f,
					ValueObj = (float)target.GetValueOrDefault(),
					TextObj = target.GetValueOrDefault().ToString()
				};
				stiLinearScale.Items.Add(element3);
			}
		}
		StiLinearMarker element4 = new StiLinearMarker
		{
			Skin = (isHorizontal ? StiMarkerSkin.TriangleBottom : StiMarkerSkin.TriangleRight),
			RelativeWidth = (isHorizontal ? 0.04f : 0.1f),
			RelativeHeight = (isHorizontal ? 0.1f : 0.04f),
			Placement = StiPlacement.Overlay,
			ValueObj = 60f,
			Brush = new StiSolidBrush(Color.FromArgb(205, 220, 57))
		};
		stiLinearScale.Items.Add(element4);
		return stiLinearScale;
	}

	public StiScaleBase CreateBullet(Stimulsoft.Dashboard.Components.Gauge.StiGaugeElement element, double minValue, double maxValue, decimal? target)
	{
		StiLinearScale stiLinearScale = new StiLinearScale
		{
			Orientation = (Orientation)0,
			RelativeHeight = 0.85f,
			StartWidth = 0.01f,
			EndWidth = 0.01f,
			Maximum = 100f,
			MajorInterval = CalculateMinorInterval(minValue, maxValue),
			BorderBrush = new StiEmptyBrush(),
			Brush = new StiEmptyBrush()
		};
		AddLinearRanges(element, minValue, maxValue, stiLinearScale);
		if (element.Labels.Visible)
		{
			StiLinearTickLabelMajor stiLinearTickLabelMajor = new StiLinearTickLabelMajor
			{
				FormatService = element.ValueFormat,
				TextBrush = GetTickLabelBrush(element)
			};
			if (target.HasValue)
			{
				stiLinearTickLabelMajor.SkipValuesObj = new float[1] { (float)target.GetValueOrDefault() };
			}
			StiLinearTickMarkMajor stiLinearTickMarkMajor = new StiLinearTickMarkMajor
			{
				RelativeHeight = 0.03f,
				RelativeWidth = 0.003f,
				BorderBrush = new StiEmptyBrush(),
				BorderWidth = 0f,
				Brush = GetTickMarkBrush(element)
			};
			if (element.Labels.Placement == StiLabelPlacement.Inside)
			{
				stiLinearTickLabelMajor.Placement = StiPlacement.Inside;
				stiLinearTickLabelMajor.Offset = 0.2f;
				stiLinearTickMarkMajor.Placement = StiPlacement.Inside;
				stiLinearTickMarkMajor.Offset = 0.15f;
			}
			else
			{
				stiLinearTickLabelMajor.Placement = StiPlacement.Outside;
				stiLinearTickLabelMajor.Offset = 0.2f;
				stiLinearTickMarkMajor.Placement = StiPlacement.Outside;
				stiLinearTickMarkMajor.Offset = 0.15f;
			}
			stiLinearScale.Items.Add(stiLinearTickMarkMajor);
			stiLinearScale.Items.Add(stiLinearTickLabelMajor);
		}
		if (target.HasValue)
		{
			StiLinearTickMarkCustom element2 = new StiLinearTickMarkCustom
			{
				Placement = StiPlacement.Overlay,
				RelativeWidth = 0.008f,
				RelativeHeight = 0.1f,
				BorderWidth = 0f,
				Brush = GetTargetBrush(element),
				ValueObj = (float)target.GetValueOrDefault()
			};
			stiLinearScale.Items.Add(element2);
			if (element.TargetSettings.ShowLabel)
			{
				StiLinearTickLabelCustom element3 = new StiLinearTickLabelCustom
				{
					AllowApplyStyle = false,
					FormatService = element.ValueFormat,
					TextBrush = GetTargetBrush(element),
					Placement = element.TargetSettings.Placement,
					Offset = 0.25f,
					ValueObj = (float)target.GetValueOrDefault(),
					TextObj = target.GetValueOrDefault().ToString()
				};
				stiLinearScale.Items.Add(element3);
			}
		}
		StiLinearBar element4 = new StiLinearBar
		{
			ValueObj = 60f,
			StartWidth = 0.15f,
			EndWidth = 0.15f,
			Brush = new StiSolidBrush(Color.FromArgb(205, 220, 57))
		};
		stiLinearScale.Items.Add(element4);
		return stiLinearScale;
	}

	private static bool AddRadialRanges(Stimulsoft.Dashboard.Components.Gauge.StiGaugeElement element, double minValue, double maxValue, StiRadialScale scale)
	{
		if (element.Ranges.Count > 0 && element.RangeType == StiGaugeRangeType.Color)
		{
			StiRadialRangeList stiRadialRangeList = new StiRadialRangeList();
			if (element.RangeMode == StiGaugeRangeMode.Percentage)
			{
				foreach (StiGaugeRange range in element.Ranges)
				{
					float num = StiValueHelper.ParseFloat(StiReportParser.Parse(range.StartExpression, element));
					float num2 = StiValueHelper.ParseFloat(StiReportParser.Parse(range.EndExpression, element));
					float startValue = (float)(minValue + (maxValue - minValue) / 100.0 * (double)num);
					float endValue = (float)(minValue + (maxValue - minValue) / 100.0 * (double)num2);
					stiRadialRangeList.Ranges.Add(new StiRadialRange
					{
						Placement = StiPlacement.Overlay,
						StartValue = startValue,
						EndValue = endValue,
						StartWidth = 0.05f,
						EndWidth = 0.05f,
						BorderBrush = new StiEmptyBrush(),
						Brush = new StiSolidBrush(range.Color)
					});
				}
			}
			else
			{
				foreach (StiGaugeRange range2 in element.Ranges)
				{
					float startValue2 = StiValueHelper.ParseFloat(StiReportParser.Parse(range2.StartExpression, element));
					float endValue2 = StiValueHelper.ParseFloat(StiReportParser.Parse(range2.EndExpression, element));
					stiRadialRangeList.Ranges.Add(new StiRadialRange
					{
						Placement = StiPlacement.Overlay,
						StartValue = startValue2,
						EndValue = endValue2,
						StartWidth = 0.05f,
						EndWidth = 0.05f,
						BorderBrush = new StiEmptyBrush(),
						Brush = new StiSolidBrush(range2.Color)
					});
				}
			}
			scale.Items.Add(stiRadialRangeList);
			return stiRadialRangeList.Ranges.Count > 0;
		}
		return false;
	}

	private static void AddLinearRanges(Stimulsoft.Dashboard.Components.Gauge.StiGaugeElement element, double minValue, double maxValue, StiLinearScale scale, bool showBorder = false)
	{
		if (element.Ranges.Count <= 0 || element.RangeType != StiGaugeRangeType.Color)
		{
			return;
		}
		StiLinearRangeList stiLinearRangeList = new StiLinearRangeList();
		float num = ((element.Type == StiGaugeType.Bullet) ? 0.3f : 0.05f);
		if (element.RangeMode == StiGaugeRangeMode.Percentage)
		{
			foreach (StiGaugeRange range in element.Ranges)
			{
				float num2 = StiValueHelper.ParseFloat(StiReportParser.Parse(range.StartExpression, element));
				float num3 = StiValueHelper.ParseFloat(StiReportParser.Parse(range.EndExpression, element));
				float startValue = (float)(minValue + (maxValue - minValue) / 100.0 * (double)num2);
				float endValue = (float)(minValue + (maxValue - minValue) / 100.0 * (double)num3);
				StiLinearRange stiLinearRange = new StiLinearRange
				{
					Placement = StiPlacement.Overlay,
					StartValue = startValue,
					EndValue = endValue,
					StartWidth = num,
					EndWidth = num,
					BorderBrush = new StiEmptyBrush(),
					Brush = new StiSolidBrush(range.Color)
				};
				if (showBorder)
				{
					stiLinearRange.BorderBrush = new StiSolidBrush(Color.FromArgb(150, 139, 138, 135));
				}
				stiLinearRangeList.Ranges.Add(stiLinearRange);
			}
		}
		else
		{
			foreach (StiGaugeRange range2 in element.Ranges)
			{
				float startValue2 = StiValueHelper.ParseFloat(StiReportParser.Parse(range2.StartExpression, element));
				float endValue2 = StiValueHelper.ParseFloat(StiReportParser.Parse(range2.EndExpression, element));
				StiLinearRange stiLinearRange2 = new StiLinearRange
				{
					Placement = StiPlacement.Overlay,
					StartValue = startValue2,
					EndValue = endValue2,
					StartWidth = num,
					EndWidth = num,
					BorderBrush = new StiEmptyBrush(),
					Brush = new StiSolidBrush(range2.Color)
				};
				if (showBorder)
				{
					stiLinearRange2.BorderBrush = new StiSolidBrush(Color.FromArgb(150, 139, 138, 135));
				}
				stiLinearRangeList.Ranges.Add(stiLinearRange2);
			}
		}
		scale.Items.Add(stiLinearRangeList);
	}

	private decimal GetMinValue(Stimulsoft.Dashboard.Components.Gauge.StiGaugeElement element, StiDataTable dataTable, int minIndex)
	{
		if (minIndex == -1)
		{
			return element.Minimum;
		}
		return ((element.CalculationMode == StiGaugeCalculationMode.Auto) ? dataTable.Rows.GetArrayItem(minIndex).FirstOrDefaultAsNullableDecimal() : new decimal?(element.Minimum)) ?? element.Minimum;
	}

	private decimal GetMaxValue(Stimulsoft.Dashboard.Components.Gauge.StiGaugeElement element, StiDataTable dataTable, int maxIndex)
	{
		if (maxIndex == -1)
		{
			return element.Maximum;
		}
		return ((element.CalculationMode == StiGaugeCalculationMode.Auto) ? dataTable.Rows.GetArrayItem(maxIndex).FirstOrDefaultAsNullableDecimal() : new decimal?(element.Maximum)) ?? element.Maximum;
	}

	private int GetValueMeterIndex(StiDataTable table)
	{
		if (table == null)
		{
			return -1;
		}
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f is StiValueGaugeMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return table.Meters.IndexOf(stiMeter);
	}

	private int GetMinMeterIndex(StiDataTable table)
	{
		if (table == null)
		{
			return -1;
		}
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f is StiMinGaugeMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return table.Meters.IndexOf(stiMeter);
	}

	private int GetMaxMeterIndex(StiDataTable table)
	{
		if (table == null)
		{
			return -1;
		}
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f is StiMaxGaugeMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return table.Meters.IndexOf(stiMeter);
	}

	private int GetSeriesMeterIndex(StiDataTable table)
	{
		if (table == null)
		{
			return -1;
		}
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f is StiSeriesGaugeMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return table.Meters.IndexOf(stiMeter);
	}

	private int GetTargetMeterIndex(StiDataTable table)
	{
		if (table == null)
		{
			return -1;
		}
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f is StiTargetGaugeMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return table.Meters.IndexOf(stiMeter);
	}
}
