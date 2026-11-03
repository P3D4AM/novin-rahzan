using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.QuickButtons;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiCrossDataBand), "Stimulsoft.Report.Images.Components.StiCrossDataBand.png")]
[StiServiceCategoryBitmap(typeof(StiComponent), "Stimulsoft.Report.Images.Components.catCrossBands.png")]
[StiToolbox(true)]
[StiDesigner("Stimulsoft.Report.Components.Design.StiDataBandDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfDataBandDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter(typeof(StiCrossBandGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiCrossBandWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiV1Builder(typeof(StiCrossDataBandV1Builder))]
[StiContextTool(typeof(IStiDataSource))]
[StiContextTool(typeof(IStiMasterComponent))]
[StiContextTool(typeof(IStiDataRelation))]
[StiContextTool(typeof(IStiSort))]
[StiContextTool(typeof(IStiPrintIfDetailEmpty))]
[StiContextTool(typeof(IStiCanGrow))]
[StiContextTool(typeof(IStiCanShrink))]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiSelectAllQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfSelectAllQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiCrossDataBand : StiDataBand
{
	public override StiComponentId ComponentId => StiComponentId.StiCrossDataBand;

	public override string HelpUrl => "user-manual/report_internals_bands_band_types_cross_bands.htm";

	[Browsable(false)]
	[StiNonSerialized]
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

	[Browsable(false)]
	[StiNonSerialized]
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

	[Browsable(false)]
	[StiNonSerialized]
	public override bool NewColumnBefore
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
	public override bool NewColumnAfter
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
	public override bool SkipFirst
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
	public override float BreakIfLessThan
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
	public override bool GrowToHeight
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
	public override bool ResetPageNumber
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(false)]
	[StiNonSerialized]
	[Browsable(false)]
	public override bool StartNewPage
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(false)]
	[StiNonSerialized]
	[Browsable(false)]
	public override float StartNewPageIfLessThan
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	[DefaultValue(false)]
	[StiNonSerialized]
	[Browsable(false)]
	public override bool KeepHeaderTogether
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(false)]
	[StiNonSerialized]
	[Browsable(false)]
	public override bool KeepFooterTogether
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(false)]
	[StiNonSerialized]
	[Browsable(false)]
	public override bool KeepChildTogether
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(false)]
	[StiNonSerialized]
	[Browsable(false)]
	public override bool KeepGroupTogether
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(false)]
	[StiNonSerialized]
	[Browsable(false)]
	public override bool PrintAtBottom
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates whether the component can or cannot break its contents on several pages.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
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

	[DefaultValue(false)]
	[StiNonSerialized]
	[Browsable(false)]
	public override bool PrintOnAllPages
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	[DefaultValue(false)]
	[StiNonSerialized]
	[Browsable(false)]
	public override StiPrintOnType PrintOn
	{
		get
		{
			return base.PrintOn;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
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

	[StiNonSerialized]
	[Browsable(false)]
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

	[StiNonSerialized]
	[Browsable(false)]
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

	[StiNonSerialized]
	[Browsable(false)]
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

	[StiNonSerialized]
	[Browsable(false)]
	public override int MinRowsInColumn
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

	[StiNonSerialized]
	[Browsable(false)]
	public override StiColumnDirection ColumnDirection
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

	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public override bool ResetDataSource
	{
		get
		{
			return base.ResetDataSource;
		}
		set
		{
			base.ResetDataSource = value;
		}
	}

	public override int ToolboxPosition => 25;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Cross;

	public override int Priority => 300;

	public override string LocalizedCategory => StiLocalization.Get("Report", "CrossBands");

	public override string LocalizedName => StiLocalization.Get("Components", "StiCrossDataBand");

	public override bool IsCross => true;

	public override StiComponentType ComponentType
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Invalid comparison between Unknown and I4
			if (Report != null && (int)Report.EngineVersion == 2)
			{
				return base.ComponentType;
			}
			if (MasterComponent != null && MasterComponent is StiCrossDataBand && MasterComponent.Parent == Parent)
			{
				return StiComponentType.Detail;
			}
			return StiComponentType.Simple;
		}
	}

	[Browsable(false)]
	internal bool IsRightToLeft { get; set; }

	[StiNonSerialized]
	[Browsable(false)]
	public override StiDockStyle DockStyle
	{
		get
		{
			if (!IsRightToLeft)
			{
				return StiDockStyle.Left;
			}
			return StiDockStyle.Right;
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
	[StiNonSerialized]
	public override double MaxHeight
	{
		get
		{
			return base.MaxHeight;
		}
		set
		{
			base.MaxHeight = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override double MinHeight
	{
		get
		{
			return base.MinHeight;
		}
		set
		{
			base.MinHeight = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(500)]
	[Description("Gets or sets minimal width of band.")]
	[DefaultValue(0.0)]
	[StiSerializable]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual double MinWidth
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			SizeD minSize = base.MinSize;
			return ((SizeD)(ref minSize)).Width;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			SizeD minSize = base.MinSize;
			if (((SizeD)(ref minSize)).Width != value)
			{
				base.MinSize = new SizeD(value, 0.0);
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(510)]
	[Description("Gets or sets maximal width of band.")]
	[DefaultValue(0.0)]
	[StiSerializable]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual double MaxWidth
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			SizeD maxSize = base.MaxSize;
			return ((SizeD)(ref maxSize)).Width;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			SizeD maxSize = base.MaxSize;
			if (((SizeD)(ref maxSize)).Width != value)
			{
				base.MaxSize = new SizeD(value, 0.0);
			}
		}
	}

	[Browsable(true)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
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
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 30.0, 50.0);

	public override RectangleD SelectRectangle
	{
		get
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			double num = Page.Unit.ConvertFromHInches(HeaderSize);
			return new RectangleD(Left, Top, Width, Height + num);
		}
		set
		{
			double num = Page.Unit.ConvertFromHInches(HeaderSize);
			Left = ((RectangleD)(ref value)).Left;
			Top = ((RectangleD)(ref value)).Top;
			Width = ((RectangleD)(ref value)).Width;
			Height = ((RectangleD)(ref value)).Height - num;
		}
	}

	[Browsable(false)]
	public override RectangleD DisplayRectangle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return SelectRectangle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			SelectRectangle = value;
		}
	}

	internal int ColumnCurrent { get; set; }

	public override double HeaderSize
	{
		get
		{
			if (Report.Info.ShowHeaders)
			{
				return StiAlignValue.AlignToMaxGrid(15.0, Page.Unit.ConvertToHInches(Page.GridSize), true) - 4.0;
			}
			return 0.0;
		}
	}

	[Browsable(false)]
	internal bool ColumnMode { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "NewPageBefore");
		JsonExtensions.RemoveProperty(val, "NewPageAfter");
		JsonExtensions.RemoveProperty(val, "NewColumnBefore");
		JsonExtensions.RemoveProperty(val, "NewColumnAfter");
		JsonExtensions.RemoveProperty(val, "SkipFirst");
		JsonExtensions.RemoveProperty(val, "BreakIfLessThan");
		JsonExtensions.RemoveProperty(val, "GrowToHeight");
		JsonExtensions.RemoveProperty(val, "ResetPageNumber");
		JsonExtensions.RemoveProperty(val, "StartNewPage");
		JsonExtensions.RemoveProperty(val, "StartNewPageIfLessThan");
		JsonExtensions.RemoveProperty(val, "KeepHeaderTogether");
		JsonExtensions.RemoveProperty(val, "KeepFooterTogether");
		JsonExtensions.RemoveProperty(val, "KeepChildTogether");
		JsonExtensions.RemoveProperty(val, "KeepGroupTogether");
		JsonExtensions.RemoveProperty(val, "PrintAtBottom");
		JsonExtensions.AddPropertyBool(val, "CanBreak", CanBreak, false, false);
		JsonExtensions.RemoveProperty(val, "PrintOnAllPages");
		JsonExtensions.RemoveProperty(val, "PrintOn");
		JsonExtensions.RemoveProperty(val, "RightToLeft");
		JsonExtensions.RemoveProperty(val, "ColumnGaps");
		JsonExtensions.RemoveProperty(val, "ColumnWidth");
		JsonExtensions.RemoveProperty(val, "Columns");
		JsonExtensions.RemoveProperty(val, "MinRowsInColumn");
		JsonExtensions.RemoveProperty(val, "ColumnDirection");
		JsonExtensions.AddPropertyBool(val, "ResetDataSource", ResetDataSource, true, false);
		JsonExtensions.AddPropertyBool(val, "ResetDataSource", ResetDataSource, true, false);
		JsonExtensions.RemoveProperty(val, "DockStyle");
		JsonExtensions.RemoveProperty(val, "MinSize");
		JsonExtensions.RemoveProperty(val, "MaxSize");
		JsonExtensions.RemoveProperty(val, "MaxHeight");
		JsonExtensions.RemoveProperty(val, "MinHeight");
		JsonExtensions.AddPropertyDouble(val, "MinWidth", MinWidth, 0.0);
		JsonExtensions.AddPropertyDouble(val, "MaxWidth", MaxWidth, 0.0);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (!(text == "MinWidth"))
			{
				if (text == "MaxWidth")
				{
					MaxWidth = StiJsonReportObjectHelper.DeserializeDouble(item);
				}
			}
			else
			{
				MinWidth = StiJsonReportObjectHelper.DeserializeDouble(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Invalid comparison between Unknown and I4
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.ComponentEditor, new StiPropertyObject[1] { propertiesHelper.DataBandEditor() });
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[1] { propertiesHelper.Width() });
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[3]
			{
				propertiesHelper.Width(),
				propertiesHelper.MinWidth(),
				propertiesHelper.MaxWidth()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[6]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.OddStyle(),
				propertiesHelper.EvenStyle()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[7]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.OddStyle(),
				propertiesHelper.EvenStyle(),
				propertiesHelper.UseParentStyles()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[3]
			{
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.Enabled()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[6]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.Enabled(),
				propertiesHelper.KeepDetails(),
				propertiesHelper.PrintIfDetailEmpty()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[8]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.CalcInvisible(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.CanBreak(),
				propertiesHelper.Enabled(),
				propertiesHelper.KeepDetails(),
				propertiesHelper.PrintIfDetailEmpty()
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
		stiEventCollection.Add(StiPropertyCategories.RenderEvents, new StiPropertyEventId[3]
		{
			StiPropertyEventId.BeginRenderEvent,
			StiPropertyEventId.EndRenderEvent,
			StiPropertyEventId.RenderingEvent
		});
		stiEventCollection.Add(StiPropertyCategories.ValueEvents, new StiPropertyEventId[3]
		{
			StiPropertyEventId.GetCollapsedEvent,
			StiPropertyEventId.GetTagEvent,
			StiPropertyEventId.GetToolTipEvent
		});
		return stiEventCollection;
	}

	public override void RestoreState(string stateName)
	{
		base.RestoreState(stateName);
		SetColumnModeToParent();
	}

	internal void SetColumnModeToParent()
	{
		if (ColumnMode && Parent is StiDataBand && DataSource == null)
		{
			((StiDataBand)Parent).isEofValue = IsEof;
			((StiDataBand)Parent).isBofValue = IsBof;
			((StiDataBand)Parent).positionValue = Position;
		}
	}

	internal void GetColumnModeFromParent()
	{
		if (ColumnMode && Parent is StiDataBand && DataSource == null)
		{
			Enabled = ((StiDataBand)Parent).Enabled;
			isEofValue = ((StiDataBand)Parent).isEofValue;
			isBofValue = ((StiDataBand)Parent).isBofValue;
			positionValue = ((StiDataBand)Parent).positionValue;
		}
	}

	public override void First()
	{
		base.First();
		SetColumnModeToParent();
	}

	public override void Prior()
	{
		base.Prior();
		SetColumnModeToParent();
	}

	public override void Next()
	{
		base.Next();
		SetColumnModeToParent();
	}

	public override void Last()
	{
		base.Last();
		SetColumnModeToParent();
	}

	public override StiComponent CreateNew()
	{
		return new StiCrossDataBand();
	}

	internal StiCrossDataBand(bool columnMode)
		: this()
	{
		ColumnMode = columnMode;
		ResetDataSource = false;
	}

	public StiCrossDataBand()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiCrossDataBand(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = false;
		ResetDataSource = true;
		DockStyle = StiDockStyle.Left;
	}
}
