using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.App;
using Stimulsoft.Report.BarCodes;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.SaveLoad;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiPage), "Stimulsoft.Report.Images.Components.StiPage.png")]
[StiDesigner("Stimulsoft.Report.Components.Design.StiPageDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfPageDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter(typeof(StiPageGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiPageWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiV1Builder(typeof(StiPageV1Builder))]
[StiV2Builder(typeof(StiPageV2Builder))]
[StiContextTool(typeof(IStiComponentDesigner))]
public class StiPage : StiPanel, IStiReportPage, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiNonSerialized, IStiResetPageNumber, IStiGlobalizationProvider
{
	private StiPageInfoV1 pageInfoV1;

	private StiPageInfoV2 pageInfoV2;

	private ArrayList selectedComponents;

	private static readonly object EventBeginRender = new object();

	private static readonly object EventRendering = new object();

	private static readonly object EventEndRender = new object();

	private static readonly object EventColumnBeginRender = new object();

	private static readonly object EventColumnEndRender = new object();

	private static readonly object EventGetExcelSheet = new object();

	private PaperKind paperSize;

	private int numberOfCopies = 1;

	private int largeHeightFactor = 4;

	private double largeHeightAutoFactor = 1.0;

	private bool titleBeforeHeader;

	private StiPageOrientation orientation;

	private double pageWidth;

	private double pageHeight;

	private int segmentPerWidth = 1;

	private int segmentPerHeight = 1;

	private StiWatermark watermark;

	private StiReport report;

	private string cacheGuid;

	public override StiComponentId ComponentId => StiComponentId.StiPage;

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(420)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Allows to reset page number on this page.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool ResetPageNumber { get; set; }

	[Browsable(false)]
	[StiNonSerialized]
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

	[Browsable(false)]
	[StiNonSerialized]
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

	[Browsable(false)]
	public StiPageInfoV1 PageInfoV1 => pageInfoV1 ?? (pageInfoV1 = new StiPageInfoV1());

	[Browsable(false)]
	public StiPageInfoV2 PageInfoV2 => pageInfoV2 ?? (pageInfoV2 = new StiPageInfoV2());

	[Browsable(false)]
	public override bool IsAutomaticDock => true;

	[Browsable(false)]
	[StiNonSerialized]
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
	public override SizeD MinSize
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.MinSize;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.MinSize = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override SizeD MaxSize
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.MaxSize;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.MaxSize = value;
		}
	}

	[Browsable(false)]
	public override double Left
	{
		get
		{
			return 0.0;
		}
		set
		{
		}
	}

	[Browsable(false)]
	public override double Top
	{
		get
		{
			return 0.0;
		}
		set
		{
		}
	}

	[Browsable(false)]
	public override double Width
	{
		get
		{
			return (PageWidth - Margins.Left - Margins.Right) * (double)SegmentPerWidth;
		}
		set
		{
			pageWidth = value / (double)SegmentPerWidth + Margins.Left + Margins.Right;
		}
	}

	[Browsable(false)]
	public override double Height
	{
		get
		{
			double num = (pageHeight - Margins.Top - Margins.Bottom) * (double)SegmentPerHeight;
			if (base.IsDesigning)
			{
				num = ((!LargeHeight) ? (num + (pageHeight - Margins.Top - Margins.Bottom) * (LargeHeightAutoFactor - 1.0)) : (num * LargeHeightAutoFactor));
			}
			if (num < 0.0)
			{
				num = 0.0;
			}
			return num;
		}
		set
		{
			pageHeight = value / (double)SegmentPerHeight + Margins.Top + Margins.Bottom;
			if (Page.LargeHeight && base.IsDesigning)
			{
				pageHeight /= LargeHeightFactor;
			}
		}
	}

	[Browsable(false)]
	public override double Right => Width;

	[Browsable(false)]
	public override double Bottom => Height;

	[Browsable(false)]
	[StiNonSerialized]
	public override RectangleD ClientRectangle
	{
		get
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			return new RectangleD(0.0, 0.0, Width, Height);
		}
		set
		{
			Width = ((RectangleD)(ref value)).Width;
			Height = ((RectangleD)(ref value)).Height;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override RectangleD DisplayRectangle => new RectangleD(0.0, 0.0, Width + Margins.Left + Margins.Right, Height + Margins.Top + Margins.Bottom);

	[Browsable(false)]
	public bool DenyDrawSegmentMode { get; set; }

	[Browsable(false)]
	public RectangleD SelectedRectangle { get; set; } = RectangleD.Empty;


	[Browsable(false)]
	public ArrayList SelectedComponents => selectedComponents ?? (selectedComponents = new ArrayList());

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
	public override StiShiftMode ShiftMode
	{
		get
		{
			return StiShiftMode.None;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override bool Printable
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[Browsable(false)]
	public override string LocalizedName
	{
		get
		{
			if (!(StiLocalization.CultureName == "en"))
			{
				return StiLocalization.Get("Components", "StiPage");
			}
			return "Page";
		}
	}

	[StiNonSerialized]
	public override StiPage Page
	{
		get
		{
			return this;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	public override StiContainer Parent
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override StiRestrictions Restrictions
	{
		get
		{
			return base.Restrictions;
		}
		set
		{
			base.Restrictions = value;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when when a page begins to render.")]
	public virtual StiBeginRenderEvent BeginRenderEvent
	{
		get
		{
			return new StiBeginRenderEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when a page rendering.")]
	public virtual StiRenderingEvent RenderingEvent
	{
		get
		{
			return new StiRenderingEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when when a page ends to render.")]
	public virtual StiEndRenderEvent EndRenderEvent
	{
		get
		{
			return new StiEndRenderEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when start render column.")]
	public virtual StiColumnBeginRenderEvent ColumnBeginRenderEvent
	{
		get
		{
			return new StiColumnBeginRenderEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when end render column.")]
	public virtual StiColumnEndRenderEvent ColumnEndRenderEvent
	{
		get
		{
			return new StiColumnEndRenderEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when the ExcelSheet is calculated.")]
	public virtual StiGetExcelSheetEvent GetExcelSheetEvent
	{
		get
		{
			return new StiGetExcelSheetEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[Browsable(false)]
	[Description("Gets or sets name of excel sheet.")]
	[DefaultValue(null)]
	[StiSerializable]
	public string ExcelSheetValue { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable]
	[Description("Gets or sets an expression used for generation name of excel sheet.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiExcelSheetExpression ExcelSheet
	{
		get
		{
			return new StiExcelSheetExpression(this, "ExcelSheet");
		}
		set
		{
			value?.Set(this, "ExcelSheet", value.Value);
		}
	}

	[Browsable(false)]
	public bool IsPrinted { get; set; }

	[Browsable(false)]
	public virtual double Zoom
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

	[Browsable(false)]
	public virtual double GridSize
	{
		get
		{
			if (IsDashboard)
			{
				return Report.Info.GridSizePoints;
			}
			if (IsScreen)
			{
				return Report.Info.GridSizeScreenPoints;
			}
			if (Report.Unit is StiMillimetersUnit)
			{
				return Report.Info.GridSizeMillimeters;
			}
			if (Report.Unit is StiCentimetersUnit)
			{
				return Report.Info.GridSizeCentimetres;
			}
			if (Report.Unit is StiHundredthsOfInchUnit)
			{
				return Report.Info.GridSizeHundredthsOfInch;
			}
			return Report.Info.GridSizeInch;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(400)]
	[Description("Gets or sets value that indicates that the page will start to be rendered on the free space of the previous page.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool PrintOnPreviousPage { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(410)]
	[Description("Gets or sets value which indicates that, on this page, it is necessary to printheaders and footers of the previous page.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool PrintHeadersFootersFromPreviousPage { get; set; }

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[Editor("Stimulsoft.Report.Components.Design.StiPaperSizeEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets the page size.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual PaperKind PaperSize
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return paperSize;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			if (paperSize == value)
			{
				return;
			}
			paperSize = value;
			if ((int)value != 0 && report != null)
			{
				PaperSize paperSizeFromPaperKind = StiPageHelper.GetPaperSizeFromPaperKind(value);
				if (paperSizeFromPaperKind != null)
				{
					SizeD val = StiPageHelper.GetPaperSize(this, paperSizeFromPaperKind);
					PageWidth = ((SizeD)(ref val)).Width;
					PageHeight = ((SizeD)(ref val)).Height;
				}
			}
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Components.Design.StiPaperSourceEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets the paper source for first page. Some printers does not support this feature.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual PaperSourceKind PaperSourceFirstPage
	{
		get
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			if (StiOptions.Print.AllowUsePaperSizesFromPrinterSettings)
			{
				PrinterSettings val = new PrinterSettings();
				foreach (PaperSource paperSource in val.PaperSources)
				{
					PaperSource val2 = paperSource;
					if (val2.SourceName == PaperSourceOfFirstPage)
					{
						return val2.Kind;
					}
				}
				return (PaperSourceKind)257;
			}
			return (PaperSourceKind)257;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Invalid comparison between Unknown and I4
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected O, but got Unknown
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if ((int)value == 257 || !StiOptions.Print.AllowUsePaperSizesFromPrinterSettings)
			{
				return;
			}
			PrinterSettings val = new PrinterSettings();
			foreach (PaperSource paperSource in val.PaperSources)
			{
				PaperSource val2 = paperSource;
				if (val2.Kind == value)
				{
					PaperSourceOfFirstPage = val2.SourceName;
				}
			}
		}
	}

	[StiSerializable]
	[DefaultValue("")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[Editor("Stimulsoft.Report.Components.Design.StiPaperSourceEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets the paper source for first page. Some printers does not support this feature.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual string PaperSourceOfFirstPage { get; set; } = string.Empty;


	[StiNonSerialized]
	[Browsable(false)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Components.Design.StiPaperSourceEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets the paper source for other pages. Some printers does not support this feature.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual PaperSourceKind PaperSourceOtherPages
	{
		get
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			if (StiOptions.Print.AllowUsePaperSizesFromPrinterSettings)
			{
				PrinterSettings val = new PrinterSettings();
				foreach (PaperSource paperSource in val.PaperSources)
				{
					PaperSource val2 = paperSource;
					if (val2.SourceName == PaperSourceOfOtherPages)
					{
						return val2.Kind;
					}
				}
				return (PaperSourceKind)257;
			}
			return (PaperSourceKind)257;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Invalid comparison between Unknown and I4
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected O, but got Unknown
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if ((int)value == 257 || !StiOptions.Print.AllowUsePaperSizesFromPrinterSettings)
			{
				return;
			}
			PrinterSettings val = new PrinterSettings();
			foreach (PaperSource paperSource in val.PaperSources)
			{
				PaperSource val2 = paperSource;
				if (val2.Kind == value)
				{
					PaperSourceOfOtherPages = val2.SourceName;
				}
			}
		}
	}

	[StiSerializable]
	[DefaultValue("")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[Editor("Stimulsoft.Report.Components.Design.StiPaperSourceEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets the paper source for other pages. Some printers does not support this feature.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual string PaperSourceOfOtherPages { get; set; } = string.Empty;


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DefaultValue(1)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(180)]
	[Description("Gets or sets a value of number of copies of the current page.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual int NumberOfCopies
	{
		get
		{
			return numberOfCopies;
		}
		set
		{
			if (numberOfCopies != value)
			{
				if (value < 1)
				{
					throw new ArgumentException("Value of NumberOfCopies must be greater than or equal to 1.");
				}
				numberOfCopies = value;
			}
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(150)]
	[Description("Gets or sets value indicates that when printing of multiple-sheet cross-reports, columns and strings are to be broken.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool UnlimitedBreakable { get; set; } = true;


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(160)]
	[Description("Gets or sets value indicates that this page has in designer large height.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool LargeHeight { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DefaultValue(4)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(170)]
	[Description("Gets or sets large height factor for LargeHeight property of this page.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual int LargeHeightFactor
	{
		get
		{
			return largeHeightFactor;
		}
		set
		{
			if (value >= 1 && value <= 20)
			{
				largeHeightFactor = value;
			}
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[StiNonSerialized]
	public virtual double LargeHeightAutoFactor
	{
		get
		{
			if (LargeHeight)
			{
				return LargeHeightFactor;
			}
			if (StiOptions.Designer.AutoLargeHeight)
			{
				return largeHeightAutoFactor;
			}
			return 1.0;
		}
		set
		{
			if (value < 1.0)
			{
				largeHeightAutoFactor = 1.0;
			}
			if (value >= 1.0 && value <= 21.0)
			{
				largeHeightAutoFactor = value;
			}
		}
	}

	[Browsable(false)]
	public virtual int CurrentWidthSegment { get; set; }

	[Browsable(false)]
	public virtual int CurrentHeightSegment { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DefaultValue(0)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[Description("Gets or sets the page number. When it is reached then stop rendering. If the property is 0 then rendering of the report will be stopped.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual int StopBeforePrint { get; set; }

	[StiNonSerialized]
	[DefaultValue(false)]
	[Browsable(false)]
	public virtual bool Skip { get; set; }

	[StiSerializable]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value, indicates that, when printing, a page stretches into print area.")]
	[StiShowInContextMenu]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool StretchToPrintArea { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value, indicates that it is necessary to put the report title before the page header.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool TitleBeforeHeader
	{
		get
		{
			return titleBeforeHeader;
		}
		set
		{
			if (titleBeforeHeader != value)
			{
				titleBeforeHeader = value;
				Correct();
			}
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(140)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value, indicates that the page has an unlimited height.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool UnlimitedHeight { get; set; }

	[StiSerializable]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value, indicates that the page has an unlimited width.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool UnlimitedWidth { get; set; } = true;


	[Browsable(false)]
	public virtual RectangleD OffsetRectangle { get; set; } = RectangleD.Empty;


	[RefreshProperties(RefreshProperties.All)]
	[StiSerializable]
	[DefaultValue(StiPageOrientation.Portrait)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(150)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets page orientation.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiPageOrientation Orientation
	{
		get
		{
			return orientation;
		}
		set
		{
			double bottom = Margins.Left;
			double right = Margins.Right;
			double right2 = Margins.Top;
			double bottom2 = Margins.Bottom;
			if (orientation != value && report != null && !report.IsSerializing)
			{
				double num = PageHeight;
				PageHeight = PageWidth;
				PageWidth = num;
				Margins = ((value == StiPageOrientation.Landscape) ? new StiMargins(right2, bottom2, right, bottom) : new StiMargins(bottom2, right2, bottom, right));
			}
			orientation = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override bool Locked
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override bool Linked
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[StiSerializable]
	[DefaultValue(827.0)]
	[Description("Gets or sets the total width of the page.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual double PageWidth
	{
		get
		{
			return pageWidth;
		}
		set
		{
			pageWidth = Math.Round(value, 2);
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(140)]
	[StiSerializable]
	[DefaultValue(1169.0)]
	[Description("Gets or sets the total height of the page.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual double PageHeight
	{
		get
		{
			return pageHeight;
		}
		set
		{
			pageHeight = Math.Round(value, 2);
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(160)]
	[StiSerializable]
	[DefaultValue(1)]
	[Description("Gets or sets the number of segments per width.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual int SegmentPerWidth
	{
		get
		{
			return segmentPerWidth;
		}
		set
		{
			if (value > 0)
			{
				segmentPerWidth = value;
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(170)]
	[StiSerializable]
	[DefaultValue(1)]
	[Description("Gets or sets the number of segments per height.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual int SegmentPerHeight
	{
		get
		{
			return segmentPerHeight;
		}
		set
		{
			if (value > 0)
			{
				segmentPerHeight = value;
			}
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(160)]
	[Description("The watermark used for the component.")]
	[Editor("Stimulsoft.Report.Components.Design.StiWatermarkEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiWatermark Watermark
	{
		get
		{
			return watermark ?? (watermark = new StiWatermark());
		}
		set
		{
			watermark = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(170)]
	[Description("Gets or sets page margins.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiMargins Margins { get; set; } = new StiMargins(39.0, 39.0, 39.0, 39.0);


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(90)]
	[Description("Gets or sets value indicates that mirror margins are set.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool MirrorMargins { get; set; }

	[StiSerializable]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(125)]
	[ParenthesizePropertyName(true)]
	[Description("Gets or sets an icon.")]
	[Editor("Stimulsoft.Report.Components.Design.StiReportIconEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiReportIconConverter))]
	public byte[] Icon { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[DefaultValue(null)]
	public override StiReport Report
	{
		get
		{
			return report;
		}
		set
		{
			report = value;
		}
	}

	internal StiDocument Document { get; set; }

	[Browsable(false)]
	public virtual StiUnit Unit
	{
		get
		{
			if (report != null)
			{
				return report.Unit;
			}
			return StiUnit.HundredthsOfInch;
		}
	}

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DefaultValue(null)]
	public StiUnit ReportUnit { get; set; }

	[Browsable(false)]
	public bool LockHighlight { get; set; }

	[Browsable(false)]
	public bool DrillDownActivated { get; set; }

	[Browsable(false)]
	public bool IsForm => this is IStiForm;

	[Browsable(false)]
	public bool IsScreen => this is IStiScreen;

	[Browsable(false)]
	public bool IsDashboard => this is IStiDashboard;

	[Browsable(false)]
	public bool IsDashboardOrScreen
	{
		get
		{
			if (!IsDashboard)
			{
				return IsScreen;
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool IsPage
	{
		get
		{
			if (!IsForm && !IsScreen)
			{
				return !IsDashboard;
			}
			return false;
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public string CacheGuid
	{
		get
		{
			if (cacheGuid == null)
			{
				NewCacheGuid();
			}
			return cacheGuid;
		}
		set
		{
			cacheGuid = value;
		}
	}

	public static event StiPagePaintEventHandler PagePainting;

	public static event StiPagePaintEventHandler PagePainted;

	public event EventHandler BeginRender
	{
		add
		{
			base.Events.AddHandler(EventBeginRender, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventBeginRender, value);
		}
	}

	public event EventHandler Rendering
	{
		add
		{
			base.Events.AddHandler(EventRendering, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventRendering, value);
		}
	}

	public event EventHandler EndRender
	{
		add
		{
			base.Events.AddHandler(EventEndRender, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventEndRender, value);
		}
	}

	public event EventHandler ColumnBeginRender
	{
		add
		{
			base.Events.AddHandler(EventColumnBeginRender, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventColumnBeginRender, value);
		}
	}

	public event EventHandler ColumnEndRender
	{
		add
		{
			base.Events.AddHandler(EventColumnEndRender, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventColumnEndRender, value);
		}
	}

	public event StiGetExcelSheetEventHandler GetExcelSheet
	{
		add
		{
			base.Events.AddHandler(EventGetExcelSheet, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventGetExcelSheet, value);
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "CanShrink");
		JsonExtensions.RemoveProperty(val, "CanGrow");
		JsonExtensions.RemoveProperty(val, "PrintOn");
		JsonExtensions.RemoveProperty(val, "CanBreak");
		JsonExtensions.RemoveProperty(val, "DockStyle");
		JsonExtensions.RemoveProperty(val, "MinSize");
		JsonExtensions.RemoveProperty(val, "MaxSize");
		JsonExtensions.RemoveProperty(val, "ClientRectangle");
		JsonExtensions.RemoveProperty(val, "DisplayRectangle");
		JsonExtensions.RemoveProperty(val, "UseParentStyles");
		JsonExtensions.RemoveProperty(val, "GrowToHeight");
		JsonExtensions.RemoveProperty(val, "ShiftMode");
		JsonExtensions.RemoveProperty(val, "Printable");
		JsonExtensions.RemoveProperty(val, "Page");
		JsonExtensions.RemoveProperty(val, "Parent");
		JsonExtensions.RemoveProperty(val, "Restrictions");
		JsonExtensions.RemoveProperty(val, "Locked");
		JsonExtensions.RemoveProperty(val, "Linked");
		JsonExtensions.AddPropertyBool(val, "ResetPageNumber", ResetPageNumber, false, false);
		JsonExtensions.AddPropertyJObject(val, "BeginRenderEvent", BeginRenderEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "RenderingEvent", RenderingEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "EndRenderEvent", EndRenderEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "ColumnBeginRenderEvent", ColumnBeginRenderEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "ColumnEndRenderEvent", ColumnEndRenderEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetExcelSheetEvent", GetExcelSheetEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ExcelSheetValue", ExcelSheetValue);
		if ((int)mode == 0)
		{
			JsonExtensions.AddPropertyJObject(val, "ExcelSheet", ExcelSheet.SaveToJsonObject(mode));
		}
		JsonExtensions.AddPropertyBool(val, "PrintOnPreviousPage", PrintOnPreviousPage, false, false);
		JsonExtensions.AddPropertyBool(val, "PrintHeadersFootersFromPreviousPage", PrintHeadersFootersFromPreviousPage, false, false);
		JsonExtensions.AddPropertyEnum(val, "PaperSize", (Enum)(object)PaperSize, (Enum)(object)(PaperKind)0);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "PaperSourceOfFirstPage", PaperSourceOfFirstPage);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "PaperSourceOfOtherPages", PaperSourceOfOtherPages);
		JsonExtensions.AddPropertyInt(val, "NumberOfCopies", NumberOfCopies, 1);
		JsonExtensions.AddPropertyBool(val, "UnlimitedBreakable", UnlimitedBreakable, true, false);
		JsonExtensions.AddPropertyBool(val, "LargeHeight", LargeHeight, false, false);
		JsonExtensions.AddPropertyInt(val, "LargeHeightFactor", LargeHeightFactor, 4);
		JsonExtensions.AddPropertyInt(val, "StopBeforePrint", StopBeforePrint, 0);
		JsonExtensions.AddPropertyBool(val, "StretchToPrintArea", StretchToPrintArea, false, false);
		JsonExtensions.AddPropertyBool(val, "TitleBeforeHeader", TitleBeforeHeader, false, false);
		JsonExtensions.AddPropertyBool(val, "UnlimitedHeight", UnlimitedHeight, false, false);
		JsonExtensions.AddPropertyBool(val, "UnlimitedWidth", UnlimitedWidth, true, false);
		JsonExtensions.AddPropertyEnum(val, "Orientation", (Enum)Orientation, (Enum)StiPageOrientation.Portrait);
		JsonExtensions.AddPropertyDouble(val, "PageWidth", PageWidth, 827.0);
		JsonExtensions.AddPropertyDouble(val, "PageHeight", PageHeight, 1169.0);
		JsonExtensions.AddPropertyInt(val, "SegmentPerWidth", SegmentPerWidth, 1);
		JsonExtensions.AddPropertyInt(val, "SegmentPerHeight", SegmentPerHeight, 1);
		JsonExtensions.AddPropertyJObject(val, "Watermark", Watermark.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Margins", Margins.SaveToJsonObject(39.0, 39.0, 39.0, 39.0));
		JsonExtensions.AddPropertyBool(val, "MirrorMargins", MirrorMargins, false, false);
		if (ReportUnit != null)
		{
			JsonExtensions.AddPropertyJObject(val, "ReportUnit", StiUnit.SaveToJsonObject(ReportUnit));
		}
		if (Icon != null)
		{
			JsonExtensions.AddPropertyByteArray(val, "Icon", Icon);
		}
		return val;
	}

	internal void LoadFromJsonInternal(string text)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			JObject jObject = (JObject)JsonConvert.DeserializeObject(text);
			LoadFromJsonObject(jObject);
		}
		catch (Exception e)
		{
			StiLogService.Write(((object)this).GetType(), "Loading report...ERROR");
			StiLogService.Write(((object)this).GetType(), e);
			if (!StiOptions.Engine.HideExceptions || base.IsDesigning)
			{
				throw;
			}
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0621: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_075b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0765: Expected O, but got Unknown
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0545: Expected O, but got Unknown
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Expected O, but got Unknown
		//IL_0743: Unknown result type (might be due to invalid IL or missing references)
		//IL_074d: Expected O, but got Unknown
		//IL_05c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Expected O, but got Unknown
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f8: Expected O, but got Unknown
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Expected O, but got Unknown
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Expected O, but got Unknown
		//IL_077c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0786: Expected O, but got Unknown
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b1: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "ResetPageNumber":
				ResetPageNumber = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "BeginRenderEvent":
				BeginRenderEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "RenderingEvent":
				RenderingEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "EndRenderEvent":
				EndRenderEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "ColumnBeginRenderEvent":
				ColumnBeginRenderEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "ColumnEndRenderEvent":
				ColumnEndRenderEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "GetExcelSheetEvent":
				GetExcelSheetEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "ExcelSheetValue":
				ExcelSheetValue = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ExcelSheet":
				ExcelSheet.LoadFromJsonObject((JObject)item.Value);
				break;
			case "PrintOnPreviousPage":
				PrintOnPreviousPage = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "PrintHeadersFootersFromPreviousPage":
				PrintHeadersFootersFromPreviousPage = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "PaperSize":
				paperSize = StiJsonReportObjectHelper.DeserializeEnum<PaperKind>(item);
				break;
			case "PaperSourceOfFirstPage":
				PaperSourceOfFirstPage = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "PaperSourceOfOtherPages":
				PaperSourceOfOtherPages = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "NumberOfCopies":
				numberOfCopies = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "UnlimitedBreakable":
				UnlimitedBreakable = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "LargeHeight":
				LargeHeight = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "LargeHeightFactor":
				largeHeightFactor = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "StopBeforePrint":
				StopBeforePrint = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "StretchToPrintArea":
				StretchToPrintArea = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "TitleBeforeHeader":
				titleBeforeHeader = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "UnlimitedHeight":
				UnlimitedHeight = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "UnlimitedWidth":
				UnlimitedWidth = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Orientation":
				orientation = StiJsonReportObjectHelper.DeserializeEnum<StiPageOrientation>(item);
				break;
			case "PageWidth":
				pageWidth = StiJsonReportObjectHelper.DeserializeDouble(item);
				break;
			case "PageHeight":
				pageHeight = StiJsonReportObjectHelper.DeserializeDouble(item);
				break;
			case "SegmentPerWidth":
				segmentPerWidth = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "SegmentPerHeight":
				segmentPerHeight = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "Watermark":
				Watermark.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Margins":
				Margins.LoadFromJsonObject((JObject)item.Value);
				break;
			case "MirrorMargins":
				MirrorMargins = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ReportUnit":
				ReportUnit = StiUnit.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Icon":
				Icon = StiJsonReportObjectHelper.DeserializeByteArray(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Invalid comparison between Unknown and I4
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Invalid comparison between Unknown and I4
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Invalid comparison between Unknown and I4
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Invalid comparison between Unknown and I4
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Invalid comparison between Unknown and I4
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Page, new StiPropertyObject[6]
			{
				propertiesHelper.PaperSize(),
				propertiesHelper.PageWidth(),
				propertiesHelper.PageHeight(),
				propertiesHelper.PageOrientation(),
				propertiesHelper.Margins(),
				propertiesHelper.Watermark()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Page, new StiPropertyObject[7]
			{
				propertiesHelper.PaperSize(),
				propertiesHelper.PageWidth(),
				propertiesHelper.PageHeight(),
				propertiesHelper.PageOrientation(),
				propertiesHelper.Margins(),
				propertiesHelper.Watermark(),
				propertiesHelper.NumberOfCopies()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Page, new StiPropertyObject[7]
			{
				propertiesHelper.PaperSize(),
				propertiesHelper.PageWidth(),
				propertiesHelper.PageHeight(),
				propertiesHelper.PageOrientation(),
				propertiesHelper.Margins(),
				propertiesHelper.Watermark(),
				propertiesHelper.NumberOfCopies()
			});
		}
		if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.PageAdditional, new StiPropertyObject[3]
			{
				propertiesHelper.MirrorMargins(),
				propertiesHelper.StopBeforePrint(),
				propertiesHelper.TitleBeforeHeader()
			});
		}
		else if ((int)level == 2)
		{
			stiPropertyCollection.Add(StiPropertyCategories.PageAdditional, new StiPropertyObject[7]
			{
				propertiesHelper.MirrorMargins(),
				propertiesHelper.StopBeforePrint(),
				propertiesHelper.TitleBeforeHeader(),
				propertiesHelper.UnlimitedHeight(),
				propertiesHelper.UnlimitedBreakable(),
				propertiesHelper.SegmentPerWidth(),
				propertiesHelper.SegmentPerHeight()
			});
		}
		stiPropertyCollection.Add(StiPropertyCategories.Columns, new StiPropertyObject[4]
		{
			propertiesHelper.Columns(),
			propertiesHelper.ColumnWidth(),
			propertiesHelper.ColumnGaps(),
			propertiesHelper.RightToLeft()
		});
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[4]
		{
			propertiesHelper.PageBrush(),
			propertiesHelper.Border(),
			propertiesHelper.Conditions(),
			propertiesHelper.ComponentStyle()
		});
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[1] { propertyGrid.PropertiesHelper.Enabled() });
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[5]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.Enabled(),
				propertiesHelper.PrintOnPreviousPage(),
				propertiesHelper.PrintHeadersFootersFromPreviousPage(),
				propertiesHelper.ResetPageNumber()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[2]
			{
				propertiesHelper.Name(),
				propertiesHelper.PageIcon()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[3]
			{
				propertiesHelper.PageName(),
				propertiesHelper.PageAlias(),
				propertiesHelper.PageIcon()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[5]
			{
				propertiesHelper.PageName(),
				propertiesHelper.PageAlias(),
				propertiesHelper.LargeHeight(),
				propertiesHelper.LargeHeightFactor(),
				propertiesHelper.PageIcon()
			});
		}
		if ((int)level == 2)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Export, new StiPropertyObject[1] { propertiesHelper.ExcelSheet() });
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
		stiEventCollection.Add(StiPropertyCategories.RenderEvents, new StiPropertyEventId[5]
		{
			StiPropertyEventId.BeginRenderEvent,
			StiPropertyEventId.ColumnBeginRenderEvent,
			StiPropertyEventId.ColumnEndRenderEvent,
			StiPropertyEventId.EndRenderEvent,
			StiPropertyEventId.RenderingEvent
		});
		stiEventCollection.Add(StiPropertyCategories.ValueEvents, new StiPropertyEventId[3]
		{
			StiPropertyEventId.GetExcelSheetEvent,
			StiPropertyEventId.GetTagEvent,
			StiPropertyEventId.GetToolTipEvent
		});
		return stiEventCollection;
	}

	public double ConvertToHInches(StiUnit unit, double value)
	{
		if (unit is StiCentimetersUnit)
		{
			if (value == 21.0)
			{
				return 827.0;
			}
			if (value == 29.7)
			{
				return 1169.0;
			}
			if (value == 1.0)
			{
				return 39.0;
			}
		}
		else if (unit is StiMillimetersUnit)
		{
			if (value == 210.0)
			{
				return 827.0;
			}
			if (value == 297.0)
			{
				return 1169.0;
			}
			if (value == 10.0)
			{
				return 39.0;
			}
		}
		return unit.ConvertToHInches(value);
	}

	public double ConvertFromHInches(StiUnit unit, double value)
	{
		if (unit is StiCentimetersUnit)
		{
			if (value == 827.0)
			{
				return 21.0;
			}
			if (value == 1169.0)
			{
				return 29.7;
			}
			if (value == 39.0)
			{
				return 1.0;
			}
		}
		else if (unit is StiMillimetersUnit)
		{
			if (value == 827.0)
			{
				return 210.0;
			}
			if (value == 1169.0)
			{
				return 297.0;
			}
			if (value == 39.0)
			{
				return 10.0;
			}
		}
		return unit.ConvertFromHInches(value);
	}

	public override void Convert(StiUnit oldUnit, StiUnit newUnit, bool isReportSnapshot = false)
	{
		PageWidth = ConvertFromHInches(newUnit, ConvertToHInches(oldUnit, PageWidth));
		PageHeight = ConvertFromHInches(newUnit, ConvertToHInches(oldUnit, PageHeight));
		Margins = new StiMargins(ConvertFromHInches(newUnit, ConvertToHInches(oldUnit, Margins.Left)), ConvertFromHInches(newUnit, ConvertToHInches(oldUnit, Margins.Right)), ConvertFromHInches(newUnit, ConvertToHInches(oldUnit, Margins.Top)), ConvertFromHInches(newUnit, ConvertToHInches(oldUnit, Margins.Bottom)));
		ColumnWidth = ConvertFromHInches(newUnit, ConvertToHInches(oldUnit, ColumnWidth));
		ColumnGaps = ConvertFromHInches(newUnit, ConvertToHInches(oldUnit, ColumnGaps));
		foreach (StiComponent component in Components)
		{
			component.Convert(oldUnit, newUnit, isReportSnapshot);
		}
	}

	public override object Clone(bool cloneProperties, bool cloneComponents)
	{
		StiPage stiPage = base.Clone(cloneProperties, cloneComponents) as StiPage;
		stiPage.pageInfoV1 = PageInfoV1.Clone() as StiPageInfoV1;
		stiPage.pageInfoV2 = PageInfoV2.Clone() as StiPageInfoV2;
		stiPage.Margins = (StiMargins)Margins.Clone();
		stiPage.orientation = orientation;
		stiPage.watermark = ((watermark != null) ? ((StiWatermark)watermark.Clone()) : null);
		stiPage.Components = new StiComponentsCollection(stiPage);
		if (cloneComponents)
		{
			foreach (StiComponent component in Components)
			{
				stiPage.Components.Add((StiComponent)((StiService)component).Clone());
			}
			StiComponentsCollection stiComponentsCollection = stiPage.GetComponents();
			foreach (StiComponent item in stiComponentsCollection)
			{
				item.Page = stiPage;
			}
		}
		return stiPage;
	}

	void IStiGlobalizationProvider.SetString(string propertyName, string value)
	{
		if (propertyName == "ExcelSheet")
		{
			ExcelSheet.Value = value;
			return;
		}
		throw new ArgumentException("Property with name " + propertyName);
	}

	string IStiGlobalizationProvider.GetString(string propertyName)
	{
		if (propertyName == "ExcelSheet")
		{
			return ExcelSheet.Value;
		}
		throw new ArgumentException("Property with name " + propertyName);
	}

	string[] IStiGlobalizationProvider.GetAllStrings()
	{
		List<string> list = new List<string> { "ExcelSheet" };
		return list.ToArray();
	}

	public string ParseExpression(string text, bool allowReturnNull = false)
	{
		return StiExpressionHelper.ParseText(this, text, allowReturnNull);
	}

	public RectangleF GetPrinterMargins(Graphics g)
	{
		try
		{
			IntPtr hdc = g.GetHdc();
			int deviceCaps = StiPageWin32.GetDeviceCaps(hdc, 112);
			int deviceCaps2 = StiPageWin32.GetDeviceCaps(hdc, 113);
			int deviceCaps3 = StiPageWin32.GetDeviceCaps(hdc, 8);
			float num = (float)StiPageWin32.GetDeviceCaps(hdc, 4) / 25.4f;
			float num2 = (float)StiPageWin32.GetDeviceCaps(hdc, 6) / 25.4f;
			g.ReleaseHdc(hdc);
			float num3 = (float)deviceCaps3 / num;
			float x = (float)deviceCaps / num3 * 100f;
			float y = (float)deviceCaps2 / num3 * 100f;
			float num4 = num * 100f;
			float num5 = num2 * 100f;
			return new RectangleF(x, y, num4, num5);
		}
		catch
		{
			return new RectangleF(39f, 39f, 39f, 39f);
		}
	}

	[StiEngine(/*Could not decode attribute arguments.*/)]
	internal void ProcessPageAfterRender()
	{
		StiPageV1Builder stiPageV1Builder = StiV1Builder.GetBuilder(typeof(StiPage)) as StiPageV1Builder;
		stiPageV1Builder.ProcessPageAfterRender(this);
	}

	[StiEngine(/*Could not decode attribute arguments.*/)]
	internal void ProcessPageBeforeRender()
	{
		StiPageV1Builder stiPageV1Builder = StiV1Builder.GetBuilder(typeof(StiPage)) as StiPageV1Builder;
		stiPageV1Builder.ProcessPageBeforeRender(this);
	}

	public void Paint(Graphics g)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		((StiComponent)this).Paint(new StiPaintEventArgs(g, RectangleD.Empty));
	}

	public override StiEventsCollection GetEvents()
	{
		StiEventsCollection stiEventsCollection = base.GetEvents();
		if (BeginRenderEvent != null)
		{
			stiEventsCollection.Add(BeginRenderEvent);
		}
		if (RenderingEvent != null)
		{
			stiEventsCollection.Add(RenderingEvent);
		}
		if (EndRenderEvent != null)
		{
			stiEventsCollection.Add(EndRenderEvent);
		}
		if (GetExcelSheetEvent != null)
		{
			stiEventsCollection.Add(GetExcelSheetEvent);
		}
		if (ColumnBeginRenderEvent != null)
		{
			stiEventsCollection.Add(ColumnBeginRenderEvent);
		}
		if (ColumnEndRenderEvent != null)
		{
			stiEventsCollection.Add(ColumnEndRenderEvent);
		}
		return stiEventsCollection;
	}

	public override void InvokeEvents()
	{
		base.InvokeEvents();
		try
		{
			if (Report != null && Report.CalculationMode == StiCalculationMode.Interpretation && ExcelSheetValue == null && ExcelSheet.Value.Length > 0)
			{
				object obj = StiParser.ParseTextValue(ExcelSheet.Value, this);
				if (obj != null)
				{
					ExcelSheetValue = Report.ToString(obj);
				}
			}
			if ((object)base.Events[EventGetExcelSheet] != null && ExcelSheetValue == null)
			{
				StiGetExcelSheetEventArgs stiGetExcelSheetEventArgs = new StiGetExcelSheetEventArgs();
				InvokeGetExcelSheet(this, stiGetExcelSheetEventArgs);
				if (stiGetExcelSheetEventArgs.Value != null)
				{
					ExcelSheetValue = stiGetExcelSheetEventArgs.Value;
				}
			}
			if (PrintOnPreviousPage && PrintHeadersFootersFromPreviousPage && Report != null && Report.RenderedPages.Count > 0)
			{
				ExcelSheetValue = Report.RenderedPages[Report.RenderedPages.Count - 1].ExcelSheetValue;
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "DoEvents...ERROR");
			StiLogService.Write(((object)this).GetType(), ex);
			if (Report != null)
			{
				Report.WriteToReportRenderingMessages(Name + " " + ex.Message);
			}
		}
	}

	public static void InvokePagePainting(StiPage sender, StiPagePaintEventArgs e)
	{
		StiPage.PagePainting?.Invoke(sender, e);
	}

	public static void InvokePagePainted(StiPage sender, StiPagePaintEventArgs e)
	{
		StiPage.PagePainted?.Invoke(sender, e);
	}

	protected virtual void OnBeginRender(EventArgs e)
	{
	}

	public void InvokeBeginRender()
	{
		try
		{
			OnBeginRender(EventArgs.Empty);
			if (base.Events[EventBeginRender] is EventHandler eventHandler)
			{
				eventHandler(this, EventArgs.Empty);
			}
			StiBlocklyHelper.InvokeBlockly(Report, this, BeginRenderEvent, EventArgs.Empty);
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), Name + " InvokeBeginRender...ERROR");
			StiLogService.Write(((object)this).GetType(), Name + " " + ex.Message);
			if (Report != null)
			{
				Report.WriteToReportRenderingMessages(Name + ".BeginRender event error: " + ex.Message);
			}
		}
	}

	protected virtual void OnRendering(EventArgs e)
	{
	}

	public void InvokeRendering()
	{
		OnRendering(EventArgs.Empty);
		if (base.Events[EventRendering] is EventHandler eventHandler)
		{
			eventHandler(this, EventArgs.Empty);
		}
		StiBlocklyHelper.InvokeBlockly(Report, this, RenderingEvent);
	}

	protected virtual void OnEndRender(EventArgs e)
	{
	}

	public void InvokeEndRender()
	{
		OnEndRender(EventArgs.Empty);
		if (base.Events[EventEndRender] is EventHandler eventHandler)
		{
			eventHandler(this, EventArgs.Empty);
		}
		StiBlocklyHelper.InvokeBlockly(Report, this, EndRenderEvent);
	}

	protected virtual void OnColumnBeginRender(EventArgs e)
	{
	}

	public void InvokeColumnBeginRender()
	{
		InvokeColumnBeginRender(this, EventArgs.Empty);
	}

	public void InvokeColumnBeginRender(object sender, EventArgs e)
	{
		OnColumnBeginRender(e);
		if (base.Events[EventColumnBeginRender] is EventHandler eventHandler)
		{
			eventHandler(sender, e);
		}
		StiBlocklyHelper.InvokeBlockly(report, sender, ColumnBeginRenderEvent);
	}

	protected virtual void OnColumnEndRender(EventArgs e)
	{
	}

	public void InvokeColumnEndRender()
	{
		InvokeColumnEndRender(this, EventArgs.Empty);
	}

	public void InvokeColumnEndRender(object sender, EventArgs e)
	{
		OnColumnEndRender(e);
		if (base.Events[EventColumnEndRender] is EventHandler eventHandler)
		{
			eventHandler(sender, e);
		}
		StiBlocklyHelper.InvokeBlockly(report, sender, ColumnEndRenderEvent);
	}

	protected virtual void OnGetExcelSheet(StiGetExcelSheetEventArgs e)
	{
	}

	public virtual void InvokeGetExcelSheet(StiComponent sender, StiGetExcelSheetEventArgs e)
	{
		try
		{
			OnGetExcelSheet(e);
			if (base.Events[EventGetExcelSheet] is StiGetExcelSheetEventHandler stiGetExcelSheetEventHandler)
			{
				stiGetExcelSheetEventHandler(sender, e);
			}
			StiBlocklyHelper.InvokeBlockly(Report, this, GetExcelSheetEvent, e);
		}
		catch (Exception ex)
		{
			string text = $"Expression in ExcelSheet property of '{Name}' can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report.WriteToReportRenderingMessages(text);
		}
	}

	private bool ShouldSerializeWatermark()
	{
		if (watermark != null)
		{
			return !watermark.IsDefault;
		}
		return true;
	}

	public void NewCacheGuid()
	{
		cacheGuid = System.Guid.NewGuid().ToString().Replace("-", "");
	}

	public void Load(StiPageSLService service, Stream stream)
	{
		service.Load(this, stream);
	}

	public void Load(StiPageSLService service, string path)
	{
		using FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
		service.Load(this, fileStream);
		fileStream.Close();
	}

	public void Load(Stream stream)
	{
		StiXmlPageSLService service = new StiXmlPageSLService();
		Load(service, stream);
	}

	public void Load(string path)
	{
		using FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
		Load(fileStream);
		fileStream.Close();
	}

	public void Save(StiPageSLService service, Stream stream)
	{
		service.Save(this, stream);
	}

	public void Save(StiPageSLService service, string path)
	{
		StiFileUtils.ProcessReadOnly(path);
		using FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
		service.Save(this, fileStream);
		fileStream.Close();
	}

	public void Save(Stream stream)
	{
		StiXmlPageSLService service = new StiXmlPageSLService();
		Save(service, stream);
	}

	public void Save(string path)
	{
		StiFileUtils.ProcessReadOnly(path);
		using FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
		Save(fileStream);
		fileStream.Close();
	}

	public void LoadDocument(StiDocumentPageSLService service, Stream stream)
	{
		service.Load(this, stream);
	}

	public void LoadDocument(StiDocumentPageSLService service, string path)
	{
		using FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
		service.Load(this, fileStream);
		fileStream.Close();
	}

	public void LoadDocument(Stream stream)
	{
		StiXmlDocumentPageSLService service = new StiXmlDocumentPageSLService();
		LoadDocument(service, stream);
	}

	public void LoadPackedDocument(Stream stream)
	{
		using Stream stream2 = StiGZipHelper.Unpack(stream);
		StiXmlDocumentPageSLService service = new StiXmlDocumentPageSLService();
		LoadDocument(service, stream2);
	}

	public void LoadDocument(string path)
	{
		using FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
		LoadDocument(fileStream);
		fileStream.Close();
	}

	public void SaveDocument(StiDocumentPageSLService service, Stream stream)
	{
		service.Save(this, stream);
	}

	public void SaveDocument(StiDocumentPageSLService service, string path)
	{
		StiFileUtils.ProcessReadOnly(path);
		using FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
		service.Save(this, fileStream);
		fileStream.Close();
	}

	public void SaveDocument(Stream stream)
	{
		StiXmlDocumentPageSLService service = new StiXmlDocumentPageSLService();
		SaveDocument(service, stream);
	}

	public void SavePackedDocument(Stream stream)
	{
		using Stream stream2 = StiGZipHelper.Pack(stream);
		StiXmlDocumentPageSLService service = new StiXmlDocumentPageSLService();
		SaveDocument(service, stream2);
	}

	public void SaveDocument(string path)
	{
		StiFileUtils.ProcessReadOnly(path);
		using FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
		SaveDocument(fileStream);
		fileStream.Close();
	}

	public DialogResult DesignPage()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return DesignPage((StiGuiMode)0);
	}

	public DialogResult DesignPageWithWpf()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return DesignPage((StiGuiMode)1);
	}

	private DialogResult DesignPage(StiGuiMode guiMode)
	{
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Invalid comparison between Unknown and I4
		//IL_056b: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Invalid comparison between Unknown and I4
		bool flag = false;
		int num = Report.Pages.IndexOf(this);
		if (num == -1)
		{
			flag = true;
			num = Report.RenderedPages.IndexOf(this);
		}
		StiPage stiPage = ((StiService)this).Clone() as StiPage;
		StiReport stiReport = Report;
		StiReport stiReport2 = StiReport.GetReport();
		stiReport2.CalculationMode = StiCalculationMode.Interpretation;
		stiReport2.Unit = stiReport.Unit;
		foreach (StiBaseStyle style in report.Styles)
		{
			stiReport2.Styles.Add((StiBaseStyle)((StiService)style).Clone());
		}
		stiReport2.Pages.Clear();
		stiReport2.Pages.AddV2Internal(stiPage);
		stiPage.Report = stiReport2;
		StiComponentsCollection stiComponentsCollection = stiPage.GetComponents();
		foreach (StiComponent item in stiComponentsCollection)
		{
			item.Page = stiPage;
			item.Report = stiReport2;
			if (item.PointerValue != null)
			{
				item.Pointer = new StiPointerExpression();
				item.Pointer.Value = item.PointerValue.ToString();
			}
			if (item.BookmarkValue != null)
			{
				item.Bookmark = new StiBookmarkExpression();
				item.Bookmark.Value = item.BookmarkValue.ToString();
			}
			if (item.ToolTipValue != null)
			{
				item.ToolTip = new StiToolTipExpression();
				item.ToolTip.Value = item.ToolTipValue.ToString();
			}
			if (item.HyperlinkValue != null)
			{
				item.Hyperlink = new StiHyperlinkExpression();
				item.Hyperlink.Value = item.HyperlinkValue.ToString();
			}
			if (item.TagValue != null)
			{
				item.Tag = new StiTagExpression();
				item.Tag.Value = item.TagValue.ToString();
			}
			if (item.Interaction == null)
			{
				item.Interaction = new StiInteraction();
			}
			if (item is StiImage stiImage)
			{
				if (stiImage.ExistImageToDraw())
				{
					stiImage.PutImage(stiImage.TakeImageToDraw());
				}
				else
				{
					stiImage.PutImageToDraw(stiImage.TakeImage());
				}
			}
			if (item is StiCheckBox stiCheckBox)
			{
				stiCheckBox.Checked = new StiCheckedExpression();
				if (stiCheckBox.CheckedValue != null)
				{
					stiCheckBox.Checked.Value = stiCheckBox.CheckedValue.ToString();
				}
			}
			if (item is StiBarCode stiBarCode)
			{
				stiBarCode.Code = new StiBarCodeExpression();
				stiBarCode.Code.Value = stiBarCode.CodeValue;
			}
			if (item is StiRichText stiRichText)
			{
				stiRichText.DataColumn = string.Empty;
			}
		}
		stiPage.ExcelSheet.Value = stiPage.ExcelSheetValue;
		stiReport2.IsPageDesigner = true;
		bool showEventsTab = StiOptions.Designer.ShowEventsTab;
		bool codeTabVisible = StiOptions.Designer.CodeTabVisible;
		bool previewReportVisible = StiOptions.Designer.PreviewReportVisible;
		bool wpfPreviewReportVisible = StiOptions.Designer.WpfPreviewReportVisible;
		StiOptions.Designer.ShowEventsTab = false;
		StiOptions.Designer.PreviewReportVisible = false;
		StiOptions.Designer.WpfPreviewReportVisible = false;
		StiOptions.Designer.CodeTabVisible = false;
		DialogResult val = (DialogResult)(((int)guiMode != 0) ? (stiReport2.DesignWithWpf() ? 6 : 7) : ((int)stiReport2.Design()));
		if ((int)val == 6)
		{
			stiPage.ExcelSheetValue = stiPage.ExcelSheet.Value;
			if (flag)
			{
				stiReport.RenderedPages[num] = stiPage;
			}
			else
			{
				stiReport.Pages[num] = stiPage;
			}
			stiComponentsCollection = stiPage.GetComponents();
			foreach (StiComponent item2 in stiComponentsCollection)
			{
				item2.Report = stiReport;
				if (item2.Pointer != null && item2.Pointer.Value.Length != 0)
				{
					item2.PointerValue = item2.Pointer.Value;
				}
				else
				{
					item2.PointerValue = null;
				}
				if (item2.Bookmark != null && item2.Bookmark.Value.Length != 0)
				{
					item2.BookmarkValue = item2.Bookmark.Value;
				}
				else
				{
					item2.BookmarkValue = null;
				}
				if (item2.ToolTip != null && item2.ToolTip.Value.Length != 0)
				{
					item2.ToolTipValue = item2.ToolTip.Value;
				}
				else
				{
					item2.ToolTipValue = null;
				}
				if (item2.Hyperlink != null && item2.Hyperlink.Value.Length != 0)
				{
					item2.HyperlinkValue = item2.Hyperlink.Value;
				}
				else
				{
					item2.HyperlinkValue = null;
				}
				if (item2.Tag != null && item2.Tag.Value.Length != 0)
				{
					item2.TagValue = item2.Tag.Value;
				}
				else
				{
					item2.TagValue = null;
				}
				if (item2.Interaction != null && item2.Interaction.IsDefault)
				{
					item2.Interaction = null;
				}
				if (item2 is StiCheckBox stiCheckBox2)
				{
					stiCheckBox2.CheckedValue = stiCheckBox2.Checked.Value;
				}
				if (item2 is StiBarCode stiBarCode2)
				{
					stiBarCode2.CodeValue = stiBarCode2.Code.Value;
				}
			}
			stiPage.Report = stiReport;
			stiPage.ClearPage();
		}
		StiOptions.Designer.ShowEventsTab = showEventsTab;
		StiOptions.Designer.CodeTabVisible = codeTabVisible;
		StiOptions.Designer.PreviewReportVisible = previewReportVisible;
		StiOptions.Designer.WpfPreviewReportVisible = wpfPreviewReportVisible;
		return val;
	}

	private bool GetIsPageTotalDataBand(string name)
	{
		if (report == null || report.DataBandsUsedInPageTotals == null)
		{
			return false;
		}
		string[] dataBandsUsedInPageTotals = report.DataBandsUsedInPageTotals;
		foreach (string text in dataBandsUsedInPageTotals)
		{
			if (text == name || "Breaked_" + text == name || "Continued_" + text == name)
			{
				return true;
			}
		}
		return false;
	}

	internal void ClearPage()
	{
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		Hashtable hashtable = new Hashtable();
		object obj = new object();
		object value = new object();
		Components.SetParent(this);
		if (Height == 100000000000.0 && StiOptions.Engine.RenderExternalSubReportsWithHelpOfUnlimitedHeightPages)
		{
			return;
		}
		RemoveNewPageContainers(this);
		MoveComponentsToPage();
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection(this);
		for (int i = 0; i < Components.Count; i++)
		{
			StiComponent stiComponent = Components[i];
			StiContainer stiContainer = stiComponent as StiContainer;
			bool flag = false;
			if (stiContainer != null)
			{
				if (hashtable[stiContainer.Name] == null)
				{
					flag = GetIsPageTotalDataBand(stiContainer.Name);
					if (flag)
					{
						hashtable[stiContainer.Name] = obj;
					}
					else
					{
						hashtable[stiContainer.Name] = value;
					}
				}
				else if (hashtable[stiContainer.Name] == obj)
				{
					flag = true;
				}
			}
			if (flag || stiContainer == null || stiContainer.TagValue != null || stiContainer.PointerValue != null || stiContainer.BookmarkValue != null || stiContainer.ToolTipValue != null || stiContainer.HyperlinkValue != null || stiContainer.Guid != null || (stiContainer.Interaction != null && !stiContainer.Interaction.IsDefault) || (stiContainer.Border != null && ((int)stiContainer.Border.Side != 0 || stiContainer.Border.DropShadow)) || (stiContainer.Brush != null && (!(stiContainer.Brush is StiSolidBrush) || !(((StiSolidBrush)stiContainer.Brush).Color == Color.Transparent))))
			{
				stiComponentsCollection.Add(stiComponent);
			}
		}
		Components.Clear();
		Components.AddRange(stiComponentsCollection);
		stiComponentsCollection.Clear();
	}

	internal void DisposeImagesAndClearComponents()
	{
		if (StiOptions.Engine.ReportCache.DisposeImagesOnPageClear)
		{
			foreach (StiComponent component in Components)
			{
				if (component is StiImage { imageBytesToDraw: not null } stiImage)
				{
					stiImage.imageBytesToDraw = null;
				}
			}
		}
		Components.Clear();
	}

	private void RemoveNewPageContainers(StiContainer cont)
	{
		bool flag = false;
		for (int num = cont.Components.Count - 1; num >= 0; num--)
		{
			StiComponent stiComponent = cont.Components[num];
			if (stiComponent is StiNewPageContainer)
			{
				cont.Components.RemoveAt(num);
				flag = true;
			}
			else if (stiComponent is StiContainer cont2)
			{
				RemoveNewPageContainers(cont2);
			}
		}
		if (flag)
		{
			if (cont.CanGrow)
			{
				cont.CanShrink = true;
			}
			StiContainerHelper.CheckSize(cont);
		}
	}

	private int GetComponentsCount(StiContainer container)
	{
		int num = container.Components.Count;
		foreach (StiComponent component in container.Components)
		{
			StiContainer stiContainer = component as StiContainer;
			if (component != container && stiContainer != null)
			{
				num += GetComponentsCount(stiContainer);
			}
		}
		return num;
	}

	public override int GetComponentsCount()
	{
		return GetComponentsCount(this);
	}

	public void ResizePage(double factorX, double factorY)
	{
		ResizePage(factorX, factorY, allowPageMarginsRescaling: true);
	}

	public void ResizePage(double factorX, double factorY, bool allowPageMarginsRescaling)
	{
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Expected O, but got Unknown
		if (allowPageMarginsRescaling)
		{
			Margins = new StiMargins((double)Math.Round((decimal)(Margins.Left * factorX), 2), (double)Math.Round((decimal)(Margins.Right * factorX), 2), (double)Math.Round((decimal)(Margins.Top * factorY), 2), (double)Math.Round((decimal)(Margins.Bottom * factorY), 2));
		}
		ColumnWidth *= factorX;
		ColumnGaps *= factorX;
		if (Conditions != null)
		{
			foreach (StiCondition condition in Conditions)
			{
				condition.Font = new Font(condition.Font.Name, condition.Font.Size * (float)factorX, condition.Font.Style);
			}
		}
		StiComponentsCollection stiComponentsCollection = GetComponents();
		foreach (StiComponent item in stiComponentsCollection)
		{
			if (item is StiLinePrimitive)
			{
				((StiLinePrimitive)item).Size *= (float)factorX;
			}
			item.Left *= factorX;
			item.Top *= factorY;
			item.Width *= factorX;
			item.Height *= factorY;
			if (item is IStiFont stiFont)
			{
				stiFont.Font = new Font(stiFont.Font.Name, stiFont.Font.Size * (float)factorX, stiFont.Font.Style);
			}
			if (item is StiDataBand stiDataBand)
			{
				stiDataBand.ColumnWidth *= factorX;
				stiDataBand.ColumnGaps *= factorX;
			}
			if (item is StiPanel stiPanel)
			{
				stiPanel.ColumnWidth *= factorX;
				stiPanel.ColumnGaps *= factorX;
			}
			if (item.Conditions != null)
			{
				foreach (StiCondition condition2 in item.Conditions)
				{
					condition2.Font = new Font(condition2.Font.Name, condition2.Font.Size * (float)factorX, condition2.Font.Style);
				}
			}
			if (item is IStiBorder stiBorder)
			{
				if (stiBorder is StiAdvancedBorder)
				{
					object obj = stiBorder.Border.Clone();
					stiBorder.Border = (StiBorder)((obj is StiBorder) ? obj : null);
					StiBorderSide leftSide = ((StiAdvancedBorder)stiBorder.Border).LeftSide;
					leftSide.Size *= factorX;
					StiBorderSide rightSide = ((StiAdvancedBorder)stiBorder.Border).RightSide;
					rightSide.Size *= factorX;
					StiBorderSide bottomSide = ((StiAdvancedBorder)stiBorder.Border).BottomSide;
					bottomSide.Size *= factorY;
					StiBorderSide topSide = ((StiAdvancedBorder)stiBorder.Border).TopSide;
					topSide.Size *= factorY;
				}
				else
				{
					object obj2 = stiBorder.Border.Clone();
					stiBorder.Border = (StiBorder)((obj2 is StiBorder) ? obj2 : null);
					StiBorder border = stiBorder.Border;
					border.Size *= factorX;
				}
			}
		}
	}

	public static StiServiceContainer GetPageServices()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		StiServiceContainer val = new StiServiceContainer();
		foreach (StiComponent item in StiOptions.Services.Components.Where((StiComponent s) => ((StiService)s).ServiceEnabled))
		{
			if (item is StiPage)
			{
				val.Add((StiService)(object)item);
			}
		}
		return val;
	}

	public override StiComponent CreateNew()
	{
		return StiActivator.CreateObject("Stimulsoft.Report.Components.StiPage") as StiPage;
	}

	public StiPage()
		: this(null)
	{
	}

	public StiPage(StiReport report)
		: base(new RectangleD(0.0, 0.0, 0.0, 0.0))
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			NewGuid();
			Border.Size = 2.0;
			Reset();
			Parent = null;
			PaperSize = StiOptions.Engine.DefaultPaperSize;
			PaperSize val = null;
			if ((int)PaperSize != 0)
			{
				val = StiPageHelper.GetPaperSizeFromPaperKind(PaperSize);
			}
			if (val == null || (int)PaperSize == 0)
			{
				pageWidth = 827.0;
				pageHeight = 1169.0;
			}
			else
			{
				SizeD val2 = StiPageHelper.GetPaperSize(this, val);
				PageWidth = ((SizeD)(ref val2)).Width;
				PageHeight = ((SizeD)(ref val2)).Height;
			}
			this.report = report;
			segmentPerWidth = 1;
			segmentPerHeight = 1;
			Margins = new StiMargins(39.0, 39.0, 39.0, 39.0);
			if (report != null && !report.IsSerializing)
			{
				Convert(StiUnit.HundredthsOfInch, Unit);
			}
		}
		catch
		{
		}
		PlaceOnToolbox = false;
	}
}
