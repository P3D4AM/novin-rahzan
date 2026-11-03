using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.QuickButtons;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiHierarchicalBand), "Stimulsoft.Report.Images.Components.StiHierarchicalBand.png")]
[StiToolbox(true)]
[StiDesigner("Stimulsoft.Report.Components.Design.StiDataBandDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfDataBandDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiV1Builder(typeof(StiHierarchicalBandV1Builder))]
[StiV2Builder(typeof(StiHierarchicalBandV2Builder))]
[StiContextTool(typeof(IStiDataSource))]
[StiContextTool(typeof(IStiMasterComponent))]
[StiContextTool(typeof(IStiDataRelation))]
[StiContextTool(typeof(IStiSort))]
[StiContextTool(typeof(IStiKeepGroupTogether))]
[StiContextTool(typeof(IStiKeepChildTogether))]
[StiContextTool(typeof(IStiKeepHeaderTogether))]
[StiContextTool(typeof(IStiKeepFooterTogether))]
[StiContextTool(typeof(IStiPrintOnAllPages))]
[StiContextTool(typeof(IStiPrintIfDetailEmpty))]
[StiContextTool(typeof(IStiStartNewPage))]
[StiContextTool(typeof(IStiCanGrow))]
[StiContextTool(typeof(IStiCanShrink))]
[StiContextTool(typeof(IStiBreakable))]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiDataSourceQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", 0)]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiDataRelationQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", 1)]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiMasterComponentQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", 2)]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiBandQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", 3)]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiSelectAllQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", 4)]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfDataSourceQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", 0)]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfDataRelationQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", 1)]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfMasterComponentQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", 2)]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfBandQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", 3)]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfSelectAllQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", 4)]
public class StiHierarchicalBand : StiDataBand
{
	private StiHierarchicalBandInfoV2 hierarchicalBandInfoV2;

	public override StiComponentId ComponentId => StiComponentId.StiHierarchicalBand;

	public override string HelpUrl => "user-manual/index.html?report_internals_hierarchical_band.htm";

	public override int ToolboxPosition => 13;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Bands;

	public override string LocalizedName => StiLocalization.Get("Components", "StiHierarchicalBand");

	[Browsable(false)]
	public override Color HeaderStartColor => Color.FromArgb(118, 167, 151);

	[Browsable(false)]
	public override Color HeaderEndColor => Color.FromArgb(118, 167, 151);

	[StiSerializable]
	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiDataColumnConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue("")]
	[Description("Gets or sets column which contains data key.")]
	[StiOrder(100)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string KeyDataColumn { get; set; } = string.Empty;


	[StiSerializable]
	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiDataColumnConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue("")]
	[Description("Gets or sets column which contains data master key.")]
	[StiOrder(110)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string MasterKeyDataColumn { get; set; } = string.Empty;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue("")]
	[Description("Gets or sets column which contains parent value which identifies parent rows.")]
	[StiOrder(120)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string ParentValue { get; set; } = string.Empty;


	[StiSerializable]
	[DefaultValue(20.0)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets indent from the left side of band for offset of data levels.")]
	[StiOrder(130)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual double Indent { get; set; } = 20.0;


	[StiSerializable]
	[Editor("Stimulsoft.Report.Components.Design.StiHeadersEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue("")]
	[Description("Gets or sets the list of headers for the hierarchical band.")]
	[StiOrder(140)]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string Headers { get; set; } = string.Empty;


	[StiSerializable]
	[Editor("Stimulsoft.Report.Components.Design.StiFootersEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue("")]
	[Description("Gets or sets the list of footers for the hierarchical band.")]
	[StiOrder(150)]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string Footers { get; set; } = string.Empty;


	[Browsable(false)]
	public StiHierarchicalBandInfoV2 HierarchicalBandInfoV2 => hierarchicalBandInfoV2 ?? (hierarchicalBandInfoV2 = new StiHierarchicalBandInfoV2());

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "KeyDataColumn", KeyDataColumn);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "MasterKeyDataColumn", MasterKeyDataColumn);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ParentValue", ParentValue);
		JsonExtensions.AddPropertyDouble(val, "Indent", Indent, 20.0);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Headers", Headers);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Footers", Footers);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "KeyDataColumn":
				KeyDataColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "MasterKeyDataColumn":
				MasterKeyDataColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ParentValue":
				ParentValue = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Indent":
				Indent = StiJsonReportObjectHelper.DeserializeDouble(item);
				break;
			case "Headers":
				Headers = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Footers":
				Footers = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Invalid comparison between Unknown and I4
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.ComponentEditor, new StiPropertyObject[1] { propertiesHelper.DataBandEditor() });
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Hierarchical, new StiPropertyObject[4]
			{
				propertiesHelper.KeyDataColumn(),
				propertiesHelper.MasterKeyDataColumn(),
				propertiesHelper.ParentValue(),
				propertiesHelper.Indent()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Hierarchical, new StiPropertyObject[6]
			{
				propertiesHelper.KeyDataColumn(),
				propertiesHelper.MasterKeyDataColumn(),
				propertiesHelper.ParentValue(),
				propertiesHelper.Indent(),
				propertiesHelper.Headers(),
				propertiesHelper.Footers()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.PageColumnBreak, new StiPropertyObject[4]
			{
				propertiesHelper.NewPageBefore(),
				propertiesHelper.NewPageAfter(),
				propertiesHelper.NewColumnBefore(),
				propertiesHelper.NewColumnAfter()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.PageColumnBreak, new StiPropertyObject[5]
			{
				propertiesHelper.NewPageBefore(),
				propertiesHelper.NewPageAfter(),
				propertiesHelper.NewColumnBefore(),
				propertiesHelper.NewColumnAfter(),
				propertiesHelper.SkipFirst()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.PageColumnBreak, new StiPropertyObject[6]
			{
				propertiesHelper.NewPageBefore(),
				propertiesHelper.NewPageAfter(),
				propertiesHelper.NewColumnBefore(),
				propertiesHelper.NewColumnAfter(),
				propertiesHelper.BreakIfLessThan(),
				propertiesHelper.SkipFirst()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Columns, new StiPropertyObject[5]
			{
				propertiesHelper.Columns(),
				propertiesHelper.ColumnWidth(),
				propertiesHelper.ColumnGaps(),
				propertiesHelper.ColumnDirection(),
				propertiesHelper.RightToLeft()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Columns, new StiPropertyObject[6]
			{
				propertiesHelper.Columns(),
				propertiesHelper.ColumnWidth(),
				propertiesHelper.ColumnGaps(),
				propertiesHelper.ColumnDirection(),
				propertiesHelper.MinRowsInColumn(),
				propertiesHelper.RightToLeft()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[1] { propertiesHelper.Height() });
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[3]
			{
				propertiesHelper.Height(),
				propertiesHelper.MaxHeight(),
				propertiesHelper.MinHeight()
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
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[4]
			{
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.CanBreak(),
				propertiesHelper.Enabled()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[12]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.CalcInvisible(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.CanBreak(),
				propertiesHelper.Enabled(),
				propertiesHelper.KeepDetails(),
				propertiesHelper.PrintAtBottom(),
				propertiesHelper.PrintIfDetailEmpty(),
				propertiesHelper.PrintOn(),
				propertiesHelper.PrintOnAllPages(),
				propertiesHelper.ResetPageNumber()
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

	public override StiComponent CreateNew()
	{
		return new StiHierarchicalBand();
	}

	public StiHierarchicalBand()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiHierarchicalBand(RectangleD rect)
		: base(rect)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = false;
	}
}
