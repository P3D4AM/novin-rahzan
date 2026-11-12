using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Dashboard.Export;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.Surface;

namespace Stimulsoft.Dashboard.Components.Panel;

[StiToolbox(true)]
[StiExportTool("Stimulsoft.Dashboard.Export.Tools.StiPanelElementExportTool, Stimulsoft.Dashboard.Export, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiServiceBitmap(typeof(StiElement), "Stimulsoft.Dashboard.Images.Toolbox.StiPanel.png")]
[StiServiceCategoryBitmap(typeof(StiComponent), "Stimulsoft.Dashboard.Images.Toolbox.StiElement.png")]
[StiGdiPainter("Stimulsoft.Dashboard.Drawing.Painters.StiPanelElementGdiPainter, Stimulsoft.Dashboard.Drawing, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Dashboard.Drawing.Wpf.Painters.StiPanelElementWpfPainter, Stimulsoft.Dashboard.Drawing.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiVisualizer("Stimulsoft.Dashboard.Viewer.Visualizers.StiPanelElementVisualizer, Stimulsoft.Dashboard.Viewer, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfVisualizer("Stimulsoft.Dashboard.Viewer.Wpf.Visualizers.StiPanelElementWpfVisualizer, Stimulsoft.Dashboard.Viewer.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiPanelElement : StiPanel, IStiSimpleBorder, IStiSimpleShadow, IStiCornerRadius, IStiPanel, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiBackColor, IStiPadding, IStiMargin, IStiAltProperties, IStiElementKey, IStiRefreshViewerElement, IStiDashboardWatermark
{
	private StiMargin margin = new StiMargin(3.0);

	private StiPadding padding = new StiPadding(0.0);

	private StiSimpleBorder border = new StiSimpleBorder();

	private Color backColor = Color.Transparent;

	private StiSimpleShadow shadow = new StiSimpleShadow();

	private StiCornerRadius cornerRadius = new StiCornerRadius();

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiPanelElement;

	[Browsable(false)]
	public IStiDashboard Dashboard => Page as IStiDashboard;

	[Browsable(false)]
	public virtual bool IsDefined => true;

	[Browsable(false)]
	public bool IsQuerable => false;

	[Browsable(false)]
	public override bool IsEnabled
	{
		get
		{
			if (StiAppExpressionHelper.IsExpressionSpecified(this, "Enabled"))
			{
				return StiAppExpressionParser.ParseBoolExpression(this, "Enabled", allowDataLoading: true);
			}
			return Enabled;
		}
	}

	[Browsable(false)]
	public double Zoom
	{
		get
		{
			if (Report != null && Report.Info != null)
			{
				return Report.Info.Zoom;
			}
			return 1.0;
		}
	}

	[StiSerializable]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(280)]
	[Description("Specifies space between this component and another component's margin.")]
	[StiPropertyLevel(StiLevel.Basic)]
	public virtual StiMargin Margin
	{
		get
		{
			return margin;
		}
		set
		{
			if (margin != value)
			{
				margin = value;
				InvokeRefreshControl("Margin");
			}
		}
	}

	[StiSerializable]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(300)]
	[Description("Specifies the interior spacing of the component.")]
	[StiPropertyLevel(StiLevel.Basic)]
	public virtual StiPadding Padding
	{
		get
		{
			return padding;
		}
		set
		{
			if (padding != value)
			{
				padding = value;
				InvokeRefreshControl("Padding");
			}
		}
	}

	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiSerializable]
	[Description("The appearance and behavior of the component border.")]
	[StiOrder(120)]
	[StiPropertyLevel(StiLevel.Basic)]
	public new StiSimpleBorder Border
	{
		get
		{
			return border;
		}
		set
		{
			if (border != value)
			{
				border = value;
				InvokeRefreshControl("Border");
			}
		}
	}

	[StiSerializable]
	[TypeConverter(typeof(StiStyleColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionStyleColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("The background color of the component.")]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiOrder(110)]
	[StiPropertyLevel(StiLevel.Basic)]
	[StiExpressionAllowed]
	public Color BackColor
	{
		get
		{
			return backColor;
		}
		set
		{
			if (backColor != value)
			{
				backColor = value;
				InvokeRefreshControl("BackColor");
			}
		}
	}

	[StiSerializable]
	[Browsable(false)]
	[DefaultValue(true)]
	public bool AltTitleVisible { get; set; } = true;


	[StiSerializable]
	[Browsable(false)]
	public RectangleD AltClientRectangle { get; set; }

	[Browsable(false)]
	public string Key
	{
		get
		{
			return base.Guid;
		}
		set
		{
			base.Guid = value;
		}
	}

	[Description("Shadow settings of the component.")]
	[StiSerializable]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiOrder(276)]
	public StiSimpleShadow Shadow
	{
		get
		{
			return shadow;
		}
		set
		{
			if (shadow != value)
			{
				shadow = value;
				InvokeRefreshControl("Shadow");
			}
		}
	}

	[Description("Represents the value to which the corners are rounded.")]
	[StiSerializable]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiOrder(150)]
	public StiCornerRadius CornerRadius
	{
		get
		{
			return cornerRadius;
		}
		set
		{
			if (cornerRadius != value)
			{
				cornerRadius = value;
				InvokeRefreshControl("CornerRadius");
			}
		}
	}

	[Browsable(false)]
	public bool AllowRefreshControl { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[Browsable(false)]
	public virtual StiAdvancedWatermark DashboardWatermark { get; set; } = new StiAdvancedWatermark();


	public override int ToolboxPosition => 312;

	public override string LocalizedName => StiLocalization.Get("Components", "StiPanel");

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 300.0, 300.0);

	public override string HelpUrl => "user-manual/dashboards_panel.htm";

	[Browsable(false)]
	[StiNonSerialized]
	public override bool CanBreak
	{
		get
		{
			return base.CanBreak;
		}
		set
		{
			base.CanBreak = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override bool RightToLeft
	{
		get
		{
			return base.RightToLeft;
		}
		set
		{
			base.RightToLeft = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override double ColumnGaps
	{
		get
		{
			return base.ColumnGaps;
		}
		set
		{
			base.ColumnGaps = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override double ColumnWidth
	{
		get
		{
			return base.ColumnWidth;
		}
		set
		{
			base.ColumnWidth = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override int Columns
	{
		get
		{
			return base.Columns;
		}
		set
		{
			base.Columns = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override bool Printable
	{
		get
		{
			return base.Printable;
		}
		set
		{
			base.Printable = value;
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
			if (base.UseParentStyles != value)
			{
				base.UseParentStyles = value;
			}
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override SizeD MinSize
	{
		get
		{
			return base.MinSize;
		}
		set
		{
			base.MinSize = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override SizeD MaxSize
	{
		get
		{
			return base.MaxSize;
		}
		set
		{
			base.MaxSize = value;
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

	[Browsable(false)]
	[StiNonSerialized]
	public override StiBrush Brush
	{
		get
		{
			return new StiSolidBrush(BackColor);
		}
		set
		{
			BackColor = StiBrush.ToColor(value);
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override StiDockStyle DockStyle
	{
		get
		{
			return base.DockStyle;
		}
		set
		{
			base.DockStyle = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override StiPrintOnType PrintOn
	{
		get
		{
			return base.PrintOn;
		}
		set
		{
			base.PrintOn = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool GrowToHeight
	{
		get
		{
			return base.GrowToHeight;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override StiAnchorMode Anchor
	{
		get
		{
			return base.Anchor;
		}
		set
		{
			base.Anchor = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiShiftMode ShiftMode
	{
		get
		{
			return base.ShiftMode;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override double Left
	{
		get
		{
			return base.Left;
		}
		set
		{
			base.Left = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override double Top
	{
		get
		{
			return base.Top;
		}
		set
		{
			base.Top = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override double Width
	{
		get
		{
			return base.Width;
		}
		set
		{
			base.Width = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override double Height
	{
		get
		{
			return base.Height;
		}
		set
		{
			base.Height = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override string ComponentStyle
	{
		get
		{
			return base.ComponentStyle;
		}
		set
		{
			base.ComponentStyle = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool CanShrink
	{
		get
		{
			return base.CanShrink;
		}
		set
		{
			base.CanShrink = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool CanGrow
	{
		get
		{
			return base.CanGrow;
		}
		set
		{
			base.CanGrow = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiBookmarkExpression Bookmark
	{
		get
		{
			return base.Bookmark;
		}
		set
		{
			base.Bookmark = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiHyperlinkExpression Hyperlink
	{
		get
		{
			return base.Hyperlink;
		}
		set
		{
			base.Hyperlink = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiConditionsCollection Conditions
	{
		get
		{
			return base.Conditions;
		}
		set
		{
			base.Conditions = value;
		}
	}

	[StiEventHide]
	[StiNonSerialized]
	public sealed override StiGetPointerEvent GetPointerEvent
	{
		get
		{
			return base.GetPointerEvent;
		}
		set
		{
			base.GetPointerEvent = value;
		}
	}

	[StiEventHide]
	[StiNonSerialized]
	public sealed override StiGetBookmarkEvent GetBookmarkEvent
	{
		get
		{
			return base.GetBookmarkEvent;
		}
		set
		{
			base.GetBookmarkEvent = value;
		}
	}

	[StiEventHide]
	[StiNonSerialized]
	public sealed override StiGetHyperlinkEvent GetHyperlinkEvent
	{
		get
		{
			return base.GetHyperlinkEvent;
		}
		set
		{
			base.GetHyperlinkEvent = value;
		}
	}

	[StiEventHide]
	[StiNonSerialized]
	public sealed override StiGetTagEvent GetTagEvent
	{
		get
		{
			return base.GetTagEvent;
		}
		set
		{
			base.GetTagEvent = value;
		}
	}

	[StiEventHide]
	[StiNonSerialized]
	public sealed override StiGetToolTipEvent GetToolTipEvent
	{
		get
		{
			return base.GetToolTipEvent;
		}
		set
		{
			base.GetToolTipEvent = value;
		}
	}

	[StiEventHide]
	[StiNonSerialized]
	public sealed override StiBeforePrintEvent BeforePrintEvent
	{
		get
		{
			return base.BeforePrintEvent;
		}
		set
		{
			base.BeforePrintEvent = value;
		}
	}

	[StiEventHide]
	[StiNonSerialized]
	public sealed override StiAfterPrintEvent AfterPrintEvent
	{
		get
		{
			return base.AfterPrintEvent;
		}
		set
		{
			base.AfterPrintEvent = value;
		}
	}

	[StiEventHide]
	[StiNonSerialized]
	public sealed override StiGetDrillDownReportEvent GetDrillDownReportEvent
	{
		get
		{
			return base.GetDrillDownReportEvent;
		}
		set
		{
			base.GetDrillDownReportEvent = value;
		}
	}

	[StiEventHide]
	[StiNonSerialized]
	public sealed override StiClickEvent ClickEvent
	{
		get
		{
			return base.ClickEvent;
		}
		set
		{
			base.ClickEvent = value;
		}
	}

	[StiEventHide]
	[StiNonSerialized]
	public sealed override StiDoubleClickEvent DoubleClickEvent
	{
		get
		{
			return base.DoubleClickEvent;
		}
		set
		{
			base.DoubleClickEvent = value;
		}
	}

	[StiEventHide]
	[StiNonSerialized]
	public sealed override StiMouseEnterEvent MouseEnterEvent
	{
		get
		{
			return base.MouseEnterEvent;
		}
		set
		{
			base.MouseEnterEvent = value;
		}
	}

	[StiEventHide]
	[StiNonSerialized]
	public sealed override StiMouseLeaveEvent MouseLeaveEvent
	{
		get
		{
			return base.MouseLeaveEvent;
		}
		set
		{
			base.MouseLeaveEvent = value;
		}
	}

	[Browsable(false)]
	public override string ServiceCategory => Loc.Get("Permissions", "ItemDashboards");

	public override string LocalizedCategory => ServiceCategory;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Dashboards;

	[StiNonSerialized]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiWatermarkEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[Description("The watermark used for the component.")]
	[StiOrder(360)]
	[StiPropertyLevel(StiLevel.Standard)]
	public StiWatermark Watermark
	{
		get
		{
			return new StiWatermark();
		}
		set
		{
		}
	}

	public override object Clone(bool cloneProperties)
	{
		StiPanelElement stiPanelElement = base.Clone(cloneProperties) as StiPanelElement;
		stiPanelElement.Shadow = Shadow?.Clone() as StiSimpleShadow;
		stiPanelElement.CornerRadius = CornerRadius?.Clone() as StiCornerRadius;
		stiPanelElement.Margin = Margin?.Clone() as StiMargin;
		stiPanelElement.Padding = Padding?.Clone() as StiPadding;
		stiPanelElement.Border = Border?.Clone() as StiSimpleBorder;
		stiPanelElement.DashboardWatermark = DashboardWatermark?.Clone() as StiAdvancedWatermark;
		return stiPanelElement;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.RemoveProperty("Border");
		jObject.RemoveProperty("UseParentStyles");
		jObject.RemoveProperty("MinSize");
		jObject.RemoveProperty("MaxSize");
		jObject.RemoveProperty("Interaction");
		jObject.RemoveProperty("PrintOn");
		jObject.RemoveProperty("GrowToHeight");
		jObject.RemoveProperty("ShiftMode");
		jObject.RemoveProperty("CanShrink");
		jObject.RemoveProperty("CanGrow");
		jObject.RemoveProperty("Printable");
		jObject.RemoveProperty("Bookmark");
		jObject.RemoveProperty("Hyperlink");
		jObject.RemoveProperty("Conditions");
		jObject.RemoveProperty("GetBookmarkEvent");
		jObject.RemoveProperty("GetHyperlinkEvent");
		jObject.RemoveProperty("GetTagEvent");
		jObject.RemoveProperty("GetToolTipEvent");
		jObject.RemoveProperty("BeforePrintEvent");
		jObject.RemoveProperty("AfterPrintEvent");
		jObject.RemoveProperty("ClickEvent");
		jObject.RemoveProperty("DoubleClickEvent");
		jObject.RemoveProperty("MouseEnterEvent");
		jObject.RemoveProperty("MouseLeaveEvent");
		jObject.RemoveProperty("GetDrillDownReportEvent");
		jObject.AddPropertyJObject("Margin", Margin.SaveToJsonObject(mode, 3.0, 3.0, 3.0, 3.0));
		jObject.AddPropertyJObject("Padding", Padding.SaveToJsonObject(mode, 0.0, 0.0, 0.0, 0.0));
		jObject.AddPropertyBorder("Border", Border);
		jObject.AddPropertyColor("BackColor", BackColor, Color.Transparent);
		jObject.AddPropertyRectangleD("AltClientRectangle", AltClientRectangle);
		jObject.AddPropertyBool("AltTitleVisible", AltTitleVisible, defaultValue: true);
		jObject.AddPropertyCornerRadius("CornerRadius", CornerRadius);
		jObject.AddPropertyShadow("Shadow", Shadow);
		jObject.AddPropertyJObject("DashboardWatermark", DashboardWatermark.SaveToJsonObject(mode));
		return jObject;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Border":
				Border = item.DeserializeSimpleBorder();
				break;
			case "BackColor":
				BackColor = item.DeserializeColor();
				break;
			case "Margin":
				Margin.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Padding":
				Padding.LoadFromJsonObject((JObject)item.Value);
				break;
			case "AltClientRectangle":
				AltClientRectangle = item.DeserializeRectangleD();
				break;
			case "AltTitleVisible":
				AltTitleVisible = item.DeserializeBool();
				break;
			case "Shadow":
				Shadow = item.DeserializeSimpleShadow();
				break;
			case "CornerRadius":
				CornerRadius = item.DeserializeCornerRadius();
				break;
			case "DashboardWatermark":
				DashboardWatermark.LoadFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	public List<IStiMeter> GetMeters(bool nested = true, string group = null)
	{
		return GetElements(nested, group).SelectMany((IStiElement c) => c.GetMeters()).ToList();
	}

	public List<IStiElement> GetElements(bool nested = true, string group = null)
	{
		IEnumerable<IStiElement> source = (from c in (nested ? GetComponents() : Components).ToList()
			where c is IStiElement
			select c).Cast<IStiElement>();
		if (group != null)
		{
			return source.Where((IStiElement c) => StiGroupElementHelper.GetGroup(c) == group).ToList();
		}
		return source.Where((IStiElement c) => !(c is IStiGroupElement) || StiGroupElementHelper.GetGroup(c) == null).ToList();
	}

	public List<IStiMeter> FetchAllMeters()
	{
		return new List<IStiMeter>();
	}

	public List<IStiMeter> GetMeters()
	{
		return new List<IStiMeter>();
	}

	public List<StiDataFilterRule> GetFilterRules()
	{
		return new List<StiDataFilterRule>();
	}

	public List<StiPage> GetNestedPages()
	{
		return new List<StiPage>();
	}

	public virtual List<string> RetrieveUsedDataNames(string group)
	{
		return new List<string>();
	}

	public IStiAppDictionary GetDictionary()
	{
		return Report?.Dictionary;
	}

	public IEnumerable<IStiAppDataSource> GetDataSources(IEnumerable<string> dataNames)
	{
		return StiDataSourcePicker.Fetch(this, null, dataNames, GetDictionary()?.FetchDataSources());
	}

	private bool ShouldSerializeMargin()
	{
		if (Margin.Left == 3.0 && Margin.Right == 3.0 && Margin.Top == 3.0)
		{
			return Margin.Bottom != 3.0;
		}
		return true;
	}

	private bool ShouldSerializePadding()
	{
		return !Padding.IsEmpty;
	}

	private bool ShouldSerializeBorder()
	{
		if (Border != null)
		{
			return !Border.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeBackColor()
	{
		return BackColor != Color.Transparent;
	}

	private bool ShouldSerializeShadow()
	{
		if (Shadow != null)
		{
			return !Shadow.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeCornerRadius()
	{
		if (CornerRadius != null)
		{
			return !CornerRadius.IsDefault;
		}
		return true;
	}

	public void InvokeRefreshControl(string propertyName)
	{
		if (AllowRefreshControl && Report != null && !Report.IsExporting)
		{
			Report.InvokeRefreshControl(this, propertyName);
		}
	}

	private bool ShouldSerializeDashboardWatermark()
	{
		if (DashboardWatermark != null)
		{
			return !DashboardWatermark.IsDefault;
		}
		return true;
	}

	public IStiApp GetApp()
	{
		return Report;
	}

	public override bool CanContainIn(StiComponent component)
	{
		return component is StiDashboard;
	}

	private bool ShouldSerializeWatermark()
	{
		if (DashboardWatermark != null)
		{
			return !DashboardWatermark.IsDefault;
		}
		return true;
	}

	public override StiComponent CreateNew()
	{
		return new StiPanelElement();
	}

	public StiPanelElement()
		: this(RectangleD.Empty)
	{
	}

	public StiPanelElement(RectangleD rect)
		: base(rect)
	{
		Key = StiKeyHelper.GenerateKey();
		Brush = new StiEmptyBrush();
		PlaceOnToolbox = true;
	}
}
