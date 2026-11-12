using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Printing;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.Design;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.Surface;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Dashboard.Components;

[StiDesigner("Stimulsoft.Dashboard.Design.Designers.StiDashboardDesigner, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter("Stimulsoft.Dashboard.Drawing.Painters.StiDashboardElementGdiPainter, Stimulsoft.Dashboard.Drawing, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Dashboard.Drawing.Wpf.Painters.StiDashboardElementWpfPainter, Stimulsoft.Dashboard.WpfDrawing, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiDashboard : StiPage, IStiDashboard, IStiSurface, IStiPanel, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiBackColor, IStiDashboardElementStyle, IStiDashboardWatermark
{
	private StiElementStyleIdent style = StiElementStyleIdent.Blue;

	private string customStyleName = "";

	private double width = 1200.0;

	private double height = 600.0;

	[Browsable(false)]
	public bool IsMobileSurfaceSelected => DashboardViewMode == StiDashboardViewMode.Mobile;

	[Browsable(false)]
	public bool IsDesktopSurfaceSelected => DashboardViewMode == StiDashboardViewMode.Desktop;

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
	public bool IsDefined => Components.Count > 0;

	[Browsable(false)]
	public bool IsQuerable => true;

	[StiSerializable]
	[TypeConverter(typeof(StiStyleColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionStyleColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("The background color of the component.")]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiOrder(110)]
	[StiPropertyLevel(StiLevel.Basic)]
	public Color BackColor { get; set; } = Color.Transparent;


	[StiSerializable]
	[DefaultValue(StiElementStyleIdent.Blue)]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiOrder(280)]
	[StiPropertyLevel(StiLevel.Basic)]
	[TypeConverter(typeof(StiElementStyleConverter))]
	[Description("Specifies the visual style used to draw this component.")]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiElementStyleEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public StiElementStyleIdent Style
	{
		get
		{
			return style;
		}
		set
		{
			if (style == value)
			{
				return;
			}
			if (value == StiElementStyleIdent.Auto)
			{
				value = Enum.GetValues(typeof(StiElementStyleIdent)).Cast<StiElementStyleIdent>().FirstOrDefault((StiElementStyleIdent c) => c != StiElementStyleIdent.Auto);
			}
			style = value;
			InvokeRefreshControl("Style");
		}
	}

	[StiSerializable]
	[DefaultValue("")]
	[Browsable(false)]
	public string CustomStyleName
	{
		get
		{
			return customStyleName;
		}
		set
		{
			if (customStyleName != value)
			{
				customStyleName = value;
				if (!string.IsNullOrWhiteSpace(value))
				{
					style = StiElementStyleIdent.Custom;
				}
				InvokeRefreshControl("CustomStyleName");
			}
		}
	}

	[StiSerializable(StiSerializationVisibility.Class)]
	[Browsable(false)]
	public virtual StiAdvancedWatermark DashboardWatermark { get; set; } = new StiAdvancedWatermark();


	[Browsable(false)]
	public override string ServiceCategory => Loc.Get("Permissions", "ItemDashboards");

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiDashboard;

	[Browsable(false)]
	public override string LocalizedName => Loc.Get("Components", "StiDashboard");

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiMargins Margins
	{
		get
		{
			return new StiMargins(10.0, 10.0, 10.0, 10.0);
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool MirrorMargins
	{
		get
		{
			return base.MirrorMargins;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool PrintHeadersFootersFromPreviousPage
	{
		get
		{
			return base.PrintHeadersFootersFromPreviousPage;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool PrintOnPreviousPage
	{
		get
		{
			return base.PrintOnPreviousPage;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override int StopBeforePrint
	{
		get
		{
			return base.StopBeforePrint;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool LargeHeight
	{
		get
		{
			return base.LargeHeight;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override int LargeHeightFactor
	{
		get
		{
			return base.LargeHeightFactor;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool ResetPageNumber
	{
		get
		{
			return base.ResetPageNumber;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiExcelSheetExpression ExcelSheet
	{
		get
		{
			return base.ExcelSheet;
		}
		set
		{
		}
	}

	[Browsable(false)]
	public override double GridSize => Report.Info.GridSizePoints;

	[Browsable(false)]
	public override StiUnit Unit => StiUnit.HundredthsOfInch;

	[StiNonSerialized]
	[Browsable(false)]
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
	public override PaperSourceKind PaperSourceFirstPage
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.PaperSourceFirstPage;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.PaperSourceFirstPage = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override string PaperSourceOfFirstPage
	{
		get
		{
			return base.PaperSourceOfFirstPage;
		}
		set
		{
			base.PaperSourceOfFirstPage = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override PaperSourceKind PaperSourceOtherPages
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.PaperSourceOtherPages;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.PaperSourceOtherPages = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override string PaperSourceOfOtherPages
	{
		get
		{
			return base.PaperSourceOfOtherPages;
		}
		set
		{
			base.PaperSourceOfOtherPages = value;
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
	public override PaperKind PaperSize
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.PaperSize;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.PaperSize = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override int NumberOfCopies
	{
		get
		{
			return base.NumberOfCopies;
		}
		set
		{
			base.NumberOfCopies = value;
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
	public override StiBorder Border
	{
		get
		{
			return base.Border;
		}
		set
		{
			base.Border = value;
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

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool UnlimitedBreakable
	{
		get
		{
			return base.UnlimitedBreakable;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool Skip
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool StretchToPrintArea
	{
		get
		{
			return base.StretchToPrintArea;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool TitleBeforeHeader
	{
		get
		{
			return base.TitleBeforeHeader;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool UnlimitedHeight
	{
		get
		{
			return base.UnlimitedHeight;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool UnlimitedWidth
	{
		get
		{
			return base.UnlimitedWidth;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiPageOrientation Orientation
	{
		get
		{
			return base.Orientation;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override double PageWidth
	{
		get
		{
			return base.PageWidth;
		}
		set
		{
			base.PageWidth = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override double PageHeight
	{
		get
		{
			return base.PageHeight;
		}
		set
		{
			base.PageHeight = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override int SegmentPerWidth
	{
		get
		{
			return base.SegmentPerWidth;
		}
		set
		{
			base.SegmentPerWidth = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override int SegmentPerHeight
	{
		get
		{
			return base.SegmentPerHeight;
		}
		set
		{
			base.SegmentPerHeight = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override double ColumnGaps
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
	public sealed override double ColumnWidth
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
	public sealed override int Columns
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
	public sealed override StiToolTipExpression ToolTip
	{
		get
		{
			return base.ToolTip;
		}
		set
		{
			base.ToolTip = value;
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

	[StiEventHide]
	[StiNonSerialized]
	public sealed override StiBeginRenderEvent BeginRenderEvent
	{
		get
		{
			return base.BeginRenderEvent;
		}
		set
		{
			base.BeginRenderEvent = value;
		}
	}

	[StiEventHide]
	[StiNonSerialized]
	public sealed override StiRenderingEvent RenderingEvent
	{
		get
		{
			return base.RenderingEvent;
		}
		set
		{
			base.RenderingEvent = value;
		}
	}

	[StiEventHide]
	[StiNonSerialized]
	public sealed override StiEndRenderEvent EndRenderEvent
	{
		get
		{
			return base.EndRenderEvent;
		}
		set
		{
			base.EndRenderEvent = value;
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
	public sealed override StiColumnBeginRenderEvent ColumnBeginRenderEvent
	{
		get
		{
			return base.ColumnBeginRenderEvent;
		}
		set
		{
			base.ColumnBeginRenderEvent = value;
		}
	}

	[StiEventHide]
	[StiNonSerialized]
	public sealed override StiColumnEndRenderEvent ColumnEndRenderEvent
	{
		get
		{
			return base.ColumnEndRenderEvent;
		}
		set
		{
			base.ColumnEndRenderEvent = value;
		}
	}

	[StiEventHide]
	[StiNonSerialized]
	public sealed override StiGetExcelSheetEvent GetExcelSheetEvent
	{
		get
		{
			return base.GetExcelSheetEvent;
		}
		set
		{
			base.GetExcelSheetEvent = value;
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

	[StiNonSerialized]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiWatermarkEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiOrder(360)]
	[StiPropertyLevel(StiLevel.Standard)]
	public sealed override StiWatermark Watermark
	{
		get
		{
			return base.Watermark;
		}
		set
		{
			base.Watermark = value;
		}
	}

	[StiCategory("Dashboard", StiCategorySource.PropertyCategory)]
	[StiOrder(101)]
	[StiSerializable]
	[DefaultValue(1200.0)]
	[Description("Gets or sets the total width of the dashboard.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiPropertyLevel(StiLevel.Basic)]
	[Browsable(true)]
	public override double Width
	{
		get
		{
			return width;
		}
		set
		{
			width = Math.Round(value, 2);
			width = Math.Max(50.0, width);
			width = Math.Min(10000.0, width);
		}
	}

	[StiCategory("Dashboard", StiCategorySource.PropertyCategory)]
	[StiOrder(102)]
	[StiSerializable]
	[DefaultValue(600.0)]
	[Description("Gets or sets the total height of the dashboard.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiPropertyLevel(StiLevel.Basic)]
	[Browsable(true)]
	public override double Height
	{
		get
		{
			return height;
		}
		set
		{
			height = Math.Round(value, 2);
			height = Math.Max(50.0, height);
			height = Math.Min(10000.0, height);
		}
	}

	[StiSerializable]
	[Browsable(false)]
	public SizeD AltSize { get; set; } = SizeD.Empty;


	[StiSerializable]
	[Browsable(false)]
	[DefaultValue(StiDashboardViewMode.Desktop)]
	public StiDashboardViewMode DashboardViewMode { get; set; }

	[StiSerializable]
	[Browsable(false)]
	[DefaultValue(StiDashboardContentAlignment.StretchXY)]
	public StiDashboardContentAlignment ContentAlignment { get; set; } = StiDashboardContentAlignment.StretchXY;


	[StiSerializable]
	[Browsable(false)]
	[DefaultValue(StiDashboardContentAlignment.StretchXY)]
	public StiDashboardContentAlignment AltContentAlignment { get; set; } = StiDashboardContentAlignment.StretchXY;


	[StiCategory("Dashboard", StiCategorySource.PropertyCategory)]
	[StiOrder(103)]
	[StiSerializable]
	[DefaultValue(480)]
	[Description("Gets or sets the mobile width of the dashboard.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiPropertyLevel(StiLevel.Professional)]
	[Browsable(true)]
	public int DeviceWidth { get; set; } = 480;


	[Browsable(false)]
	public bool IsMobileSurfacePresent => !AltSize.IsEmpty;

	public override object Clone(bool cloneProperties)
	{
		StiDashboard stiDashboard = base.Clone(cloneProperties) as StiDashboard;
		stiDashboard.DashboardWatermark = DashboardWatermark?.Clone() as StiAdvancedWatermark;
		return stiDashboard;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.RemoveProperty("Margins");
		jObject.RemoveProperty("MirrorMargins");
		jObject.RemoveProperty("PrintHeadersFootersFromPreviousPage");
		jObject.RemoveProperty("PrintOnPreviousPage");
		jObject.RemoveProperty("StopBeforePrint");
		jObject.RemoveProperty("LargeHeight");
		jObject.RemoveProperty("LargeHeightFactor");
		jObject.RemoveProperty("ResetPageNumber");
		jObject.RemoveProperty("Watermark");
		jObject.RemoveProperty("ExcelSheet");
		jObject.RemoveProperty("CanBreak");
		jObject.RemoveProperty("Interaction");
		jObject.RemoveProperty("PaperSourceFirstPage");
		jObject.RemoveProperty("PaperSourceOfFirstPage");
		jObject.RemoveProperty("PaperSourceOtherPages");
		jObject.RemoveProperty("PaperSourceOfOtherPages");
		jObject.RemoveProperty("UseParentStyles");
		jObject.RemoveProperty("Conditions");
		jObject.RemoveProperty("PaperSize");
		jObject.RemoveProperty("NumberOfCopies");
		jObject.RemoveProperty("BeginRenderEvent");
		jObject.RemoveProperty("RenderingEvent");
		jObject.RemoveProperty("EndRenderEvent");
		jObject.RemoveProperty("GetToolTipEvent");
		jObject.RemoveProperty("GetTagEvent");
		jObject.RemoveProperty("GetBookmarkEvent");
		jObject.RemoveProperty("GetHyperlinkEvent");
		jObject.RemoveProperty("BeforePrintEvent");
		jObject.RemoveProperty("AfterPrintEvent");
		jObject.RemoveProperty("MouseEnterEvent");
		jObject.RemoveProperty("MouseLeaveEvent");
		jObject.RemoveProperty("ClickEvent");
		jObject.RemoveProperty("DoubleClickEvent");
		jObject.RemoveProperty("ColumnBeginRenderEvent");
		jObject.RemoveProperty("ColumnEndRenderEvent");
		jObject.RemoveProperty("GetExcelSheetEvent");
		jObject.RemoveProperty("GetDrillDownReportEvent");
		jObject.AddPropertyDouble("Width", Width, 1200.0);
		jObject.AddPropertyDouble("Height", Height, 600.0);
		jObject.AddPropertyDouble("DeviceWidth", DeviceWidth, 480.0);
		jObject.AddPropertyEnum("Style", Style, StiElementStyleIdent.Blue);
		jObject.AddPropertyBorder("Border", Border);
		jObject.AddPropertyColor("BackColor", BackColor, Color.Transparent);
		jObject.AddPropertyEnum("DashboardViewMode", DashboardViewMode, StiDashboardViewMode.Desktop);
		jObject.AddPropertyEnum("ContentAlignment", ContentAlignment, StiDashboardContentAlignment.StretchXY);
		jObject.AddPropertyEnum("AltContentAlignment", AltContentAlignment, StiDashboardContentAlignment.StretchXY);
		jObject.AddPropertySizeD("AltSize", AltSize);
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
				Border = item.DeserializeBorder();
				break;
			case "BackColor":
				BackColor = item.DeserializeColor();
				break;
			case "Style":
				Style = item.DeserializeEnum<StiElementStyleIdent>();
				break;
			case "Width":
				Width = item.DeserializeDouble();
				break;
			case "Height":
				Height = item.DeserializeDouble();
				break;
			case "DeviceWidth":
				DeviceWidth = item.DeserializeInt();
				break;
			case "DashboardViewMode":
				DashboardViewMode = item.DeserializeEnum<StiDashboardViewMode>();
				break;
			case "ContentAlignment":
				ContentAlignment = item.DeserializeEnum<StiDashboardContentAlignment>();
				break;
			case "AltContentAlignment":
				AltContentAlignment = item.DeserializeEnum<StiDashboardContentAlignment>();
				break;
			case "AltSize":
				AltSize = item.DeserializeSizeD();
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
		if (group == null)
		{
			return (from c in (nested ? GetComponents() : Components).ToList()
				where c is IStiElement
				select c).Cast<IStiElement>().ToList();
		}
		if (string.IsNullOrWhiteSpace(group))
		{
			return (from IStiElement c in from c in (nested ? GetComponents() : Components).ToList()
					where c is IStiElement
					select c
				where string.IsNullOrWhiteSpace(StiGroupElementHelper.GetGroup(c))
				select c).ToList();
		}
		return (from IStiElement c in from c in (from p in Report.Pages.ToList()
					where p.IsDashboard
					select p).SelectMany((StiPage p) => (nested ? p.GetComponents() : p.Components).ToList())
				where c is IStiElement
				select c
			where StiGroupElementHelper.GetGroup(c) == @group
			select c).ToList();
	}

	List<StiDataFilterRule> IStiDashboard.GetUserFilters(IStiElement element)
	{
		if (element == null)
		{
			return null;
		}
		string group = StiGroupElementHelper.GetGroup(element);
		List<IStiElement> elements = GetElements(nested: true, group);
		IStiFilterElement filterElement = element as IStiFilterElement;
		if (filterElement != null)
		{
			if (StiCrossLinkedFilterHelper.IsCrossLinkedFilter(filterElement))
			{
				return null;
			}
			IStiFilterElement stiFilterElement = elements.Where((IStiElement c) => c is IStiFilterElement).Cast<IStiFilterElement>().FirstOrDefault((IStiFilterElement l) => l.GetKey() == filterElement.GetParentKey());
			if (stiFilterElement == null)
			{
				return null;
			}
			List<StiDataFilterRule> list = new List<StiDataFilterRule>();
			List<StiDataFilterRule> list2 = (stiFilterElement as IStiUserFilters)?.UserFilters;
			if (list2 != null)
			{
				list.AddRange(list2);
			}
			list2 = ((IStiDashboard)this).GetUserFilters(stiFilterElement as IStiElement);
			if (list2 != null)
			{
				list.AddRange(list2);
			}
			return list;
		}
		return elements.Where((IStiElement c) => c is IStiUserFilters && (c != element || !(c is IStiSkipOwnFilter) || !((IStiSkipOwnFilter)c).AllowSkipOwnFilter)).Cast<IStiUserFilters>().SelectMany(GetUserFilters)
			.ToList();
	}

	private List<StiDataFilterRule> GetUserFilters(IStiUserFilters filtersElement)
	{
		filtersElement.UserFilters.ForEach(delegate(StiDataFilterRule f)
		{
			f.ElementKey = (filtersElement as IStiElement).Key;
		});
		return filtersElement.UserFilters;
	}

	public List<StiComponent> GetUnplacedElements()
	{
		return (from e in GetComponents().ToList()
			where e.Width == 0.0 || e.Height == 0.0
			select e).Cast<StiComponent>().ToList();
	}

	public void SwitchSurfaceViewMode(StiSurfaceViewMode value)
	{
		StiDashboardViewMode stiDashboardViewMode = ((value != 0) ? StiDashboardViewMode.Mobile : StiDashboardViewMode.Desktop);
		if (DashboardViewMode == stiDashboardViewMode)
		{
			return;
		}
		DashboardViewMode = stiDashboardViewMode;
		bool flag = false;
		if (AltSize.IsEmpty && value == StiSurfaceViewMode.Mobile)
		{
			AltSize = new SizeD(480.0, 800.0);
			AltContentAlignment = ContentAlignment;
			flag = true;
		}
		StiComponentsCollection stiComponentsCollection = GetComponents();
		foreach (StiComponent item in stiComponentsCollection)
		{
			if (!(item is IStiAltProperties stiAltProperties))
			{
				continue;
			}
			RectangleD clientRectangle = item.ClientRectangle;
			item.ClientRectangle = stiAltProperties.AltClientRectangle;
			stiAltProperties.AltClientRectangle = clientRectangle;
			IStiTitleElement stiTitleElement = item as IStiTitleElement;
			if (stiTitleElement?.Title != null)
			{
				bool visible = stiTitleElement.Title.Visible;
				if (!flag)
				{
					stiTitleElement.Title.Visible = stiAltProperties.AltTitleVisible;
				}
				stiAltProperties.AltTitleVisible = visible;
			}
		}
		SizeD altSize = new SizeD(Width, Height);
		Width = AltSize.Width;
		Height = AltSize.Height;
		AltSize = altSize;
		StiDashboardContentAlignment contentAlignment = ContentAlignment;
		ContentAlignment = AltContentAlignment;
		AltContentAlignment = contentAlignment;
	}

	public void RemoveMobileSurface()
	{
		AltSize = SizeD.Empty;
		StiComponentsCollection stiComponentsCollection = GetComponents();
		foreach (StiComponent item in stiComponentsCollection)
		{
			if (item is IStiAltProperties stiAltProperties)
			{
				stiAltProperties.AltClientRectangle = RectangleD.Empty;
			}
		}
		AltContentAlignment = StiDashboardContentAlignment.StretchXY;
	}

	public List<IStiMeter> FetchAllMeters()
	{
		return new List<IStiMeter>();
	}

	public List<IStiMeter> GetMeters()
	{
		return GetMeters(nested: true, null);
	}

	public List<StiPage> GetNestedPages()
	{
		return new List<StiPage>();
	}

	private bool ShouldSerializeBackColor()
	{
		return BackColor != Color.Transparent;
	}

	public List<string> RetrieveUsedDataNames(string group)
	{
		return GetElements(nested: true, group).SelectMany((IStiElement c) => c.RetrieveUsedDataNames(group)).Distinct().ToList();
	}

	public IStiAppDictionary GetDictionary()
	{
		return Report?.Dictionary;
	}

	public IEnumerable<IStiAppDataSource> GetDataSources(IEnumerable<string> dataNames)
	{
		return StiDataSourcePicker.Fetch(this, null, dataNames, GetDictionary()?.FetchDataSources());
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

	public override void Convert(StiUnit oldUnit, StiUnit newUnit, bool isReportSnapshot = false)
	{
	}

	protected void InvokeRefreshControl(string propertyName)
	{
		Report?.InvokeRefreshControl(this, propertyName);
	}

	public string GetKey()
	{
		return Key;
	}

	public override StiComponent CreateNew()
	{
		return new StiDashboard();
	}

	public override void RemoveAllSelected()
	{
		if (base.IsDesigning && IsMobileSurfaceSelected)
		{
			StiComponentsCollection stiComponentsCollection = GetSelectedComponents();
			{
				foreach (StiComponent item in stiComponentsCollection)
				{
					item.ClientRectangle = RectangleD.Empty;
				}
				return;
			}
		}
		base.RemoveAllSelected();
	}

	private bool ShouldSerializeWatermark()
	{
		if (DashboardWatermark != null)
		{
			return !DashboardWatermark.IsDefault;
		}
		return true;
	}

	public StiDashboard()
		: this(null)
	{
	}

	public StiDashboard(StiReport report)
		: base(report)
	{
		Key = StiKeyHelper.GenerateKey();
		Width = 1200.0;
		Height = 600.0;
	}
}
