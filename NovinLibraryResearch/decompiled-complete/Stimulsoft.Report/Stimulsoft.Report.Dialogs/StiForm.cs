using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Dialogs;

[StiServiceBitmap(typeof(StiForm), "Stimulsoft.Report.Dialogs.Bmp.StiForm.gif")]
[StiServiceCategoryBitmap(typeof(StiForm), "Stimulsoft.Report.Dialogs.Bmp.StiReportControl.gif")]
[StiDesigner("Stimulsoft.Report.Dialogs.Design.StiFormDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfFormDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter(typeof(StiFormGdiPainter))]
[StiToolbox(false)]
public class StiForm : StiPage, IStiForm
{
	private static readonly object EventFormClose = new object();

	private static readonly object EventClosedForm = new object();

	private static readonly object EventClosingForm = new object();

	private static readonly object EventLoadForm = new object();

	private Form control;

	private object controlWpf;

	private Point location = new Point(0, 0);

	private DialogResult dialogResult;

	private RightToLeft rightToLeft;

	private string text = "Form";

	private Font font = new Font("Microsoft Sans Serif", 8f);

	private Color backColor = SystemColors.Control;

	private FormStartPosition startPosition = (FormStartPosition)1;

	private FormWindowState windowState;

	private StiFormStartMode startMode;

	private bool visible = true;

	[Browsable(false)]
	public override string ServiceCategory => StiLocalization.Get("Report", "Dialogs");

	public override StiComponentId ComponentId => StiComponentId.StiForm;

	[Browsable(false)]
	public override string LocalizedName => StiLocalization.Get("Dialogs", "StiForm");

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiMargins Margins
	{
		get
		{
			return new StiMargins(4.0, 4.0, 20.0, 4.0);
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
	public sealed override StiWatermark Watermark
	{
		get
		{
			return base.Watermark;
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
	public override double Zoom => 1.0;

	[Browsable(false)]
	public override double GridSize => Report.Info.GridSizePixels;

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

	[StiSerializable]
	[DefaultValue("")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a style of a component.")]
	[Editor("Stimulsoft.Report.Design.StiDialogStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiOrder(220)]
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
			return base.Brush;
		}
		set
		{
			base.Brush = value;
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
	public sealed override StiPointerExpression Pointer
	{
		get
		{
			return base.Pointer;
		}
		set
		{
			base.Pointer = value;
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

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool Enabled
	{
		get
		{
			return base.Enabled;
		}
		set
		{
			base.Enabled = value;
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

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event Click.")]
	public override StiClickEvent ClickEvent
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

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public override StiGetTagEvent GetTagEvent
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

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event ClosedForm.")]
	public virtual StiClosedFormEvent ClosedFormEvent
	{
		get
		{
			return new StiClosedFormEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event ClosingForm.")]
	public virtual StiClosingFormEvent ClosingFormEvent
	{
		get
		{
			return new StiClosingFormEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event LoadForm.")]
	public virtual StiLoadFormEvent LoadFormEvent
	{
		get
		{
			return new StiLoadFormEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[Browsable(false)]
	public Form Control
	{
		get
		{
			return control;
		}
		set
		{
			control = value;
		}
	}

	[Browsable(false)]
	public object ControlWpf
	{
		get
		{
			return controlWpf;
		}
		set
		{
			controlWpf = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public new int Width
	{
		get
		{
			return Size.Width;
		}
		set
		{
			Size = new Size(value, Size.Height);
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public new int Height
	{
		get
		{
			return Size.Height;
		}
		set
		{
			Size = new Size(Size.Width, value);
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public new int Left
	{
		get
		{
			return Location.X;
		}
		set
		{
			Location = new Point(value, Location.Y);
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public new int Top
	{
		get
		{
			return Location.Y;
		}
		set
		{
			Location = new Point(Location.X, value);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	[Description("Gets or sets the size of the form.")]
	[StiOrder(670)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public Size Size
	{
		get
		{
			return new Size((int)base.PageWidth, (int)base.PageHeight);
		}
		set
		{
			base.PageWidth = value.Width;
			base.PageHeight = value.Height;
			UpdateReportControl("Size");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	[Description("Gets or sets the coordinates of the upper-left corner of the form.")]
	[StiOrder(480)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public Point Location
	{
		get
		{
			return location;
		}
		set
		{
			location = value;
			UpdateReportControl("Location");
		}
	}

	[Browsable(false)]
	[Description("Gets or sets the dialog result for the form.")]
	[StiOrder(260)]
	public DialogResult DialogResult
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return dialogResult;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			dialogResult = value;
			UpdateReportControl("DialogResult");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets a value indicating whether control's elements are aligned to support locales using right-to-left fonts.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(600)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public new RightToLeft RightToLeft
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return rightToLeft;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			rightToLeft = value;
			UpdateReportControl("RightToLeft");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the text associated with this control.")]
	[StiOrder(730)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual string Text
	{
		get
		{
			return text;
		}
		set
		{
			text = value;
			UpdateReportControl("Text");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets font of control.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(330)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Font Font
	{
		get
		{
			return font;
		}
		set
		{
			font = value;
			UpdateReportControl("Font");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the background color for the control.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(140)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Color BackColor
	{
		get
		{
			return backColor;
		}
		set
		{
			backColor = value;
			UpdateReportControl("BackColor");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets the starting position of the form at run time.")]
	[StiOrder(710)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual FormStartPosition StartPosition
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return startPosition;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			startPosition = value;
			UpdateReportControl("StartPosition");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets the form's window state.")]
	[StiOrder(810)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual FormWindowState WindowState
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return windowState;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			windowState = value;
			UpdateReportControl("WindowState");
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(720)]
	[Browsable(true)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override StiTagExpression Tag
	{
		get
		{
			return base.Tag;
		}
		set
		{
			base.Tag = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(StiFormStartMode.OnStart)]
	[Description("Gets or sets value which indicates time when form appears.")]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiOrder(700)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiFormStartMode StartMode
	{
		get
		{
			return startMode;
		}
		set
		{
			startMode = value;
			UpdateReportControl("StartMode");
		}
	}

	[StiSerializable]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value indicating whether the control is displayed.")]
	[StiOrder(790)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool Visible
	{
		get
		{
			return visible;
		}
		set
		{
			visible = value;
			UpdateReportControl("Visible");
		}
	}

	public event StiReportControlUpdateEventHandler ReportControlUpdate;

	public event EventHandler FormClose
	{
		add
		{
			base.Events.AddHandler(EventFormClose, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventFormClose, value);
		}
	}

	public event EventHandler ClosedForm
	{
		add
		{
			base.Events.AddHandler(EventClosedForm, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventClosedForm, value);
		}
	}

	public event CancelEventHandler ClosingForm
	{
		add
		{
			base.Events.AddHandler(EventClosingForm, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventClosingForm, value);
		}
	}

	public event EventHandler LoadForm
	{
		add
		{
			base.Events.AddHandler(EventLoadForm, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventLoadForm, value);
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "Margins");
		JsonExtensions.RemoveProperty(val, "PrintHeadersFootersFromPreviousPage");
		JsonExtensions.RemoveProperty(val, "PrintOnPreviousPage");
		JsonExtensions.RemoveProperty(val, "StopBeforePrint");
		JsonExtensions.RemoveProperty(val, "LargeHeight");
		JsonExtensions.RemoveProperty(val, "LargeHeightFactor");
		JsonExtensions.RemoveProperty(val, "ResetPageNumber");
		JsonExtensions.RemoveProperty(val, "Watermark");
		JsonExtensions.RemoveProperty(val, "ExcelSheet");
		JsonExtensions.RemoveProperty(val, "CanBreak");
		JsonExtensions.RemoveProperty(val, "Interaction");
		JsonExtensions.RemoveProperty(val, "PaperSourceFirstPage");
		JsonExtensions.RemoveProperty(val, "PaperSourceOfFirstPage");
		JsonExtensions.RemoveProperty(val, "PaperSourceOtherPages");
		JsonExtensions.RemoveProperty(val, "PaperSourceOfOtherPages");
		JsonExtensions.RemoveProperty(val, "UseParentStyles");
		JsonExtensions.RemoveProperty(val, "Conditions");
		JsonExtensions.RemoveProperty(val, "PaperSize");
		JsonExtensions.RemoveProperty(val, "NumberOfCopies");
		JsonExtensions.RemoveProperty(val, "PrintOn");
		JsonExtensions.RemoveProperty(val, "Border");
		JsonExtensions.RemoveProperty(val, "UnlimitedBreakable");
		JsonExtensions.RemoveProperty(val, "Skip");
		JsonExtensions.RemoveProperty(val, "StretchToPrintArea");
		JsonExtensions.RemoveProperty(val, "TitleBeforeHeader");
		JsonExtensions.RemoveProperty(val, "UnlimitedHeight");
		JsonExtensions.RemoveProperty(val, "UnlimitedWidth");
		JsonExtensions.RemoveProperty(val, "Orientation");
		JsonExtensions.RemoveProperty(val, "PageWidth");
		JsonExtensions.RemoveProperty(val, "PageHeight");
		JsonExtensions.RemoveProperty(val, "SegmentPerWidth");
		JsonExtensions.RemoveProperty(val, "SegmentPerHeight");
		JsonExtensions.RemoveProperty(val, "ColumnGaps");
		JsonExtensions.RemoveProperty(val, "ColumnWidth");
		JsonExtensions.RemoveProperty(val, "Columns");
		JsonExtensions.RemoveProperty(val, "CanShrink");
		JsonExtensions.RemoveProperty(val, "CanGrow");
		JsonExtensions.RemoveProperty(val, "Printable");
		JsonExtensions.RemoveProperty(val, "Pointer");
		JsonExtensions.RemoveProperty(val, "Bookmark");
		JsonExtensions.RemoveProperty(val, "ToolTip");
		JsonExtensions.RemoveProperty(val, "Hyperlink");
		JsonExtensions.RemoveProperty(val, "Enabled");
		JsonExtensions.RemoveProperty(val, "BeginRenderEvent");
		JsonExtensions.RemoveProperty(val, "RenderingEvent");
		JsonExtensions.RemoveProperty(val, "EndRenderEvent");
		JsonExtensions.RemoveProperty(val, "GetToolTipEvent");
		JsonExtensions.RemoveProperty(val, "GetPointerEvent");
		JsonExtensions.RemoveProperty(val, "GetBookmarkEvent");
		JsonExtensions.RemoveProperty(val, "GetHyperlinkEvent");
		JsonExtensions.RemoveProperty(val, "BeforePrintEvent");
		JsonExtensions.RemoveProperty(val, "AfterPrintEvent");
		JsonExtensions.RemoveProperty(val, "MouseEnterEvent");
		JsonExtensions.RemoveProperty(val, "MouseLeaveEvent");
		JsonExtensions.RemoveProperty(val, "ColumnBeginRenderEvent");
		JsonExtensions.RemoveProperty(val, "ColumnEndRenderEvent");
		JsonExtensions.RemoveProperty(val, "GetExcelSheetEvent");
		JsonExtensions.RemoveProperty(val, "GetDrillDownReportEvent");
		JsonExtensions.AddPropertyJObject(val, "ClosedFormEvent", ClosedFormEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "ClosingFormEvent", ClosingFormEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "LoadFormEvent", LoadFormEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertySize(val, "Size", Size);
		JsonExtensions.AddPropertyPoint(val, "Location", Location);
		JsonExtensions.AddPropertyString(val, "Text", Text);
		JsonExtensions.AddPropertyFontMicrosoftSansSerif8(val, "Font", Font);
		JsonExtensions.AddPropertyColor(val, "BackColor", BackColor, SystemColors.Control);
		JsonExtensions.AddPropertyEnum(val, "StartPosition", (Enum)(object)StartPosition, (Enum)(object)(FormStartPosition)1);
		JsonExtensions.AddPropertyEnum(val, "WindowState", (Enum)(object)WindowState, (Enum)(object)(FormWindowState)0);
		JsonExtensions.AddPropertyEnum(val, "StartMode", (Enum)StartMode, (Enum)StiFormStartMode.OnStart);
		JsonExtensions.AddPropertyBool(val, "Visible", Visible, true, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "ClosedFormEvent":
				ClosedFormEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "ClosingFormEvent":
				ClosingFormEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "LoadFormEvent":
				LoadFormEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Size":
				Size = StiJsonReportObjectHelper.DeserializeSize(item);
				break;
			case "Location":
				location = StiJsonReportObjectHelper.DeserializePoint(item);
				break;
			case "Text":
				text = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Font":
				font = StiJsonReportObjectHelper.DeserializeFont(item, font);
				break;
			case "BackColor":
				backColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "StartPosition":
				startPosition = StiJsonReportObjectHelper.DeserializeEnum<FormStartPosition>(item);
				break;
			case "WindowState":
				windowState = StiJsonReportObjectHelper.DeserializeEnum<FormWindowState>(item);
				break;
			case "StartMode":
				startMode = StiJsonReportObjectHelper.DeserializeEnum<StiFormStartMode>(item);
				break;
			case "Visible":
				visible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public override List<StiFont> GetFonts()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		List<StiFont> fonts = base.GetFonts();
		fonts.Add(new StiFont(Font));
		return fonts.Distinct().ToList();
	}

	public override object Clone()
	{
		return base.Clone();
	}

	public override StiEventsCollection GetEvents()
	{
		StiEventsCollection stiEventsCollection = new StiEventsCollection();
		if (ClickEvent != null)
		{
			stiEventsCollection.Add(ClickEvent);
		}
		if (GetTagEvent != null)
		{
			stiEventsCollection.Add(GetTagEvent);
		}
		if (ClosedFormEvent != null)
		{
			stiEventsCollection.Add(ClosedFormEvent);
		}
		if (ClosingFormEvent != null)
		{
			stiEventsCollection.Add(ClosingFormEvent);
		}
		if (LoadFormEvent != null)
		{
			stiEventsCollection.Add(LoadFormEvent);
		}
		return stiEventsCollection;
	}

	public override void Convert(StiUnit oldUnit, StiUnit newUnit, bool isReportSnapshot = false)
	{
	}

	public void InvokeReportControlUpdate(object sender, string propertyName)
	{
		InvokeReportControlUpdate(sender, new StiReportControlUpdateEventArgs(propertyName));
	}

	public void InvokeReportControlUpdate(object sender, StiReportControlUpdateEventArgs e)
	{
		if (this.ReportControlUpdate != null)
		{
			this.ReportControlUpdate(sender, e);
		}
	}

	public void Close()
	{
		if (base.Events[EventFormClose] is EventHandler eventHandler)
		{
			eventHandler(this, EventArgs.Empty);
		}
	}

	public void InvokeClosedForm(EventArgs e)
	{
		if (base.Events[EventClosedForm] is EventHandler eventHandler)
		{
			eventHandler(this, e);
		}
		StiBlocklyHelper.InvokeBlockly(Report, this, ClosedFormEvent);
	}

	public void InvokeClosingForm(CancelEventArgs e)
	{
		if (base.Events[EventClosingForm] is CancelEventHandler cancelEventHandler)
		{
			cancelEventHandler(this, e);
		}
		StiBlocklyHelper.InvokeBlockly(Report, this, ClosingFormEvent);
	}

	public void InvokeLoadForm(EventArgs e)
	{
		if (base.Events[EventLoadForm] is EventHandler eventHandler)
		{
			eventHandler(this, e);
		}
		StiBlocklyHelper.InvokeBlockly(Report, this, LoadFormEvent);
	}

	private bool ShouldSerializeBackColor()
	{
		return backColor != SystemColors.Control;
	}

	public override StiComponent CreateNew()
	{
		return new StiForm();
	}

	protected void UpdateReportControl(string propertyName)
	{
		if (Page != null)
		{
			((StiForm)Page).InvokeReportControlUpdate(this, propertyName);
		}
	}

	public DialogResult ShowDialog()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		StiDialogsProvider provider = StiDialogsProvider.GetProvider(Report);
		bool flag = Visible;
		if (!Visible)
		{
			Visible = true;
		}
		try
		{
			return (DialogResult)(provider.RenderForm(this) ? 1 : 2);
		}
		finally
		{
			Visible = flag;
		}
	}

	public StiEvent GetDefaultEvent()
	{
		return LoadFormEvent;
	}

	public StiForm()
		: this(null)
	{
	}

	public StiForm(StiReport report)
		: base(report)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		Size = new Size(304, 232);
	}
}
