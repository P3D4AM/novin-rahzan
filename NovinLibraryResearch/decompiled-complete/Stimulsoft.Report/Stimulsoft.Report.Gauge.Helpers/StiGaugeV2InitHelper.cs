using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components.Gauge;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Gauge.Events;

namespace Stimulsoft.Report.Gauge.Helpers;

public static class StiGaugeV2InitHelper
{
	public static bool AllowOldEditor
	{
		get
		{
			return StiSettings.GetBool("Designer", "ShowOldGaugeEditorV1", nullValue: false);
		}
		set
		{
			StiSettings.Set("Designer", "ShowOldGaugeEditorV1", value);
		}
	}

	public static bool IsGaugeV2(StiGauge gauge)
	{
		if (gauge.Mode == StiScaleMode.V1)
		{
			return false;
		}
		if (gauge.Scales.Count != 1)
		{
			return false;
		}
		StiScaleBase scale = gauge.Scales[0];
		switch (gauge.Type)
		{
		case StiGaugeType.FullCircular:
			return IsFullCircularScale(scale);
		case StiGaugeType.HalfCircular:
			return IsHalfCircularScale(scale);
		case StiGaugeType.Linear:
		case StiGaugeType.HorizontalLinear:
			return IsLinearScale(scale);
		case StiGaugeType.Bullet:
			return IsBulletScale(scale);
		default:
			return false;
		}
	}

	public static void Init(StiGauge gauge, StiGaugeType type, bool addRange, bool skipText)
	{
		string value = null;
		StiIndicatorBase stiIndicatorBase = null;
		if (gauge.Scales.Count > 0 && gauge.Scales[0].Items.ToArray().FirstOrDefault((StiGaugeElement x) => x is StiIndicatorBase) is StiIndicatorBase { Value: not null } stiIndicatorBase2)
		{
			value = stiIndicatorBase2.Value.Value;
		}
		StiScaleBase stiScaleBase = null;
		switch (type)
		{
		case StiGaugeType.FullCircular:
			stiScaleBase = CreateFullCircularScale(skipText);
			break;
		case StiGaugeType.HalfCircular:
			stiScaleBase = CreateHalfCircularScale(skipText);
			break;
		case StiGaugeType.Linear:
			stiScaleBase = CreateLinearScale(skipText, isHorizontal: false);
			break;
		case StiGaugeType.HorizontalLinear:
			stiScaleBase = CreateLinearScale(skipText, isHorizontal: true);
			break;
		case StiGaugeType.Bullet:
			stiScaleBase = CreateBullet(gauge, addRange, skipText);
			break;
		}
		if (!string.IsNullOrEmpty(value))
		{
			stiIndicatorBase = stiScaleBase.Items.ToArray().FirstOrDefault((StiGaugeElement x) => x is StiIndicatorBase) as StiIndicatorBase;
			stiIndicatorBase.Value.Value = value;
		}
		gauge.Scales.Clear();
		gauge.Scales.Add(stiScaleBase);
	}

	public static void GetScaleMinMax(StiGaugeType type, out float min, out float max)
	{
		switch (type)
		{
		case StiGaugeType.FullCircular:
			min = 0f;
			max = 180f;
			break;
		case StiGaugeType.HalfCircular:
			min = 0f;
			max = 150f;
			break;
		case StiGaugeType.Linear:
		case StiGaugeType.HorizontalLinear:
		case StiGaugeType.Bullet:
			min = 0f;
			max = 100f;
			break;
		default:
			throw new NotSupportedException();
		}
	}

