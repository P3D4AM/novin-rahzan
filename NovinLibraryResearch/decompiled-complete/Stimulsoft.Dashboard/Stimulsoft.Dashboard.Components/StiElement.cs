using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Dashboard.Components.Panel;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Surface;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Dashboard.Components;

[StiToolbox(true)]
[StiServiceBitmap(typeof(StiElement), "Stimulsoft.Dashboard.Images.Toolbox.StiElement.png")]
[StiServiceCategoryBitmap(typeof(StiComponent), "Stimulsoft.Dashboard.Images.Toolbox.StiElement.png")]
[StiEngine(StiEngineVersion.All)]
[StiDataTransformationDesigner("Stimulsoft.Dashboard.Design.Designers.StiDataTransformationElementDesigner, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiDataTopNDesigner("Stimulsoft.Dashboard.Design.Designers.StiDataTopNElementDesigner, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiDataFiltersDesigner("Stimulsoft.Dashboard.Design.Designers.StiDataFiltersElementDesigner, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiDataSortDesigner("Stimulsoft.Dashboard.Design.Designers.StiDataSortElementDesigner, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiElementInteractionDesigner("Stimulsoft.Dashboard.Design.Designers.StiElementInteractionDesigner, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public abstract class StiElement : StiComponent, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiSimpleBorder, IStiBackColor, IStiPadding, IStiMargin, IStiAltProperties, IStiElementKey, IStiRefreshViewerElement
{
	private StiSimpleBorder border = new StiSimpleBorder();

	private Color backColor = Color.Transparent;

	private StiMargin margin = new StiMargin(3.0);

	private StiPadding padding = new StiPadding(5.0);

	[Browsable(false)]
	public IStiDashboard Dashboard => Page as IStiDashboard;

	[Browsable(false)]
	public virtual bool IsDefined => true;

	[Browsable(false)]
	public virtual bool IsQuerable => true;

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
	public double Zoom => Page?.Zoom ?? 1.0;

	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiSerializable]
	[Description("Appearance and behavior of the component border.")]
	[StiOrder(120)]
	[StiPropertyLevel(StiLevel.Basic)]
	public virtual StiSimpleBorder Border
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
	public virtual Color BackColor
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

	[Browsable(false)]
	public bool AllowRefreshControl { get; set; }

	[Browsable(false)]
	public override string ServiceCategory => Loc.Get("Permissions", "ItemDashboards");

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
	public sealed override bool Printable
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
	public override StiConditionsCollection Conditions
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

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Dashboards;

	public override string LocalizedCategory => ServiceCategory;

	public override string LocalizedName => Loc.Get("Components", "StiDashboard");

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 280.0, 280.0);

	[StiSerializable]
	[StiOrder(170)]
	public override bool Enabled
	{
		get
		{
			return base.Enabled;
		}
		set
		{
			if (base.Enabled != value)
			{
				base.Enabled = value;
				InvokeRefreshControl("Enabled");
			}
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
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
		jObject.AddPropertyJObject("Padding", Padding.SaveToJsonObject(mode, 5.0, 5.0, 5.0, 5.0));
		jObject.AddPropertyBorder("Border", Border);
		jObject.AddPropertyColor("BackColor", BackColor, Color.Transparent);
		jObject.AddPropertyRectangleD("AltClientRectangle", AltClientRectangle);
		jObject.AddPropertyBool("AltTitleVisible", AltTitleVisible, defaultValue: true);
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
			}
		}
	}

	public override void Convert(StiUnit oldUnit, StiUnit newUnit, bool isReportSnapshot = false)
	{
	}

	public virtual List<IStiMeter> FetchAllMeters()
	{
		return new List<IStiMeter>();
	}

	public virtual List<IStiMeter> GetMeters()
	{
		return new List<IStiMeter>();
	}

	public virtual List<string> RetrieveUsedDataNames(string group)
	{
		return RetrieveUsedDataNames();
	}

	public virtual List<string> RetrieveUsedDataNames()
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

	public override object Clone(bool cloneProperties)
	{
		StiElement stiElement = (StiElement)base.Clone(cloneProperties);
		stiElement.Border = (StiSimpleBorder)(Border?.Clone());
		stiElement.Margin = Margin?.Clone() as StiMargin;
		stiElement.Padding = Padding?.Clone() as StiPadding;
		return stiElement;
	}

	private bool ShouldSerializeBorder()
	{
		if (Border != null)
		{
			return !Border.IsDefault;
		}
		return true;
	}

	protected virtual bool ShouldSerializeBackColor()
	{
		return BackColor != Color.Transparent;
	}

	private bool ShouldSerializeMargin()
	{
		if (Margin != null)
		{
			return !Margin.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializePadding()
	{
		if (Padding != null)
		{
			return !Padding.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeAltClientRectangle()
	{
		return !AltClientRectangle.IsEmpty;
	}

	public void InvokeRefreshControl(string propertyName)
	{
		if (AllowRefreshControl && Report != null && !Report.IsExporting)
		{
			Report.InvokeRefreshControl(this, propertyName);
		}
	}

	public override StiEventsCollection GetEvents()
	{
		return new StiEventsCollection();
	}

	public override bool CanContainIn(StiComponent component)
	{
		if (!(component is StiDashboard))
		{
			return component is StiPanelElement;
		}
		return true;
	}

	public virtual List<StiPage> GetNestedPages()
	{
		return null;
	}

	public virtual StiEvent GetDefaultEvent()
	{
		return null;
	}

	public StiElement()
		: this(RectangleD.Empty)
	{
	}

	public StiElement(RectangleD rect)
		: base(rect)
	{
		Key = StiKeyHelper.GenerateKey();
		PlaceOnToolbox = true;
	}
}
