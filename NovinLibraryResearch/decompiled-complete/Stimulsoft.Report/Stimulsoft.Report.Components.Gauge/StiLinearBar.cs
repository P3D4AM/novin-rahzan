using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.GaugeGeoms;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge;

public class StiLinearBar : StiBarBase
{
	private class LinearColorModeHelper
	{
		public bool standardBackground;

		public int defaultIndex = -99;

		public int mixedColorIndex = -99;

		public void Reset()
		{
			standardBackground = false;
			defaultIndex = -99;
			mixedColorIndex = -99;
		}
	}

	private class LinearBarGeometryHelper
	{
		public RectangleF rect;

		public float maxWidth;

		public float minWidth;

		public bool isStartGreaterEnd;

		public bool isThisStartGreaterEnd;

		public StiLinearScale scale;

		public bool state;

		public float offset;
	}

	private LinearColorModeHelper colorModeHelper = new LinearColorModeHelper();

	private StiBrush actualBackground;

	private StiLinearRangeColorMode rangeColorMode;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiLinearBar;

	[StiSerializable]
	[DefaultValue(StiLinearBarSkin.Default)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the skin of the component rendering.")]
	public StiLinearBarSkin Skin { get; set; }

	[StiSerializable]
	[DefaultValue(StiLinearRangeColorMode.Default)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets value which indicates the mode that is used to build bar indicator background.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiLinearRangeColorMode RangeColorMode
	{
		get
		{
			return rangeColorMode;
		}
		set
		{
			rangeColorMode = value;
			OnRangeColorChanged();
		}
	}

	protected override StiBarRangeListType BarType => StiBarRangeListType.LinearBar;

	public override string LocalizeName => "LinearBar";

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		JsonExtensions.AddPropertyEnum(val, "Skin", (Enum)Skin);
		JsonExtensions.AddPropertyEnum(val, "RangeColorMode", (Enum)RangeColorMode);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "Skin"))
			{
				if (name == "RangeColorMode")
				{
					RangeColorMode = StiJsonReportObjectHelper.DeserializeEnum<StiLinearRangeColorMode>(item);
				}
			}
			else
			{
				Skin = StiJsonReportObjectHelper.DeserializeEnum<StiLinearBarSkin>(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.Value() };
		stiPropertyCollection.Add(StiPropertyCategories.Value, value);
		value = new StiPropertyObject[7]
		{
			propertiesHelper.EndWidth(),
			propertiesHelper.Offset(),
			propertiesHelper.Placement(),
			propertiesHelper.RangeColorMode(),
			propertiesHelper.Skin(),
			propertiesHelper.StartWidth(),
			propertiesHelper.UseRangeColor()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Indicator, value);
		value = new StiPropertyObject[6]
		{
			propertiesHelper.Brush(),
			propertiesHelper.BorderBrush(),
			propertiesHelper.BorderWidth(),
			propertiesHelper.EmptyBrush(),
			propertiesHelper.EmptyBorderBrush(),
			propertiesHelper.EmptyBorderWidth()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[1] { propertiesHelper.AllowApplyStyle() };
		stiPropertyCollection.Add(StiPropertyCategories.Misc, value);
		return stiPropertyCollection;
	}

	public override void ApplyStyle(IStiGaugeStyle style)
	{
		if (AllowApplyStyle)
		{
			Brush = style.Core.LinearBarBrush;
			BorderBrush = style.Core.LinearBarBorderBrush;
			base.EmptyBrush = style.Core.LinearBarEmptyBrush;
			base.EmptyBorderBrush = style.Core.LinearBarEmptyBorderBrush;
			base.StartWidth = style.Core.LinearBarStartWidth;
			base.EndWidth = style.Core.LinearBarEndWidth;
		}
	}

	protected override void OnRangeColorChanged()
	{
		colorModeHelper.Reset();
	}

	protected override void CheckActualBrushForTopGeometry()
	{
		if (base.UseRangeColor)
		{
			actualBackground = GetRangeBrush();
		}
		else
		{
			actualBackground = Brush;
		}
	}

	private StiBrush GetRangeBrush()
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		float? actualValue = GetActualValue();
		if (!actualValue.HasValue)
		{
			return null;
		}
		float valueOrDefault = actualValue.GetValueOrDefault();
		switch (RangeColorMode)
		{
		case StiLinearRangeColorMode.Default:
		{
			for (int num2 = base.RangeList.Count - 1; num2 >= 0; num2--)
			{
				StiLinearIndicatorRangeInfo stiLinearIndicatorRangeInfo2 = base.RangeList[num2] as StiLinearIndicatorRangeInfo;
				if (valueOrDefault >= stiLinearIndicatorRangeInfo2.Value && colorModeHelper.defaultIndex != num2)
				{
					colorModeHelper.defaultIndex = num2;
					if (stiLinearIndicatorRangeInfo2.Brush == null)
					{
						return (StiBrush)new StiSolidBrush(stiLinearIndicatorRangeInfo2.Color);
					}
					return stiLinearIndicatorRangeInfo2.Brush;
				}
			}
			break;
		}
		case StiLinearRangeColorMode.MixedColor:
		{
			for (int num = base.RangeList.Count - 1; num >= 0; num--)
			{
				StiLinearIndicatorRangeInfo stiLinearIndicatorRangeInfo = base.RangeList[num] as StiLinearIndicatorRangeInfo;
				if (valueOrDefault >= stiLinearIndicatorRangeInfo.Value)
				{
					if (colorModeHelper.mixedColorIndex == num)
					{
						break;
					}
					colorModeHelper.mixedColorIndex = num;
					List<Color> list = new List<Color>();
					for (int i = 0; i <= num; i++)
					{
						list.Add(((StiLinearIndicatorRangeInfo)base.RangeList[i]).Color);
					}
					return (StiBrush)new StiSolidBrush(StiMixedColorHelper.ColorMixed(list));
				}
			}
			break;
		}
		}
		return null;
	}

	public override StiGaugeElement CreateNew()
	{
		return new StiLinearBar();
	}

	protected internal override void InteractiveClick(RectangleF rect, Point p)
	{
	}

	private float GetValueObj()
	{
		if (base.Scale.Gauge.Report == null)
		{
			return base.ValueObj;
		}
		return GetActualValue() ?? 0f;
	}