	internal static void Prepare(StiGauge gauge)
	{
		if (IsGaugeV2(gauge))
		{
			StiScaleBase stiScaleBase = gauge.Scales[0];
			StiIndicatorBase stiIndicatorBase = stiScaleBase.Items.ToArray().FirstOrDefault((StiGaugeElement x) => x is StiIndicatorBase) as StiIndicatorBase;
			StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
			stiIndicatorBase.InvokeGetValue(stiIndicatorBase, stiGetValueEventArgs);
			float value = StiGaugeHelper.GetFloatValueFromObject(stiGetValueEventArgs.Value, stiScaleBase);
			(from x in stiScaleBase.Items.ToArray()
				where x is StiIndicatorBase
				select x).Cast<StiIndicatorBase>().ToList().ForEach(delegate(StiIndicatorBase x)
			{
				x.ValueObj = value;
			});
			if (gauge.CalculationMode == StiGaugeCalculationMode.Auto)
			{
				if (value == 0f)
				{
					stiScaleBase.Minimum = 0f;
					stiScaleBase.Maximum = 100f;
				}
				else
				{
					stiScaleBase.Minimum = value - Math.Abs(value);
					stiScaleBase.Maximum = value + Math.Abs(value);
				}
			}
			else
			{
				stiScaleBase.Minimum = (float)gauge.Minimum;
				stiScaleBase.Maximum = (float)gauge.Maximum;
			}
			StiScaleRangeList stiScaleRangeList = null;
			foreach (object item in stiScaleBase.Items)
			{
				if (item is StiScaleRangeList)
				{
					stiScaleRangeList = (StiScaleRangeList)item;
					break;
				}
			}
			if (stiScaleRangeList != null && stiScaleRangeList.Ranges.Count > 0)
			{
				float minimum = stiScaleBase.Minimum;
				float num = Math.Max(stiScaleBase.Maximum, stiScaleBase.Minimum) - Math.Min(stiScaleBase.Maximum, stiScaleBase.Minimum);
				float num2 = (float)Math.Round(num / (float)stiScaleRangeList.Ranges.Count);
				float num3 = minimum;
				foreach (StiRangeBase range in stiScaleRangeList.Ranges)
				{
					range.StartValue = num3;
					num3 = (range.EndValue = num3 + num2);
				}
				stiScaleRangeList.Ranges[stiScaleRangeList.Ranges.Count - 1].EndValue = stiScaleBase.Maximum;
			}
			float num5 = stiScaleBase.Maximum - stiScaleBase.Minimum;
			stiScaleBase.MajorInterval = Math.Abs((float)Math.Round(num5 / 10f));
			stiScaleBase.MinorInterval = Math.Abs(stiScaleBase.MajorInterval / 2f);
			stiScaleBase.CalculateMinMaxScaleHelper();
			stiScaleBase.CalculateWidthScaleHelper();
			return;
		}
		foreach (StiScaleBase scale in gauge.Scales)
		{
			scale.CalculateMinMaxScaleHelper();
			scale.CalculateWidthScaleHelper();
		}
	}

	private static bool IsFullCircularScale(StiScaleBase scale)
	{
		if (!(scale is StiRadialScale))
		{
			return false;
		}
		if (scale.Items.Count < 3)
		{
			return false;
		}
		return true;
	}

