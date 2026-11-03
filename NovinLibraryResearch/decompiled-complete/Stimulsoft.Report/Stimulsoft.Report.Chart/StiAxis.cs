using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public abstract class StiAxis : IStiAxis, ICloneable, IStiJsonReportObject, IStiDefault
{
	private bool allowApplyStyle = true;

	private IStiAxisTitle title;

	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			if (!LogarithmicScale && AllowApplyStyle && StartFromZero && !ShouldSerializeInteraction() && !ShouldSerializeLabels() && !ShouldSerializeRange() && !ShouldSerializeTitle() && !ShouldSerializeTicks() && ArrowStyle == StiArrowStyle.None && (int)LineStyle == 0 && LineWidth == 1f)
			{
				return !ShouldSerializeVisible();
			}
			return false;
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that logarithmic scale will be used.")]
	public virtual bool LogarithmicScale { get; set; }

	[Browsable(false)]
	public StiAxisCoreXF Core { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that chart style will be used.")]
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
				if (value && Area != null && Area.Chart != null)
				{
					Core.ApplyStyle(Area.Chart.Style);
				}
			}
		}
	}

	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that all arguments will be shows from zero.")]
	public virtual bool StartFromZero { get; set; } = true;


	[StiSerializable]
	[DefaultValue(0f)]
	[Obsolete("Step property is obsolete. Please use Labels.Step property instead it.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public virtual float Step
	{
		get
		{
			if (Labels == null)
			{
				return 0f;
			}
			return Labels.Step;
		}
		set
		{
			if (Labels != null)
			{
				Labels.Step = value;
			}
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	public IStiAxisInteraction Interaction { get; set; } = new StiAxisInteraction();


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	public IStiAxisLabels Labels { get; set; } = new StiAxisLabels();


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	public virtual IStiAxisRange Range { get; set; } = new StiAxisRange();


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	public IStiAxisTitle Title
	{
		get
		{
			if (title == null)
			{
				title = new StiAxisTitle();
				if (this is StiXBottomAxis)
				{
					title.Direction = StiDirection.LeftToRight;
				}
				else if (this is StiXTopAxis)
				{
					title.Direction = StiDirection.LeftToRight;
				}
				else if (this is StiYRightAxis)
				{
					title.Direction = StiDirection.TopToBottom;
				}
				else if (this is StiYLeftAxis)
				{
					title.Direction = StiDirection.BottomToTop;
				}
			}
			return title;
		}
		set
		{
			if (title == null)
			{
				title = new StiAxisTitle();
				if (this is StiXBottomAxis)
				{
					title.Direction = StiDirection.LeftToRight;
				}
				else if (this is StiXTopAxis)
				{
					title.Direction = StiDirection.LeftToRight;
				}
				else if (this is StiYRightAxis)
				{
					title.Direction = StiDirection.TopToBottom;
				}
				else if (this is StiYLeftAxis)
				{
					title.Direction = StiDirection.BottomToTop;
				}
			}
			title = value;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	public IStiAxisTicks Ticks { get; set; } = new StiAxisTicks();


	[DefaultValue(StiArrowStyle.None)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiArrowStyle ArrowStyle { get; set; }

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiPenStyle LineStyle { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color LineColor { get; set; } = Color.Gray;


	[DefaultValue(1f)]
	[StiSerializable]
	public float LineWidth { get; set; } = 1f;


	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of axis.")]
	public virtual bool Visible { get; set; } = true;


	[Description("'TitleDirection' property is obsolete. Please Title.Direction property instead it.")]
	[Obsolete("'TitleDirection' property is obsolete. Please Title.Direction property instead it.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public virtual StiLegendDirection TitleDirection
	{
		get
		{
			if (Title == null)
			{
				return StiLegendDirection.BottomToTop;
			}
			return Title.Direction switch
			{
				StiDirection.BottomToTop => StiLegendDirection.BottomToTop, 
				StiDirection.LeftToRight => StiLegendDirection.LeftToRight, 
				StiDirection.RightToLeft => StiLegendDirection.RightToLeft, 
				StiDirection.TopToBottom => StiLegendDirection.TopToBottom, 
				_ => StiLegendDirection.BottomToTop, 
			};
		}
		set
		{
			if (Title != null)
			{
				switch (value)
				{
				case StiLegendDirection.BottomToTop:
					Title.Direction = StiDirection.BottomToTop;
					break;
				case StiLegendDirection.LeftToRight:
					Title.Direction = StiDirection.LeftToRight;
					break;
				case StiLegendDirection.RightToLeft:
					Title.Direction = StiDirection.RightToLeft;
					break;
				case StiLegendDirection.TopToBottom:
					Title.Direction = StiDirection.TopToBottom;
					break;
				}
			}
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public IStiAxisArea Area { get; set; }

	[Browsable(false)]
	public StiAxisInfoXF Info { get; set; } = new StiAxisInfoXF();


	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		JsonExtensions.AddPropertyBool(val, "LogarithmicScale", LogarithmicScale, false, false);
		JsonExtensions.AddPropertyBool(val, "AllowApplyStyle", AllowApplyStyle, true, false);
		JsonExtensions.AddPropertyBool(val, "StartFromZero", StartFromZero, true, false);
		JsonExtensions.AddPropertyJObject(val, "Interaction", ((IStiJsonReportObject)Interaction).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Labels", ((IStiJsonReportObject)Labels).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Ticks", ((IStiJsonReportObject)Ticks).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Range", ((IStiJsonReportObject)Range).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyBool(val, "Visible", Visible, true, false);
		JsonExtensions.AddPropertyEnum(val, "ArrowStyle", (Enum)ArrowStyle, (Enum)StiArrowStyle.None);
		JsonExtensions.AddPropertyEnum(val, "LineStyle", (Enum)(object)LineStyle, (Enum)(object)(StiPenStyle)0);
		JsonExtensions.AddPropertyColor(val, "LineColor", LineColor, Color.Gray);
		JsonExtensions.AddPropertyFloat(val, "LineWidth", LineWidth, 1f);
		if (title != null)
		{
			JsonExtensions.AddPropertyJObject(val, "Title", ((IStiJsonReportObject)title).SaveToJsonObject(mode));
		}
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Expected O, but got Unknown
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "LogarithmicScale":
				LogarithmicScale = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AllowApplyStyle":
				AllowApplyStyle = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "StartFromZero":
				StartFromZero = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Interaction":
				((IStiJsonReportObject)Interaction).LoadFromJsonObject((JObject)item.Value);
				break;
			case "Labels":
				((IStiJsonReportObject)Labels).LoadFromJsonObject((JObject)item.Value);
				break;
			case "Ticks":
				((IStiJsonReportObject)Ticks).LoadFromJsonObject((JObject)item.Value);
				break;
			case "Range":
				((IStiJsonReportObject)Range).LoadFromJsonObject((JObject)item.Value);
				break;
			case "Title":
				((IStiJsonReportObject)Title).LoadFromJsonObject((JObject)item.Value);
				break;
			case "Visible":
				Visible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ArrowStyle":
				ArrowStyle = StiJsonReportObjectHelper.DeserializeEnum<StiArrowStyle>(item);
				break;
			case "LineStyle":
				LineStyle = StiJsonReportObjectHelper.DeserializeEnum<StiPenStyle>(item);
				break;
			case "LineColor":
				LineColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "LineWidth":
				LineWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			}
		}
	}

	public object Clone()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		IStiAxis stiAxis = MemberwiseClone() as IStiAxis;
		stiAxis.ArrowStyle = ArrowStyle;
		stiAxis.LineStyle = LineStyle;
		stiAxis.Labels = Labels.Clone() as IStiAxisLabels;
		stiAxis.Range = Range.Clone() as IStiAxisRange;
		stiAxis.Title = Title.Clone() as IStiAxisTitle;
		stiAxis.Ticks = Ticks.Clone() as IStiAxisTicks;
		if (Core != null)
		{
			stiAxis.Core = Core.Clone() as StiAxisCoreXF;
			stiAxis.Core.Axis = stiAxis;
		}
		return stiAxis;
	}

	private bool ShouldSerializeInteraction()
	{
		if (Interaction != null)
		{
			return !((IStiDefault)Interaction).IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeLabels()
	{
		if (Labels != null)
		{
			return !((IStiDefault)Labels).IsDefault;
		}
		return true;
	}

	protected virtual bool ShouldSerializeRange()
	{
		if (Range != null)
		{
			return !((IStiDefault)Range).IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeTitle()
	{
		if (Title != null)
		{
			return !((IStiDefault)Title).IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeTicks()
	{
		if (Ticks != null)
		{
			return !((IStiDefault)Ticks).IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeLineColor()
	{
		return LineColor != Color.Gray;
	}

	protected virtual bool ShouldSerializeVisible()
	{
		return !Visible;
	}

	public StiAxis()
	{
	}

	public StiAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, bool allowApplyStyle)
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		Labels = labels;
		Range = range;
		this.title = title;
		Ticks = ticks;
		ArrowStyle = arrowStyle;
		LineStyle = lineStyle;
		LineColor = lineColor;
		LineWidth = lineWidth;
		Visible = visible;
		StartFromZero = startFromZero;
		this.allowApplyStyle = allowApplyStyle;
	}

	public StiAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, IStiAxisInteraction interaction, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, bool allowApplyStyle)
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		Labels = labels;
		Range = range;
		this.title = title;
		Ticks = ticks;
		Interaction = interaction;
		ArrowStyle = arrowStyle;
		LineStyle = lineStyle;
		LineColor = lineColor;
		LineWidth = lineWidth;
		Visible = visible;
		StartFromZero = startFromZero;
		this.allowApplyStyle = allowApplyStyle;
	}

	public StiAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, IStiAxisInteraction interaction, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, bool allowApplyStyle, bool logarithmicScale)
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		Labels = labels;
		Range = range;
		this.title = title;
		Ticks = ticks;
		Interaction = interaction;
		ArrowStyle = arrowStyle;
		LineStyle = lineStyle;
		LineColor = lineColor;
		LineWidth = lineWidth;
		Visible = visible;
		StartFromZero = startFromZero;
		this.allowApplyStyle = allowApplyStyle;
		LogarithmicScale = logarithmicScale;
	}
}