	protected internal override void DrawElement(StiGaugeContextPainter context)
	{
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Invalid comparison between Unknown and I4
		if (base.Scale == null)
		{
			return;
		}
		switch (Skin)
		{
		case StiLinearBarSkin.HorizontalThermometer:
			DrawHorizontalThermometer(context);
			break;
		case StiLinearBarSkin.VerticalThermometer:
			DrawVerticalThermometer(context);
			break;
		}
		RectangleF rect = RectangleF.Empty;
		StiGraphicsPathLinesGaugeGeom stiGraphicsPathLinesGaugeGeom = base.Scale.barGeometry.DrawGeometry(context, base.Scale.Minimum, base.Scale.Maximum, base.StartWidth, base.EndWidth, base.Offset, Placement, ref rect, returnOnlyRect: false);
		if (stiGraphicsPathLinesGaugeGeom != null)
		{
			StiGraphicsPathGaugeGeom stiGraphicsPathGaugeGeom = new StiGraphicsPathGaugeGeom(rect, stiGraphicsPathLinesGaugeGeom.Points[0], base.EmptyBrush, base.EmptyBorderBrush, base.EmptyBorderWidth);
			stiGraphicsPathGaugeGeom.Geoms.Add(stiGraphicsPathLinesGaugeGeom);
			stiGraphicsPathGaugeGeom.AddGraphicsPathCloseFigureGaugeGeom();
			context.AddGraphicsPathGaugeGeom(stiGraphicsPathGaugeGeom);
		}
		float value = GetValueObj();
		LinearBarGeometryHelper geometryHelperForTopIndicator = GetGeometryHelperForTopIndicator(value);
		RectangleF rect2;
		StiGraphicsPathLinesGaugeGeom topGeometry = GetTopGeometry(geometryHelperForTopIndicator, out rect2);
		if (topGeometry != null)
		{
			CheckActualBrushForTopGeometry();
			StiGraphicsPathGaugeGeom stiGraphicsPathGaugeGeom2 = new StiGraphicsPathGaugeGeom(rect2, topGeometry.Points[0], actualBackground, BorderBrush, BorderWidth);
			stiGraphicsPathGaugeGeom2.Geoms.Add(topGeometry);
			stiGraphicsPathGaugeGeom2.AddGraphicsPathCloseFigureGaugeGeom();
			int num = 0;
			int num2 = 1;
			int num3 = 1;
			int num4 = 1;
			float num5 = rect.X;
			float num6 = rect.Y + rect.Height / 2f;
			if (base.Scale is StiLinearScale stiLinearScale && (int)stiLinearScale.Orientation == 1)
			{
				num = 1;
				num2 = 1;
				num3 = 0;
				num4 = 1;
				num5 = rect.X + rect.Width / 2f;
				num6 = rect.Y + rect.Height;
			}
			if (base.Scale.Gauge.IsAnimation)
			{
				stiGraphicsPathGaugeGeom2.Animation = new StiScaleAnimation(num, num2, num3, num4, num5, num6, StiGaugeHelper.GlobalDurationElement, TimeSpan.Zero);
			}
			context.AddGraphicsPathGaugeGeom(stiGraphicsPathGaugeGeom2);
		}
	}

	private void DrawHorizontalThermometer(StiGaugeContextPainter context)
	{
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		//IL_00f6: Expected O, but got Unknown
		RectangleF rect = RectangleF.Empty;
		base.Scale.barGeometry.DrawGeometry(context, base.Scale.Minimum, base.Scale.Maximum, base.StartWidth, base.EndWidth, base.Offset, Placement, ref rect, returnOnlyRect: false);
		rect.Y -= 2f;
		rect.Height += 4f;
		rect.X -= 3f;
		rect.Width += 3f;
		StiGraphicsPathGaugeGeom stiGraphicsPathGaugeGeom = new StiGraphicsPathGaugeGeom(rect, rect.Location, (StiBrush)new StiGradientBrush(Color.FromArgb(225, 230, 233), Color.FromArgb(242, 243, 244), 90.0), (StiBrush)new StiSolidBrush(Color.FromArgb(157, 157, 157)), 0.4f);
		stiGraphicsPathGaugeGeom.AddGraphicsPathLineGaugeGeom(rect.Location, new PointF(rect.Right, rect.Top));
		stiGraphicsPathGaugeGeom.AddGraphicsPathArcGaugeGeom(rect.Right, rect.Top, 4f, rect.Height, 270f, 180f);
		stiGraphicsPathGaugeGeom.AddGraphicsPathLineGaugeGeom(new PointF(rect.Right, rect.Bottom), new PointF(rect.Left, rect.Bottom));
		stiGraphicsPathGaugeGeom.AddGraphicsPathCloseFigureGaugeGeom();
		context.AddGraphicsPathGaugeGeom(stiGraphicsPathGaugeGeom);
	}

	private void DrawVerticalThermometer(StiGaugeContextPainter context)
	{
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		//IL_00f6: Expected O, but got Unknown
		RectangleF rect = RectangleF.Empty;
		base.Scale.barGeometry.DrawGeometry(context, base.Scale.Minimum, base.Scale.Maximum, base.StartWidth, base.EndWidth, base.Offset, Placement, ref rect, returnOnlyRect: false);
		rect.X -= 2f;
		rect.Width += 4f;
		rect.Y -= 3f;
		rect.Height += 3f;
		StiGraphicsPathGaugeGeom stiGraphicsPathGaugeGeom = new StiGraphicsPathGaugeGeom(rect, rect.Location, (StiBrush)new StiGradientBrush(Color.FromArgb(225, 230, 233), Color.FromArgb(242, 243, 244), 90.0), (StiBrush)new StiSolidBrush(Color.FromArgb(157, 157, 157)), 0.4f);
		stiGraphicsPathGaugeGeom.AddGraphicsPathArcGaugeGeom(rect.Left, rect.Top - 4f, rect.Width, 4f, 180f, 180f);
		stiGraphicsPathGaugeGeom.AddGraphicsPathLineGaugeGeom(new PointF(rect.Right, rect.Top), new PointF(rect.Right, rect.Bottom));
		stiGraphicsPathGaugeGeom.AddGraphicsPathLineGaugeGeom(new PointF(rect.Right, rect.Bottom), new PointF(rect.Left, rect.Bottom));
		stiGraphicsPathGaugeGeom.AddGraphicsPathCloseFigureGaugeGeom();
		context.AddGraphicsPathGaugeGeom(stiGraphicsPathGaugeGeom);
	}

