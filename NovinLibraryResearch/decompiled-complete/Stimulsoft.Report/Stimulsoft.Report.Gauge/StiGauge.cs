using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Gauge.Collections;
using Stimulsoft.Report.Gauge.Design;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Gauge;

[StiToolbox(true)]
[StiServiceBitmap(typeof(StiComponent), "Stimulsoft.Report.Images.Components.StiGauge.png")]
[StiServiceCategoryBitmap(typeof(StiComponent), "Stimulsoft.Report.Images.Components.catInfographics.png")]
[StiGdiPainter(typeof(StiGaugeGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiGaugeWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiDesigner("Stimulsoft.Gauge.Design.StiGaugeDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfGaugeDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiV2Builder(typeof(StiGaugeV2Builder))]
[StiContextTool(typeof(IStiComponentDesigner))]
public class StiGauge : StiComponent, IStiExportImageExtended, IStiExportImage, IStiBorder, IStiBrush
{
	private IStiGaugeStyle style = new StiGaugeStyleXF26();

	private bool allowApplyStyle = true;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiGauge;

	public override string HelpUrl => "user-manual/introduction.htm";

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable]
	[Description("The appearance and behavior of the component border.")]
	public StiBorder Border { get; set; } = new StiBorder();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[StiSerializable]
	[Description("The brush, which is used to draw background.")]
	[TypeConverter(typeof(StiExpressionBrushConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionBrushEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush(Color.Transparent);


	[Editor("Stimulsoft.Report.Components.TextFormats.Design.StiTextFormatGaugeElementEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(495)]
	[Description("Gets or sets the format of the text.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[StiBrowsable(false)]
	public virtual StiFormatService ValueFormat { get; set; } = GetValueFormatDefault();


	public override int ToolboxPosition => 210;

	public override string LocalizedCategory => StiLocalization.Get("Components", "StiGauge");

	public override string LocalizedName => StiLocalization.Get("Components", "StiGauge");

	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 240.0, 240.0);

	[StiNonSerialized]
	[Browsable(false)]
	public override string ComponentStyle
	{
		get
		{
			return base.ComponentStyle;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool UseParentStyles
	{
		get
		{
			return base.UseParentStyles;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool CanGrow
	{
		get
		{
			return base.CanGrow;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool CanShrink
	{
		get
		{
			return base.CanShrink;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool GrowToHeight
	{
		get
		{
			return base.GrowToHeight;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override StiInteraction Interaction
	{
		get
		{
			return base.Interaction;
		}
		set
		{
			base.Interaction = value;
		}
	}

	[StiSerializable]
	[DefaultValue(StiScaleMode.V2)]
	[Browsable(false)]
	[StiBrowsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiScaleMode Mode { get; set; } = StiScaleMode.V2;


	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	public bool ShortValue { get; set; } = true;


	[StiSerializable]
	[DefaultValue(0.0)]
	[StiBrowsable(false)]
	[Browsable(false)]
	public decimal Minimum { get; set; }

	[StiSerializable]
	[DefaultValue(100.0)]
	[StiBrowsable(false)]
	[Browsable(false)]
	public decimal Maximum { get; set; } = 100m;


	[StiSerializable]
	[DefaultValue(StiGaugeType.FullCircular)]
	[Browsable(false)]
	[StiBrowsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiGaugeType Type { get; set; }

	[StiSerializable]
	[DefaultValue(StiGaugeCalculationMode.Auto)]
	[Browsable(false)]
	[StiBrowsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiGaugeCalculationMode CalculationMode { get; set; } = StiGaugeCalculationMode.Auto;


	[Browsable(false)]
	public StiGaugeContextPainter Painter { get; set; }

	[TypeConverter(typeof(StiGaugeStyleConverter))]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets style of the chart.")]
	public IStiGaugeStyle Style
	{
		get
		{
			return style;
		}
		set
		{
			if (style != value)
			{
				style = value;
				if (value != null)
				{
					value.Core.Gauge = this;
				}
			}
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(90)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that gauge style will be used.")]
	[DefaultValue(true)]
	public bool AllowApplyStyle
	{
		get
		{
			return allowApplyStyle;
		}
		set
		{
			if (allowApplyStyle != value)
			{
				allowApplyStyle = value;
				if (value)
				{
					ApplyStyle(Style);
				}
			}
		}
	}

	[StiSerializable]
	[Browsable(false)]
	public string CustomStyleName { get; set; } = "";


	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiScaleCollection Scales { get; set; }

	[StiNonSerialized]
	[TypeConverter(typeof(StiBoolConverter))]
	[Browsable(false)]
	public bool IsAnimation { get; set; } = true;


	public List<StiAnimation> PreviousAnimations { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "ComponentStyle");
		JsonExtensions.RemoveProperty(val, "UseParentStyles");
		JsonExtensions.RemoveProperty(val, "CanGrow");
		JsonExtensions.RemoveProperty(val, "CanShrink");
		JsonExtensions.RemoveProperty(val, "GrowToHeight");
		JsonExtensions.RemoveProperty(val, "Interaction");
		JsonExtensions.RemoveProperty(val, "IsAnimation");
		JsonExtensions.AddPropertyBool(val, "ShortValue", ShortValue, true, false);
		JsonExtensions.AddPropertyDecimal(val, "Minimum", Minimum, 0m);
		JsonExtensions.AddPropertyDecimal(val, "Maximum", Maximum, 100m);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "CustomStyleName", CustomStyleName);
		JsonExtensions.AddPropertyBorder(val, "Border", Border);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyEnum(val, "Type", (Enum)Type, (Enum)StiGaugeType.FullCircular);
		JsonExtensions.AddPropertyEnum(val, "CalculationMode", (Enum)CalculationMode, (Enum)StiGaugeCalculationMode.Auto);
		JsonExtensions.AddPropertyEnum(val, "Mode", (Enum)Mode, (Enum)StiScaleMode.V2);
		JsonExtensions.AddPropertyJObject(val, "Scales", Scales.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyBool(val, "AllowApplyStyle", AllowApplyStyle, true, false);
		JsonExtensions.AddPropertyJObject(val, "Style", ((IStiJsonReportObject)Style).SaveToJsonObject(mode));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "CustomStyleName":
				CustomStyleName = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Style":
				Style = StiGaugeStyleXF.CreateFromJsonObject((JObject)item.Value);
				break;
			case "Type":
				Type = StiJsonReportObjectHelper.DeserializeEnum<StiGaugeType>(item);
				break;
			case "ShortValue":
				ShortValue = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Minimum":
				Minimum = StiJsonReportObjectHelper.DeserializeDecimal(item);
				break;
			case "Maximum":
				Maximum = StiJsonReportObjectHelper.DeserializeDecimal(item);
				break;
			case "CalculationMode":
				CalculationMode = StiJsonReportObjectHelper.DeserializeEnum<StiGaugeCalculationMode>(item);
				break;
			case "Mode":
				Mode = StiJsonReportObjectHelper.DeserializeEnum<StiScaleMode>(item);
				break;
			case "AllowApplyStyle":
				AllowApplyStyle = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Border":
				Border = StiJsonReportObjectHelper.DeserializeBorder(item);
				break;
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Scales":
				Scales.LoadFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Invalid comparison between Unknown and I4
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.Value, new StiPropertyObject[2]
		{
			propertiesHelper.AllowApplyStyle(),
			propertiesHelper.ShortValue()
		});
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[4]
			{
				propertiesHelper.Left(),
				propertiesHelper.Top(),
				propertiesHelper.Width(),
				propertiesHelper.Height()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[6]
			{
				propertiesHelper.Left(),
				propertiesHelper.Top(),
				propertiesHelper.Width(),
				propertiesHelper.Height(),
				propertiesHelper.MinSize(),
				propertiesHelper.MaxSize()
			});
		}
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[3]
		{
			propertiesHelper.Brush(),
			propertiesHelper.Border(),
			propertiesHelper.Conditions()
		});
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[1] { propertiesHelper.Enabled() });
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[5]
			{
				propertiesHelper.AnchorMode(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ShiftMode()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[6]
			{
				propertiesHelper.AnchorMode(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.Printable(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ShiftMode()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[1] { propertiesHelper.Name() });
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[2]
			{
				propertiesHelper.Name(),
				propertiesHelper.Alias()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[5]
			{
				propertiesHelper.Name(),
				propertiesHelper.Alias(),
				propertiesHelper.Restrictions(),
				propertiesHelper.Locked(),
				propertiesHelper.Linked()
			});
		}
		return stiPropertyCollection;
	}

	public override StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		StiEventCollection stiEventCollection = new StiEventCollection();
		stiEventCollection.Add(StiPropertyCategories.MouseEvents, new StiPropertyEventId[4]
		{
			StiPropertyEventId.ClickEvent,
			StiPropertyEventId.DoubleClickEvent,
			StiPropertyEventId.MouseEnterEvent,
			StiPropertyEventId.MouseLeaveEvent
		});
		stiEventCollection.Add(StiPropertyCategories.NavigationEvents, new StiPropertyEventId[4]
		{
			StiPropertyEventId.GetBookmarkEvent,
			StiPropertyEventId.GetDrillDownReportEvent,
			StiPropertyEventId.GetHyperlinkEvent,
			StiPropertyEventId.GetPointerEvent
		});
		stiEventCollection.Add(StiPropertyCategories.PrintEvents, new StiPropertyEventId[2]
		{
			StiPropertyEventId.AfterPrintEvent,
			StiPropertyEventId.BeforePrintEvent
		});
		stiEventCollection.Add(StiPropertyCategories.ValueEvents, new StiPropertyEventId[2]
		{
			StiPropertyEventId.GetTagEvent,
			StiPropertyEventId.GetToolTipEvent
		});
		return stiEventCollection;
	}

	public override object Clone(bool cloneProperties)
	{
		StiGauge stiGauge = (StiGauge)base.Clone(cloneProperties);
		stiGauge.Scales = new StiScaleCollection(stiGauge);
		lock (((ICollection)Scales).SyncRoot)
		{
			foreach (StiScaleBase scale in Scales)
			{
				stiGauge.Scales.Add((StiScaleBase)scale.Clone());
			}
			return stiGauge;
		}
	}

	public virtual Image GetImage(ref float zoom)
	{
		return GetImage(ref zoom, StiExportFormat.None);
	}

	public virtual Image GetImage(ref float zoom, StiExportFormat format)
	{
		StiPainter painter = StiPainter.GetPainter(((object)this).GetType(), (StiGuiMode)0);
		return painter.GetImage(this, ref zoom, format);
	}

	[Browsable(false)]
	public override bool IsExportAsImage(StiExportFormat format)
	{
		return format != StiExportFormat.Pdf;
	}

	private bool ShouldSerializeBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (Brush is StiSolidBrush)
		{
			return !(((StiSolidBrush)Brush).Color == Color.Transparent);
		}
		return true;
	}

	private static StiFormatService GetValueFormatDefault()
	{
		return new StiNumberFormatService
		{
			DecimalDigits = 0,
			State = (StiTextFormatState.DecimalDigits | StiTextFormatState.Abbreviation)
		};
	}

	internal IStiGaugeStyle GetGaugeStyle()
	{
		if (!string.IsNullOrEmpty(CustomStyleName) && Report != null && Report.Styles.ToList().FirstOrDefault((StiBaseStyle x) => x.Name == CustomStyleName) is StiGaugeStyle stiGaugeStyle)
		{
			return new StiCustomGaugeStyle(stiGaugeStyle);
		}
		return Style;
	}

	public void DrawGauge(StiGaugeContextPainter context)
	{
		ApplyStyle(GetGaugeStyle());
		DrawGaugeInternal(context);
	}

	public void DrawGaugeInternal(StiGaugeContextPainter context)
	{
		int num = -1;
		while (++num < Scales.Count)
		{
			StiScaleBase stiScaleBase = Scales[num];
			if (stiScaleBase != null)
			{
				stiScaleBase.barGeometry.CheckRectGeometry(context.Rect);
				stiScaleBase.DrawElement(context);
			}
		}
	}

	public override StiComponent CreateNew()
	{
		return new StiGauge();
	}

	public void ApplyStyle(IStiGaugeStyle style)
	{
		if (AllowApplyStyle)
		{
			Brush = style.Core.Brush;
			Border.Color = style.Core.BorderColor;
			Border.Size = style.Core.BorderWidth;
		}
		foreach (StiScaleBase scale in Scales)
		{
			scale.ApplyStyle(style);
		}
	}

	public StiGauge()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiGauge(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		Scales = new StiScaleCollection(this);
		PlaceOnToolbox = true;
		PreviousAnimations = new List<StiAnimation>();
	}
}
