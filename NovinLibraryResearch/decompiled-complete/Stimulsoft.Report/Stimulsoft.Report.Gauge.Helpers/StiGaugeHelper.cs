using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components.Gauge;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Gauge.Helpers;

public static class StiGaugeHelper
{
	public static TimeSpan GlobalDurationElement = TimeSpan.FromMilliseconds(1800.0);

	public static TimeSpan GlobalBeginTimeElement = TimeSpan.FromMilliseconds(300.0);

	public static float GetFloatValueFromObject(object valueObj, StiScaleBase scale)
	{
		float result = scale.Minimum;
		if (valueObj != null)
		{
			if (valueObj is string)
			{
				CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
				string s = ((string)valueObj).Replace(",", ".");
				if (float.TryParse(s, out result))
				{
					if (result < scale.Minimum)
					{
						result = scale.Minimum;
					}
					else if (result > scale.Maximum)
					{
						result = scale.Maximum;
					}
				}
				else
				{
					result = scale.Minimum;
				}
				Thread.CurrentThread.CurrentCulture = currentCulture;
			}
			else
			{
				result = Convert.ToSingle(valueObj);
			}
		}
		return result;
	}

	public static float GetFloatValueFromObject(object valueObj, float defaultValue)
	{
		float result = defaultValue;
		if (valueObj != null)
		{
			if (valueObj is string)
			{
				CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
				if (!float.TryParse((string)valueObj, out result))
				{
					result = defaultValue;
				}
				Thread.CurrentThread.CurrentCulture = currentCulture;
			}
			else
			{
				result = Convert.ToSingle(valueObj);
			}
		}
		return result;
	}

	public static float[] GetFloatArrayValueFromString(object value)
	{
		string text = value as string;
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		string[] array = text.Split(new char[1] { ';' });
		float[] array2 = new float[array.Length];
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
		int num = -1;
		while (++num < array.Length)
		{
			if (float.TryParse(array[num], out var result))
			{
				array2[num] = result;
				continue;
			}
			array2 = null;
			break;
		}
		Thread.CurrentThread.CurrentCulture = currentCulture;
		return array2;
	}