	private LinearBarGeometryHelper GetGeometryHelperForTopIndicator(float value)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		StiLinearScale stiLinearScale = base.Scale as StiLinearScale;
		LinearBarGeometryHelper linearBarGeometryHelper = new LinearBarGeometryHelper();
		SizeF size = base.Scale.barGeometry.Size;
		RectangleF rectGeometry = base.Scale.barGeometry.RectGeometry;
		float num = StiMathHelper.MaxMinusMin(base.Scale.StartWidth, base.Scale.EndWidth);
		num /= 2f;
		num = (((int)stiLinearScale.Orientation == 0) ? (num * size.Height) : (num * size.Width));
		RectangleF rect = (((int)stiLinearScale.Orientation == 0) ? new RectangleF(rectGeometry.X, rectGeometry.Y + num, rectGeometry.Width, rectGeometry.Height - 2f * num) : new RectangleF(rectGeometry.X + num, rectGeometry.Y, rectGeometry.Width - 2f * num, rectGeometry.Height));
		float num2 = ((base.Scale.Minimum > base.Scale.Maximum) ? base.Scale.Maximum : base.Scale.Minimum);
		float num3 = Math.Abs(base.Scale.Minimum) + Math.Abs(base.Scale.Maximum);
		float num4 = StiMathHelper.Length(num2, value);
		if (num4 < 0f)
		{
			num4 = 0f;
		}
		else if (num4 > num3)
		{
			num4 = num3;
		}
		float num5 = num4 / num3;
		float? num6 = null;
		float? num7 = null;
		RectangleF rectangleF = new RectangleF(0f, 0f, 0f, 0f);
		bool flag = base.Scale.StartWidth > base.Scale.EndWidth;
		bool flag2 = base.StartWidth > base.EndWidth;
		float num8 = (((int)stiLinearScale.Orientation == 0) ? (rectGeometry.Width * num5) : (rectGeometry.Height * num5));
		float num9 = 0f;
		float num10 = 0f;
		float num11 = 0f;
		float num12 = 0f;
		if (base.StartWidth > base.EndWidth)
		{
			num9 = base.StartWidth;
			num10 = base.EndWidth;
		}
		else
		{
			num10 = base.StartWidth;
			num9 = base.EndWidth;
		}
		float num13 = num9 - num10;
		if ((int)stiLinearScale.Orientation == 0)
		{
			num11 = num9 * size.Height;
			num12 = num10 * size.Height;
			if (base.Scale.IsReversed)
			{
				if (flag)
				{
					switch (Placement)
					{
					case StiPlacement.Outside:
						if (flag2)
						{
							rectangleF = new RectangleF(rectGeometry.X + (rect.Width - num8), rect.Y - num - num11, num8, num + num11 - num * (1f - num5));
							num7 = rectangleF.Height - (num + num11 - num12) * num5;
						}
						else if (num11 > num + num12)
						{
							float num14 = (num11 - num - num12) * (1f - num5);
							rectangleF = new RectangleF(rect.X + (rect.Width - num8), rect.Y - num11 + num14, num8, num11 - num * (1f - num5) - num14);
							linearBarGeometryHelper.offset = num * num5;
						}
						else
						{
							float num14 = num * (1f - num5);
							rectangleF = new RectangleF(rect.X + (rect.Width - num8), rect.Y - num - num12, num8, num + num12 - num14);
							num6 = rectangleF.Height - (num + num12 - num11) * num5;
							linearBarGeometryHelper.state = true;
						}
						break;
					case StiPlacement.Overlay:
						if (flag2)
						{
							rectangleF = new RectangleF(rect.X + (rect.Width - num8), StiRectangleHelper.CenterY(rect) - num11 / 2f, num8, num11);
							num7 = rectangleF.Height - (num11 - num12) * num5;
						}
						else
						{
							float num14 = (num11 - num12) / 2f * (1f - num5);
							rectangleF = new RectangleF(rect.X + (rect.Width - num8), StiRectangleHelper.CenterY(rect) - num11 / 2f + num14, num8, num11 - 2f * num14);
						}
						break;
					default:
						if (flag2)
						{
							float num14 = num * (1f - num5);
							rectangleF = new RectangleF(rect.X + (rect.Width - num8), rect.Bottom + num14, num8, num + num11 - num14);
							num7 = rectangleF.Height - (num + num11 - num12) * num5;
						}
						else if (num11 > num + num12)
						{
							float num14 = num * (1f - num5);
							float num15 = (num11 - num - num12) * (1f - num5);
							rectangleF = new RectangleF(rect.X + (rect.Width - num8), rect.Bottom + num14, num8, num11 - num14 - num15);
							linearBarGeometryHelper.offset = num11 - num - num12 - num15;
						}
						else
						{
							float num14 = num * (1f - num5);
							rectangleF = new RectangleF(rect.X + (rect.Width - num8), rect.Bottom + num14, num8, num + num12 - num14);
							num6 = rectangleF.Height - (num + num12 - num11) * num5;
							linearBarGeometryHelper.state = true;
						}
						break;
					}
				}
				else
				{
					switch (Placement)
					{
					case StiPlacement.Outside:
						if (flag2)
						{
							if (num11 > num + num12)
							{
								rectangleF = new RectangleF(rect.X + (rect.Width - num8), rect.Y - num11, num8, num11);
								linearBarGeometryHelper.offset = (num11 - num - num12) * num5;
								num7 = rectangleF.Height - linearBarGeometryHelper.offset - num * num5;
							}
							else
							{
								float num14 = (num + num12 - num11) * (1f - num5);
								rectangleF = new RectangleF(rectGeometry.X + (rectGeometry.Width - num8), rectGeometry.Y - num12 + num14, num8, num + num12 - num14);
								num7 = rectangleF.Height - num * num5;
								linearBarGeometryHelper.state = true;
							}
						}
						else
						{
							float num14 = (num + num11 - num12) * (1f - num5);
							rectangleF = new RectangleF(rectGeometry.X + (rectGeometry.Width - num8), rectGeometry.Y - num11 + num14, num8, num11 + num - num14);
							num6 = rectangleF.Height - num * num5;
						}
						break;
					case StiPlacement.Overlay:
						if (flag2)
						{
							rectangleF = new RectangleF(rect.X + (rectGeometry.Width - num8), StiRectangleHelper.CenterY(rect) - num11 / 2f, num8, num11);
							num7 = rectangleF.Height - 2f * ((num11 - num12) / 2f * num5);
						}
						else
						{
							float num14 = (num11 - num12) / 2f * (1f - num5);
							rectangleF = new RectangleF(rect.X + (rectGeometry.Width - num8), StiRectangleHelper.CenterY(rect) - num11 / 2f + num14, num8, num11 - 2f * num14);
						}
						break;
					default:
						if (flag2)
						{
							if (num11 > num + num12)
							{
								rectangleF = new RectangleF(rect.X + (rectGeometry.Width - num8), rect.Bottom, num8, num11);
								linearBarGeometryHelper.offset = num * num5;
								num7 = rectangleF.Height - (num11 - num - num12) * num5 - linearBarGeometryHelper.offset;
							}
							else
							{
								float num14 = (num + num12 - num11) * num5;
								rectangleF = new RectangleF(rect.X + (rectGeometry.Width - num8), rect.Bottom, num8, num11 + num14);
								num7 = rectangleF.Height - num * num5;
								linearBarGeometryHelper.state = true;
							}
						}
						else
						{
							float num14 = (num + num11 - num12) * (1f - num5);
							rectangleF = new RectangleF(rect.X + (rectGeometry.Width - num8), rect.Bottom, num8, num + num11 - num14);
							num6 = rectangleF.Height - num * num5;
						}
						break;
					}
				}
			}
			else if (flag)
			{
				switch (Placement)
				{
				case StiPlacement.Outside:
					if (flag2)
					{
						rectangleF = new RectangleF(rectGeometry.X, rectGeometry.Y - num11, num8, num11 + num * num5);
						num7 = rectangleF.Height - (num + num11 - num12) * num5;
					}
					else
					{
						rectangleF = new RectangleF(rectGeometry.X, rectGeometry.Y - num12, num8, num + num12 - num * (1f - num5));
						num6 = rectangleF.Height - (num + num12 - num11) * num5;
					}
					break;
				case StiPlacement.Overlay:
					if (flag2)
					{
						rectangleF = new RectangleF(rect.X, StiRectangleHelper.CenterY(rect) - num11 / 2f, num8, num11);
						num7 = num12 + (num11 - num12) / 2f * num5;
					}
					else
					{
						float num14 = (num11 - num12) / 2f * (1f - num5);
						rectangleF = new RectangleF(rect.X, StiRectangleHelper.CenterY(rect) - num11 / 2f + num14, num8, num11 - 2f * num14);
					}
					break;
				default:
					if (flag2)
					{
						float num14 = num * num5;
						rectangleF = new RectangleF(rectGeometry.X, rectGeometry.Bottom - num14, num8, num14 + num11);
						num7 = rectangleF.Height - (num + num11 - num12) * num5;
					}
					else if (num11 > num + num12)
					{
						float num14 = num * (1f - num5);
						linearBarGeometryHelper.offset = num - num14;
						rectangleF = new RectangleF(rect.X, rect.Bottom + num14, num8, num11 - num14 - (num11 - num - num12) * (1f - num5));
					}
					else
					{
						float num14 = num * num5;
						rectangleF = new RectangleF(rectGeometry.X, rectGeometry.Bottom - num14, num8, num14 + num12);
						num6 = rectangleF.Height - (num + num12 - num11) * num5;
						linearBarGeometryHelper.state = true;
					}
					break;
				}
			}
			else
			{
				float num16 = (num + num11 - num12) * num5 + num12;
				switch (Placement)
				{
				case StiPlacement.Outside:
					if (flag2)
					{
						if (num11 > num + num12)
						{
							rectangleF = new RectangleF(rect.X, rect.Y - num11, num8, num11);
							linearBarGeometryHelper.offset = (rectangleF.Height - num - num12) * num5;
							num7 = rectangleF.Height - linearBarGeometryHelper.offset - num * num5;
						}
						else
						{
							float num14 = (num + num12 - num11) * num5;
							rectangleF = new RectangleF(rect.X, rect.Y - num11 - num14, num8, num11 + num14);
							num7 = rectangleF.Height - num * num5;
							linearBarGeometryHelper.state = true;
						}
					}
					else
					{
						rectangleF = new RectangleF(rect.X, rect.Y - num16, num8, num16);
						num6 = rectangleF.Height - num * num5;
					}
					break;
				case StiPlacement.Overlay:
				{
					float num14 = (num11 - num12) / 2f * (1f - num5);
					if (flag2)
					{
						rectangleF = new RectangleF(rectGeometry.X, StiRectangleHelper.CenterY(rect) - num11 / 2f, num8, num11);
						num7 = num12 + (num11 - num12) / 2f * (1f - num5);
					}
					else
					{
						rectangleF = new RectangleF(rectGeometry.X, StiRectangleHelper.CenterY(rect) - num11 / 2f + num14, num8, num11 - 2f * num14);
					}
					break;
				}
				default:
					if (flag2)
					{
						if (num11 > num + num12)
						{
							rectangleF = new RectangleF(rect.X, rect.Bottom, num8, num11);
							linearBarGeometryHelper.offset = num * num5;
							num7 = rectangleF.Height - (num11 - num - num12) * num5 - linearBarGeometryHelper.offset;
						}
						else
						{
							float num14 = (num + num12 - num11) * num5;
							rectangleF = new RectangleF(rect.X, rect.Bottom, num8, num11 + num14);
							num7 = rectangleF.Height - num * num5;
							linearBarGeometryHelper.state = true;
						}
					}
					else
					{
						rectangleF = new RectangleF(rect.X, rect.Bottom, num8, num16);
						num6 = num12 + (num11 - num12) * num5;
					}
					break;
				}
			}
		}
		else
		{
			num11 = num9 * size.Width;
			num12 = num10 * size.Width;
			if (base.Scale.IsReversed)
			{
				if (flag)
				{
					switch (Placement)
					{
					case StiPlacement.Outside:
					{
						float num17 = num * (1f - num5);
						if (flag2)
						{
							rectangleF = new RectangleF(rect.X - num - num11, rect.Y, num + num11 - num17, num8);
							num7 = rectangleF.Width - (num + num11 - num12) * num5;
						}
						else if (num11 > num + num12)
						{
							float num19 = (num11 - num12 - num) * (1f - num5);
							rectangleF = new RectangleF(rect.X - num11 + num19, rect.Y, num11 - num17 - num19, num8);
							linearBarGeometryHelper.offset = num * num5;
						}
						else
						{
							rectangleF = new RectangleF(rectGeometry.X - num12, rectGeometry.Y, num + num12 - num17, num8);
							num6 = rectangleF.Width - (num + num12 - num11) * num5;
							linearBarGeometryHelper.state = true;
						}
						break;
					}
					case StiPlacement.Overlay:
						if (flag2)
						{
							rectangleF = new RectangleF(StiRectangleHelper.CenterX(rect) - num11 / 2f, rect.Y, num11, num8);
							linearBarGeometryHelper.offset = (num11 - num12) / 2f * num5;
						}
						else
						{
							float num17 = (num11 - num12) / 2f * (1f - num5);
							rectangleF = new RectangleF(StiRectangleHelper.CenterX(rect) - num11 / 2f + num17, rect.Y, num11 - 2f * num17, num8);
						}
						break;
					default:
					{
						float num17 = num * (1f - num5);
						if (flag2)
						{
							rectangleF = new RectangleF(rect.Right + num17, rect.Y, num + num11 - num17, num8);
							num7 = rectangleF.Width - (num + num11 - num12) * num5;
						}
						else if (num11 > num + num12)
						{
							float num18 = (num11 - num - num12) * (1f - num5);
							rectangleF = new RectangleF(rect.Right + num17, rect.Y, num11 - num17 - num18, num8);
							num6 = (num11 - num - num12) * num5;
							linearBarGeometryHelper.offset = num * num5;
						}
						else
						{
							rectangleF = new RectangleF(rect.Right + num17, rect.Y, num + num12 - num17, num8);
							num6 = rectangleF.Width - (num + num12 - num11) * num5;
							linearBarGeometryHelper.state = true;
						}
						break;
					}
					}
				}
				else
				{
					switch (Placement)
					{
					case StiPlacement.Outside:
						if (flag2)
						{
							if (num11 > num + num12)
							{
								rectangleF = new RectangleF(rect.X - num11, rect.Y, num11, num8);
								linearBarGeometryHelper.offset = num * num5;
								num7 = rectangleF.Width - linearBarGeometryHelper.offset - (num11 - num2 - num) * (1f - num5);
							}
							else
							{
								float num17 = (num + num12 - num11) * (1f - num5);
								rectangleF = new RectangleF(rect.X - num - num12 + num17, rect.Y, num + num12 - num17, num8);
								num7 = rectangleF.Width - num * num5;
								linearBarGeometryHelper.state = true;
							}
						}
						else
						{
							float num17 = (num + num11 - num12) * (1f - num5);
							rectangleF = new RectangleF(rect.X - num - num11 + num17, rect.Y, num + num11 - num17, num8);
							num6 = rectangleF.Width - num * num5;
						}
						break;
					case StiPlacement.Overlay:
						if (flag2)
						{
							rectangleF = new RectangleF(StiRectangleHelper.CenterX(rect) - num11 / 2f, rect.Y, num11, num8);
							linearBarGeometryHelper.offset = (num11 - num12) / 2f * num5;
						}
						else
						{
							float num17 = (num11 - num12) / 2f * (1f - num5);
							rectangleF = new RectangleF(StiRectangleHelper.CenterX(rect) - num11 / 2f + num17, rect.Y, num11 - 2f * num17, num8);
						}
						break;
					default:
						if (flag2)
						{
							if (num11 > num + num12)
							{
								rectangleF = new RectangleF(rect.Right, rect.Y, num11, num8);
								linearBarGeometryHelper.offset = num * num5;
								num7 = rectangleF.Width - linearBarGeometryHelper.offset - (num11 - num12 - num) * num5;
							}
							else
							{
								float num17 = (num + num12 - num11) * num5;
								rectangleF = new RectangleF(rect.Right, rect.Y, num + num12 - num17, num8);
								num7 = rectangleF.Width - num * num5;
								linearBarGeometryHelper.state = true;
							}
						}
						else
						{
							float num17 = (num + num11 - num12) * (1f - num5);
							rectangleF = new RectangleF(rect.Right, rectGeometry.Y, num + num11 - num17, num8);
							num6 = rectangleF.Width - num * num5;
						}
						break;
					}
				}
			}
			else if (flag)
			{
				switch (Placement)
				{
				case StiPlacement.Outside:
				{
					float num17;
					if (flag2)
					{
						num17 = num * (1f - num5);
						rectangleF = new RectangleF(rect.X - num - num11, rect.Y + (rectGeometry.Height - num8), num + num11 - num17, num8);
						num7 = rectangleF.Width - (num + num11 - num12) * num5;
						break;
					}
					num17 = num * (1f - num5);
					if (num11 > num + num12)
					{
						float num21 = (num11 - num - num12) * (1f - num5);
						rectangleF = new RectangleF(rect.X - num11 + num21, rect.Y + (rectGeometry.Height - num8), num11 - num17 - num21, num8);
						linearBarGeometryHelper.offset = num * num5;
					}
					else
					{
						rectangleF = new RectangleF(rect.X - num - num12, rect.Y + (rectGeometry.Height - num8), num + num12 - num17, num8);
						num6 = rectangleF.Width - (num + num12 - num11) * num5;
						linearBarGeometryHelper.state = true;
					}
					break;
				}
				case StiPlacement.Overlay:
					if (flag2)
					{
						rectangleF = new RectangleF(StiRectangleHelper.CenterX(rect) - num11 / 2f, rect.Y + (rectGeometry.Height - num8), num11, num8);
						linearBarGeometryHelper.offset = (num11 - num12) / 2f * num5;
					}
					else
					{
						float num17 = (num11 - num12) / 2f * (1f - num5);
						rectangleF = new RectangleF(StiRectangleHelper.CenterX(rect) - num11 / 2f + num17, rect.Y + (rectGeometry.Height - num8), num11 - 2f * num17, num8);
					}
					break;
				default:
				{
					float num17 = num * (1f - num5);
					if (flag2)
					{
						rectangleF = new RectangleF(rect.Right + num17, rect.Y + (rectGeometry.Height - num8), num11 + num - num17, num8);
						num7 = rectangleF.Width - (num + num11 - num12) * num5;
					}
					else if (num11 >= num + num12)
					{
						float num20 = (num11 - num - num12) * (1f - num5);
						rectangleF = new RectangleF(rect.Right + num17, rect.Y + (rectGeometry.Height - num8), num11 - num17 - num20, num8);
						linearBarGeometryHelper.offset = (num11 - num - num12) * num5;
					}
					else
					{
						rectangleF = new RectangleF(rect.Right + num17, rect.Y + (rectGeometry.Height - num8), num + num12 - num17, num8);
						num6 = rectangleF.Width - (num + num12 - num11) * num5;
						linearBarGeometryHelper.state = true;
					}
					break;
				}
				}
			}
			else
			{
				switch (Placement)
				{
				case StiPlacement.Outside:
					if (flag2)
					{
						if (num11 > num + num12)
						{
							rectangleF = new RectangleF(rect.X - num11, rect.Y + (rectGeometry.Height - num8), num11, num8);
							linearBarGeometryHelper.offset = num * num5;
							float num17 = (num11 - num12 - num) * num5;
							num7 = rectangleF.Width - linearBarGeometryHelper.offset - num17;
						}
						else
						{
							float num17 = (num + num12 - num11) * (1f - num5);
							rectangleF = new RectangleF(rectGeometry.X - num12 + num17, rectGeometry.Y + (rectGeometry.Height - num8), num + num12 - num17, num8);
							num7 = rectangleF.Width - num * num5;
							linearBarGeometryHelper.state = true;
						}
					}
					else
					{
						float num17 = (num + num11 - num12) * (1f - num5);
						rectangleF = new RectangleF(rectGeometry.X - num11 + num17, rectGeometry.Y + (rectGeometry.Height - num8), num11 + num - num17, num8);
						num6 = rectangleF.Width - num * num5;
					}
					break;
				case StiPlacement.Overlay:
					if (flag2)
					{
						rectangleF = new RectangleF(StiRectangleHelper.CenterX(rect) - num11 / 2f, rectGeometry.Y + (rectGeometry.Height - num8), num11, num8);
						linearBarGeometryHelper.offset = (num11 - num12) * num5 / 2f;
					}
					else
					{
						float num17 = (num11 - num12) / 2f * (1f - num5);
						rectangleF = new RectangleF(StiRectangleHelper.CenterX(rect) - num11 / 2f + num17, rectGeometry.Y + (rectGeometry.Height - num8), num11 - 2f * num17, num8);
					}
					break;
				default:
					if (flag2)
					{
						if (num11 > num + num12)
						{
							rectangleF = new RectangleF(rect.Right, rectGeometry.Y + (rectGeometry.Height - num8), num11, num8);
							linearBarGeometryHelper.offset = num * num5;
							num7 = rectangleF.Width - linearBarGeometryHelper.offset - (num11 - num - num12) * num5;
						}
						else
						{
							float num17 = (num + num12 - num11) * num5;
							rectangleF = new RectangleF(rect.Right, rectGeometry.Y + (rectGeometry.Height - num8), num + num12 - num17, num8);
							num7 = rectangleF.Width - num * num5;
							linearBarGeometryHelper.state = true;
						}
					}
					else
					{
						float num17 = (num + num11 - num12) * (1f - num5);
						rectangleF = new RectangleF(rect.Right, rectGeometry.Y + (rectGeometry.Height - num8), num + num11 - num17, num8);
						num6 = rectangleF.Width - num * num5;
					}
					break;
				}
			}
		}
		linearBarGeometryHelper.rect = rectangleF;
		linearBarGeometryHelper.isStartGreaterEnd = flag;
		linearBarGeometryHelper.isThisStartGreaterEnd = flag2;
		linearBarGeometryHelper.scale = stiLinearScale;
		linearBarGeometryHelper.maxWidth = ((!num6.HasValue) ? num11 : num6.Value);
		linearBarGeometryHelper.minWidth = ((!num7.HasValue) ? num12 : num7.Value);
		return linearBarGeometryHelper;
	}

	private StiGraphicsPathLinesGaugeGeom GetTopGeometry(LinearBarGeometryHelper helper, out RectangleF rect)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e93: Unknown result type (might be due to invalid IL or missing references)
		rect = new RectangleF(0f, 0f, helper.rect.Width, helper.rect.Height);
		PointF[] array = new PointF[4];
		if ((int)helper.scale.Orientation == 0)
		{
			if (helper.scale.IsReversed)
			{
				if (helper.isStartGreaterEnd)
				{
					if (helper.isThisStartGreaterEnd)
					{
						switch (Placement)
						{
						case StiPlacement.Outside:
							array[0] = new PointF(0f, rect.Height - helper.minWidth);
							array[1] = new PointF(rect.Width, 0f);
							array[2] = new PointF(rect.Width, helper.maxWidth);
							array[3] = new PointF(0f, rect.Height);
							break;
						case StiPlacement.Overlay:
							array[0] = new PointF(0f, (rect.Height - helper.minWidth) / 2f);
							array[1] = new PointF(rect.Width, 0f);
							array[2] = new PointF(rect.Width, rect.Height);
							array[3] = new PointF(0f, rect.Height - (rect.Height - helper.minWidth) / 2f);
							break;
						default:
							array[0] = new PointF(0f, 0f);
							array[1] = new PointF(rect.Width, rect.Height - helper.maxWidth);
							array[2] = new PointF(rect.Width, rect.Height);
							array[3] = new PointF(0f, helper.minWidth);
							break;
						}
					}
					else
					{
						switch (Placement)
						{
						case StiPlacement.Outside:
							if (helper.state)
							{
								array[0] = new PointF(0f, rect.Height - helper.maxWidth);
								array[1] = new PointF(rect.Width, 0f);
								array[2] = new PointF(rect.Width, helper.minWidth);
								array[3] = new PointF(0f, rect.Height);
							}
							else
							{
								array[0] = new PointF(0f, 0f);
								array[1] = new PointF(rect.Width, rect.Height - helper.offset - helper.minWidth);
								array[2] = new PointF(rect.Width, rect.Height - helper.offset);
								array[3] = new PointF(0f, rect.Height);
							}
							break;
						case StiPlacement.Overlay:
							array[0] = new PointF(0f, 0f);
							array[1] = new PointF(rect.Width, StiRectangleHelper.CenterY(rect) - helper.minWidth / 2f);
							array[2] = new PointF(rect.Width, StiRectangleHelper.CenterY(rect) + helper.minWidth / 2f);
							array[3] = new PointF(0f, rect.Height);
							break;
						default:
							if (helper.state)
							{
								array[0] = new PointF(0f, 0f);
								array[1] = new PointF(rect.Width, rect.Height - helper.minWidth);
								array[2] = new PointF(rect.Width, rect.Height);
								array[3] = new PointF(0f, helper.maxWidth);
							}
							else
							{
								array[0] = new PointF(0f, 0f);
								array[1] = new PointF(rect.Width, rect.Height - helper.offset - helper.minWidth);
								array[2] = new PointF(rect.Width, rect.Height - helper.offset);
								array[3] = new PointF(0f, rect.Height);
							}
							break;
						}
					}
				}
				else if (helper.isThisStartGreaterEnd)
				{
					switch (Placement)
					{
					case StiPlacement.Outside:
						if (helper.state)
						{
							array[0] = new PointF(0f, 0f);
							array[1] = new PointF(rect.Width, rect.Height - helper.maxWidth);
							array[2] = new PointF(rect.Width, rect.Height);
							array[3] = new PointF(0f, helper.minWidth);
						}
						else
						{
							array[0] = new PointF(0f, helper.offset);
							array[1] = new PointF(rect.Width, 0f);
							array[2] = new PointF(rect.Width, rect.Height);
							array[3] = new PointF(0f, helper.minWidth + helper.offset);
						}
						break;
					case StiPlacement.Overlay:
						array[0] = new PointF(0f, StiRectangleHelper.CenterY(rect) - helper.minWidth / 2f);
						array[1] = new PointF(rect.Width, 0f);
						array[2] = new PointF(rect.Width, rect.Height);
						array[3] = new PointF(0f, StiRectangleHelper.CenterY(rect) + helper.minWidth / 2f);
						break;
					default:
						if (helper.state)
						{
							array[0] = new PointF(0f, rect.Height - helper.minWidth);
							array[1] = new PointF(rect.Width, 0f);
							array[2] = new PointF(rect.Width, helper.maxWidth);
							array[3] = new PointF(0f, rect.Height);
						}
						else
						{
							array[0] = new PointF(0f, helper.offset);
							array[1] = new PointF(rect.Width, 0f);
							array[2] = new PointF(rect.Width, rect.Height);
							array[3] = new PointF(0f, helper.offset + helper.minWidth);
						}
						break;
					}
				}
				else
				{
					switch (Placement)
					{
					case StiPlacement.Outside:
						array[0] = new PointF(0f, 0f);
						array[1] = new PointF(rect.Width, rect.Height - helper.minWidth);
						array[2] = new PointF(rect.Width, rect.Height);
						array[3] = new PointF(0f, helper.maxWidth);
						break;
					case StiPlacement.Overlay:
						array[0] = new PointF(0f, 0f);
						array[1] = new PointF(rect.Width, StiRectangleHelper.CenterY(rect) - helper.minWidth / 2f);
						array[2] = new PointF(rect.Width, StiRectangleHelper.CenterY(rect) + helper.minWidth / 2f);
						array[3] = new PointF(0f, rect.Height);
						break;
					default:
						array[0] = new PointF(0f, rect.Height - helper.maxWidth);
						array[1] = new PointF(rect.Width, 0f);
						array[2] = new PointF(rect.Width, helper.minWidth);
						array[3] = new PointF(0f, rect.Height);
						break;
					}
				}
			}
			else if (helper.isStartGreaterEnd)
			{
				if (helper.isThisStartGreaterEnd)
				{
					switch (Placement)
					{
					case StiPlacement.Outside:
						array[0] = new PointF(0f, 0f);
						array[1] = new PointF(rect.Width, rect.Height - helper.minWidth);
						array[2] = new PointF(rect.Width, rect.Height);
						array[3] = new PointF(0f, helper.maxWidth);
						break;
					case StiPlacement.Overlay:
						array[0] = new PointF(0f, 0f);
						array[1] = new PointF(rect.Width, StiRectangleHelper.CenterY(rect) - helper.minWidth / 2f);
						array[2] = new PointF(rect.Width, StiRectangleHelper.CenterY(rect) + helper.minWidth / 2f);
						array[3] = new PointF(0f, rect.Height);
						break;
					default:
						array[0] = new PointF(0f, rect.Height - helper.maxWidth);
						array[1] = new PointF(rect.Width, 0f);
						array[2] = new PointF(rect.Width, helper.minWidth);
						array[3] = new PointF(0f, rect.Height);
						break;
					}
				}
				else
				{
					switch (Placement)
					{
					case StiPlacement.Outside:
						array[0] = new PointF(0f, 0f);
						array[1] = new PointF(rect.Width, rect.Height - helper.maxWidth);
						array[2] = new PointF(rect.Width, rect.Height);
						array[3] = new PointF(0f, helper.minWidth);
						break;
					case StiPlacement.Overlay:
						array[0] = new PointF(0f, StiRectangleHelper.CenterY(rect) - helper.minWidth / 2f);
						array[1] = new PointF(rect.Width, 0f);
						array[2] = new PointF(rect.Width, rect.Height);
						array[3] = new PointF(0f, StiRectangleHelper.CenterY(rect) + helper.minWidth / 2f);
						break;
					default:
						if (helper.state)
						{
							array[0] = new PointF(0f, rect.Height - helper.minWidth);
							array[1] = new PointF(rect.Width, 0f);
							array[2] = new PointF(rect.Width, helper.maxWidth);
							array[3] = new PointF(0f, rect.Height);
						}
						else
						{
							array[0] = new PointF(0f, helper.offset);
							array[1] = new PointF(rect.Width, 0f);
							array[2] = new PointF(rect.Width, rect.Height);
							array[3] = new PointF(0f, helper.offset + helper.minWidth);
						}
						break;
					}
				}
			}
			else if (helper.isThisStartGreaterEnd)
			{
				switch (Placement)
				{
				case StiPlacement.Outside:
					if (helper.state)
					{
						array[0] = new PointF(0f, rect.Height - helper.maxWidth);
						array[1] = new PointF(rect.Width, 0f);
						array[2] = new PointF(rect.Width, helper.minWidth);
						array[3] = new PointF(0f, rect.Height);
					}
					else
					{
						array[0] = new PointF(0f, 0f);
						array[1] = new PointF(rect.Width, helper.offset);
						array[2] = new PointF(rect.Width, helper.offset + helper.minWidth);
						array[3] = new PointF(0f, rect.Height);
					}
					break;
				case StiPlacement.Overlay:
					array[0] = new PointF(0f, 0f);
					array[1] = new PointF(rect.Width, StiRectangleHelper.CenterY(rect) - helper.minWidth / 2f);
					array[2] = new PointF(rect.Width, StiRectangleHelper.CenterY(rect) + helper.minWidth / 2f);
					array[3] = new PointF(0f, rect.Height);
					break;
				default:
					if (helper.state)
					{
						array[0] = new PointF(0f, 0f);
						array[1] = new PointF(rect.Width, rect.Height - helper.minWidth);
						array[2] = new PointF(rect.Width, rect.Height);
						array[3] = new PointF(0f, helper.maxWidth);
					}
					else
					{
						array[0] = new PointF(0f, 0f);
						array[1] = new PointF(rect.Width, helper.offset);
						array[2] = new PointF(rect.Width, helper.offset + helper.minWidth);
						array[3] = new PointF(0f, rect.Height);
					}
					break;
				}
			}
			else
			{
				switch (Placement)
				{
				case StiPlacement.Outside:
					array[0] = new PointF(0f, rect.Height - helper.minWidth);
					array[1] = new PointF(rect.Width, 0f);
					array[2] = new PointF(rect.Width, helper.maxWidth);
					array[3] = new PointF(0f, rect.Height);
					break;
				case StiPlacement.Overlay:
					array[0] = new PointF(0f, StiRectangleHelper.CenterY(rect) - helper.minWidth / 2f);
					array[1] = new PointF(rect.Width, 0f);
					array[2] = new PointF(rect.Width, rect.Height);
					array[3] = new PointF(0f, StiRectangleHelper.CenterY(rect) + helper.minWidth / 2f);
					break;
				default:
					array[0] = new PointF(0f, 0f);
					array[1] = new PointF(rect.Width, rect.Height - helper.maxWidth);
					array[2] = new PointF(rect.Width, rect.Height);
					array[3] = new PointF(0f, helper.minWidth);
					break;
				}
			}
		}
		else if (helper.scale.IsReversed)
		{
			if (helper.isStartGreaterEnd)
			{
				if (helper.isThisStartGreaterEnd)
				{
					switch (Placement)
					{
					case StiPlacement.Outside:
						array[0] = new PointF(0f, 0f);
						array[1] = new PointF(helper.maxWidth, 0f);
						array[2] = new PointF(rect.Width, rect.Height);
						array[3] = new PointF(rect.Width - helper.minWidth, rect.Height);
						break;
					case StiPlacement.Overlay:
						array[0] = new PointF(0f, 0f);
						array[1] = new PointF(rect.Width, 0f);
						array[2] = new PointF(rect.Width - helper.offset, rect.Height);
						array[3] = new PointF(helper.offset, rect.Height);
						break;
					default:
						array[0] = new PointF(rect.Width - helper.maxWidth, 0f);
						array[1] = new PointF(rect.Width, 0f);
						array[2] = new PointF(helper.minWidth, rect.Height);
						array[3] = new PointF(0f, rect.Height);
						break;
					}
				}
				else
				{
					switch (Placement)
					{
					case StiPlacement.Outside:
						if (helper.state)
						{
							array[0] = new PointF(0f, 0f);
							array[1] = new PointF(helper.minWidth, 0f);
							array[2] = new PointF(rect.Width, rect.Height);
							array[3] = new PointF(rect.Width - helper.maxWidth, rect.Height);
						}
						else
						{
							array[0] = new PointF(rect.Width - helper.offset - helper.minWidth, 0f);
							array[1] = new PointF(rect.Width - helper.offset, 0f);
							array[2] = new PointF(rect.Width, rect.Height);
							array[3] = new PointF(0f, rect.Height);
						}
						break;
					case StiPlacement.Overlay:
						array[0] = new PointF((rect.Width - helper.minWidth) / 2f, 0f);
						array[1] = new PointF(rect.Width - (rect.Width - helper.minWidth) / 2f, 0f);
						array[2] = new PointF(rect.Width, rect.Height);
						array[3] = new PointF(0f, rect.Height);
						break;
					default:
						if (helper.state)
						{
							array[0] = new PointF(rect.Width - helper.minWidth, 0f);
							array[1] = new PointF(rect.Width, 0f);
							array[2] = new PointF(helper.maxWidth, rect.Height);
							array[3] = new PointF(0f, rect.Height);
						}
						else
						{
							array[0] = new PointF(helper.offset, 0f);
							array[1] = new PointF(helper.offset + helper.minWidth, 0f);
							array[2] = new PointF(rect.Width, rect.Height);
							array[3] = new PointF(0f, rect.Height);
						}
						break;
					}
				}
			}
			else if (helper.isThisStartGreaterEnd)
			{
				switch (Placement)
				{
				case StiPlacement.Outside:
					if (helper.state)
					{
						array[0] = new PointF(rect.Width - helper.maxWidth, 0f);
						array[1] = new PointF(rect.Width, 0f);
						array[2] = new PointF(helper.minWidth, rect.Height);
						array[3] = new PointF(0f, rect.Height);
					}
					else
					{
						array[0] = new PointF(0f, 0f);
						array[1] = new PointF(rect.Width, 0f);
						array[2] = new PointF(rect.Width - helper.offset, rect.Height);
						array[3] = new PointF(rect.Width - helper.offset - helper.minWidth, rect.Height);
					}
					break;
				case StiPlacement.Overlay:
					array[0] = new PointF(0f, 0f);
					array[1] = new PointF(rect.Width, 0f);
					array[2] = new PointF(rect.Width - helper.offset, rect.Height);
					array[3] = new PointF(helper.offset, rect.Height);
					break;
				default:
					if (helper.state)
					{
						array[0] = new PointF(0f, 0f);
						array[1] = new PointF(helper.maxWidth, 0f);
						array[2] = new PointF(rect.Width, rect.Height);
						array[3] = new PointF(rect.Width - helper.minWidth, rect.Height);
					}
					else
					{
						array[0] = new PointF(0f, 0f);
						array[1] = new PointF(rect.Width, 0f);
						array[2] = new PointF(helper.offset + helper.minWidth, rect.Height);
						array[3] = new PointF(helper.offset, rect.Height);
					}
					break;
				}
			}
			else
			{
				switch (Placement)
				{
				case StiPlacement.Outside:
					array[0] = new PointF(rect.Width - helper.minWidth, 0f);
					array[1] = new PointF(rect.Width, 0f);
					array[2] = new PointF(helper.maxWidth, rect.Height);
					array[3] = new PointF(0f, rect.Height);
					break;
				case StiPlacement.Overlay:
					array[0] = new PointF((rect.Width - helper.minWidth) / 2f, 0f);
					array[1] = new PointF(rect.Width - (rect.Width - helper.minWidth) / 2f, 0f);
					array[2] = new PointF(rect.Width, rect.Height);
					array[3] = new PointF(0f, rect.Height);
					break;
				default:
					array[0] = new PointF(0f, 0f);
					array[1] = new PointF(helper.minWidth, 0f);
					array[2] = new PointF(rect.Width, rect.Height);
					array[3] = new PointF(rect.Width - helper.maxWidth, rect.Height);
					break;
				}
			}
		}
		else if (helper.isStartGreaterEnd)
		{
			if (helper.isThisStartGreaterEnd)
			{
				switch (Placement)
				{
				case StiPlacement.Outside:
					array[0] = new PointF(rect.Width - helper.minWidth, 0f);
					array[1] = new PointF(rect.Width, 0f);
					array[2] = new PointF(helper.maxWidth, rect.Height);
					array[3] = new PointF(0f, rect.Height);
					break;
				case StiPlacement.Overlay:
					array[0] = new PointF(helper.offset, 0f);
					array[1] = new PointF(rect.Width - helper.offset, 0f);
					array[2] = new PointF(rect.Width, rect.Height);
					array[3] = new PointF(0f, rect.Height);
					break;
				default:
					array[0] = new PointF(0f, 0f);
					array[1] = new PointF(helper.minWidth, 0f);
					array[2] = new PointF(rect.Width, rect.Height);
					array[3] = new PointF(rect.Width - helper.maxWidth, rect.Height);
					break;
				}
			}
			else
			{
				switch (Placement)
				{
				case StiPlacement.Outside:
					if (helper.state)
					{
						array[0] = new PointF(rect.Width - helper.maxWidth, 0f);
						array[1] = new PointF(rect.Width, 0f);
						array[2] = new PointF(helper.minWidth, rect.Height);
						array[3] = new PointF(0f, rect.Height);
					}
					else
					{
						array[0] = new PointF(0f, 0f);
						array[1] = new PointF(rect.Width, 0f);
						array[2] = new PointF(rect.Width - helper.offset, rect.Height);
						array[3] = new PointF(rect.Width - helper.offset - helper.minWidth, rect.Height);
					}
					break;
				case StiPlacement.Overlay:
					array[0] = new PointF(0f, 0f);
					array[1] = new PointF(rect.Width, 0f);
					array[2] = new PointF(rect.Width - (rect.Width - helper.minWidth) / 2f, rect.Height);
					array[3] = new PointF((rect.Width - helper.minWidth) / 2f, rect.Height);
					break;
				default:
					if (helper.state)
					{
						array[0] = new PointF(0f, 0f);
						array[1] = new PointF(helper.maxWidth, 0f);
						array[2] = new PointF(rect.Width, rect.Height);
						array[3] = new PointF(rect.Width - helper.minWidth, rect.Height);
					}
					else
					{
						array[0] = new PointF(0f, 0f);
						array[1] = new PointF(rect.Width, 0f);
						array[2] = new PointF(rect.Width - helper.offset, rect.Height);
						array[3] = new PointF(rect.Width - helper.offset - helper.minWidth, rect.Height);
					}
					break;
				}
			}
		}
		else if (helper.isThisStartGreaterEnd)
		{
			switch (Placement)
			{
			case StiPlacement.Outside:
				if (helper.state)
				{
					array[0] = new PointF(0f, 0f);
					array[1] = new PointF(helper.minWidth, 0f);
					array[2] = new PointF(rect.Width, rect.Height);
					array[3] = new PointF(rect.Width - helper.maxWidth, rect.Height);
				}
				else
				{
					array[0] = new PointF(rect.Width - helper.offset - helper.minWidth, 0f);
					array[1] = new PointF(rect.Width - helper.offset, 0f);
					array[2] = new PointF(rect.Width, rect.Height);
					array[3] = new PointF(0f, rect.Height);
				}
				break;
			case StiPlacement.Overlay:
				array[0] = new PointF(helper.offset, 0f);
				array[1] = new PointF(rect.Width - helper.offset, 0f);
				array[2] = new PointF(rect.Width, rect.Height);
				array[3] = new PointF(0f, rect.Height);
				break;
			default:
				if (helper.state)
				{
					array[0] = new PointF(rect.Width - helper.minWidth, 0f);
					array[1] = new PointF(rect.Width, 0f);
					array[2] = new PointF(helper.maxWidth, rect.Height);
					array[3] = new PointF(0f, rect.Height);
				}
				else
				{
					array[0] = new PointF(helper.offset, 0f);
					array[1] = new PointF(helper.offset + helper.minWidth, 0f);
					array[2] = new PointF(rect.Width, rect.Height);
					array[3] = new PointF(0f, rect.Height);
				}
				break;
			}
		}
		else
		{
			switch (Placement)
			{
			case StiPlacement.Outside:
				array[0] = new PointF(0f, 0f);
				array[1] = new PointF(helper.maxWidth, 0f);
				array[2] = new PointF(rect.Width, rect.Height);
				array[3] = new PointF(rect.Width - helper.minWidth, rect.Height);
				break;
			case StiPlacement.Overlay:
				array[0] = new PointF(0f, 0f);
				array[1] = new PointF(rect.Width, 0f);
				array[2] = new PointF(StiRectangleHelper.CenterX(rect) + helper.minWidth / 2f, rect.Height);
				array[3] = new PointF(StiRectangleHelper.CenterX(rect) - helper.minWidth / 2f, rect.Height);
				break;
			default:
				array[0] = new PointF(rect.Width - helper.maxWidth, 0f);
				array[1] = new PointF(rect.Width, 0f);
				array[2] = new PointF(helper.minWidth, rect.Height);
				array[3] = new PointF(0f, rect.Height);
				break;
			}
		}
		float num = helper.rect.Left;
		float num2 = helper.rect.Top;
		float num3 = base.Scale.barGeometry.Size.Width * base.Offset;
		if ((int)helper.scale.Orientation != 0)
		{
			num = ((Placement != 0) ? (num + num3) : (num - num3));
		}
		else
		{
			num2 = ((Placement != 0) ? (num2 + num3) : (num2 - num3));
		}
		array[0].X += num;
		array[1].X += num;
		array[2].X += num;
		array[3].X += num;
		array[0].Y += num2;
		array[1].Y += num2;
		array[2].Y += num2;
		array[3].Y += num2;
		return new StiGraphicsPathLinesGaugeGeom(array);
	}
}
