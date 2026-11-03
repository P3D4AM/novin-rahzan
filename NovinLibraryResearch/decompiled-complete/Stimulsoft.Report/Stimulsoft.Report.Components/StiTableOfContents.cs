using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiComponent), "Stimulsoft.Report.Images.Components.StiTableOfContents.png")]
[StiToolbox(true)]
[StiDesigner("Stimulsoft.Report.Components.Design.StiTableOfContentsDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfTableOfContentsDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Report.Painters.StiTableOfContentsWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter(typeof(StiTableOfContentsGdiPainter))]
[StiV2Builder(typeof(StiTableOfContentsV2Builder))]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiEngine(/*Could not decode attribute arguments.*/)]
public class StiTableOfContents : StiDataBand
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiTableOfContents;

	public override int Priority => 300;

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 300.0, 100.0);

	public override int ToolboxPosition => 117;

	public override string LocalizedName => StiLocalization.Get("Components", "StiTableOfContents");

	public override string HelpUrl => "user-manual/index.html?report_internals_tableofcontents.htm";

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override int CountData
	{
		get
		{
			return 1;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool CanBreak
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
	public sealed override bool CanGrow
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
	public sealed override bool CanShrink
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
	public sealed override StiDataSource DataSource => base.DataSource;

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiDataRelation DataRelation => base.DataRelation;

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiComponent MasterComponent
	{
		get
		{
			return base.MasterComponent;
		}
		set
		{
			base.MasterComponent = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiBusinessObject BusinessObject => base.BusinessObject;

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool FilterOn
	{
		get
		{
			return base.FilterOn;
		}
		set
		{
			base.FilterOn = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiFiltersCollection Filters
	{
		get
		{
			return base.Filters;
		}
		set
		{
			base.Filters = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiFilterEngine FilterEngine
	{
		get
		{
			return base.FilterEngine;
		}
		set
		{
			base.FilterEngine = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiFilterMode FilterMode
	{
		get
		{
			return base.FilterMode;
		}
		set
		{
			base.FilterMode = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool SkipFirst
	{
		get
		{
			return base.SkipFirst;
		}
		set
		{
			base.SkipFirst = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override string[] Sort
	{
		get
		{
			return base.Sort;
		}
		set
		{
			base.Sort = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool NewColumnAfter
	{
		get
		{
			return base.NewColumnAfter;
		}
		set
		{
			base.NewColumnAfter = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool NewColumnBefore
	{
		get
		{
			return base.NewColumnBefore;
		}
		set
		{
			base.NewColumnBefore = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override float BreakIfLessThan
	{
		get
		{
			return base.BreakIfLessThan;
		}
		set
		{
			base.BreakIfLessThan = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override string LimitRows
	{
		get
		{
			return base.LimitRows;
		}
		set
		{
			base.LimitRows = value;
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
	public sealed override StiColumnDirection ColumnDirection
	{
		get
		{
			return base.ColumnDirection;
		}
		set
		{
			base.ColumnDirection = value;
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
	public sealed override int MinRowsInColumn
	{
		get
		{
			return base.MinRowsInColumn;
		}
		set
		{
			base.MinRowsInColumn = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override string OddStyle
	{
		get
		{
			return base.OddStyle;
		}
		set
		{
			base.OddStyle = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override string EvenStyle
	{
		get
		{
			return base.EvenStyle;
		}
		set
		{
			base.EvenStyle = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool CalcInvisible
	{
		get
		{
			return base.CalcInvisible;
		}
		set
		{
			base.CalcInvisible = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiKeepDetails KeepDetails
	{
		get
		{
			return base.KeepDetails;
		}
		set
		{
			base.KeepDetails = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool PrintAtBottom
	{
		get
		{
			return base.PrintAtBottom;
		}
		set
		{
			base.PrintAtBottom = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool PrintIfDetailEmpty
	{
		get
		{
			return base.PrintIfDetailEmpty;
		}
		set
		{
			base.PrintIfDetailEmpty = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool PrintOnAllPages
	{
		get
		{
			return base.PrintOnAllPages;
		}
		set
		{
			base.PrintOnAllPages = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiPrintOnType PrintOn
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
	public sealed override bool ResetPageNumber
	{
		get
		{
			return base.ResetPageNumber;
		}
		set
		{
			base.ResetPageNumber = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
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

	[StiEventHide]
	[StiNonSerialized]
	public override StiBeginRenderEvent BeginRenderEvent
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
	public override StiRenderingEvent RenderingEvent
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
	public override StiEndRenderEvent EndRenderEvent
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
	public override StiGetCollapsedEvent GetCollapsedEvent
	{
		get
		{
			return base.GetCollapsedEvent;
		}
		set
		{
			base.GetCollapsedEvent = value;
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
	public override Color HeaderStartColor => StiColor.Get("edc87e");

	[Browsable(false)]
	public override Color HeaderEndColor => StiColor.Get("77edc87e");

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	public override bool NewPageAfter
	{
		get
		{
			return base.NewPageAfter;
		}
		set
		{
			base.NewPageAfter = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	public override bool NewPageBefore
	{
		get
		{
			return base.NewPageBefore;
		}
		set
		{
			base.NewPageBefore = value;
		}
	}

	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets horizontal output direction.")]
	[StiOrder(140)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
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

	internal bool IsFirstInReport
	{
		get
		{
			if (Report == null)
			{
				return false;
			}
			return (from StiComponent c in Report.GetComponents()
				where c is StiTableOfContents && c.Enabled
				select c).ToList().IndexOf(this) == 0;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Design.StiStylesEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiStylesConverter))]
	[Description("Gets a collection which consists of report styles.")]
	[StiOrder(150)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiStylesCollection Styles { get; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets indent from the left side of band for offset of data levels. ")]
	[StiOrder(100)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[DefaultValue(15)]
	public int Indent { get; set; } = 15;


	[StiSerializable]
	[DefaultValue("")]
	[Browsable(false)]
	public string ReportPointer { get; set; } = string.Empty;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[Description("Gets or sets table margins.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiMargins Margins { get; set; } = new StiMargins(10.0);


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "CanBreak");
		JsonExtensions.RemoveProperty(val, "CanGrow");
		JsonExtensions.RemoveProperty(val, "CanShrink");
		JsonExtensions.RemoveProperty(val, "CountData");
		JsonExtensions.RemoveProperty(val, "DataSource");
		JsonExtensions.RemoveProperty(val, "DataRelation");
		JsonExtensions.RemoveProperty(val, "MasterComponent");
		JsonExtensions.RemoveProperty(val, "BusinessObject");
		JsonExtensions.RemoveProperty(val, "Filters");
		JsonExtensions.RemoveProperty(val, "FilterOn");
		JsonExtensions.RemoveProperty(val, "FilterEngine");
		JsonExtensions.RemoveProperty(val, "FilterMode");
		JsonExtensions.RemoveProperty(val, "SkipFirst");
		JsonExtensions.RemoveProperty(val, "Sort");
		JsonExtensions.RemoveProperty(val, "NewColumnAfter");
		JsonExtensions.RemoveProperty(val, "NewColumnBefore");
		JsonExtensions.RemoveProperty(val, "BreakIfLessThan");
		JsonExtensions.RemoveProperty(val, "LimitRows");
		JsonExtensions.RemoveProperty(val, "Columns");
		JsonExtensions.RemoveProperty(val, "ColumnWidth");
		JsonExtensions.RemoveProperty(val, "ColumnGaps");
		JsonExtensions.RemoveProperty(val, "ColumnDirection");
		JsonExtensions.RemoveProperty(val, "MinRowsInColumn");
		JsonExtensions.RemoveProperty(val, "EvenStyle");
		JsonExtensions.RemoveProperty(val, "OddStyle");
		JsonExtensions.RemoveProperty(val, "CalcInvisible");
		JsonExtensions.RemoveProperty(val, "KeepDetails");
		JsonExtensions.RemoveProperty(val, "PrintAtBottom");
		JsonExtensions.RemoveProperty(val, "PrintIfDetailEmpty");
		JsonExtensions.RemoveProperty(val, "PrintOnAllPages");
		JsonExtensions.RemoveProperty(val, "PrintOn");
		JsonExtensions.RemoveProperty(val, "ResetPageNumber");
		JsonExtensions.RemoveProperty(val, "Interaction");
		JsonExtensions.RemoveProperty(val, "BeginRenderEvent");
		JsonExtensions.RemoveProperty(val, "RenderingEvent");
		JsonExtensions.RemoveProperty(val, "EndRenderEvent");
		JsonExtensions.RemoveProperty(val, "GetCollapsedEvent");
		JsonExtensions.RemoveProperty(val, "GetBookmarkEvent");
		JsonExtensions.RemoveProperty(val, "GetHyperlinkEvent");
		JsonExtensions.RemoveProperty(val, "GetTagEvent");
		JsonExtensions.RemoveProperty(val, "GetBookmarkEvent");
		JsonExtensions.RemoveProperty(val, "GetHyperlinkEvent");
		JsonExtensions.RemoveProperty(val, "GetTagEvent");
		JsonExtensions.RemoveProperty(val, "GetToolTipEvent");
		JsonExtensions.RemoveProperty(val, "BeforePrintEvent");
		JsonExtensions.RemoveProperty(val, "AfterPrintEvent");
		JsonExtensions.RemoveProperty(val, "GetDrillDownReportEvent");
		JsonExtensions.RemoveProperty(val, "GetPointerEvent");
		JsonExtensions.RemoveProperty(val, "ClickEvent");
		JsonExtensions.RemoveProperty(val, "DoubleClickEvent");
		JsonExtensions.RemoveProperty(val, "MouseEnterEvent");
		JsonExtensions.RemoveProperty(val, "MouseLeaveEvent");
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ReportPointer", ReportPointer);
		JsonExtensions.AddPropertyInt(val, "Indent", Indent, 10);
		JsonExtensions.AddPropertyJObject(val, "Styles", Styles.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Margins", Margins.SaveToJsonObject(10.0, 10.0, 10.0, 10.0));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		Styles.Clear();
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "ReportPointer":
				ReportPointer = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Indent":
				Indent = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "Styles":
				Styles.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Margins":
			{
				StiMargins stiMargins = new StiMargins();
				stiMargins.LoadFromJsonObject((JObject)item.Value);
				Margins = stiMargins;
				break;
			}
			}
		}
	}

	public override object Clone(bool cloneProperties)
	{
		StiTableOfContents stiTableOfContents = base.Clone(cloneProperties) as StiTableOfContents;
		StiBorder border = Border;
		object obj = ((border != null) ? border.Clone() : null);
		stiTableOfContents.Border = (StiBorder)((obj is StiBorder) ? obj : null);
		StiBrush brush = Brush;
		object obj2 = ((brush != null) ? brush.Clone() : null);
		stiTableOfContents.Brush = (StiBrush)((obj2 is StiBrush) ? obj2 : null);
		return stiTableOfContents;
	}

	public override string GetHeaderText()
	{
		return ((object)this).ToString();
	}

	public override StiComponent CreateNew()
	{
		return new StiTableOfContents();
	}

	internal List<StiStyle> GetStylesList()
	{
		return (from StiBaseStyle s in Styles
			where s is StiStyle
			select s).Cast<StiStyle>().ToList();
	}

	private bool ShouldSerializeMargins()
	{
		if (Margins != null && Margins.Left == 10.0 && Margins.Right == 10.0 && Margins.Top == 10.0)
		{
			return Margins.Bottom != 10.0;
		}
		return true;
	}

	public StiTableOfContents()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiTableOfContents(RectangleD rect)
		: base(rect)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		PlaceOnToolbox = false;
		Styles = new StiStylesCollection
		{
			new StiStyle(Loc.GetMain("Heading") + "1")
			{
				Font = new Font("Arial", 10f, (FontStyle)1)
			},
			new StiStyle(Loc.GetMain("Heading") + "2")
			{
				Font = new Font("Arial", 8f)
			},
			new StiStyle(Loc.GetMain("Heading") + "3")
			{
				Font = new Font("Arial", 8f),
				TextBrush = (StiBrush)new StiSolidBrush(Color.DimGray)
			}
		};
	}
}
