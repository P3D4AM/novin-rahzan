using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Images;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report;

public abstract class StiBaseStyle : StiService, IStiPropertyGridObject, IStiJsonReportObject
{
	private StiStyleConditionsCollection conditions;

	[Browsable(false)]
	public override Type ServiceType => typeof(StiBaseStyle);

	public static double? WindowsScaleInternal { get; set; }

	[Browsable(false)]
	public abstract StiComponentId ComponentId { get; }

	[Browsable(false)]
	public string PropName { get; internal set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(111)]
	[DefaultValue("")]
	[Description("Gets or sets a name of the styles collection.")]
	public string CollectionName { get; set; } = string.Empty;


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a collection of the style conditions.")]
	[StiOrder(112)]
	[TypeConverter(typeof(StiStyleConditionsCollectionConverter))]
	[Editor("Stimulsoft.Report.Design.StiStyleConditionsCollectionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual StiStyleConditionsCollection Conditions
	{
		get
		{
			return conditions ?? (conditions = new StiStyleConditionsCollection());
		}
		set
		{
			conditions = value;
		}
	}

	[StiSerializable]
	[DefaultValue("")]
	[Description("Gets or sets a style description.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	public string Description { get; set; }

	[StiSerializable]
	[Description("Gets or sets a style name.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	public virtual string Name
	{
		get
		{
			return PropName;
		}
		set
		{
			if (Report != null && Report.IsDesigning && string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentException();
			}
			if (PropName != value && Report != null)
			{
				Report.RenameStyle(PropName, value);
			}
			PropName = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	[StiBrowsable(false)]
	public virtual string DashboardName => null;

	[Browsable(false)]
	internal StiReport Report { get; set; }

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyIdent(val, "Ident", ((object)this).GetType().Name);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "CollectionName", CollectionName);
		JsonExtensions.AddPropertyJObject(val, "Conditions", Conditions.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Description", Description);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Name", Name);
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "CollectionName":
				CollectionName = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Conditions":
				Conditions.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Description":
				Description = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Name":
				PropName = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public abstract StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level);

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public override object Clone()
	{
		StiBaseStyle stiBaseStyle = MemberwiseClone() as StiBaseStyle;
		stiBaseStyle.conditions = ((conditions != null) ? ((StiStyleConditionsCollection)conditions.Clone()) : null);
		return stiBaseStyle;
	}

	public override int GetHashCode()
	{
		return ((object)this).GetHashCode();
	}

	public override bool Equals(object obj)
	{
		return Equals(obj, allowEqualName: true, allowEqualDescription: true);
	}

	public bool Equals(object obj, bool allowEqualName, bool allowEqualDescription)
	{
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		if (this is StiStyle)
		{
			if (!(obj is StiStyle stiStyle))
			{
				return false;
			}
			StiStyle stiStyle2 = this as StiStyle;
			if (!((object)stiStyle.Border).Equals((object?)stiStyle2.Border))
			{
				return false;
			}
			if (!((object)stiStyle.Brush).Equals((object?)stiStyle2.Brush))
			{
				return false;
			}
			if (!((object)stiStyle.Font).Equals((object?)stiStyle2.Font))
			{
				return false;
			}
			if (stiStyle.Image == null && stiStyle.Image != null)
			{
				return false;
			}
			if (stiStyle.Image != null && stiStyle.Image == null)
			{
				return false;
			}
			if (stiStyle.Image != null && stiStyle2.Image != null && !((object)stiStyle.Image).Equals((object?)stiStyle2.Image))
			{
				return false;
			}
			if (!((object)stiStyle.TextBrush).Equals((object?)stiStyle2.TextBrush))
			{
				return false;
			}
			if (!stiStyle.CollectionName.Equals(stiStyle2.CollectionName))
			{
				return false;
			}
			if (!stiStyle.Conditions.Equals(stiStyle2.Conditions))
			{
				return false;
			}
			if (stiStyle.AllowUseBorderFormatting != stiStyle2.AllowUseBorderFormatting)
			{
				return false;
			}
			if (stiStyle.AllowUseBorderSides != stiStyle2.AllowUseBorderSides)
			{
				return false;
			}
			if (stiStyle.AllowUseBorderSidesFromLocation != stiStyle2.AllowUseBorderSidesFromLocation)
			{
				return false;
			}
			if (stiStyle.AllowUseBrush != stiStyle2.AllowUseBrush)
			{
				return false;
			}
			if (stiStyle.AllowUseFont != stiStyle2.AllowUseFont)
			{
				return false;
			}
			if (stiStyle.AllowUseHorAlignment != stiStyle2.AllowUseHorAlignment)
			{
				return false;
			}
			if (stiStyle.AllowUseImage != stiStyle2.AllowUseImage)
			{
				return false;
			}
			if (stiStyle.AllowUseTextBrush != stiStyle2.AllowUseTextBrush)
			{
				return false;
			}
			if (stiStyle.AllowUseVertAlignment != stiStyle2.AllowUseVertAlignment)
			{
				return false;
			}
			if (stiStyle.HorAlignment != stiStyle2.HorAlignment)
			{
				return false;
			}
			if (stiStyle.VertAlignment != stiStyle2.VertAlignment)
			{
				return false;
			}
			if (allowEqualName && stiStyle.Name != stiStyle2.Name)
			{
				return false;
			}
			if (allowEqualDescription && stiStyle.Description != stiStyle2.Description)
			{
				return false;
			}
			return true;
		}
		if (this is StiChartStyle)
		{
			if (!(obj is StiChartStyle stiChartStyle))
			{
				return false;
			}
			StiChartStyle stiChartStyle2 = this as StiChartStyle;
			if (!((object)stiChartStyle.Border).Equals((object?)stiChartStyle2.Border))
			{
				return false;
			}
			if (!((object)stiChartStyle.Brush).Equals((object?)stiChartStyle2.Brush))
			{
				return false;
			}
			if (!stiChartStyle.BasicStyleColor.Equals((object?)stiChartStyle2.BasicStyleColor))
			{
				return false;
			}
			if (!stiChartStyle.BrushType.Equals(stiChartStyle2.BrushType))
			{
				return false;
			}
			if (!stiChartStyle.StyleColors.Equals(stiChartStyle2.StyleColors))
			{
				return false;
			}
			if (stiChartStyle.AllowUseBorderFormatting != stiChartStyle2.AllowUseBorderFormatting)
			{
				return false;
			}
			if (stiChartStyle.AllowUseBorderSides != stiChartStyle2.AllowUseBorderSides)
			{
				return false;
			}
			if (stiChartStyle.AllowUseBrush != stiChartStyle2.AllowUseBrush)
			{
				return false;
			}
			if (allowEqualName && stiChartStyle.Name != stiChartStyle2.Name)
			{
				return false;
			}
			if (allowEqualDescription && stiChartStyle.Description != stiChartStyle2.Description)
			{
				return false;
			}
			return true;
		}
		if (this is StiCardsStyle)
		{
			if (!(obj is StiCardsStyle stiCardsStyle))
			{
				return false;
			}
			StiCardsStyle stiCardsStyle2 = this as StiCardsStyle;
			if (!stiCardsStyle.BackColor.Equals((object?)stiCardsStyle2.BackColor))
			{
				return false;
			}
			if (!stiCardsStyle.ForeColor.Equals((object?)stiCardsStyle2.ForeColor))
			{
				return false;
			}
			if (!stiCardsStyle.LineColor.Equals((object?)stiCardsStyle2.LineColor))
			{
				return false;
			}
			if (allowEqualName && stiCardsStyle.Name != stiCardsStyle2.Name)
			{
				return false;
			}
			if (allowEqualDescription && stiCardsStyle.Description != stiCardsStyle2.Description)
			{
				return false;
			}
			return true;
		}
		if (this is StiIndicatorStyle)
		{
			if (!(obj is StiIndicatorStyle stiIndicatorStyle))
			{
				return false;
			}
			StiIndicatorStyle stiIndicatorStyle2 = this as StiIndicatorStyle;
			if (!stiIndicatorStyle.BackColor.Equals((object?)stiIndicatorStyle2.BackColor))
			{
				return false;
			}
			if (!stiIndicatorStyle.GlyphColor.Equals((object?)stiIndicatorStyle2.GlyphColor))
			{
				return false;
			}
			if (!stiIndicatorStyle.ForeColor.Equals((object?)stiIndicatorStyle2.ForeColor))
			{
				return false;
			}
			if (!stiIndicatorStyle.HotBackColor.Equals((object?)stiIndicatorStyle2.HotBackColor))
			{
				return false;
			}
			if (!stiIndicatorStyle.PositiveColor.Equals((object?)stiIndicatorStyle2.PositiveColor))
			{
				return false;
			}
			if (!stiIndicatorStyle.NegativeColor.Equals((object?)stiIndicatorStyle2.NegativeColor))
			{
				return false;
			}
			if (allowEqualName && stiIndicatorStyle.Name != stiIndicatorStyle2.Name)
			{
				return false;
			}
			if (allowEqualDescription && stiIndicatorStyle.Description != stiIndicatorStyle2.Description)
			{
				return false;
			}
			return true;
		}
		if (this is StiProgressStyle)
		{
			if (!(obj is StiProgressStyle stiProgressStyle))
			{
				return false;
			}
			StiProgressStyle stiProgressStyle2 = this as StiProgressStyle;
			if (!stiProgressStyle.TrackColor.Equals((object?)stiProgressStyle2.TrackColor))
			{
				return false;
			}
			if (!stiProgressStyle.BandColor.Equals((object?)stiProgressStyle2.BandColor))
			{
				return false;
			}
			if (!stiProgressStyle.SeriesColors.Equals(stiProgressStyle2.SeriesColors))
			{
				return false;
			}
			if (!stiProgressStyle.ForeColor.Equals((object?)stiProgressStyle2.ForeColor))
			{
				return false;
			}
			if (!stiProgressStyle.BackColor.Equals((object?)stiProgressStyle2.BackColor))
			{
				return false;
			}
			if (allowEqualName && stiProgressStyle.Name != stiProgressStyle2.Name)
			{
				return false;
			}
			if (allowEqualDescription && stiProgressStyle.Description != stiProgressStyle2.Description)
			{
				return false;
			}
			return true;
		}
		if (this is Stimulsoft.Report.Chart.StiChartStyle)
		{
			if (!(obj is Stimulsoft.Report.Chart.StiChartStyle stiChartStyle3))
			{
				return false;
			}
			Stimulsoft.Report.Chart.StiChartStyle stiChartStyle4 = this as Stimulsoft.Report.Chart.StiChartStyle;
			if (stiChartStyle3 is StiCustomStyle && stiChartStyle4 is StiCustomStyle)
			{
				return ((StiCustomStyleCoreXF)stiChartStyle3.Core).ReportStyleName == ((StiCustomStyleCoreXF)stiChartStyle4.Core).ReportStyleName;
			}
			if (stiChartStyle3 is StiCustomStyle || stiChartStyle4 is StiCustomStyle)
			{
				return false;
			}
			return ((object)stiChartStyle3).GetType() == ((object)stiChartStyle4).GetType();
		}
		if (this is StiCrossTabStyle)
		{
			if (!(obj is StiCrossTabStyle stiCrossTabStyle))
			{
				return false;
			}
			StiCrossTabStyle stiCrossTabStyle2 = this as StiCrossTabStyle;
			if (!stiCrossTabStyle.Color.Equals((object?)stiCrossTabStyle2.Color))
			{
				return false;
			}
			if (allowEqualName && stiCrossTabStyle.Name != stiCrossTabStyle2.Name)
			{
				return false;
			}
			if (allowEqualDescription && stiCrossTabStyle.Description != stiCrossTabStyle2.Description)
			{
				return false;
			}
			return true;
		}
		if (this is StiTableStyle)
		{
			if (!(obj is StiTableStyle stiTableStyle))
			{
				return false;
			}
			StiTableStyle stiTableStyle2 = this as StiTableStyle;
			if (!stiTableStyle.HeaderColor.Equals((object?)stiTableStyle2.HeaderColor))
			{
				return false;
			}
			if (!stiTableStyle.HeaderForeground.Equals((object?)stiTableStyle2.HeaderForeground))
			{
				return false;
			}
			if (!stiTableStyle.FooterForeground.Equals((object?)stiTableStyle2.FooterForeground))
			{
				return false;
			}
			if (!stiTableStyle.DataColor.Equals((object?)stiTableStyle2.DataColor))
			{
				return false;
			}
			if (!stiTableStyle.DataForeground.Equals((object?)stiTableStyle2.DataForeground))
			{
				return false;
			}
			if (!stiTableStyle.GridColor.Equals((object?)stiTableStyle2.GridColor))
			{
				return false;
			}
			if (allowEqualName && stiTableStyle.Name != stiTableStyle2.Name)
			{
				return false;
			}
			if (allowEqualDescription && stiTableStyle.Description != stiTableStyle2.Description)
			{
				return false;
			}
			return true;
		}
		if (this is StiGaugeStyleXF)
		{
			if (!(obj is StiGaugeStyleXF stiGaugeStyleXF))
			{
				return false;
			}
			StiGaugeStyleXF stiGaugeStyleXF2 = this as StiGaugeStyleXF;
			return stiGaugeStyleXF.StyleIdent == stiGaugeStyleXF2.StyleIdent;
		}
		if (this is StiTableElementStyle)
		{
			if (!(obj is StiTableElementStyle stiTableElementStyle))
			{
				return false;
			}
			StiTableElementStyle stiTableElementStyle2 = this as StiTableElementStyle;
			return stiTableElementStyle.Ident == stiTableElementStyle2.Ident;
		}
		if (this is StiPivotElementStyle)
		{
			if (!(obj is StiPivotElementStyle stiPivotElementStyle))
			{
				return false;
			}
			StiPivotElementStyle stiPivotElementStyle2 = this as StiPivotElementStyle;
			return stiPivotElementStyle.Ident == stiPivotElementStyle2.Ident;
		}
		if (this is StiIndicatorElementStyle)
		{
			if (!(obj is StiIndicatorElementStyle stiIndicatorElementStyle))
			{
				return false;
			}
			StiIndicatorElementStyle stiIndicatorElementStyle2 = this as StiIndicatorElementStyle;
			return stiIndicatorElementStyle.Ident == stiIndicatorElementStyle2.Ident;
		}
		if (this is StiProgressElementStyle)
		{
			if (!(obj is StiProgressElementStyle stiProgressElementStyle))
			{
				return false;
			}
			StiProgressElementStyle stiProgressElementStyle2 = this as StiProgressElementStyle;
			return stiProgressElementStyle.Ident == stiProgressElementStyle2.Ident;
		}
		if (this is StiMapStyleFX)
		{
			if (!(obj is StiMapStyleFX stiMapStyleFX))
			{
				return false;
			}
			StiMapStyleFX stiMapStyleFX2 = this as StiMapStyleFX;
			if (!stiMapStyleFX.AllowDashboard || stiMapStyleFX2.AllowDashboard)
			{
				return false;
			}
			return stiMapStyleFX.StyleIdent == stiMapStyleFX2.StyleIdent;
		}
		if (this is StiControlElementStyle)
		{
			if (!(obj is StiControlElementStyle stiControlElementStyle))
			{
				return false;
			}
			StiControlElementStyle stiControlElementStyle2 = this as StiControlElementStyle;
			return stiControlElementStyle.Ident == stiControlElementStyle2.Ident;
		}
		if (this is StiGaugeStyle)
		{
			if (!(obj is StiGaugeStyle stiGaugeStyle))
			{
				return false;
			}
			StiGaugeStyle stiGaugeStyle2 = this as StiGaugeStyle;
			if (!((object)stiGaugeStyle.Brush).Equals((object?)stiGaugeStyle2.Brush))
			{
				return false;
			}
			if (!stiGaugeStyle.BorderColor.Equals((object?)stiGaugeStyle2.BorderColor))
			{
				return false;
			}
			if (!stiGaugeStyle.ForeColor.Equals((object?)stiGaugeStyle2.ForeColor))
			{
				return false;
			}
			if (!stiGaugeStyle.BorderWidth.Equals(stiGaugeStyle2.BorderWidth))
			{
				return false;
			}
			if (allowEqualName && stiGaugeStyle.Name != stiGaugeStyle2.Name)
			{
				return false;
			}
			if (allowEqualDescription && stiGaugeStyle.Description != stiGaugeStyle2.Description)
			{
				return false;
			}
			return true;
		}
		if (this is StiMapStyle)
		{
			if (!(obj is StiMapStyle stiMapStyle))
			{
				return false;
			}
			StiMapStyle stiMapStyle2 = this as StiMapStyle;
			if (!stiMapStyle.Colors.Equals(stiMapStyle2.Colors))
			{
				return false;
			}
			if (!stiMapStyle.Heatmap.Equals(stiMapStyle2.Heatmap))
			{
				return false;
			}
			if (!stiMapStyle.HeatmapWithGroup.Equals(stiMapStyle2.HeatmapWithGroup))
			{
				return false;
			}
			if (!stiMapStyle.DefaultColor.Equals((object?)stiMapStyle2.DefaultColor))
			{
				return false;
			}
			if (!stiMapStyle.BackColor.Equals((object?)stiMapStyle2.BackColor))
			{
				return false;
			}
			if (!stiMapStyle.LabelForeground.Equals((object?)stiMapStyle2.LabelForeground))
			{
				return false;
			}
			if (!stiMapStyle.LabelShadowForeground.Equals((object?)stiMapStyle2.LabelShadowForeground))
			{
				return false;
			}
			if (!stiMapStyle.BorderSize.Equals(stiMapStyle2.BorderSize))
			{
				return false;
			}
			if (!stiMapStyle.BorderColor.Equals((object?)stiMapStyle2.BorderColor))
			{
				return false;
			}
			if (!stiMapStyle.BubbleBackColor.Equals((object?)stiMapStyle2.BubbleBackColor))
			{
				return false;
			}
			if (!stiMapStyle.BubbleBorderColor.Equals((object?)stiMapStyle2.BubbleBorderColor))
			{
				return false;
			}
			if (allowEqualName && stiMapStyle.Name != stiMapStyle2.Name)
			{
				return false;
			}
			if (allowEqualDescription && stiMapStyle.Description != stiMapStyle2.Description)
			{
				return false;
			}
			return true;
		}
		if (this is StiDialogStyle)
		{
			if (!(obj is StiDialogStyle stiDialogStyle))
			{
				return false;
			}
			StiDialogStyle stiDialogStyle2 = this as StiDialogStyle;
			if (!stiDialogStyle.ForeColor.Equals((object?)stiDialogStyle2.ForeColor))
			{
				return false;
			}
			if (!stiDialogStyle.BackColor.Equals((object?)stiDialogStyle2.BackColor))
			{
				return false;
			}
			if (stiDialogStyle.AllowUseFont != stiDialogStyle2.AllowUseFont)
			{
				return false;
			}
			if (stiDialogStyle.AllowUseBackColor != stiDialogStyle2.AllowUseBackColor)
			{
				return false;
			}
			if (stiDialogStyle.AllowUseForeColor != stiDialogStyle2.AllowUseForeColor)
			{
				return false;
			}
			if (allowEqualName && stiDialogStyle.Name != stiDialogStyle2.Name)
			{
				return false;
			}
			if (allowEqualDescription && stiDialogStyle.Description != stiDialogStyle2.Description)
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public static void DrawBox(Graphics g, Rectangle rect, StiBaseStyle style, bool paintValue, bool paintImage)
	{
		style.DrawBox(g, rect, paintValue, paintImage);
	}

	public static void DrawStyle(Graphics g, Rectangle rect, StiBaseStyle style, DrawItemState state, bool fillBackground, bool paintImage)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		style?.DrawStyle(g, rect, state, fillBackground, paintImage);
	}

	public static void DrawStyleForEditor(StiBaseStyle style, Graphics g, Rectangle rect, DrawItemState state)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		style?.DrawStyleForEditor(g, rect, state);
	}

	public static StiBaseStyle GetStyle(StiComponent component, StiStyleElements styleElements)
	{
		return GetStyle(component, styleElements, null);
	}

	internal static StiBaseStyle GetStyle(StiComponent component, StiStyleElements styleElements, StiBaseStyle componentStyle)
	{
		if (component is StiChart)
		{
			StiChart stiChart = (StiChart)component;
			return stiChart.Style as StiBaseStyle;
		}
		if (!(component is StiCrossTab))
		{
			StiStyle stiStyle = new StiStyle();
			stiStyle.GetStyleFromComponent(component, styleElements, componentStyle);
			return stiStyle;
		}
		return null;
	}

	public static StiBaseStyle GetStyle(StiComponent component)
	{
		return GetStyle(component, StiStyleElements.All, null);
	}

	internal static StiBaseStyle GetStyle(StiComponent component, StiBaseStyle componentStyle)
	{
		return GetStyle(component, StiStyleElements.All, componentStyle);
	}

	public abstract void DrawStyle(Graphics g, Rectangle rect, bool paintValue, bool paintImage);

	public abstract void DrawBox(Graphics g, Rectangle rect, bool paintValue, bool paintImage);

	public virtual void DrawStyleImage(Graphics g, Rectangle rect)
	{
		Rectangle rectangle = new Rectangle(rect.X + 2, rect.Y + (rect.Height - 16) / 2, 16, 16);
		Image val = null;
		if (this is StiStyle)
		{
			val = (Image)(object)StylesResource.Style2013;
		}
		else if (this is StiChartStyle || this is Stimulsoft.Report.Chart.StiChartStyle)
		{
			val = (Image)(object)StylesResource.StyleChart2013;
		}
		else if (this is StiGaugeStyle)
		{
			val = (Image)(object)StylesResource.StyleGauge2013;
		}
		else if (this is StiCrossTabStyle)
		{
			val = (Image)(object)StylesResource.StyleCrossTab2013;
		}
		else if (this is StiDialogStyle)
		{
			val = (Image)(object)StylesResource.StyleDialog2013;
		}
		g.DrawImageUnscaled(val, rectangle);
	}

	public abstract void GetStyleFromComponent(StiComponent component, StiStyleElements styleElements);

	public abstract void SetStyleToComponent(StiComponent component);

	public void DrawStyle(Graphics g, Rectangle rect, DrawItemState state, bool fillBackground, bool paintImage)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		if (fillBackground)
		{
			StiControlPaint.DrawItemBackground(g, rect, state, (Color?)null);
		}
		rect.Inflate(-1, -1);
		DrawBox(g, rect, this, paintValue: false, paintImage);
	}

	public virtual void DrawStyleForEditor(Graphics g, Rectangle rect, DrawItemState state)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		StiControlPaint.DrawItemBackground(g, rect, state, (Color?)null);
	}

	public virtual void Save(Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Serialize((object)this, stream, "StiStyle");
	}

	public virtual void Load(Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Deserialize((object)this, stream, "StiStyle");
	}

	public virtual void Save(string file)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		StiFileUtils.ProcessReadOnly(file);
		FileStream fileStream = new FileStream(file, FileMode.Create, FileAccess.Write);
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Serialize((object)this, (Stream)fileStream, "StiStyle");
		fileStream.Flush();
		fileStream.Close();
	}

	public virtual string SaveToString()
	{
		MemoryStream memoryStream = null;
		StreamReader streamReader = null;
		string result = null;
		try
		{
			memoryStream = new MemoryStream();
			streamReader = new StreamReader(memoryStream);
			Save(memoryStream);
			memoryStream.Flush();
			memoryStream.Seek(0L, SeekOrigin.Begin);
			result = streamReader.ReadToEnd();
		}
		catch (Exception e)
		{
			StiLogService.Write(((object)this).GetType(), "Saving style to string");
			StiLogService.Write(((object)this).GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		finally
		{
			streamReader?.Close();
			memoryStream?.Close();
		}
		return result;
	}

	public virtual void LoadFromString(string styleStr)
	{
		MemoryStream memoryStream = null;
		StreamWriter streamWriter = null;
		try
		{
			memoryStream = new MemoryStream();
			streamWriter = new StreamWriter(memoryStream);
			streamWriter.Write(styleStr);
			streamWriter.Flush();
			memoryStream.Flush();
			memoryStream.Seek(0L, SeekOrigin.Begin);
			Load(memoryStream);
		}
		catch (Exception e)
		{
			StiLogService.Write(((object)this).GetType(), "Loading style from string");
			StiLogService.Write(((object)this).GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		finally
		{
			streamWriter?.Close();
			memoryStream?.Close();
		}
	}

	public static StiBaseStyle LoadStyleFromString(string styleStr)
	{
		StiBaseStyle stiBaseStyle = null;
		if (styleStr.IndexOf("StiChartStyle", StringComparison.InvariantCulture) != -1)
		{
			stiBaseStyle = new StiChartStyle();
		}
		else if (styleStr.IndexOf("StiGaugeStyle", StringComparison.InvariantCulture) != -1)
		{
			stiBaseStyle = new StiGaugeStyle();
		}
		else if (styleStr.IndexOf("StiCrossTabStyle", StringComparison.InvariantCulture) != -1)
		{
			stiBaseStyle = new StiCrossTabStyle();
		}
		else if (styleStr.IndexOf("StiStyle", StringComparison.InvariantCulture) != -1)
		{
			stiBaseStyle = new StiStyle();
		}
		stiBaseStyle?.LoadFromString(styleStr);
		return stiBaseStyle;
	}

	public virtual void Load(string file)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		if (File.Exists(file))
		{
			FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
			StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
			val.Deserialize((object)this, (Stream)fileStream, "StiStyle");
			fileStream.Flush();
			fileStream.Close();
		}
	}

	public void GetStyleFromComponents(StiComponentsCollection comps, StiStyleElements styleElements)
	{
		for (int num = comps.Count - 1; num >= 0; num--)
		{
			GetStyleFromComponent(comps[num], styleElements);
		}
	}

	public override string ToString()
	{
		if (!string.IsNullOrEmpty(Description))
		{
			return Description;
		}
		return Name;
	}

	private bool ShouldSerializeConditions()
	{
		if (Conditions != null)
		{
			return Conditions.Count > 0;
		}
		return true;
	}

	internal StiBaseStyle(string name, string description, StiReport report)
	{
		Report = report;
		PropName = name;
		Description = description;
	}

	public StiBaseStyle(string name, string description)
		: this(name, description, null)
	{
	}

	public StiBaseStyle(string name)
		: this(name, "")
	{
	}

	public StiBaseStyle()
		: this("")
	{
	}
}