	private static StiScaleBase CreateFullCircularScale(bool skipText)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected O, but got Unknown
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Expected O, but got Unknown
		StiRadialScale stiRadialScale = new StiRadialScale
		{
			StartAngle = 120f,
			SweepAngle = 300f,
			StartWidth = 0.05f,
			EndWidth = 0.05f,
			Minimum = 0f,
			Maximum = 180f,
			MajorInterval = 90f,
			MinorInterval = 10f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(238, 238, 238))
		};
		if (!skipText)
		{
			stiRadialScale.Items.Add(new StiRadialTickMarkMajor
			{
				Placement = StiPlacement.Inside,
				RelativeWidth = 0.05f,
				RelativeHeight = 0.015f,
				Skin = StiTickMarkSkin.Rectangle,
				Brush = (StiBrush)new StiSolidBrush(Color.Black)
			});
		}
		if (!skipText)
		{
			stiRadialScale.Items.Add(new StiRadialTickLabelMajor
			{
				Offset = 0.1f,
				Placement = StiPlacement.Inside
			});
		}
		stiRadialScale.Items.Add(new StiNeedle
		{
			RelativeHeight = 0.3f,
			RelativeWidth = 0.4f,
			StartWidth = 0.1f,
			EndWidth = 1f,
			CapBrush = (StiBrush)new StiEmptyBrush(),
			Brush = (StiBrush)new StiEmptyBrush(),
			TextBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(0, 150, 136)),
			ValueObj = 135f
		});
		stiRadialScale.Items.Add(new StiRadialBar
		{
			StartWidth = 0.1f,
			EndWidth = 0.1f,
			Offset = 0.1f,
			ValueObj = 135f,
			Placement = StiPlacement.Outside,
			Brush = (StiBrush)new StiSolidBrush(Color.Red)
		});
		return stiRadialScale;
	}

	private static bool IsHalfCircularScale(StiScaleBase scale)
	{
		if (!(scale is StiRadialScale))
		{
			return false;
		}
		if (scale.Items.Count != 2)
		{
			return false;
		}
		if (!(scale.Items[0] is StiNeedle))
		{
			return false;
		}
		if (!(scale.Items[1] is StiRadialBar))
		{
			return false;
		}
		StiRadialBar stiRadialBar = (StiRadialBar)scale.Items[1];
		if (stiRadialBar.RangeList.Count != 0)
		{
			return false;
		}
		return true;
	}

	private static StiScaleBase CreateHalfCircularScale(bool skipText)
	{
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Expected O, but got Unknown
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Expected O, but got Unknown
		StiRadialScale stiRadialScale = new StiRadialScale
		{
			StartAngle = 180f,
			SweepAngle = 180f,
			StartWidth = 0.05f,
			EndWidth = 0.05f,
			Minimum = 0f,
			Maximum = 180f,
			Radius = 0.8f,
			MajorInterval = 90f,
			MinorInterval = 10f,
			Center = new PointF(0.5f, 0.7f),
			RadiusMode = StiRadiusMode.Auto,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(238, 238, 238))
		};
		stiRadialScale.Items.Add(new StiNeedle
		{
			RelativeHeight = 0.05f,
			RelativeWidth = 0.4f,
			StartWidth = 0.1f,
			EndWidth = 0.2f,
			CapBrush = (StiBrush)new StiSolidBrush(Color.White),
			CapBorderBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(0, 150, 136)),
			CapBorderWidth = 2f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(0, 150, 136)),
			BorderWidth = 0f,
			TextBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(0, 150, 136)),
			ValueObj = 135f
		});
		stiRadialScale.Items.Add(new StiRadialBar
		{
			StartWidth = 0.05f,
			EndWidth = 0.05f,
			Offset = 0.1f,
			ValueObj = 135f,
			Placement = StiPlacement.Overlay,
			Brush = (StiBrush)new StiSolidBrush(Color.Red)
		});
		return stiRadialScale;
	}

	private static bool IsBulletScale(StiScaleBase scale)
	{
		if (!(scale is StiLinearScale))
		{
			return false;
		}
		if (scale.Items.Count != 3)
		{
			return false;
		}
		if (!(scale.Items[0] is StiLinearRangeList))
		{
			return false;
		}
		if (!(scale.Items[1] is StiLinearTickLabelMajor))
		{
			return false;
		}
		if (!(scale.Items[2] is StiLinearBar))
		{
			return false;
		}
		return true;
	}

	private static bool IsLinearScale(StiScaleBase scale)
	{
		if (!(scale is StiLinearScale))
		{
			return false;
		}
		if (scale.Items.Count != 3)
		{
			return false;
		}
		if (!(scale.Items[0] is StiLinearTickLabelMajor))
		{
			return false;
		}
		if (!(scale.Items[1] is StiLinearTickMarkMajor))
		{
			return false;
		}
		if (!(scale.Items[2] is StiLinearMarker))
		{
			return false;
		}
		return true;
	}

	private static StiScaleBase CreateLinearScale(bool skipText, bool isHorizontal)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		StiLinearScale stiLinearScale = new StiLinearScale
		{
			Orientation = (Orientation)(!isHorizontal),
			StartWidth = 0.1f,
			EndWidth = 0.1f,
			Maximum = 100f,
			MinorInterval = 5f,
			BorderBrush = (StiBrush)new StiEmptyBrush(),
			Brush = (StiBrush)new StiEmptyBrush()
		};
		if (!skipText)
		{
			stiLinearScale.Items.Add(new StiLinearTickLabelMajor
			{
				Placement = StiPlacement.Inside,
				Font = new Font("Arial", 8f),
				TextBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(158, 158, 158))
			});
			stiLinearScale.Items.Add(new StiLinearTickMarkMajor
			{
				BorderBrush = (StiBrush)new StiEmptyBrush(),
				BorderWidth = 0f,
				RelativeHeight = 0.005f,
				RelativeWidth = 0.05f,
				Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(158, 158, 158))
			});
		}
		StiLinearMarker stiLinearMarker = new StiLinearMarker
		{
			RelativeWidth = 0.1f,
			RelativeHeight = 0.04f,
			Placement = StiPlacement.Overlay,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(205, 220, 57)),
			ValueObj = 85f
		};
		stiLinearScale.Items.Add(stiLinearMarker);
		if (skipText)
		{
			stiLinearMarker.RelativeWidth = 0.07f;
			stiLinearMarker.RelativeHeight = 0.08f;
		}
		return stiLinearScale;
	}

	private static StiScaleBase CreateBullet(StiGauge gauge, bool addRange, bool skipText)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		StiLinearScale stiLinearScale = new StiLinearScale
		{
			Orientation = (Orientation)0,
			RelativeHeight = 0.85f,
			StartWidth = 0.01f,
			EndWidth = 0.01f,
			Maximum = 100f,
			BorderBrush = (StiBrush)new StiEmptyBrush(),
			Brush = (StiBrush)new StiEmptyBrush()
		};
		if (addRange)
		{
			AddLinearRanges(0.0, 100.0, stiLinearScale);
		}
		if (!skipText)
		{
			stiLinearScale.Items.Add(new StiLinearTickLabelMajor
			{
				FormatService = gauge.ValueFormat,
				Placement = StiPlacement.Outside,
				Offset = 0.25f
			});
		}
		StiLinearBar stiLinearBar = new StiLinearBar
		{
			StartWidth = 0.15f,
			EndWidth = 0.15f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(205, 220, 57)),
			ValueObj = 85f
		};
		stiLinearBar.Value.Value = "85";
		stiLinearScale.Items.Add(stiLinearBar);
		return stiLinearScale;
	}

	private static void AddLinearRanges(double minValue, double maxValue, StiLinearScale scale)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Expected O, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		StiLinearRangeList stiLinearRangeList = new StiLinearRangeList();
		StiLinearRange element = new StiLinearRange
		{
			Placement = StiPlacement.Overlay,
			StartValue = (float)(minValue + (maxValue - minValue) * 0.0),
			EndValue = (float)(minValue + (maxValue - minValue) * 0.3),
			StartWidth = 0.3f,
			EndWidth = 0.3f,
			BorderBrush = (StiBrush)new StiEmptyBrush(),
			Brush = (StiBrush)new StiSolidBrush(Color.Red)
		};
		stiLinearRangeList.Ranges.Add(element);
		StiLinearRange element2 = new StiLinearRange
		{
			Placement = StiPlacement.Overlay,
			StartValue = (float)(minValue + (maxValue - minValue) * 0.3),
			EndValue = (float)(minValue + (maxValue - minValue) * 0.65),
			StartWidth = 0.3f,
			EndWidth = 0.3f,
			BorderBrush = (StiBrush)new StiEmptyBrush(),
			Brush = (StiBrush)new StiSolidBrush(Color.Yellow)
		};
		stiLinearRangeList.Ranges.Add(element2);
		StiLinearRange element3 = new StiLinearRange
		{
			Placement = StiPlacement.Overlay,
			StartValue = (float)(minValue + (maxValue - minValue) * 0.65),
			EndValue = (float)(minValue + (maxValue - minValue) * 1.0),
			StartWidth = 0.3f,
			EndWidth = 0.3f,
			BorderBrush = (StiBrush)new StiEmptyBrush(),
			Brush = (StiBrush)new StiSolidBrush(Color.Green)
		};
		stiLinearRangeList.Ranges.Add(element3);
		scale.Items.Add(stiLinearRangeList);
	}
}
