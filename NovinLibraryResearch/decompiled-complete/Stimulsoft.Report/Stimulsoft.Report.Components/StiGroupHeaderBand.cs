using System;
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
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.QuickButtons;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiGroupHeaderBand), "Stimulsoft.Report.Images.Components.StiGroupHeaderBand.png")]
[StiToolbox(true)]
[StiDesigner("Stimulsoft.Report.Components.Design.StiGroupHeaderBandDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfGroupHeaderBandDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiV1Builder(typeof(StiGroupHeaderBandV1Builder))]
[StiV2Builder(typeof(StiGroupHeaderBandV2Builder))]
[StiContextTool(typeof(IStiGroup))]
[StiContextTool(typeof(IStiCanGrow))]
[StiContextTool(typeof(IStiCanShrink))]
[StiContextTool(typeof(IStiBreakable))]
[StiContextTool(typeof(IStiPrintOnAllPages))]
[StiContextTool(typeof(IStiKeepHeaderTogether))]
[StiContextTool(typeof(IStiStartNewPage))]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiContextTool(typeof(IStiKeepGroupTogether))]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiSelectAllQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfSelectAllQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiGroupHeaderBand : StiDynamicBand, IStiGroup, IStiPrintOnAllPages, IStiKeepGroupTogether, IStiStartNewPage
{
	private bool keepGroupHeaderTogether = true;

	private StiGroupSortDirection summarySortDirection = StiGroupSortDirection.None;

	private StiGroupHeaderBandInfoV1 groupHeaderBandInfoV1;

	private StiGroupHeaderBandInfoV2 groupHeaderBandInfoV2;

	private static readonly object EventGetSummaryExpression = new object();

	private static readonly object EventGetValue = new object();

	private static readonly object EventGetCollapsed = new object();

	private static readonly object EventBeginRender = new object();

	private static readonly object EventRendering = new object();

	private static readonly object EventEndRender = new object();

	public override StiComponentId ComponentId => StiComponentId.StiGroupHeaderBand;

	public override string HelpUrl => "user-manual/report_internals_groups_groupheaderband.htm";

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates whether the component can or cannot break its contents on several pages.")]
	[StiShowInContextMenu]
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
			GroupHeaderBandInfoV1.ForceCanBreak = value;
		}
	}

	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override bool ResetPageNumber
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

	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that group header is printed with data together.")]
	[StiShowInContextMenu]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool KeepGroupHeaderTogether
	{
		get
		{
			return keepGroupHeaderTogether;
		}
		set
		{
			if (keepGroupHeaderTogether != value)
			{
				CheckBlockedException("KeepGroupHeaderTogether");
				keepGroupHeaderTogether = value;
			}
		}
	}

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that group is to be kept together.")]
	[StiShowInContextMenu]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool KeepGroupTogether { get; set; }

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(460)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that it is necessary to print every new string on a new page.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool StartNewPage { get; set; }

	[DefaultValue(100f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(470)]
	[Description("Gets or sets value which indicates how much free space on a page (in percentage terms) should be reserved for formation of a new page. The value should be set in the range from 0 to 100. If the value is 100 then, in any case, a new page will be formed. This property is used together with the StartNewPage property.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual float StartNewPageIfLessThan
	{
		get
		{
			return BreakIfLessThan;
		}
		set
		{
			BreakIfLessThan = value;
		}
	}

	[DefaultValue(StiGroupSortDirection.Ascending)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(220)]
	[Description("Gets or sets sorting direction of group by totals")]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiGroupSortDirection SortDirection { get; set; }

	[DefaultValue(StiGroupSortDirection.None)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(230)]
	[Description("Gets or sets function of calculating group totals for its sorting by totals.")]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	public StiGroupSortDirection SummarySortDirection
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Invalid comparison between Unknown and I4
			if (Report != null && (int)Report.EngineVersion != 1)
			{
				return summarySortDirection;
			}
			return StiGroupSortDirection.None;
		}
		set
		{
			summarySortDirection = value;
		}
	}

	[DefaultValue(StiGroupSummaryType.Sum)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(250)]
	[Description("Gets or sets the sorting direction of grouped data.")]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	public StiGroupSummaryType SummaryType { get; set; } = StiGroupSummaryType.Sum;


	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(370)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that the component is printed on all pages.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool PrintOnAllPages { get; set; }

	[Browsable(false)]
	public StiGroupHeaderBandInfoV1 GroupHeaderBandInfoV1 => groupHeaderBandInfoV1 ?? (groupHeaderBandInfoV1 = new StiGroupHeaderBandInfoV1());

	[Browsable(false)]
	public StiGroupHeaderBandInfoV2 GroupHeaderBandInfoV2 => groupHeaderBandInfoV2 ?? (groupHeaderBandInfoV2 = new StiGroupHeaderBandInfoV2());

	[Browsable(false)]
	public int Line { get; set; }

	public override int ToolboxPosition => 5;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Bands;

	public override StiComponentType ComponentType => StiComponentType.Detail;

	public override string LocalizedName => StiLocalization.Get("Components", "StiGroupHeaderBand");

	public override int Priority => 300;

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(160)]
	[StiSerializable]
	[Description("Gets or sets grouping condition.")]
	[Editor("Stimulsoft.Report.Components.Design.StiGroupConditionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiGroupConditionExpression Condition
	{
		get
		{
			return new StiGroupConditionExpression(this, "Condition");
		}
		set
		{
			value?.Set(this, "Condition", value.Value);
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(240)]
	[StiSerializable]
	[Description("Gets or sets summary expression which is used for group summary totals calculation.")]
	[Editor("Stimulsoft.Report.Components.Design.StiGroupSummaryEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	public StiGroupSummaryExpression SummaryExpression
	{
		get
		{
			return new StiGroupSummaryExpression(this, "SummaryExpression");
		}
		set
		{
			value?.Set(this, "SummaryExpression", value.Value);
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets an expression to fill a collapsed value.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiCollapsedExpression Collapsed
	{
		get
		{
			return new StiCollapsedExpression(this, "Collapsed");
		}
		set
		{
			value?.Set(this, "Collapsed", value.Value);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when when group summary expression is calculated.")]
	public StiGetSummaryExpressionEvent GetSummaryExpressionEvent
	{
		get
		{
			return new StiGetSummaryExpressionEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when when is checked condition of the group.")]
	public StiGetGroupConditionEvent GetValueEvent
	{
		get
		{
			return new StiGetGroupConditionEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when the Collapsed value is calculated.")]
	public StiGetCollapsedEvent GetCollapsedEvent
	{
		get
		{
			return new StiGetCollapsedEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when band is begin render.")]
	public StiBeginRenderEvent BeginRenderEvent
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

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when occurs rendering of one line data.")]
	public StiRenderingEvent RenderingEvent
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

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when ends rendering band.")]
	public StiEndRenderEvent EndRenderEvent
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

	[Browsable(false)]
	public override Color HeaderStartColor => Color.FromArgb(239, 155, 52);

	[Browsable(false)]
	public override Color HeaderEndColor => Color.FromArgb(239, 155, 52);

	public event StiValueEventHandler GetSummaryExpression
	{
		add
		{
			base.Events.AddHandler(EventGetSummaryExpression, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventGetSummaryExpression, value);
		}
	}

	public event StiValueEventHandler GetValue
	{
		add
		{
			base.Events.AddHandler(EventGetValue, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventGetValue, value);
		}
	}

	public event StiValueEventHandler GetCollapsed
	{
		add
		{
			base.Events.AddHandler(EventGetCollapsed, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventGetCollapsed, value);
		}
	}

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

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "CanBreak", CanBreak, false, false);
		JsonExtensions.AddPropertyBool(val, "KeepGroupHeaderTogether", KeepGroupHeaderTogether, true, false);
		JsonExtensions.AddPropertyBool(val, "KeepGroupTogether", KeepGroupTogether, false, false);
		JsonExtensions.AddPropertyBool(val, "StartNewPage", StartNewPage, false, false);
		JsonExtensions.AddPropertyFloat(val, "StartNewPageIfLessThan", StartNewPageIfLessThan, 100f);
		JsonExtensions.AddPropertyEnum(val, "SortDirection", (Enum)SortDirection, (Enum)StiGroupSortDirection.Ascending);
		JsonExtensions.AddPropertyEnum(val, "SummarySortDirection", (Enum)SummarySortDirection, (Enum)StiGroupSortDirection.None);
		JsonExtensions.AddPropertyEnum(val, "SummaryType", (Enum)SummaryType, (Enum)StiGroupSummaryType.Sum);
		JsonExtensions.AddPropertyBool(val, "PrintOnAllPages", PrintOnAllPages, false, false);
		JsonExtensions.AddPropertyJObject(val, "Condition", Condition.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "SummaryExpression", SummaryExpression.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetSummaryExpressionEvent", GetSummaryExpressionEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetValueEvent", GetValueEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetCollapsedEvent", GetCollapsedEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "BeginRenderEvent", BeginRenderEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "RenderingEvent", RenderingEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "EndRenderEvent", EndRenderEvent.SaveToJsonObject(mode));
		if ((int)mode == 0)
		{
			JsonExtensions.AddPropertyJObject(val, "Collapsed", Collapsed.SaveToJsonObject(mode));
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Expected O, but got Unknown
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Expected O, but got Unknown
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Expected O, but got Unknown
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Expected O, but got Unknown
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Expected O, but got Unknown
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "KeepGroupHeaderTogether":
				keepGroupHeaderTogether = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "KeepGroupTogether":
				KeepGroupTogether = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "StartNewPage":
				StartNewPage = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "StartNewPageIfLessThan":
				StartNewPageIfLessThan = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "SortDirection":
				SortDirection = StiJsonReportObjectHelper.DeserializeEnum<StiGroupSortDirection>(item);
				break;
			case "SummarySortDirection":
				summarySortDirection = StiJsonReportObjectHelper.DeserializeEnum<StiGroupSortDirection>(item);
				break;
			case "SummaryType":
				SummaryType = StiJsonReportObjectHelper.DeserializeEnum<StiGroupSummaryType>(item);
				break;
			case "PrintOnAllPages":
				PrintOnAllPages = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Condition":
			{
				StiGroupConditionExpression stiGroupConditionExpression = new StiGroupConditionExpression();
				stiGroupConditionExpression.LoadFromJsonObject((JObject)item.Value);
				Condition = stiGroupConditionExpression;
				break;
			}
			case "SummaryExpression":
			{
				StiGroupSummaryExpression stiGroupSummaryExpression = new StiGroupSummaryExpression();
				stiGroupSummaryExpression.LoadFromJsonObject((JObject)item.Value);
				SummaryExpression = stiGroupSummaryExpression;
				break;
			}
			case "Collapsed":
			{
				StiCollapsedExpression stiCollapsedExpression = new StiCollapsedExpression();
				stiCollapsedExpression.LoadFromJsonObject((JObject)item.Value);
				Collapsed = stiCollapsedExpression;
				break;
			}
			case "GetSummaryExpressionEvent":
			{
				StiGetSummaryExpressionEvent stiGetSummaryExpressionEvent = new StiGetSummaryExpressionEvent();
				stiGetSummaryExpressionEvent.LoadFromJsonObject((JObject)item.Value);
				GetSummaryExpressionEvent = stiGetSummaryExpressionEvent;
				break;
			}
			case "GetValueEvent":
			{
				StiGetGroupConditionEvent stiGetGroupConditionEvent = new StiGetGroupConditionEvent();
				stiGetGroupConditionEvent.LoadFromJsonObject((JObject)item.Value);
				GetValueEvent = stiGetGroupConditionEvent;
				break;
			}
			case "GetCollapsedEvent":
			{
				StiGetCollapsedEvent stiGetCollapsedEvent = new StiGetCollapsedEvent();
				stiGetCollapsedEvent.LoadFromJsonObject((JObject)item.Value);
				GetCollapsedEvent = stiGetCollapsedEvent;
				break;
			}
			case "BeginRenderEvent":
			{
				StiBeginRenderEvent stiBeginRenderEvent = new StiBeginRenderEvent();
				stiBeginRenderEvent.LoadFromJsonObject((JObject)item.Value);
				BeginRenderEvent = stiBeginRenderEvent;
				break;
			}
			case "RenderingEvent":
			{
				StiRenderingEvent stiRenderingEvent = new StiRenderingEvent();
				stiRenderingEvent.LoadFromJsonObject((JObject)item.Value);
				RenderingEvent = stiRenderingEvent;
				break;
			}
			case "EndRenderEvent":
			{
				StiEndRenderEvent stiEndRenderEvent = new StiEndRenderEvent();
				stiEndRenderEvent.LoadFromJsonObject((JObject)item.Value);
				EndRenderEvent = stiEndRenderEvent;
				break;
			}
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Invalid comparison between Unknown and I4
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.ComponentEditor, new StiPropertyObject[1] { propertiesHelper.GroupHeaderEditor() });
		if ((int)level != 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Data, new StiPropertyObject[5]
			{
				propertiesHelper.GroupHeaderExpression(),
				propertiesHelper.SortDirection(),
				propertiesHelper.SummarySortDirection(),
				propertiesHelper.SummaryExpression(),
				propertiesHelper.SummaryType()
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
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[1] { propertiesHelper.Height() });
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[3]
			{
				propertiesHelper.Height(),
				propertiesHelper.MinHeight(),
				propertiesHelper.MaxHeight()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[4]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[5]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
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
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[11]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.KeepGroupHeaderTogether(),
				propertiesHelper.KeepGroupTogether(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.CanBreak(),
				propertiesHelper.Enabled(),
				propertiesHelper.PrintAtBottom(),
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
			StiPropertyEventId.BeforePrintEvent,
			StiPropertyEventId.AfterPrintEvent
		});
		stiEventCollection.Add(StiPropertyCategories.RenderEvents, new StiPropertyEventId[3]
		{
			StiPropertyEventId.BeginRenderEvent,
			StiPropertyEventId.RenderingEvent,
			StiPropertyEventId.EndRenderEvent
		});
		stiEventCollection.Add(StiPropertyCategories.ValueEvents, new StiPropertyEventId[5]
		{
			StiPropertyEventId.GetCollapsedEvent,
			StiPropertyEventId.GetSummaryExpressionEvent,
			StiPropertyEventId.GetTagEvent,
			StiPropertyEventId.GetToolTipEvent,
			StiPropertyEventId.GetValueEvent
		});
		return stiEventCollection;
	}

	public override object Clone(bool cloneProperties, bool cloneComponents)
	{
		StiGroupHeaderBand stiGroupHeaderBand = base.Clone(cloneProperties, cloneComponents) as StiGroupHeaderBand;
		stiGroupHeaderBand.groupHeaderBandInfoV1 = GroupHeaderBandInfoV1.Clone() as StiGroupHeaderBandInfoV1;
		stiGroupHeaderBand.groupHeaderBandInfoV2 = GroupHeaderBandInfoV2.Clone() as StiGroupHeaderBandInfoV2;
		return stiGroupHeaderBand;
	}

	public override void SaveState(string stateName)
	{
		base.SaveState(stateName);
		base.States.PushInt(stateName, this, "lastPositionRendering", GroupHeaderBandInfoV1.LastPositionRendering);
		base.States.PushInt(stateName, this, "lastPositionLineRendering", GroupHeaderBandInfoV1.LastPositionLineRendering);
		base.States.PushInt(stateName, this, "line", Line);
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			GroupHeaderBandInfoV1.LastPositionRendering = base.States.PopInt(stateName, this, "lastPositionRendering");
			GroupHeaderBandInfoV1.LastPositionLineRendering = base.States.PopInt(stateName, this, "lastPositionLineRendering");
			Line = base.States.PopInt(stateName, this, "line");
		}
		base.RestoreState(stateName);
	}

	public override StiDataBand GetDataBand()
	{
		StiContainer parent = Parent;
		if (parent == null)
		{
			return null;
		}
		int num = Parent.Components.IndexOf(this) + 1;
		for (int i = num; i < Parent.Components.Count; i++)
		{
			if (Parent.Components[i] is StiDataBand)
			{
				return Parent.Components[i] as StiDataBand;
			}
		}
		return null;
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
		if (GetValueEvent != null)
		{
			stiEventsCollection.Add(GetValueEvent);
		}
		if (GetCollapsedEvent != null)
		{
			stiEventsCollection.Add(GetCollapsedEvent);
		}
		if (GetSummaryExpressionEvent != null)
		{
			stiEventsCollection.Add(GetSummaryExpressionEvent);
		}
		return stiEventsCollection;
	}

	protected virtual void OnGetSummaryExpression(StiValueEventArgs e)
	{
	}

	public void InvokeGetSummaryExpression(StiValueEventArgs e)
	{
		try
		{
			if (Report.CalculationMode == StiCalculationMode.Compilation)
			{
				OnGetSummaryExpression(e);
				if (base.Events[EventGetSummaryExpression] is StiValueEventHandler stiValueEventHandler)
				{
					stiValueEventHandler(this, e);
				}
			}
			else
			{
				OnGetSummaryExpression(e);
				e.Value = StiParser.ParseTextValue(SummaryExpression.Value, this);
				if (base.Events[EventGetSummaryExpression] is StiValueEventHandler stiValueEventHandler2)
				{
					stiValueEventHandler2(this, e);
				}
			}
			StiBlocklyHelper.InvokeBlockly(Report, this, GetSummaryExpressionEvent, e);
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "DoGetSummaryExpression...ERROR");
			StiLogService.Write(((object)this).GetType(), ex.Message);
			if (Report != null)
			{
				Report.WriteToReportRenderingMessages(Name + " " + ex.Message);
			}
		}
	}

	protected virtual void OnGetValue(StiValueEventArgs e)
	{
	}

	public void InvokeGetValue(StiValueEventArgs e)
	{
		try
		{
			if (Report.CalculationMode == StiCalculationMode.Compilation)
			{
				OnGetValue(e);
				if (base.Events[EventGetValue] is StiValueEventHandler stiValueEventHandler)
				{
					stiValueEventHandler(this, e);
				}
			}
			else
			{
				OnGetValue(e);
				e.Value = StiParser.ParseTextValue(Condition.Value, this);
				if (base.Events[EventGetValue] is StiValueEventHandler stiValueEventHandler2)
				{
					stiValueEventHandler2(this, e);
				}
			}
			StiBlocklyHelper.InvokeBlockly(Report, this, GetValueEvent, e);
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "DoGetValue...ERROR");
			StiLogService.Write(((object)this).GetType(), ex.Message);
			if (Report != null)
			{
				Report.WriteToReportRenderingMessages(Name + " " + ex.Message);
			}
		}
	}

	protected virtual void OnGetCollapsed(StiValueEventArgs e)
	{
	}

	public void InvokeGetCollapsed(StiValueEventArgs e)
	{
		try
		{
			OnGetCollapsed(e);
			if (Report != null && Report.CalculationMode == StiCalculationMode.Interpretation && Collapsed.Value.Length > 0)
			{
				e.Value = StiParser.ParseTextValue(Collapsed.Value, this);
			}
			if (base.Events[EventGetCollapsed] is StiValueEventHandler stiValueEventHandler)
			{
				stiValueEventHandler(this, e);
			}
			StiBlocklyHelper.InvokeBlockly(Report, this, GetCollapsedEvent, e);
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "DoGetCollapsed...ERROR");
			StiLogService.Write(((object)this).GetType(), ex.Message);
			if (Report != null)
			{
				Report.WriteToReportRenderingMessages(Name + " " + ex.Message);
			}
		}
	}

	protected virtual void OnBeginRender(EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		if (Report != null && (int)Report.EngineVersion == 1 && ResetPageNumber)
		{
			Report.EngineV1.ProcessResetPageNumber(Report);
		}
	}

	[StiEngine(/*Could not decode attribute arguments.*/)]
	public void InvokeBeginRender()
	{
		OnBeginRender(EventArgs.Empty);
		if (base.Events[EventBeginRender] is EventHandler eventHandler)
		{
			eventHandler(this, EventArgs.Empty);
		}
	}

	[StiEngine(/*Could not decode attribute arguments.*/)]
	public void InvokeBeginRender(StiDataBand dataBand, int position)
	{
		OnBeginRender(EventArgs.Empty);
		if (GroupHeaderBandInfoV1.LastPositionLineRendering != position)
		{
			GroupHeaderBandInfoV1.LastPositionLineRendering = position;
			Line++;
			StiComponentsCollection groupHeaderComponents = dataBand.DataBandInfoV1.GroupHeaderComponents;
			bool flag = false;
			foreach (StiGroupHeaderBand item in groupHeaderComponents)
			{
				if (flag)
				{
					item.Line = 0;
				}
				else if (item == this)
				{
					flag = true;
				}
			}
		}
		if (base.Events[EventBeginRender] is EventHandler eventHandler)
		{
			eventHandler(this, EventArgs.Empty);
		}
	}

	protected virtual void OnRendering(EventArgs e)
	{
	}

	[StiEngine(/*Could not decode attribute arguments.*/)]
	public void InvokeRendering()
	{
		try
		{
			OnRendering(EventArgs.Empty);
			if (base.Events[EventRendering] is EventHandler eventHandler)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), Name + " InvokeRendering...ERROR");
			StiLogService.Write(((object)this).GetType(), Name + " " + ex.Message);
			if (Report != null)
			{
				Report.WriteToReportRenderingMessages("Error in the 'Rendering' event of '" + Name + "' component: " + ex.Message);
			}
		}
	}

	[StiEngine(/*Could not decode attribute arguments.*/)]
	public void InvokeRendering(int position)
	{
		if (GroupHeaderBandInfoV1.LastPositionRendering != position)
		{
			GroupHeaderBandInfoV1.LastPositionRendering = position;
			OnRendering(EventArgs.Empty);
			if (base.Events[EventRendering] is EventHandler eventHandler)
			{
				eventHandler(this, EventArgs.Empty);
			}
			StiBlocklyHelper.InvokeBlockly(Report, this, RenderingEvent);
		}
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

	public override string GetHeaderText()
	{
		string text;
		if (string.IsNullOrEmpty(Alias))
		{
			text = Name;
		}
		else
		{
			bool? flag = Report?.Designer?.UseAliases;
			text = ((!StiOptions.Dictionary.ShowOnlyAliasForComponents || !flag.GetValueOrDefault(false)) ? (Name + " [" + Alias + "]") : Alias);
		}
		string text2 = (string.IsNullOrWhiteSpace(Condition.Value) ? Loc.Get("FormStyleDesigner", "NotSpecified") : Condition.Value);
		return text + "; " + Loc.Get("PropertyMain", "Condition") + ": " + text2;
	}

	public override StiComponent CreateNew()
	{
		return new StiGroupHeaderBand();
	}

	public object GetCurrentConditionValue()
	{
		return StiGroupHeaderBandV2Builder.GetCurrentConditionValue(this);
	}

	public StiGroupHeaderBand()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiGroupHeaderBand(RectangleD rect)
		: base(rect)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
	}
}