	private static void InitializeGauge(StiGauge gauge, double width, double height)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		gauge.Scales.Clear();
		if (gauge.Page != null)
		{
			StiUnit unit = gauge.Page.Unit;
			gauge.ClientRectangle = new RectangleD(gauge.Left, gauge.Top, unit.ConvertFromHInches(width), unit.ConvertFromHInches(height));
		}
	}

	private static void InitializeName(StiGauge gauge, StiReport report)
	{
		if (!string.IsNullOrEmpty(gauge.Name))
		{
			return;
		}
		string name = StiNameCreation.CreateSimpleName(report, StiNameCreation.GenerateName(gauge));
		gauge.Name = name;
		int num = -1;
		while (++num < gauge.Scales.Count)
		{
			Hashtable hashtable = new Hashtable();
			StiScaleBase stiScaleBase = gauge.Scales[num];
			int num2 = -1;
			while (++num2 < stiScaleBase.Items.Count)
			{
				StiGaugeElement stiGaugeElement = stiScaleBase.Items[num2];
				Type type = stiGaugeElement.GetType();
				int num3 = 0;
				if (hashtable.ContainsKey(type))
				{
					num3 = (int)hashtable[type];
				}
				num3++;
				hashtable[type] = num3;
			}
		}
	}

	public static void CheckGaugeName(StiGauge gauge)
	{
		int num = -1;
		while (++num < gauge.Scales.Count)
		{
			Hashtable hashtable = new Hashtable();
			StiScaleBase stiScaleBase = gauge.Scales[num];
			int num2 = -1;
			while (++num2 < stiScaleBase.Items.Count)
			{
				StiGaugeElement stiGaugeElement = stiScaleBase.Items[num2];
				Type type = stiGaugeElement.GetType();
				int num3 = 0;
				if (hashtable.ContainsKey(type))
				{
					num3 = (int)hashtable[type];
				}
				num3++;
				hashtable[type] = num3;
			}
		}
	}

	public static void SimpleRadialGauge(StiGauge gauge, StiReport report)
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		InitializeGauge(gauge, 250.0, 250.0);
		StiRadialScale stiRadialScale = new StiRadialScale
		{
			StartAngle = 0f,
			SweepAngle = 340f,
			StartWidth = 0.05f,
			EndWidth = 0.05f,
			Minimum = 0f,
			Maximum = 100f,
			MajorInterval = 10f,
			MinorInterval = 5f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(238, 238, 238))
		};
		StiRadialTickMarkMajor element = new StiRadialTickMarkMajor
		{
			Placement = StiPlacement.Overlay,
			OffsetAngle = -2f,
			RelativeWidth = 0.05f,
			RelativeHeight = 0.02f,
			Skin = StiTickMarkSkin.Rectangle,
			Brush = (StiBrush)new StiSolidBrush(Color.Black)
		};
		StiRadialTickMarkMinor element2 = new StiRadialTickMarkMinor
		{
			Placement = StiPlacement.Overlay,
			RelativeWidth = 0.01f,
			RelativeHeight = 0.01f,
			Skin = StiTickMarkSkin.Rectangle
		};
		StiRadialTickLabelMajor element3 = new StiRadialTickLabelMajor();
		StiNeedle element4 = new StiNeedle
		{
			RelativeHeight = 0.06f,
			RelativeWidth = 0.45f,
			StartWidth = 0.1f,
			EndWidth = 1f,
			CapBrush = (StiBrush)new StiSolidBrush(Color.White),
			CapBorderBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(244, 67, 54)),
			CapBorderWidth = 2f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(244, 67, 54)),
			Value = new StiValueExpression("60")
		};
		stiRadialScale.Items.Add(element);
		stiRadialScale.Items.Add(element2);
		stiRadialScale.Items.Add(element3);
		stiRadialScale.Items.Add(element4);
		gauge.Scales.Add(stiRadialScale);
		InitializeName(gauge, report);
	}

	public static void RadialTwoScalesGauge(StiGauge gauge, StiReport report)
	{
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Expected O, but got Unknown
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Expected O, but got Unknown
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Expected O, but got Unknown
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Expected O, but got Unknown
		InitializeGauge(gauge, 250.0, 250.0);
		StiRadialScale stiRadialScale = new StiRadialScale
		{
			StartAngle = 270f,
			SweepAngle = 360f,
			StartWidth = 0.05f,
			EndWidth = 0.05f,
			Minimum = 0f,
			Maximum = 60f,
			Radius = 0.5f,
			MajorInterval = 10f,
			MinorInterval = 1f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(238, 238, 238))
		};
		StiRadialTickMarkMajor element = new StiRadialTickMarkMajor
		{
			Placement = StiPlacement.Overlay,
			RelativeWidth = 0.05f,
			OffsetAngle = -2f,
			RelativeHeight = 0.03f,
			Skin = StiTickMarkSkin.Rectangle,
			Brush = (StiBrush)new StiSolidBrush(Color.Black)
		};
		StiRadialTickLabelMajor element2 = new StiRadialTickLabelMajor
		{
			MinimumValue = 1f,
			Font = new Font("Arial", 7f)
		};
		stiRadialScale.Items.Add(element);
		stiRadialScale.Items.Add(element2);
		gauge.Scales.Add(stiRadialScale);
		StiRadialScale stiRadialScale2 = new StiRadialScale
		{
			StartAngle = 270f,
			SweepAngle = 360f,
			StartWidth = 0.05f,
			EndWidth = 0.05f,
			Minimum = 0f,
			Maximum = 12f,
			Radius = 0.75f,
			MajorInterval = 1f,
			MinorInterval = 0f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(238, 238, 238))
		};
		StiRadialTickMarkMajor element3 = new StiRadialTickMarkMajor
		{
			MinimumValue = 1f,
			Placement = StiPlacement.Overlay,
			OffsetAngle = -2f,
			RelativeHeight = 0.03f,
			RelativeWidth = 0.05f,
			Skin = StiTickMarkSkin.Rectangle,
			Brush = (StiBrush)new StiSolidBrush(Color.Black),
			BorderBrush = (StiBrush)new StiSolidBrush(Color.White),
			BorderWidth = 3f
		};
		StiRadialTickLabelMajor element4 = new StiRadialTickLabelMajor
		{
			MinimumValue = 1f
		};
		StiNeedle element5 = new StiNeedle
		{
			Value = new StiValueExpression("2"),
			OffsetNeedle = 0.15f,
			StartWidth = 0.05f,
			EndWidth = 0.5f,
			RelativeHeight = 0.12f,
			RelativeWidth = 0.6f,
			CapBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(244, 67, 54)),
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(244, 67, 54))
		};
		StiNeedle element6 = new StiNeedle
		{
			Value = new StiValueExpression("10"),
			OffsetNeedle = 0.15f,
			StartWidth = 0.05f,
			EndWidth = 0.5f,
			RelativeHeight = 0.08f,
			RelativeWidth = 0.4f,
			CapBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(3, 169, 244)),
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(3, 169, 244))
		};
		stiRadialScale2.Items.Add(element3);
		stiRadialScale2.Items.Add(element4);
		stiRadialScale2.Items.Add(element5);
		stiRadialScale2.Items.Add(element6);
		gauge.Scales.Add(stiRadialScale2);
		InitializeName(gauge, report);
	}

	public static void RadialBarGauge(StiGauge gauge, StiReport report)
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Expected O, but got Unknown
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Expected O, but got Unknown
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Expected O, but got Unknown
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Expected O, but got Unknown
		InitializeGauge(gauge, 250.0, 250.0);
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
		StiRadialTickMarkMajor element = new StiRadialTickMarkMajor
		{
			Placement = StiPlacement.Inside,
			OffsetAngle = -1f,
			RelativeWidth = 0.05f,
			RelativeHeight = 0.015f,
			Skin = StiTickMarkSkin.Rectangle,
			Brush = (StiBrush)new StiSolidBrush(Color.Black)
		};
		StiRadialTickMarkMinor element2 = new StiRadialTickMarkMinor
		{
			Placement = StiPlacement.Inside,
			RelativeWidth = 0.03f,
			RelativeHeight = 0.005f,
			Skin = StiTickMarkSkin.Rectangle,
			Brush = (StiBrush)new StiSolidBrush(Color.Black)
		};
		StiRadialTickLabelMajor element3 = new StiRadialTickLabelMajor
		{
			Placement = StiPlacement.Inside,
			MinimumValue = 1f,
			Offset = 0.15f
		};
		StiRadialBar element4 = new StiRadialBar
		{
			StartWidth = 0.1f,
			EndWidth = 0.1f,
			Offset = 0.1f,
			Placement = StiPlacement.Outside,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(0, 150, 136)),
			Value = new StiValueExpression("90")
		};
		StiNeedle element5 = new StiNeedle
		{
			RelativeHeight = 0.3f,
			RelativeWidth = 0.4f,
			StartWidth = 0.1f,
			EndWidth = 1f,
			CapBrush = (StiBrush)new StiEmptyBrush(),
			Brush = (StiBrush)new StiEmptyBrush(),
			TextBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(0, 150, 136)),
			Font = new Font("Arial", 36f),
			Value = new StiValueExpression("90")
		};
		stiRadialScale.Items.Add(element);
		stiRadialScale.Items.Add(element2);
		stiRadialScale.Items.Add(element3);
		stiRadialScale.Items.Add(element4);
		stiRadialScale.Items.Add(element5);
		gauge.Scales.Add(stiRadialScale);
		InitializeName(gauge, report);
	}

	public static void SimpleTwoBarGauge(StiGauge gauge, StiReport report)
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		InitializeGauge(gauge, 250.0, 250.0);
		StiRadialScale stiRadialScale = new StiRadialScale
		{
			Minimum = 0f,
			Maximum = 180f,
			MajorInterval = 20f,
			MinorInterval = 10f,
			StartWidth = 0.05f,
			EndWidth = 0.05f,
			StartAngle = 120f,
			SweepAngle = 300f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(238, 238, 238))
		};
		StiRadialTickMarkMajor element = new StiRadialTickMarkMajor
		{
			Placement = StiPlacement.Overlay,
			OffsetAngle = -1f,
			RelativeWidth = 0.05f,
			RelativeHeight = 0.02f,
			Skin = StiTickMarkSkin.Rectangle,
			Brush = (StiBrush)new StiSolidBrush(Color.Black),
			BorderBrush = (StiBrush)new StiSolidBrush(Color.White),
			BorderWidth = 1f
		};
		StiRadialTickMarkMinor element2 = new StiRadialTickMarkMinor
		{
			Placement = StiPlacement.Overlay,
			RelativeWidth = 0.01f,
			RelativeHeight = 0.01f,
			Skin = StiTickMarkSkin.Diamond,
			Brush = (StiBrush)new StiSolidBrush(Color.Black),
			BorderBrush = (StiBrush)new StiSolidBrush(Color.Gray),
			BorderWidth = 1f
		};
		StiRadialTickLabelMajor element3 = new StiRadialTickLabelMajor
		{
			Placement = StiPlacement.Inside,
			Offset = 0.15f
		};
		StiRadialBar element4 = new StiRadialBar
		{
			StartWidth = 0.05f,
			EndWidth = 0.05f,
			Offset = 0f,
			Placement = StiPlacement.Outside,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(3, 169, 244)),
			Value = new StiValueExpression("80")
		};
		StiRadialBar element5 = new StiRadialBar
		{
			StartWidth = 0.05f,
			EndWidth = 0.05f,
			Offset = 0f,
			Placement = StiPlacement.Inside,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(255, 235, 59)),
			Value = new StiValueExpression("120")
		};
		StiRadialMarker element6 = new StiRadialMarker
		{
			Value = new StiValueExpression("100"),
			Offset = 0.25f,
			RelativeHeight = 0.05f,
			RelativeWidth = 0.05f,
			Skin = StiMarkerSkin.TriangleRight,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(76, 175, 80))
		};
		stiRadialScale.Items.Add(element);
		stiRadialScale.Items.Add(element2);
		stiRadialScale.Items.Add(element3);
		stiRadialScale.Items.Add(element4);
		stiRadialScale.Items.Add(element5);
		stiRadialScale.Items.Add(element6);
		gauge.Scales.Add(stiRadialScale);
		InitializeName(gauge, report);
	}

	public static void DefaultRadialGauge(StiGauge gauge, StiReport report)
	{
		InitializeGauge(gauge, 250.0, 250.0);
		StiRadialScale stiRadialScale = new StiRadialScale
		{
			StartAngle = 0f,
			SweepAngle = 340f,
			StartWidth = 0.05f,
			EndWidth = 0.05f,
			Minimum = 0f,
			Maximum = 100f,
			MajorInterval = 10f,
			MinorInterval = 5f
		};
		StiRadialTickMarkMajor element = new StiRadialTickMarkMajor
		{
			Placement = StiPlacement.Overlay,
			OffsetAngle = -2f,
			RelativeWidth = 0.06f,
			RelativeHeight = 0.03f,
			Skin = StiTickMarkSkin.TriangleRight
		};
		StiRadialTickLabelMajor element2 = new StiRadialTickLabelMajor();
		StiNeedle element3 = new StiNeedle
		{
			RelativeHeight = 0.14f,
			RelativeWidth = 0.5f
		};
		stiRadialScale.Items.Add(element);
		stiRadialScale.Items.Add(element2);
		stiRadialScale.Items.Add(element3);
		gauge.Scales.Add(stiRadialScale);
		InitializeName(gauge, report);
	}

	public static void DefaultLinearGauge(StiGauge gauge, StiReport report)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		InitializeGauge(gauge, 120.0, 240.0);
		StiLinearScale stiLinearScale = new StiLinearScale
		{
			StartWidth = 0.1f,
			EndWidth = 0.1f,
			Maximum = 100f,
			MinorInterval = 5f,
			BorderBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(158, 158, 158)),
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(238, 238, 238))
		};
		StiLinearTickMarkMajor element = new StiLinearTickMarkMajor
		{
			BorderBrush = (StiBrush)new StiEmptyBrush(),
			BorderWidth = 0f,
			RelativeHeight = 0.005f,
			RelativeWidth = 0.05f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(158, 158, 158))
		};
		StiLinearTickLabelMajor element2 = new StiLinearTickLabelMajor
		{
			Placement = StiPlacement.Inside,
			TextBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(158, 158, 158))
		};
		StiLinearBar element3 = new StiLinearBar
		{
			StartWidth = 0.1f,
			EndWidth = 0.1f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(0, 150, 136)),
			Value = new StiValueExpression("65")
		};
		stiLinearScale.Items.Add(element);
		stiLinearScale.Items.Add(element2);
		stiLinearScale.Items.Add(element3);
		gauge.Scales.Add(stiLinearScale);
		InitializeName(gauge, report);
	}

	public static void LinearGaugeRangeList(StiGauge gauge, StiReport report)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Expected O, but got Unknown
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Expected O, but got Unknown
		InitializeGauge(gauge, 120.0, 240.0);
		StiLinearScale stiLinearScale = new StiLinearScale
		{
			StartWidth = 0.1f,
			EndWidth = 0.1f,
			Maximum = 100f,
			MinorInterval = 5f,
			BorderBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(158, 158, 158)),
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(238, 238, 238))
		};
		StiLinearRangeList stiLinearRangeList = new StiLinearRangeList();
		StiLinearRange element = new StiLinearRange
		{
			StartValue = 0f,
			EndValue = 50f,
			Brush = (StiBrush)new StiGradientBrush(Color.FromArgb(205, 220, 57), Color.FromArgb(139, 195, 74), 90.0),
			StartWidth = 0.07f,
			EndWidth = 0.07f
		};
		StiLinearRange element2 = new StiLinearRange
		{
			StartValue = 45f,
			EndValue = 75f,
			Brush = (StiBrush)new StiGradientBrush(Color.FromArgb(255, 193, 7), Color.FromArgb(255, 235, 59), 90.0),
			StartWidth = 0.07f,
			EndWidth = 0.07f
		};
		StiLinearRange element3 = new StiLinearRange
		{
			StartValue = 75f,
			EndValue = 100f,
			Brush = (StiBrush)new StiGradientBrush(Color.FromArgb(255, 152, 0), Color.FromArgb(255, 152, 0), 90.0),
			StartWidth = 0.07f,
			EndWidth = 0.07f
		};
		stiLinearRangeList.Ranges.Add(element);
		stiLinearRangeList.Ranges.Add(element2);
		stiLinearRangeList.Ranges.Add(element3);
		StiLinearTickLabelMajor element4 = new StiLinearTickLabelMajor
		{
			Placement = StiPlacement.Inside,
			TextBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(158, 158, 158))
		};
		StiLinearTickMarkMajor element5 = new StiLinearTickMarkMajor
		{
			BorderBrush = (StiBrush)new StiEmptyBrush(),
			BorderWidth = 0f,
			RelativeHeight = 0.005f,
			RelativeWidth = 0.05f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(158, 158, 158))
		};
		StiLinearTickMarkMinor element6 = new StiLinearTickMarkMinor
		{
			Placement = StiPlacement.Overlay,
			RelativeWidth = 0.08f,
			RelativeHeight = 0.006f,
			BorderBrush = (StiBrush)new StiEmptyBrush()
		};
		StiLinearMarker element7 = new StiLinearMarker
		{
			RelativeWidth = 0.18f,
			RelativeHeight = 0.04f,
			Placement = StiPlacement.Overlay,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(205, 220, 57))
		};
		stiLinearScale.Items.Add(stiLinearRangeList);
		stiLinearScale.Items.Add(element4);
		stiLinearScale.Items.Add(element5);
		stiLinearScale.Items.Add(element6);
		stiLinearScale.Items.Add(element7);
		gauge.Scales.Add(stiLinearScale);
		InitializeName(gauge, report);
	}

	public static void BulletGraphsGreen(StiGauge gauge, StiReport report)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Expected O, but got Unknown
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Expected O, but got Unknown
		InitializeGauge(gauge, 300.0, 100.0);
		StiLinearScale stiLinearScale = new StiLinearScale
		{
			RelativeHeight = 0.94f,
			StartWidth = 0.01f,
			EndWidth = 0.01f,
			Maximum = 100f,
			MajorInterval = 20f,
			Orientation = (Orientation)0,
			Brush = (StiBrush)new StiEmptyBrush(),
			BorderBrush = (StiBrush)new StiEmptyBrush()
		};
		StiLinearRangeList stiLinearRangeList = new StiLinearRangeList();
		float num = 0f;
		for (int i = 1; i <= 10; i++)
		{
			StiLinearRange stiLinearRange = new StiLinearRange
			{
				StartValue = num,
				EndValue = num + 10f,
				StartWidth = 0.5f,
				EndWidth = 0.5f,
				Placement = StiPlacement.Overlay,
				BorderBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(150, 139, 138, 135))
			};
			if (i >= 1 && i <= 3)
			{
				stiLinearRange.Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(165, 214, 167));
			}
			else if (i >= 4 && i <= 7)
			{
				stiLinearRange.Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(76, 175, 80));
			}
			else
			{
				stiLinearRange.Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(46, 125, 50));
			}
			stiLinearRangeList.Ranges.Add(stiLinearRange);
			num += 10f;
		}
		StiLinearTickLabelMajor element = new StiLinearTickLabelMajor
		{
			Offset = 0.25f
		};
		StiLinearTickMarkCustom element2 = new StiLinearTickMarkCustom
		{
			Placement = StiPlacement.Overlay,
			RelativeWidth = 0.015f,
			RelativeHeight = 0.3f,
			Brush = (StiBrush)new StiGradientBrush(Color.FromArgb(100, 100, 100), Color.FromArgb(10, 10, 10), 90.0),
			ValueObj = 80f
		};
		StiLinearBar element3 = new StiLinearBar
		{
			Brush = (StiBrush)new StiSolidBrush(Color.Black),
			StartWidth = 0.1f,
			EndWidth = 0.1f
		};
		stiLinearScale.Items.Add(stiLinearRangeList);
		stiLinearScale.Items.Add(element);
		stiLinearScale.Items.Add(element2);
		stiLinearScale.Items.Add(element3);
		gauge.Scales.Add(stiLinearScale);
		InitializeName(gauge, report);
	}

	public static void HalfDonutsGauge(StiGauge gauge, StiReport report)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Expected O, but got Unknown
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Expected O, but got Unknown
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Expected O, but got Unknown
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		InitializeGauge(gauge, 280.0, 165.0);
		StiRadialScale stiRadialScale = new StiRadialScale
		{
			StartAngle = 180f,
			SweepAngle = 180f,
			Minimum = 0f,
			Maximum = 150f,
			Radius = 0.75f,
			Center = new PointF(0.5f, 0.8f),
			RadiusMode = StiRadiusMode.Width,
			Skin = StiRadialScaleSkin.Empty
		};
		StiRadialBar stiRadialBar = new StiRadialBar
		{
			BorderBrush = (StiBrush)new StiEmptyBrush(),
			BorderWidth = 0f,
			StartWidth = 0.3f,
			EndWidth = 0.3f,
			EmptyBrush = (StiBrush)new StiGradientBrush(Color.FromArgb(221, 221, 221), Color.FromArgb(240, 240, 240), 90.0),
			UseRangeColor = true,
			Value = new StiValueExpression("60")
		};
		StiRadialIndicatorRangeInfo element = new StiRadialIndicatorRangeInfo
		{
			Value = 0f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(139, 195, 74))
		};
		StiRadialIndicatorRangeInfo element2 = new StiRadialIndicatorRangeInfo
		{
			Value = 50f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(255, 235, 59))
		};
		StiRadialIndicatorRangeInfo element3 = new StiRadialIndicatorRangeInfo
		{
			Value = 100f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(244, 67, 54))
		};
		stiRadialBar.RangeList.Add(element);
		stiRadialBar.RangeList.Add(element2);
		stiRadialBar.RangeList.Add(element3);
		StiNeedle element4 = new StiNeedle
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
			Value = new StiValueExpression("60")
		};
		stiRadialScale.Items.Add(stiRadialBar);
		stiRadialScale.Items.Add(element4);
		gauge.Scales.Add(stiRadialScale);
		InitializeName(gauge, report);
	}

	public static void HalfDonutsGauge2(StiGauge gauge, StiReport report)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Expected O, but got Unknown
		InitializeGauge(gauge, 280.0, 165.0);
		StiRadialScale stiRadialScale = new StiRadialScale
		{
			StartAngle = 180f,
			SweepAngle = 180f,
			Minimum = 0f,
			Maximum = 100f,
			Radius = 0.75f,
			Center = new PointF(0.5f, 0.8f),
			RadiusMode = StiRadiusMode.Width,
			Skin = StiRadialScaleSkin.Empty
		};
		StiRadialBar element = new StiRadialBar
		{
			BorderBrush = (StiBrush)new StiEmptyBrush(),
			StartWidth = 0.3f,
			EndWidth = 0.3f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(79, 134, 194)),
			EmptyBrush = (StiBrush)new StiGradientBrush(Color.FromArgb(221, 221, 221), Color.FromArgb(240, 240, 240), 90.0),
			EmptyBorderBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(79, 134, 194)),
			EmptyBorderWidth = 3f
		};
		stiRadialScale.Items.Add(element);
		gauge.Scales.Add(stiRadialScale);
		InitializeName(gauge, report);
	}

	public static void RadialGaugeHalfCircleN(StiGauge gauge, StiReport report)
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		InitializeGauge(gauge, 260.0, 150.0);
		StiRadialScale stiRadialScale = new StiRadialScale
		{
			RadiusMode = StiRadiusMode.Width,
			Skin = StiRadialScaleSkin.RadialScaleHalfCircleN,
			StartAngle = 180f,
			SweepAngle = 180f,
			Maximum = 100f,
			Center = new PointF(0.5f, 0.85f),
			Radius = 0.75f,
			StartWidth = 0.005f,
			EndWidth = 0.005f,
			MajorInterval = 10f,
			MinorInterval = 5f,
			Brush = (StiBrush)new StiSolidBrush(Color.Gray)
		};
		StiRadialTickMarkMajor element = new StiRadialTickMarkMajor
		{
			Placement = StiPlacement.Overlay,
			Offset = 0.035f,
			RelativeWidth = 0.05f,
			RelativeHeight = 0.007f,
			Brush = (StiBrush)new StiSolidBrush(Color.Gray),
			BorderBrush = (StiBrush)new StiEmptyBrush()
		};
		StiRadialTickMarkMinor element2 = new StiRadialTickMarkMinor
		{
			Placement = StiPlacement.Overlay,
			Offset = 0.017f,
			RelativeWidth = 0.03f,
			RelativeHeight = 0.007f,
			Brush = (StiBrush)new StiSolidBrush(Color.LightGray),
			BorderBrush = (StiBrush)new StiEmptyBrush()
		};
		StiRadialTickLabelMajor element3 = new StiRadialTickLabelMajor
		{
			LabelRotationMode = StiLabelRotationMode.Automatic,
			TextBrush = (StiBrush)new StiSolidBrush(Color.Black)
		};
		StiNeedle element4 = new StiNeedle
		{
			Value = new StiValueExpression("45"),
			CapBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(3, 169, 244)),
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(3, 169, 244)),
			StartWidth = 0.1f,
			EndWidth = 0.99f,
			RelativeWidth = 0.5f,
			RelativeHeight = 0.04f
		};
		stiRadialScale.Items.Add(element);
		stiRadialScale.Items.Add(element2);
		stiRadialScale.Items.Add(element3);
		stiRadialScale.Items.Add(element4);
		gauge.Scales.Add(stiRadialScale);
		InitializeName(gauge, report);
	}

	public static void RadialGaugeHalfCircleS(StiGauge gauge, StiReport report)
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		InitializeGauge(gauge, 260.0, 150.0);
		StiRadialScale stiRadialScale = new StiRadialScale
		{
			RadiusMode = StiRadiusMode.Width,
			Skin = StiRadialScaleSkin.RadialScaleHalfCircleS,
			StartAngle = 0f,
			SweepAngle = 180f,
			Maximum = 100f,
			Center = new PointF(0.5f, 0.15f),
			Radius = 0.75f,
			StartWidth = 0.005f,
			EndWidth = 0.005f,
			MajorInterval = 10f,
			MinorInterval = 5f,
			Brush = (StiBrush)new StiSolidBrush(Color.Gray)
		};
		StiRadialTickMarkMajor element = new StiRadialTickMarkMajor
		{
			Placement = StiPlacement.Overlay,
			Offset = 0.035f,
			RelativeWidth = 0.05f,
			RelativeHeight = 0.007f,
			Brush = (StiBrush)new StiSolidBrush(Color.Gray),
			BorderBrush = (StiBrush)new StiEmptyBrush()
		};
		StiRadialTickMarkMinor element2 = new StiRadialTickMarkMinor
		{
			Placement = StiPlacement.Overlay,
			Offset = 0.017f,
			RelativeWidth = 0.03f,
			RelativeHeight = 0.007f,
			Brush = (StiBrush)new StiSolidBrush(Color.LightGray),
			BorderBrush = (StiBrush)new StiEmptyBrush()
		};
		StiRadialTickLabelMajor element3 = new StiRadialTickLabelMajor
		{
			LabelRotationMode = StiLabelRotationMode.Automatic,
			TextBrush = (StiBrush)new StiSolidBrush(Color.Black)
		};
		StiNeedle element4 = new StiNeedle
		{
			Value = new StiValueExpression("45"),
			CapBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(3, 169, 244)),
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(3, 169, 244)),
			StartWidth = 0.1f,
			EndWidth = 0.99f,
			RelativeWidth = 0.5f,
			RelativeHeight = 0.04f
		};
		stiRadialScale.Items.Add(element);
		stiRadialScale.Items.Add(element2);
		stiRadialScale.Items.Add(element3);
		stiRadialScale.Items.Add(element4);
		gauge.Scales.Add(stiRadialScale);
		InitializeName(gauge, report);
	}

	public static void RadialGaugeQuarterCircleNW(StiGauge gauge, StiReport report)
	{
		RadialGaugeQuarterCircle(gauge, report, StiRadialScaleSkin.RadialScaleQuarterCircleNW, 180f, new PointF(0.9f, 0.9f));
	}

	public static void RadialGaugeQuarterCircleNE(StiGauge gauge, StiReport report)
	{
		RadialGaugeQuarterCircle(gauge, report, StiRadialScaleSkin.RadialScaleQuarterCircleNE, 270f, new PointF(0.1f, 0.9f));
	}

	public static void RadialGaugeQuarterCircleSW(StiGauge gauge, StiReport report)
	{
		RadialGaugeQuarterCircle(gauge, report, StiRadialScaleSkin.RadialScaleQuarterCircleSW, 90f, new PointF(0.9f, 0.1f));
	}

	public static void RadialGaugeQuarterCircleSE(StiGauge gauge, StiReport report)
	{
		RadialGaugeQuarterCircle(gauge, report, StiRadialScaleSkin.RadialScaleQuarterCircleSE, 0f, new PointF(0.1f, 0.1f));
	}

	private static void RadialGaugeQuarterCircle(StiGauge gauge, StiReport report, StiRadialScaleSkin scaleSkin, float startAngle, PointF center)
	{
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Expected O, but got Unknown
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		InitializeGauge(gauge, 250.0, 250.0);
		StiRadialScale stiRadialScale = new StiRadialScale
		{
			Skin = scaleSkin,
			StartAngle = startAngle,
			SweepAngle = 90f,
			Maximum = 100f,
			Center = center,
			Radius = 1.5f,
			StartWidth = 0.005f,
			EndWidth = 0.005f,
			MajorInterval = 10f,
			MinorInterval = 5f
		};
		StiRadialTickMarkMajor element = new StiRadialTickMarkMajor
		{
			Placement = StiPlacement.Overlay,
			Offset = 0.045f,
			RelativeWidth = 0.03f,
			RelativeHeight = 0.004f,
			BorderBrush = (StiBrush)new StiEmptyBrush(),
			Brush = (StiBrush)new StiSolidBrush(Color.Gray)
		};
		StiRadialTickMarkMinor element2 = new StiRadialTickMarkMinor
		{
			Placement = StiPlacement.Overlay,
			Offset = 0.04f,
			RelativeWidth = 0.02f,
			RelativeHeight = 0.004f,
			BorderBrush = (StiBrush)new StiEmptyBrush(),
			Brush = (StiBrush)new StiSolidBrush(Color.Gray)
		};
		StiRadialTickLabelMajor element3 = new StiRadialTickLabelMajor
		{
			LabelRotationMode = StiLabelRotationMode.Automatic
		};
		StiNeedle element4 = new StiNeedle
		{
			Value = new StiValueExpression("45"),
			CapBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(244, 67, 54)),
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(244, 67, 54)),
			StartWidth = 0.1f,
			EndWidth = 0.99f,
			RelativeWidth = 0.5f,
			RelativeHeight = 0.04f
		};
		stiRadialScale.Items.Add(element);
		stiRadialScale.Items.Add(element2);
		stiRadialScale.Items.Add(element3);
		stiRadialScale.Items.Add(element4);
		gauge.Scales.Add(stiRadialScale);
		InitializeName(gauge, report);
	}

	public static void HorizontalThermometer(StiGauge gauge, StiReport report)
	{
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Expected O, but got Unknown
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Expected O, but got Unknown
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Expected O, but got Unknown
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Expected O, but got Unknown
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Expected O, but got Unknown
		InitializeGauge(gauge, 300.0, 50.0);
		StiLinearScale stiLinearScale = new StiLinearScale
		{
			Orientation = (Orientation)0,
			RelativeHeight = 0.85f,
			Left = 0.03f,
			StartWidth = 0.17f,
			EndWidth = 0.17f,
			Maximum = 80f,
			MajorInterval = 20f,
			MinorInterval = 4f
		};
		StiLinearBar stiLinearBar = new StiLinearBar
		{
			Skin = StiLinearBarSkin.HorizontalThermometer,
			Placement = StiPlacement.Outside,
			StartWidth = 0.2f,
			EndWidth = 0.2f,
			UseRangeColor = true
		};
		StiLinearIndicatorRangeInfo element = new StiLinearIndicatorRangeInfo
		{
			Value = 0f,
			Brush = (StiBrush)new StiGradientBrush(Color.FromArgb(137, 188, 41), Color.FromArgb(111, 163, 14), 90.0)
		};
		StiLinearIndicatorRangeInfo element2 = new StiLinearIndicatorRangeInfo
		{
			Value = 40f,
			Brush = (StiBrush)new StiGradientBrush(Color.FromArgb(217, 173, 45), Color.FromArgb(222, 166, 0), 90.0)
		};
		StiLinearIndicatorRangeInfo element3 = new StiLinearIndicatorRangeInfo
		{
			Value = 65f,
			Brush = (StiBrush)new StiGradientBrush(Color.FromArgb(208, 49, 52), Color.FromArgb(186, 6, 10), 90.0)
		};
		stiLinearBar.RangeList.Add(element);
		stiLinearBar.RangeList.Add(element2);
		stiLinearBar.RangeList.Add(element3);
		StiLinearTickMarkMajor element4 = new StiLinearTickMarkMajor
		{
			Placement = StiPlacement.Overlay,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(159, 159, 159)),
			RelativeWidth = 0.01f,
			RelativeHeight = 0.18f,
			Offset = -0.1f
		};
		StiLinearTickMarkMinor element5 = new StiLinearTickMarkMinor
		{
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(159, 159, 159)),
			RelativeWidth = 0.005f,
			RelativeHeight = 0.1f,
			Offset = -0.14f
		};
		StiLinearTickLabelMajor element6 = new StiLinearTickLabelMajor
		{
			Placement = StiPlacement.Inside,
			Offset = 0.11f,
			TextBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(83, 85, 86)),
			Font = new Font("Arial", 10f)
		};
		StiStateIndicator stiStateIndicator = new StiStateIndicator
		{
			Left = 0.02f,
			Top = 0.07f,
			RelativeWidth = 0.083f,
			RelativeHeight = 0.5f
		};
		StiStateIndicatorFilter element7 = new StiStateIndicatorFilter
		{
			StartValue = 0f,
			EndValue = 40f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(112, 156, 28))
		};
		StiStateIndicatorFilter element8 = new StiStateIndicatorFilter
		{
			StartValue = 40f,
			EndValue = 65f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(225, 174, 25))
		};
		StiStateIndicatorFilter element9 = new StiStateIndicatorFilter
		{
			StartValue = 65f,
			EndValue = 100f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(194, 45, 48))
		};
		stiStateIndicator.Filters.Add(element7);
		stiStateIndicator.Filters.Add(element8);
		stiStateIndicator.Filters.Add(element9);
		stiLinearScale.Items.Add(stiLinearBar);
		stiLinearScale.Items.Add(element4);
		stiLinearScale.Items.Add(element5);
		stiLinearScale.Items.Add(element6);
		stiLinearScale.Items.Add(stiStateIndicator);
		gauge.Scales.Add(stiLinearScale);
		InitializeName(gauge, report);
	}

	public static void VerticalThermometer(StiGauge gauge, StiReport report)
	{
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Expected O, but got Unknown
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Expected O, but got Unknown
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Expected O, but got Unknown
		InitializeGauge(gauge, 50.0, 300.0);
		StiLinearScale stiLinearScale = new StiLinearScale
		{
			Orientation = (Orientation)1,
			RelativeHeight = 0.85f,
			StartWidth = 0.17f,
			EndWidth = 0.17f,
			Maximum = 80f,
			MajorInterval = 20f,
			MinorInterval = 4f
		};
		StiLinearBar stiLinearBar = new StiLinearBar
		{
			Skin = StiLinearBarSkin.VerticalThermometer,
			Placement = StiPlacement.Outside,
			StartWidth = 0.2f,
			EndWidth = 0.2f,
			UseRangeColor = true
		};
		StiLinearIndicatorRangeInfo element = new StiLinearIndicatorRangeInfo
		{
			Value = 0f,
			Brush = (StiBrush)new StiGradientBrush(Color.FromArgb(137, 188, 41), Color.FromArgb(111, 163, 14), 0.0)
		};
		StiLinearIndicatorRangeInfo element2 = new StiLinearIndicatorRangeInfo
		{
			Value = 40f,
			Brush = (StiBrush)new StiGradientBrush(Color.FromArgb(217, 173, 45), Color.FromArgb(222, 166, 0), 0.0)
		};
		StiLinearIndicatorRangeInfo element3 = new StiLinearIndicatorRangeInfo
		{
			Value = 65f,
			Brush = (StiBrush)new StiGradientBrush(Color.FromArgb(208, 49, 52), Color.FromArgb(186, 6, 10), 0.0)
		};
		stiLinearBar.RangeList.Add(element);
		stiLinearBar.RangeList.Add(element2);
		stiLinearBar.RangeList.Add(element3);
		StiLinearTickMarkMajor element4 = new StiLinearTickMarkMajor
		{
			Placement = StiPlacement.Overlay,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(159, 159, 159)),
			RelativeWidth = 0.18f,
			RelativeHeight = 0.01f,
			Offset = -0.1f
		};
		StiLinearTickMarkMinor element5 = new StiLinearTickMarkMinor
		{
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(159, 159, 159)),
			RelativeWidth = 0.1f,
			RelativeHeight = 0.005f,
			Offset = -0.14f
		};
		StiLinearTickLabelMajor element6 = new StiLinearTickLabelMajor
		{
			Placement = StiPlacement.Inside,
			Offset = 0.07f,
			TextBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(83, 85, 86)),
			Font = new Font("Arial", 10f)
		};
		StiStateIndicator stiStateIndicator = new StiStateIndicator
		{
			Left = 0.05f,
			Top = 0.9f,
			RelativeWidth = 0.5f,
			RelativeHeight = 0.083f
		};
		StiStateIndicatorFilter element7 = new StiStateIndicatorFilter
		{
			StartValue = 0f,
			EndValue = 40f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(112, 156, 28))
		};
		StiStateIndicatorFilter element8 = new StiStateIndicatorFilter
		{
			StartValue = 40f,
			EndValue = 65f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(225, 174, 25))
		};
		StiStateIndicatorFilter element9 = new StiStateIndicatorFilter
		{
			StartValue = 65f,
			EndValue = 100f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(194, 45, 48))
		};
		stiStateIndicator.Filters.Add(element7);
		stiStateIndicator.Filters.Add(element8);
		stiStateIndicator.Filters.Add(element9);
		stiLinearScale.Items.Add(stiLinearBar);
		stiLinearScale.Items.Add(element4);
		stiLinearScale.Items.Add(element5);
		stiLinearScale.Items.Add(element6);
		stiLinearScale.Items.Add(stiStateIndicator);
		gauge.Scales.Add(stiLinearScale);
		InitializeName(gauge, report);
	}

	public static void LightSpeedometer(StiGauge gauge, StiReport report)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Expected O, but got Unknown
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Expected O, but got Unknown
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Expected O, but got Unknown
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Expected O, but got Unknown
		InitializeGauge(gauge, 350.0, 350.0);
		StiRadialScale stiRadialScale = new StiRadialScale
		{
			Skin = StiRadialScaleSkin.Empty,
			Radius = 0.65f,
			StartAngle = 115f,
			SweepAngle = 310f,
			StartWidth = 0.03f,
			EndWidth = 0.03f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(81, 84, 101)),
			MajorInterval = 10f,
			MinorInterval = 2f,
			Maximum = 100f
		};
		StiRadialRangeList stiRadialRangeList = new StiRadialRangeList();
		StiRadialRange element = new StiRadialRange
		{
			StartValue = 40f,
			EndValue = 101.2f,
			StartWidth = 0.04f,
			EndWidth = 0.04f,
			Placement = StiPlacement.Inside,
			UseValuesFromTheSpecifiedRange = false,
			Brush = (StiBrush)new StiGradientBrush(Color.FromArgb(213, 227, 118), Color.FromArgb(118, 71, 24), 90.0),
			BorderBrush = (StiBrush)new StiSolidBrush(Color.White)
		};
		stiRadialRangeList.Ranges.Add(element);
		StiRadialTickMarkMajor element2 = new StiRadialTickMarkMajor
		{
			SkipValues = new StiSkipValuesExpression("100"),
			RelativeHeight = 0.03f,
			RelativeWidth = 0.055f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(81, 84, 101))
		};
		StiRadialTickMarkMinor element3 = new StiRadialTickMarkMinor
		{
			Offset = 0.04f,
			RelativeHeight = 0.01f,
			RelativeWidth = 0.03f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(81, 84, 101))
		};
		StiRadialTickMarkCustom element4 = new StiRadialTickMarkCustom
		{
			ValueObj = 100f,
			Offset = -0.057f,
			RelativeWidth = 0.08f,
			RelativeHeight = 0.03f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(81, 84, 101))
		};
		StiRadialTickLabelMajor element5 = new StiRadialTickLabelMajor
		{
			LabelRotationMode = StiLabelRotationMode.None,
			Offset = 0.14f,
			TextBrush = (StiBrush)new StiSolidBrush(Color.Black),
			Font = new Font("Arial", 13f, (FontStyle)1)
		};
		StiNeedle element6 = new StiNeedle
		{
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(250, 250, 250)),
			BorderBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(163, 163, 163)),
			BorderWidth = 1f,
			Placement = StiPlacement.Outside,
			RelativeWidth = 0.63f,
			RelativeHeight = 0.14f,
			Skin = StiNeedleSkin.SpeedometerNeedle
		};
		stiRadialScale.Items.Add(stiRadialRangeList);
		stiRadialScale.Items.Add(element2);
		stiRadialScale.Items.Add(element3);
		stiRadialScale.Items.Add(element4);
		stiRadialScale.Items.Add(element5);
		stiRadialScale.Items.Add(element6);
		gauge.Scales.Add(stiRadialScale);
		InitializeName(gauge, report);
	}

	public static void DarkSpeedometer(StiGauge gauge, StiReport report)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Expected O, but got Unknown
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Expected O, but got Unknown
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Expected O, but got Unknown
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Expected O, but got Unknown
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0545: Expected O, but got Unknown
		InitializeGauge(gauge, 350.0, 350.0);
		StiRadialScale stiRadialScale = new StiRadialScale
		{
			Skin = StiRadialScaleSkin.Empty,
			Radius = 0.65f,
			StartAngle = 115f,
			SweepAngle = 310f,
			StartWidth = 0.03f,
			EndWidth = 0.03f,
			Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(81, 84, 101)),
			MajorInterval = 10f,
			MinorInterval = 2f,
			Maximum = 200f
		};
		StiRadialRangeList stiRadialRangeList = new StiRadialRangeList();
		StiRadialRange element = new StiRadialRange
		{
			StartValue = 0f,
			EndValue = 60f,
			BorderWidth = 1f,
			StartWidth = 0.06f,
			EndWidth = 0.06f,
			Placement = StiPlacement.Overlay,
			Brush = (StiBrush)new StiGradientBrush(Color.FromArgb(101, 134, 101), Color.FromArgb(66, 228, 66), 90.0)
		};
		StiRadialRange element2 = new StiRadialRange
		{
			StartValue = 60f,
			EndValue = 100f,
			BorderWidth = 1f,
			StartWidth = 0.06f,
			EndWidth = 0.06f,
			Placement = StiPlacement.Overlay,
			Brush = (StiBrush)new StiGradientBrush(Color.FromArgb(255, 255, 0), Color.FromArgb(143, 174, 126), 90.0)
		};
		StiRadialRange element3 = new StiRadialRange
		{
			StartValue = 100f,
			EndValue = 140f,
			BorderWidth = 1f,
			StartWidth = 0.06f,
			EndWidth = 0.06f,
			Placement = StiPlacement.Overlay,
			Brush = (StiBrush)new StiGradientBrush(Color.FromArgb(255, 255, 0), Color.FromArgb(156, 156, 124), 90.0)
		};
		StiRadialRange element4 = new StiRadialRange
		{
			StartValue = 140f,
			EndValue = 200f,
			BorderWidth = 1f,
			StartWidth = 0.06f,
			EndWidth = 0.06f,
			Placement = StiPlacement.Overlay,
			Brush = (StiBrush)new StiGradientBrush(Color.FromArgb(125, 86, 80), Color.FromArgb(208, 45, 44), 90.0)
		};
		stiRadialRangeList.Ranges.Add(element);
		stiRadialRangeList.Ranges.Add(element2);
		stiRadialRangeList.Ranges.Add(element3);
		stiRadialRangeList.Ranges.Add(element4);
		StiRadialTickMarkMajor element5 = new StiRadialTickMarkMajor
		{
			Placement = StiPlacement.Overlay,
			Offset = 0.04f,
			OffsetAngle = -2f,
			RelativeWidth = 0.1f,
			RelativeHeight = 0.035f,
			Skin = StiTickMarkSkin.TriangleLeft
		};
		StiRadialTickMarkMinor element6 = new StiRadialTickMarkMinor
		{
			Placement = StiPlacement.Overlay,
			SkipMajorValues = false,
			RelativeWidth = 0.04f,
			RelativeHeight = 0.018f,
			Skin = StiTickMarkSkin.Rectangle,
			SkipIndices = new StiSkipIndicesExpression("0;4;8;12;16;20;24;28;32;36;40")
		};
		StiRadialTickLabelMajor element7 = new StiRadialTickLabelMajor
		{
			LabelRotationMode = StiLabelRotationMode.None,
			Offset = 0.05f,
			Font = new Font("Arial", 11f)
		};
		StiRadialTickMarkCustom stiRadialTickMarkCustom = new StiRadialTickMarkCustom
		{
			Placement = StiPlacement.Inside,
			Offset = 0.3f,
			RelativeHeight = 0.05f,
			RelativeWidth = 0.05f,
			Skin = StiTickMarkSkin.Ellipse
		};
		StiRadialTickMarkCustomValue element8 = new StiRadialTickMarkCustomValue(20f)
		{
			Brush = (StiBrush)new StiGradientBrush(Color.FromArgb(68, 223, 68), Color.FromArgb(0, 153, 0), 90.0)
		};
		StiRadialTickMarkCustomValue element9 = new StiRadialTickMarkCustomValue(97f)
		{
			Brush = (StiBrush)new StiGradientBrush(Color.FromArgb(255, 255, 0), Color.FromArgb(186, 169, 2), 90.0)
		};
		StiRadialTickMarkCustomValue element10 = new StiRadialTickMarkCustomValue(173f)
		{
			Brush = (StiBrush)new StiGradientBrush(Color.FromArgb(184, 29, 29), Color.FromArgb(121, 30, 30), 90.0)
		};
		stiRadialTickMarkCustom.Values.Add(element8);
		stiRadialTickMarkCustom.Values.Add(element9);
		stiRadialTickMarkCustom.Values.Add(element10);
		StiRadialTickLabelCustom stiRadialTickLabelCustom = new StiRadialTickLabelCustom
		{
			Placement = StiPlacement.Inside,
			LabelRotationMode = StiLabelRotationMode.None,
			Font = new Font("Arial", 10f)
		};
		stiRadialTickLabelCustom.Values.Add(new StiRadialTickLabelCustomValue(17f, "Safe", 0.16f));
		stiRadialTickLabelCustom.Values.Add(new StiRadialTickLabelCustomValue(102f, "Caution", 0.25f));
		stiRadialTickLabelCustom.Values.Add(new StiRadialTickLabelCustomValue(181f, "Danger", 0.06f));
		StiNeedle element11 = new StiNeedle
		{
			BorderBrush = (StiBrush)new StiSolidBrush(Color.FromArgb(153, 9, 8)),
			Brush = (StiBrush)new StiGradientBrush(Color.FromArgb(255, 198, 172), Color.FromArgb(197, 25, 19), 90.0),
			BorderWidth = 1f,
			Placement = StiPlacement.Outside,
			RelativeWidth = 0.57f,
			RelativeHeight = 0.17f,
			Skin = StiNeedleSkin.SpeedometerNeedle2
		};
		stiRadialScale.Items.Add(stiRadialRangeList);
		stiRadialScale.Items.Add(element5);
		stiRadialScale.Items.Add(element6);
		stiRadialScale.Items.Add(element7);
		stiRadialScale.Items.Add(stiRadialTickMarkCustom);
		stiRadialScale.Items.Add(stiRadialTickLabelCustom);
		stiRadialScale.Items.Add(element11);
		gauge.Scales.Add(stiRadialScale);
		InitializeName(gauge, report);
	}
}
