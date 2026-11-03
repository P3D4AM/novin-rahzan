using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.QuickButtons;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiFooterBand), "Stimulsoft.Report.Images.Components.StiFooterBand.png")]
[StiToolbox(true)]
[StiV1Builder(typeof(StiFooterBandV1Builder))]
[StiV2Builder(typeof(StiFooterBandV2Builder))]
[StiContextTool(typeof(IStiPrintOnAllPages))]
[StiContextTool(typeof(IStiPrintIfEmpty))]
[StiContextTool(typeof(IStiStartNewPage))]
[StiContextTool(typeof(IStiBreakable))]
[StiContextTool(typeof(IStiCanGrow))]
[StiContextTool(typeof(IStiCanShrink))]
[StiContextTool(typeof(IStiKeepFooterTogether))]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiSelectAllQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfSelectAllQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiFooterBand : StiDynamicBand, IStiPrintOnAllPages, IStiPrintIfEmpty, IStiStartNewPage, IStiKeepFooterTogether, IStiPrintOnEvenOddPages
{
	private bool keepFooterTogether = true;

	private bool printIfEmpty;

	private StiFooterBandInfoV1 footerBandInfoV1;

	private StiFooterBandInfoV2 footerBandInfoV2;

	private static readonly object EventMoveFooterToBottom = new object();

	public override StiComponentId ComponentId => StiComponentId.StiFooterBand;

	public override string HelpUrl => "user-manual/report_internals_creating_lists_list_with_footer.htm";

	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that the footer is printed with data.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool KeepFooterTogether
	{
		get
		{
			return keepFooterTogether;
		}
		set
		{
			if (keepFooterTogether != value)
			{
				CheckBlockedException("KeepFooterTogether");
				keepFooterTogether = value;
			}
		}
	}

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
			FooterBandInfoV1.ForceCanBreak = value;
		}
	}

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

	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(330)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that the footer if data not present.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool PrintIfEmpty
	{
		get
		{
			return printIfEmpty;
		}
		set
		{
			if (printIfEmpty != value)
			{
				CheckBlockedException("PrintIfEmpty");
				printIfEmpty = value;
			}
		}
	}

	[DefaultValue(StiPrintOnEvenOddPagesType.Ignore)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(380)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets value indicates that the component is printed on even-odd pages.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiPrintOnEvenOddPagesType PrintOnEvenOddPages { get; set; }

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
	public StiFooterBandInfoV1 FooterBandInfoV1 => footerBandInfoV1 ?? (footerBandInfoV1 = new StiFooterBandInfoV1());

	[Browsable(false)]
	public StiFooterBandInfoV2 FooterBandInfoV2 => footerBandInfoV2 ?? (footerBandInfoV2 = new StiFooterBandInfoV2());

	[Browsable(false)]
	public override Color HeaderStartColor => Color.FromArgb(178, 197, 223);

	[Browsable(false)]
	public override Color HeaderEndColor => Color.FromArgb(178, 197, 223);

	public override int ToolboxPosition => 8;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Bands;

	public override int Priority => 300;

	public override StiComponentType ComponentType => StiComponentType.Detail;

	public override string LocalizedName => StiLocalization.Get("Components", "StiFooterBand");

	public event EventHandler MoveFooterToBottom
	{
		add
		{
			base.Events.AddHandler(EventMoveFooterToBottom, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventMoveFooterToBottom, value);
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "CanBreak", CanBreak, false, false);
		JsonExtensions.AddPropertyBool(val, "KeepFooterTogether", KeepFooterTogether, true, false);
		JsonExtensions.AddPropertyBool(val, "StartNewPage", StartNewPage, false, false);
		JsonExtensions.AddPropertyFloat(val, "StartNewPageIfLessThan", StartNewPageIfLessThan, 100f);
		JsonExtensions.AddPropertyBool(val, "PrintIfEmpty", PrintIfEmpty, false, false);
		JsonExtensions.AddPropertyEnum(val, "PrintOnEvenOddPages", (Enum)PrintOnEvenOddPages, (Enum)StiPrintOnEvenOddPagesType.Ignore);
		JsonExtensions.AddPropertyBool(val, "PrintOnAllPages", PrintOnAllPages, false, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "KeepFooterTogether":
				keepFooterTogether = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "StartNewPage":
				StartNewPage = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "StartNewPageIfLessThan":
				StartNewPageIfLessThan = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "PrintIfEmpty":
				printIfEmpty = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "PrintOnEvenOddPages":
				PrintOnEvenOddPages = StiJsonReportObjectHelper.DeserializeEnum<StiPrintOnEvenOddPagesType>(item);
				break;
			case "PrintOnAllPages":
				PrintOnAllPages = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
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
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[12]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.KeepFooterTogether(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.CanBreak(),
				propertiesHelper.Enabled(),
				propertiesHelper.PrintAtBottom(),
				propertiesHelper.PrintIfEmpty(),
				propertiesHelper.PrintOn(),
				propertiesHelper.PrintOnAllPages(),
				propertiesHelper.PrintOnEvenOddPages(),
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
		stiEventCollection.Add(StiPropertyCategories.ValueEvents, new StiPropertyEventId[2]
		{
			StiPropertyEventId.GetTagEvent,
			StiPropertyEventId.GetToolTipEvent
		});
		return stiEventCollection;
	}

	public override object Clone(bool cloneProperties, bool cloneComponents)
	{
		StiFooterBand stiFooterBand = base.Clone(cloneProperties, cloneComponents) as StiFooterBand;
		stiFooterBand.footerBandInfoV1 = FooterBandInfoV1.Clone() as StiFooterBandInfoV1;
		stiFooterBand.footerBandInfoV2 = FooterBandInfoV2.Clone() as StiFooterBandInfoV2;
		return stiFooterBand;
	}

	protected virtual void OnMoveFooterToBottom(EventArgs e)
	{
	}

	public void InvokeMoveFooterToBottom()
	{
		OnMoveFooterToBottom(EventArgs.Empty);
		if (base.Events[EventMoveFooterToBottom] is EventHandler eventHandler)
		{
			eventHandler(this, EventArgs.Empty);
		}
	}

	public override StiComponent CreateNew()
	{
		return new StiFooterBand();
	}

	public StiFooterBand()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiFooterBand(RectangleD rect)
		: base(rect)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
	}
}
